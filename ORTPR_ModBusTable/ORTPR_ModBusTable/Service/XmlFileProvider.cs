using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ORTPR_ModBusTable.Service
{
    class XmlFileProvider<T> where T : class
    {
        /// <summary>
        /// Десериализация содержимого указанного файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns></returns>
        public T Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("Файл не существует", nameof(filePath));
            }
            using (StreamReader file = File.OpenText(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(file);
            }
        }

        /// <summary>
        /// Сериализация в файл
        /// </summary>
        /// <param name="objectToSerialize">Объект для сериализации</param>
        /// <param name="filePath">Путь к файлу</param>
        public void Save(T objectToSerialize, string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Наименование файла не может быть null", nameof(filePath));
            }
            using (StreamWriter file = File.CreateText(filePath))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                XmlWriter writer = XmlWriter.Create(file, settings);
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty) });
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, objectToSerialize, ns);
            }
        }


        public void Save1(T objectToSerialize, string filePath)
        {
            StreamWriter file = File.CreateText(filePath);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            XmlWriter writer = XmlWriter.Create(file, settings);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty) });
            serializer.Serialize(writer, objectToSerialize, ns);
        }

    }
}
