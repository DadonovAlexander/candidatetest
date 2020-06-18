using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORTPR_ModBusTable.Service
{
    class JsonFileProvider<T> where T : class
    {
        /// <summary>
        /// Десериализация содержимого указанного файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns></returns>
        public T Load(string filePath)
        {
            if(!File.Exists(filePath))
            {
                throw new ArgumentException("Файл не существует", nameof(filePath));
            }
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (T)serializer.Deserialize(file, typeof(T));
            }
        }

        /// <summary>
        /// Сериализация в файл
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        public void Save(T obj, string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Наименование файла не может быть null", nameof(filePath));
            }
            using (StreamWriter file = File.CreateText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, obj);
            }
        }


    }
}
