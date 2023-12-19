using System.Text.Json.Serialization;
using System.Text.Json;

using RDBJsonExport.Model.Gen.TaskImage;
using RDBJsonExport.Model.Gen.IOVariable;
using RDBJsonExport.Model.ES.NetVariables;

namespace RDBJsonExport
{
    internal class ConfigRW
    {
        static string sSamplegenConfigPath = "Config";
        static string sTaskImagePartPath = "TaskImagePart";
        static string sTaskIOVariablePath = "TaskIOVariable";
        static string sNetVarPath = "NetVar";

        static string sSamplegenConfigFile = "SampleGenConfig.json";
        static string sImagePartFile = "ImageParts.json";
        static string sTaskIOVariableFile = "TaskIOVariables.json";
        static string sNetVarFile = "NVSSet.json";

        static JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        static private string CreateSubDirectory(string path, string subDir, bool deleteExists = true)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string path2 = Path.Combine(path, subDir);
            if (Directory.Exists(path2))
            {
                if (deleteExists)
                {
                    Directory.Delete(path2, true);
                    Directory.CreateDirectory(path2);
                }
            }
            else
            {
                Directory.CreateDirectory(path2);
            }

            return path2;
        }

        static public void WriteConfig(string path, Config config)
        {
            string subPath = CreateSubDirectory(path, sSamplegenConfigPath);

            string fileName = Path.Combine(subPath, sSamplegenConfigFile);
            string jsonString = JsonSerializer.Serialize(config, JsonOptions);

            File.WriteAllText(fileName, jsonString);
        }

        static public Config ReadConfig(string path)
        {
            string subPath = Path.Combine(path, sSamplegenConfigPath);
            string fileName = Path.Combine(subPath, sSamplegenConfigFile);

            if (!File.Exists(fileName))
                return null;
            string jsonString = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<Config>(jsonString, JsonOptions);
        }

        static public void WriteTaskImageParts(string path, NVSTaskImageParts taskImageParts)
        {
            string subPath = CreateSubDirectory(path, sTaskImagePartPath);
            string fileName = Path.Combine(subPath, sImagePartFile);

            string jsonString = JsonSerializer.Serialize(taskImageParts, JsonOptions);
            File.WriteAllText(fileName, jsonString);
        }

        static public void WriteTaskIOVariables(string path, IOVariableSet ioVariableSet)
        {
            string subPath = CreateSubDirectory(path, sTaskIOVariablePath);
            string fileName = Path.Combine(subPath, sTaskIOVariableFile);

            string jsonString = JsonSerializer.Serialize(ioVariableSet, JsonOptions);
            File.WriteAllText(fileName, jsonString);
        }

        static public void WriteNetworkVariables(string path, NVSSet netGVS)
        {
            string subPath = CreateSubDirectory(path, sNetVarPath);
            string fileName = Path.Combine(subPath, sNetVarFile);

            string jsonString = JsonSerializer.Serialize(netGVS, JsonOptions);
            File.WriteAllText(fileName, jsonString);
        }

        static public void WriteNetVarDDSXMLProfile(string path, Dictionary<string, string> xmlProfiles)
        {
            if (xmlProfiles == null || xmlProfiles.Count == 0)
                return;

            string subPath = CreateSubDirectory(path, sNetVarPath, false);

            foreach (var profile in xmlProfiles)
            {
                string fileName = Path.Combine(subPath, profile.Key + ".xml");
                File.WriteAllText(fileName, profile.Value);
            }
        }
    }
}
