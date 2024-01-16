
using RDBJsonExport.Model.ES.Task;

namespace RDBJsonExport.SampleCreator
{
    internal class ConfigCreator
    {
        public static Config CreateOrLoadConfig(string path)
        {
            Config config = ConfigRW.ReadConfig(path);
            if (config != null)
                return config;

            config  = CreateConfig();
            ConfigRW.WriteConfig(path, config);
            return config;
        }

        private static Config CreateConfig()
        {
            return new Config()
            {
                Plcs = CreatePlcs(),
                Tasks = CreateTasks(),
                NetVarSets = CreateNetVarSets()
            };
        }

        private static PLCAddress[] CreatePlcs()
        {
            return new PLCAddress[]
            {
                new PLCAddress() {Name ="Plc1", IPAddress = "192.168.2.94", Port = 5001 },
                new PLCAddress() {Name ="Plc2", IPAddress = "192.168.2.213", Port = 5001 },
            };
        }

        private static PlcTask[] CreateTasks()
        {
            return new PlcTask[]
            {
                new PlcTask("Task_Cycle5",  5, 100),
                new PlcTask("Task_Cycle15", 15, 101),
                new PlcTask("Task_Cycle1000", 1000, 102),
                new PlcTask("Task_Cycle2000", 2000, 103),
            };
        }

        private static NetVarConfigSet[] CreateNetVarSets()
        {
            List<NetVarConfigSet> netVarSets = new List<NetVarConfigSet>();

            netVarSets.Add(CreateNetVarSet("set1"));
            netVarSets.Add(CreateNetVarSet("set2"));

            return netVarSets.ToArray();
        }

        private static NetVarConfigSet CreateNetVarSet(string setName)
        {
            return new NetVarConfigSet()
            {
                SetName = setName,
                NetVars = CreateNetVars(setName)
            };
        }

        private static NetVarConfig[] CreateNetVars(string setName)
        {
            List<NetVarConfig> netVars = new List<NetVarConfig>();

            netVars.Add(CreateNetVar("NVS_1", "Task_Cycle1000"));
            netVars.Add(CreateNetVar("NVS_2", "Task_Cycle2000"));

            return netVars.ToArray();
        }
        private static NetVarConfig CreateNetVar(string nvsName, string taskName)
        {
            List<NetVarTarget> targets = new List<NetVarTarget>();
            targets.Add(new NetVarTarget() { Source = "Plc1", Destination = "Plc2" });

            return new NetVarConfig() { Name = nvsName, Task = taskName, Cycle = 10, Targets = targets.ToArray() };
        }
    }
}
