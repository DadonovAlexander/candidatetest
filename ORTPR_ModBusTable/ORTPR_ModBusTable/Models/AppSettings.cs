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
    /// Потоконебезопасная реализация класса настроек приложения в соответствии с паттерном Singlton
    /// </summary>
    class AppSettings
    {
        /// <summary>
        /// Разделитель данных в CSV по умолчанию
        /// </summary>
        const string DEFAULT_CSV_DELIMETER = ";";
        /// <summary>
        /// 
        /// </summary>
        private static AppSettings instance;

        private string _defaultCsvFilePath;
        private string _defaultTypeInfosFilePath;
        private string _defaultTypeOffsetFilePath;
        private string _defaultCsvDelimeter;
        private string _defaultOutFilePath;

        /// <summary>
        /// Путь по умолчанию к файлу устройств
        /// </summary>
        public string DefaultCsvFilePath
        {
            get { return _defaultCsvFilePath; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Параметр не может быть пустым или null", nameof(DefaultCsvFilePath));
                _defaultCsvFilePath = value;
            }
        }
        /// <summary>
        /// Путь по умолчанию к файлу списка параметров устройств
        /// </summary>
        public string DefaultTypeInfosFilePath
        {
            get { return _defaultTypeInfosFilePath; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Параметр не может быть пустым или null", nameof(DefaultTypeInfosFilePath));
                _defaultTypeInfosFilePath = value;
            }
        }
        /// <summary>
        /// Путь по умолчанию к файлу определения смещения ModBus-адресса в зависимости от типа тега
        /// </summary>
        public string DefaultTypeOffsetFilePath
        {
            get { return _defaultTypeOffsetFilePath; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Параметр не может быть пустым или null", nameof(DefaultTypeOffsetFilePath));
                _defaultTypeOffsetFilePath = value;
            }
        }
        /// <summary>
        /// Разделитель данных в CSV-файле
        /// </summary>
        public string DefaultCsvDelimeter
        {
            get { return _defaultCsvDelimeter; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Параметр не может быть пустым или null", nameof(DefaultCsvDelimeter));
                _defaultCsvDelimeter = value;
            }
        }
        /// <summary>
        /// Путь по умолчанию к генерируемому файлу
        /// </summary>
        public string DefaultOutFilePath
        {
            get { return _defaultOutFilePath; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Параметр не может быть пустым или null", nameof(DefaultOutFilePath));
                _defaultOutFilePath = value;
            }
        }

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

        /// <summary>
        /// Получение настроек
        /// </summary>
        /// <returns></returns>
        public static AppSettings GetSettings()
        {
            if (instance == null)
                instance = new AppSettings();
            return instance;
        }

        public static void SetSettings(AppSettings settings)
        {
            if(instance == null)
                instance = new AppSettings();
            instance.DefaultCsvDelimeter = settings.DefaultCsvDelimeter;
            instance.DefaultCsvFilePath = settings.DefaultCsvFilePath;
            instance.DefaultTypeInfosFilePath = settings.DefaultTypeInfosFilePath;
            instance.DefaultTypeOffsetFilePath = settings.DefaultTypeOffsetFilePath;
            instance.DefaultOutFilePath = settings.DefaultOutFilePath;
        }
     
    }
}
