using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORTPR_ModBusTable.Models
{
    /// <summary>
    /// Класс декларирован только для корректной десериализации заданного формата JSON-файла
    /// </summary>
    class DeviceTypeInfos
    {
        public List<DeviceType> TypeInfos;
    }
    /// <summary>
    /// Описание структуры устройства в карте ModBus
    /// </summary>
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
