
namespace RDBJsonExport.Model.ES.Task
{
    internal class PlcTask
    {
        public PlcTask(string name, int cycle, int priority)
        {
            Name = name;
            Priority = priority;
            Cycle = cycle;
        }

        public string Name { get; set; }
        public int Priority { get; set; }
        public int Cycle { get; set; }
    }
}
