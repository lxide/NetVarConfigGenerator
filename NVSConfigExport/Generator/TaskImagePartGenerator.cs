
using RDBJsonExport.Model.ES.NetVariables;
using RDBJsonExport.Model.ES.Task;
using RDBJsonExport.Model.Gen.TaskImage;

namespace RDBJsonExport.Generator
{
    internal class TaskImagePartGenerator
    {
        static public NVSTaskImageParts CreateTaskImageParts(NVSSet nvsSet, PlcTask[] plcTasks)
        {
            List<NVSTaskImagePart> result = new List<NVSTaskImagePart>();

            var taskNameMap = CreateTaskNameMap(plcTasks);
            var taskNVSMap = CreateTaskNVSMap(nvsSet, plcTasks);
            foreach (var task in taskNVSMap.Keys)
            {
                result.Add(CreateTaskImagePart(taskNameMap[task], taskNVSMap[task]));
            }

            return new NVSTaskImageParts() {  TaskImageParts = result.ToArray() };
        }

        static private Dictionary<string, PlcTask> CreateTaskNameMap(PlcTask[] plcTasks)
        {
            Dictionary<string, PlcTask> taskNameMap = new Dictionary<string, PlcTask>();
            foreach (var plcTask in plcTasks)
                taskNameMap.Add(plcTask.Name, plcTask);

            return taskNameMap;
        }

        static private Dictionary<string, List<NVS>> CreateTaskNVSMap(NVSSet nvsSet, PlcTask[] plcTasks)
        {
            Dictionary<string, List<NVS>> taskNVSMap = new Dictionary<string, List<NVS>>();
            foreach (var plcTask in plcTasks)
                taskNVSMap.Add(plcTask.Name, new List<NVS>());

            foreach(var nvs in nvsSet.NVS)
                taskNVSMap[nvs.TaskName].Add(nvs);

            return taskNVSMap;
        }

        static private NVSTaskImagePart CreateTaskImagePart(PlcTask task, List<NVS> nvsList)
        {
            List<NVSPart> inputImagePart = new List<NVSPart>();
            List<NVSPart> outputImagePart = new List<NVSPart>();

            int inputLength = 0;
            int outputLength = 0;
            foreach (var nvs in nvsList)
            {
                NVSPart nvsPart;

                if (nvs.IsSend)
                    nvsPart = CreateNVSPart(nvs, outputImagePart, ref outputLength);
                else
                    nvsPart = CreateNVSPart(nvs, inputImagePart, ref inputLength);
            }

            return new NVSTaskImagePart()
            {
                Name = task.Name,
                Cycle= task.Cycle,
                Priority= task.Priority,
                InputLength = inputLength,
                OutputLength = outputLength,
                InputParts = inputImagePart.ToArray(),
                OutputParts = outputImagePart.ToArray()
            };
        }

        static private NVSPart CreateNVSPart(NVS nvs, List<NVSPart> imageParts, ref int imagePartlength)
        {
            NVSPart nvsPart = new NVSPart()
            {
                Length = nvs.Length,
                Name = nvs.Name,
                PartOffset = imagePartlength
            };

            imageParts.Add(nvsPart);
            imagePartlength += nvs.Length;

            return nvsPart;
        }
    }
}
