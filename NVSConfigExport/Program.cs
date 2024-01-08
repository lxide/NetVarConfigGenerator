
using RDBJsonExport.Generator;
using RDBJsonExport.Model.ES.NetVariables;
using RDBJsonExport.Model.ES.Task;
using RDBJsonExport.Model.Gen.IOVariable;
using RDBJsonExport.Model.Gen.TaskImage;
using RDBJsonExport.SampleCreator;


namespace RDBJsonExport
{
    internal class Program
    {
        static string sPath = "D:\\Git\\RDB\\NetVar_RDB\\_SampleData\\";

        class OutData
        {
            public PlcTask[] PlcTasks { get; set; }
            public NVSSet NVSSet { get; set; }
            public NVSTaskImageParts TaskImageParts { get; set; }
            public IOVariableSet TaskIOVariableSet { get; set; }
            public Dictionary<string, string> DDSXMLProfile { get; set; }
        }

        static void Main(string[] args)
        {
            Config config = ConfigCreator.CreateOrLoadConfig(sPath);

            foreach(PLCAddress plc in config.Plcs)
                CreatePLCData(plc.Name, config);
        }

        static void CreatePLCData (string plcName, Config config)
        {
            foreach (var varConfigSet in config.NetVarSets)
                CreateDataSet(plcName, config, varConfigSet);
        }

        static void CreateDataSet(string plcName, Config config, NetVarConfigSet varConfigSet)
        {
            OutData data = new OutData();

            CreateESConfig(plcName, varConfigSet, config, data);
            GenerateConfig(data);
            WriteFiles(plcName, varConfigSet.SetName, data);
        }

        // Create sample config as user input
        static void CreateESConfig(string plcName, NetVarConfigSet varConfigSet, Config config, OutData data)
        {
            // Tasks
            data.PlcTasks = config.Tasks;
            CreateNetVarConfig(plcName, varConfigSet, config, data);
        }

        // Create sample config as user input
        static void CreateNetVarConfig(string plcName, NetVarConfigSet varConfigSet, Config config, OutData data)
        {
            // Network Variables
            data.NVSSet = NVSCreator.CreateSampleNetGVS(plcName, varConfigSet, config);
        }

        // Generate Config from ES config
        static void GenerateConfig(OutData data)
        {
            // Calculated ImageParts
            data.TaskImageParts = TaskImagePartGenerator.CreateTaskImageParts(data.NVSSet, data.PlcTasks);
            // Task IO Variables
            data.TaskIOVariableSet = IOVariableConfigGenerator.CreateTaskIOVariables(data.TaskImageParts, data.NVSSet);
            // Create DDSXMLProfile
            data.DDSXMLProfile = NVSXMLProfileCreator.CreateNetVarXMLProfile(data.NVSSet);
        }

        static void WriteFiles(string plcName, string netvarSetName, OutData data)
        {
            string path = sPath + plcName + "\\" + netvarSetName + "\\";
            ConfigRW writer = new ConfigRW();

            // Write all files
            ConfigRW.WriteNetworkVariables(path, data.NVSSet);
            ConfigRW.WriteTaskImageParts(path, data.TaskImageParts);
            ConfigRW.WriteTaskIOVariables(path, data.TaskIOVariableSet);
            ConfigRW.WriteNetVarDDSXMLProfile(path, data.DDSXMLProfile);
        }
    }
}