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

        public static NVSSet CreateSampleNetGVS(string hostPLC, Config config)
        {
            List<NVS> NetGVSList = new List<NVS>();

            int i = 0;
            var tasks = config.Tasks;
            int taskNumber = tasks.Length;
            foreach(var netVarTarget in config.NetVar.Targets)
            {
                if (hostPLC == netVarTarget.Source)
                {
                    var host = GetPlcAddress(netVarTarget.Source, config.Plcs);
                    var target = GetPlcAddress(netVarTarget.Destination, config.Plcs);

                    NetGVSList.Add(CreateNVS(host, target, true,  tasks[i % taskNumber], config.NetVar.MessageSize, config.NetVar.Cycle));
                    NetGVSList.Add(CreateNVS(host, target, false, tasks[i % taskNumber], config.NetVar.MessageSize, config.NetVar.Cycle));
                }
                i++;
            }

            return new NVSSet()
            {
                NVS = NetGVSList.ToArray()
            };
        }

        private static NVS CreateNVS(PLCAddress host, PLCAddress target, bool isSend, PlcTask task, int msgSize, int cycle)
        {
            string gvsName = "NVS_";
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
            int length = CreateVariables(gvsName, msgSize, ref variables);
            return new NVS() 
            { 
                Name = gvsName,
                IsSend = isSend,
                IPAddress = address.IPAddress, 
                Port = address.Port, 
                Protocol = "UDP", 
                ScanCycle = cycle, 
                TaskName = task.Name, 
                Length = length,
                Variables = variables.ToArray()
            };
        }

        private static int CreateVariables(string gvsName, int msgSize, ref List<Variable> variables)
        {
            int i = 0;
            int offset = 0;

            foreach (var elemType in ElementaryTypes.Keys)
            {
                int len = ElementaryTypes[elemType];
                AddVariable(gvsName, elemType, i++, offset, ref variables);

                offset += len;
                if (offset >= msgSize)
                    break;
            }
            return offset;
        }

        private static void AddVariable(string gvsName, string varType, int index, int offset, ref List<Variable> variables)
        {
            variables.Add(new Variable() { Name = gvsName + "_" + varType + "_" + index, Offset = offset, Type = varType });
        }
    }
}