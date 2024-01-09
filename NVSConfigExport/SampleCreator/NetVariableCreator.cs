using RDBJsonExport.Model.ES;
using RDBJsonExport.Model.ES.NetVariables;
using RDBJsonExport.Model.ES.Task;

namespace RDBJsonExport.SampleCreator
{
    
    internal class PLCAddress
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }  
        public int Port { get; set; }
    }

    internal class NVSCreator
    {
        public static Dictionary<string, int> ElementaryTypes = new Dictionary<string, int>()
        {
            {ElementaryType.BBOOL.ToString(), 1},
            {ElementaryType.BYTE.ToString(),  1},
            {ElementaryType.INT.ToString(),   2},
            {ElementaryType.UINT.ToString(),  2},
            {ElementaryType.DINT.ToString(),  4},
            {ElementaryType.UDINT.ToString(), 4},
            {ElementaryType.WORD.ToString(),  2},
            {ElementaryType.DWORD.ToString(), 4},
            {ElementaryType.REAL.ToString(),  4}
        };

        static private PLCAddress GetPlcAddress(string plcName, PLCAddress[] netVarPLCs)
        {
            foreach(var plcAddr in netVarPLCs) 
            {
                if (plcAddr.Name == plcName) 
                    return plcAddr;
            }
            return null;
        }

        static private List<PLCAddress> GetTargetPlcAddress(string plcName, List<PLCAddress> netVarPLCs)
        {
            List <PLCAddress> targetAddrs = new List<PLCAddress>();
            foreach (var plcAddr in netVarPLCs)
            {
                if (plcAddr.Name != plcName)
                    targetAddrs.Add(plcAddr);
            }
            return targetAddrs;
        }

        public static NVSSet CreateSampleNetGVS(string hostPLC, NetVarConfigSet varConfigSet, Config config)
        {
            List<NVS> NetGVSList = new List<NVS>();

            string setName = varConfigSet.SetName;
            foreach (NetVarConfig netVarConfig in varConfigSet.NetVars)
            {
                foreach (var netVarTarget in netVarConfig.Targets)
                {
                    var host = GetPlcAddress(netVarTarget.Source, config.Plcs);
                    var target = GetPlcAddress(netVarTarget.Destination, config.Plcs);

                    if (hostPLC == netVarTarget.Source)
                        NetGVSList.Add(CreateNVS(setName, host, target, true, netVarConfig.Task, netVarConfig.Cycle));
                    else if (hostPLC == netVarTarget.Destination)
                        NetGVSList.Add(CreateNVS(setName, target, host, false, netVarConfig.Task, netVarConfig.Cycle));

                }
            }

            return new NVSSet()
            {
                NVS = NetGVSList.ToArray()
            };
        }

        private static NVS CreateNVS(string setName, PLCAddress host, PLCAddress target, bool isSend, string task, int cycle)
        {
            string gvsName = "NVS_" + setName + "_" + task + "_";
            PLCAddress address;

            if (isSend)
            {
                gvsName += (host.Name + "_TO_" + target.Name);
                address = host;
            }
            else
            {
                gvsName += (target.Name + "_TO_" + host.Name);
                address = target;
            }

            List<Variable> variables = new List<Variable>();
            int length = CreateVariables(gvsName, ref variables);
            return new NVS() 
            { 
                Name = gvsName,
                IsSend = isSend,
                IPAddress = address.IPAddress, 
                Port = address.Port, 
                Protocol = "UDP", 
                ScanCycle = cycle, 
                TaskName = task, 
                Length = length,
                Variables = variables.ToArray()
            };
        }

        private static int CreateVariables(string gvsName, ref List<Variable> variables)
        {
            int i = 0;
            int offset = 0;

            foreach (var elemType in ElementaryTypes.Keys)
            {
                int len = ElementaryTypes[elemType];
                AddVariable(gvsName, elemType, i++, offset, ref variables);

                offset += len;
            }
            return offset;
        }

        private static void AddVariable(string gvsName, string varType, int index, int offset, ref List<Variable> variables)
        {
            variables.Add(new Variable() { Name = gvsName + "_" + varType + "_" + index, Offset = offset, Type = varType });
        }
    }
}