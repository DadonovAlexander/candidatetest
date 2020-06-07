using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORTPR_ModBusTable.Models
{
    class DeviceTypeInfos
    {
        public List<DeviceType> TypeInfos;

        public static List<DeviceType> LoadFromJsonFile(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return ((DeviceTypeInfos)serializer.Deserialize(file, typeof(DeviceTypeInfos))).TypeInfos;
            }
        }
    }
    class DeviceType
    {
        public string TypeName;
        public Dictionary<string, string> Propertys;

        public DeviceType()
        {
            Propertys = new Dictionary<string, string>();
        }

        public DeviceType(string typeName, Dictionary<string, string> propertys)
        {
            TypeName = typeName;
            Propertys = propertys;
        }
    }
}
