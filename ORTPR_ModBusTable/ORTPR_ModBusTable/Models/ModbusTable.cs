using ORTPR_ModBusTable.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ORTPR_ModBusTable.Models
{
    /// <summary>
    /// Реализация логики получения конфига ModBus карты Альфа-сервера
    /// </summary>
    class ModbusTable
    {
        /// <summary>
        /// Список устройств
        /// </summary>
        List<Device> Devices { get; set; }
        /// <summary>
        /// Описание структуры устройств
        /// </summary>
        List<DeviceType> TypeInfos { get; set; }
        /// <summary>
        /// Словарь соответствия смещения по памяти от типа данных
        /// </summary>
        Dictionary<string, int> TypeOffset { get; set; }
        /// <summary>
        /// Список привязок устройств к ModBus-карте
        /// </summary>
        Bindings Bindings { get; set; }

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
            Bindings = new Bindings(); 
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
                    addr = AddNewBinding(Bindings, addr, device, prop);
                }
            }
        }

        /// <summary>
        /// Сохранение ModBus-карты для Альфа-сервера 
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            XmlFileProvider<Bindings> XmlService = new XmlFileProvider<Bindings>();
            XmlService.Save(Bindings, fileName);
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
        private int AddNewBinding(Bindings bindings, int addr, Device device, KeyValuePair<string, string> prop)
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
