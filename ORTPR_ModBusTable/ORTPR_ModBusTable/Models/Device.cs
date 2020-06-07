using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORTPR_ModBusTable.Models
{
    /// <summary>
    /// Устройство (задвижка, вспомка, OIP)
    /// </summary>
    class Device
    {
        /// <summary>
        /// Корневая часть тега, до наименования устройства
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// Тип устройства
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Флаг игнорирования данной записи
        /// </summary>
        public bool IsIgnore { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Device() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="type"></param>
        public Device(string tag, string type)
        {
            Tag = tag;
            Type = type;
        }


        public override string ToString()
        {
            return this.Tag + " - " + this.Type + " - " + this.IsIgnore;
        }

        /// <summary>
        /// Загрузка списка устройств из csv-файла
        /// </summary>
        /// <param name="csvFilePath">полный путь к csv-файлу</param>
        /// <param name="delimiter">системный разделитель данный в csv-файле</param>
        /// <returns></returns>
        public static List<Device> LoadFromCsvFile(string csvFilePath, string delimiter = ";")
        {
            using (StreamReader streamReader = new StreamReader(csvFilePath))
            {
                using (CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    // указываем используемый разделитель
                    csvReader.Configuration.Delimiter = delimiter;
                    csvReader.Configuration.HeaderValidated = null;
                    csvReader.Configuration.MissingFieldFound = null;
                    // получаем строки
                    return csvReader.GetRecords<Device>().ToList();
                }
            }
        }
    }
}
