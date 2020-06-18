using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORTPR_ModBusTable.Models
{
    class AppSettings
    {
        const string DEFAULT_CSV_DELIMETER = ";";
        /// <summary>
        /// Путь по умолчанию к файлу устройств
        /// </summary>
        public string DefaultCsvFilePath { get; set; }
        /// <summary>
        /// Путь по умолчанию к файлу списка параметров устройств
        /// </summary>
        public string DefaultTypeInfosFilePath { get; set; }
        /// <summary>
        /// Путь по умолчанию к файлу определения смещения ModBus-адресса в зависимости от типа тега
        /// </summary>
        public string DefaultTypeOffsetFilePath { get; set; }
        /// <summary>
        /// Разделитель данных в CSV-файле
        /// </summary>
        public string DefaultCsvDelimeter { get; set; }
        /// <summary>
        /// Путь по умолчанию к генерируемому файлу
        /// </summary>
        public string DefaultOutFilePath { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public AppSettings()
        {
            string path = Environment.CurrentDirectory;
            DefaultCsvFilePath = path + "\\Input.csv";
            DefaultTypeInfosFilePath = path + "\\TypeInfos.json";
            DefaultTypeOffsetFilePath = path + "\\TypeOffset.json";
            DefaultCsvDelimeter = DEFAULT_CSV_DELIMETER;
        }

     
    }
}
