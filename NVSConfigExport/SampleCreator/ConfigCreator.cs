
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
                NetVar = CreateNetVar()
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
                new PlcTask("Task_Cycle20", 20, 50),
                new PlcTask("Task_Cycle15", 15, 100),
                new PlcTask("Task_Cycle10", 10, 150),
                new PlcTask("Task_Cycle5", 5, 200),
            };
        }

        private static NetVarConfig CreateNetVar()
        {
            List<NetVarTarget> targets = new List<NetVarTarget>();
            targets.Add(new NetVarTarget() { Source = "Plc1", Destination = "Plc2"} );
            targets.Add(new NetVarTarget() { Source = "Plc2", Destination = "Plc1" });

            return new NetVarConfig() { MessageSize = 4000, Cycle = 10, Targets = targets.ToArray() };
        }
    }
}
