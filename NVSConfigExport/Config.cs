
using RDBJsonExport.Model.ES.Task;
using RDBJsonExport.SampleCreator;

namespace RDBJsonExport
{

    internal class NetVarTarget
    {
        public string Source { get; set; }
        public string Destination { get; set; }
    }
    internal class NetVarConfig
    {
        public string Name { get; set; }
        public string Task { get; set; }
        public short Cycle { get; set; }
        public NetVarTarget[] Targets { get; set; }
    }

    internal class NetVarConfigSet
    {
        public string SetName { get; set; }
        public NetVarConfig[] NetVars { get; set; }
    }
    internal class Config
    {
        public PLCAddress[] Plcs { get; set; }
        public PlcTask[] Tasks { get; set; }
        public NetVarConfigSet[] NetVarSets { get; set; }
    }
}