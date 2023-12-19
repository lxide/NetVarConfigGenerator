

using RDBJsonExport.Model.ES.NetVariables;
using System.Text;
using System.Xml;

namespace RDBJsonExport.Generator
{
    internal class NVSXMLProfileCreator
    {
        // Create DDS XML Profiles for one Station
        static public Dictionary<string, string> CreateNetVarXMLProfile(NVSSet nvsSet)
        {
            Dictionary<string, string> xmlProfiles = new Dictionary<string, string>();

            CreatePubSubXMLProfiles(nvsSet, true, ref xmlProfiles);
            CreatePubSubXMLProfiles(nvsSet, false, ref xmlProfiles);

            return xmlProfiles;
        }

        static private void CreatePubSubXMLProfiles (NVSSet nvsSet, bool isSend, ref Dictionary<string, string> xmlProfiles)
        {
            string xmlProfileName;
            string xmlProfile;

            foreach (var nvs in nvsSet.NVS)
            {
                if (nvs.IsSend != isSend)
                    continue;

                xmlProfile = CreateNetGVSProfile(nvs);
                if (isSend)
                    xmlProfileName = "DDS_Publisher_" + nvs.Name;
                else
                    xmlProfileName = "DDS_Subscriber_" + nvs.Name;

                xmlProfiles[xmlProfileName] = xmlProfile;
            }
        }

        static private string CreateNetGVSProfile (NVS nvs) 
        {
            string typeName = nvs.Name;

            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = new UTF8Encoding(false);

                // New Line
                settings.NewLineChars = Environment.NewLine;

                using (XmlWriter xmlWriter = XmlWriter.Create(ms, settings))
                {
                    xmlWriter.WriteStartDocument(false);

                    xmlWriter.WriteStartElement("types", "http://www.eprosima.com/XMLSchemas/fastRTPS_Profiles");
                    xmlWriter.WriteStartElement("type");
                    xmlWriter.WriteStartElement("struct");
                    xmlWriter.WriteAttributeString("name", typeName);

                    foreach (var variable in nvs.Variables)
                        WriteGVSMember(xmlWriter, variable);

                    xmlWriter.WriteEndElement(); // struct
                    xmlWriter.WriteEndElement(); // type
                    xmlWriter.WriteEndElement(); // types

                    xmlWriter.WriteEndDocument();
                }

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private static void WriteGVSMember(XmlWriter xmlWriter, Variable variable)
        {
            xmlWriter.WriteStartElement("member");

            xmlWriter.WriteAttributeString("name", variable.Name);
            xmlWriter.WriteAttributeString("type", GetDDSType(variable.Type));

            xmlWriter.WriteEndElement();
        }

        private static string GetDDSType(string type)
        {
            string ddsType = string.Empty;
            type = type.ToUpper();
            switch(type)
            {
                case "BIT":   ddsType = "boolean"; break;
                case "BOOL":  ddsType = "boolean"; break;
                case "BBOOL": ddsType = "boolean"; break;
                case "BYTE":  ddsType = "byte";    break;
                case "WORD":  ddsType = "uint16";  break;
                case "DWORD": ddsType = "uint32";  break;
                case "LWORD": ddsType = "uint64";  break;
                case "SINT":  ddsType = "int8";    break;
                case "USINT": ddsType = "uint8";   break;
                case "INT":   ddsType = "int16";   break;
                case "UINT":  ddsType = "uint16";  break;
                case "DINT":  ddsType = "int32";   break;
                case "UDINT": ddsType = "uint32";  break;
                case "LINT":  ddsType = "int64";   break;
                case "ULINT": ddsType = "uint64";  break;
                case "REAL":  ddsType = "float32"; break;
                case "LREAL": ddsType = "float64"; break;
                case "CHAR":  ddsType = "char8";   break;
                default:      ddsType = type;      break;
            }
            return ddsType;
        }
    }
}
