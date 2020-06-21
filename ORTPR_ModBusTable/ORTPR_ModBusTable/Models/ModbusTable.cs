using Microsoft.Win32;
using ORTPR_ModBusTable.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ORTPR_ModBusTable.Models
{
    /// <summary>
    /// Реализация логики получения конфига ModBus карты Альфа-сервера
    /// </summary>
    class ModbusTable
    {
        List<Device> Devices { get; set; }
        List<DeviceType> TypeInfos { get; set; }
        Dictionary<string, int> TypeOffset { get; set; }

        XmlDocument XmlFile = new XmlDocument();

        public ModbusTable() { }

        /// <summary>
        /// Загрузка исходный данных
        /// </summary>
        /// <param name="devices"></param>
        public void LoadSource(List<Device> devices)
        {
            Devices = devices;
            AppSettings settings = AppSettings.GetSettings();
            JsonFileProvider<DeviceTypeInfos> typeInfosService = new JsonFileProvider<DeviceTypeInfos>();
            TypeInfos = typeInfosService.Load(settings.DefaultTypeInfosFilePath).TypeInfos;
            JsonFileProvider<Dictionary<string, int>> typeOffsetService = new JsonFileProvider<Dictionary<string, int>>();
            TypeOffset = typeOffsetService.Load(settings.DefaultTypeOffsetFilePath);

        }

        /// <summary>
        /// Генерирование ModBus-карты для Альфа-сервера
        /// </summary>
        public void Generate()
        {
            List<Binding> bindings = new List<Binding>();
            DeviceType typeInfo;
            int addr = 0;
            // конкатенация
            foreach (Device device in Devices)
            {
                if (device.IsIgnore)
                    continue;
                typeInfo = TypeInfos.Single(t => t.TypeName.Equals(device.Type));
                foreach (KeyValuePair<string, string> prop in typeInfo.Propertys)
                {
                    addr = AddNewBinding(bindings, addr, device, prop);
                }

            }

            var root = XmlFile.CreateElement("root");
            foreach (Binding binding in bindings)
            {
                var bindingItem = XmlFile.CreateElement("item");
                var attribute = XmlFile.CreateAttribute("Binding");     // Создаем атрибут и нужным именем.
                attribute.InnerText = "Introduced";                     // Устанавливаем содержимое атрибута.
                bindingItem.Attributes.Append(attribute);               // Добавляем атрибут к элементу.
                var child_tag = XmlFile.CreateElement("node-path");
                child_tag.InnerText = binding.Tag;
                bindingItem.AppendChild(child_tag);
                var child_addr = XmlFile.CreateElement("address");
                child_addr.InnerText = binding.Address.ToString();
                bindingItem.AppendChild(child_addr);

                root.AppendChild(bindingItem);
            }

            XmlFile.AppendChild(root);
        }

        /// <summary>
        /// Сохранение ModBus-карты для Альфа-сервера 
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            XmlFile.Save(fileName);
        }

        /// <summary>
        /// Добавление новой записи в таблицу привязок
        /// </summary>
        /// <param name="typeOffset">Таблица смещения адреса новой записи в зависимости от типа тега</param>
        /// <param name="bindings">Таблица привязок ModBusTable</param>
        /// <param name="addr">ModBus адресс</param>
        /// <param name="device">Устройство</param>
        /// <param name="prop">Параметр устройства</param>
        /// <returns></returns>
        private int AddNewBinding(List<Binding> bindings, int addr, Device device, KeyValuePair<string, string> prop)
        {
            bindings.Add(new Binding(device.Tag + "." + prop.Key, prop.Value, addr));
            if (TypeOffset.ContainsKey(prop.Value))
            {
                addr += TypeOffset[prop.Value];
            }
            else
            {
                throw new Exception($"В файле определения смещения адресса от типа данных отсутствует заданный тип данных: {prop.Value}");
            }

            return addr;
        }


    }
}
