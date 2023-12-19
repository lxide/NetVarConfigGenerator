
using RDBJsonExport.Model.ES.NetVariables;
using RDBJsonExport.Model.Gen.IOVariable;
using RDBJsonExport.Model.Gen.TaskImage;

namespace RDBJsonExport.Generator
{
    internal class IOVariableConfigGenerator
    {
        public static IOVariableSet CreateTaskIOVariables (NVSTaskImageParts taskImageParts, NVSSet nvsSet)
        {
            List<TaskIOVariables> configs = new List<TaskIOVariables>();

            Dictionary<string, NVS> nvsNameMap = CreateNVSNameMap(nvsSet);

            foreach(var taskImagePart in taskImageParts.TaskImageParts)
            {
                List<TaskIOVariable> inputVariables = new List<TaskIOVariable>();
                List<TaskIOVariable> outputVariables = new List<TaskIOVariable>();

                foreach (var inputPart in taskImagePart.InputParts)
                    inputVariables.AddRange(CreateNVSTaskIOVariables(inputPart, nvsNameMap[inputPart.Name]));

                foreach (var outputPart in taskImagePart.OutputParts)
                    outputVariables.AddRange(CreateNVSTaskIOVariables(outputPart, nvsNameMap[outputPart.Name]));

                TaskIOVariables config = new TaskIOVariables()
                {
                    TaskName = taskImagePart.Name,
                    InputImagePartLength = taskImagePart.InputLength,
                    OutputImagePartLength = taskImagePart.OutputLength,
                    Cycle = taskImagePart.Cycle,
                    InputIOVariables = inputVariables.ToArray(),
                    OutputIOVariables = outputVariables.ToArray()
                };
                configs.Add(config);
            }

            return new IOVariableSet() { TaskIOVariablesSet = configs.ToArray() };
        }

        private static Dictionary<string, NVS> CreateNVSNameMap(NVSSet nvsSet)
        {
            Dictionary<string, NVS> nvsNameMap = new Dictionary<string, NVS>();

            foreach(NVS nvs in nvsSet.NVS)
                nvsNameMap[nvs.Name] = nvs;

            return nvsNameMap;
        }

        private static List<TaskIOVariable> CreateNVSTaskIOVariables(NVSPart nvsPart, NVS nvs)
        {
            List<TaskIOVariable> taskIOVars = new List<TaskIOVariable>();
            foreach (var v in nvs.Variables)
            {
                taskIOVars.Add(new TaskIOVariable()
                {
                    Name = v.Name,
                    IOType = v.Type,
                    VariableType = v.Type,
                    BitMask = 0,
                    ImagePartOffset = nvsPart.PartOffset + v.Offset
                });
            }
            return taskIOVars;
        }
    }
}
