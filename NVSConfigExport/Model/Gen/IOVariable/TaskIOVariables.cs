namespace RDBJsonExport.Model.Gen.IOVariable
{
    internal class TaskIOVariables
    {
        public string TaskName { get; set; }
        public int Cycle { get; set; }
        public int InputImagePartLength { get; set; }
        public int OutputImagePartLength { get; set; }
        public TaskIOVariable[] InputIOVariables { get; set; }
        public TaskIOVariable[] OutputIOVariables { get; set; }
    }

    internal class IOVariableSet
    {
        public TaskIOVariables[] TaskIOVariablesSet { get; set;}
    }
}
