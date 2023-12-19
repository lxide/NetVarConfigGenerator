
namespace RDBJsonExport.Model.Gen.TaskImage
{
    internal class NVSTaskImagePart
    {
        public string Name { get; set; }
        public int Cycle { get; set; }
        public int Priority { get; set; }
        public int InputLength { get; set; }
        public int OutputLength { get; set; }
        public NVSPart[] InputParts { get; set; }
        public NVSPart[] OutputParts { get; set; }
    }

    internal class NVSTaskImageParts
    {
        public NVSTaskImagePart[] TaskImageParts { get; set; }
    }
}
