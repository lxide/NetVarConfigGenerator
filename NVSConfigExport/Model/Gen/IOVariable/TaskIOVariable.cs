using RDBJsonExport.Model.ES;

namespace RDBJsonExport.Model.Gen.IOVariable
{
    internal class TaskIOVariable
    {
        public string Name { get; set; }
        public int ImagePartOffset { get; set; }
        public byte BitMask { get; set; }
        public string VariableType { get; set; }
        public string IOType { get; set; }
    }
}
