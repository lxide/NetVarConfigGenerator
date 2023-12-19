
namespace RDBJsonExport.Model.ES.NetVariables 
{
    internal class Variable
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Offset { get; set; }
    }

    internal class NVS
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public int ScanCycle { get; set; }
        public bool IsSend { get; set; }
        public int Length { get; set; }
        public string Protocol { get; set; }
        public string TaskName { get; set; }
        public Variable[] Variables { get; set; }
    }

    internal class NVSSet
    {
        public NVS[] NVS { get; set; }
    }

}
