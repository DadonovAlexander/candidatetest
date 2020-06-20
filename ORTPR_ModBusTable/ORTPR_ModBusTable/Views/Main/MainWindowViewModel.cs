using Microsoft.Win32;
using Newtonsoft.Json;
using ORTPR_ModBusTable.Models;
using ORTPR_ModBusTable.Service;
using ORTPR_ModBusTable.Views.Settings;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml;

namespace ORTPR_ModBusTable.Views.Main
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public DelegateCommand OpenDeviceFileCmd { get; protected set; }
        public DelegateCommand SaveDeviceFileCmd { get; protected set; }
        public DelegateCommand GenModBusTableCmd { get; protected set; }
        public DelegateCommand OpenSettingsWindowCmd { get; protected set; }
        public DelegateCommand CloseWindowCommand { get; protected set; }
        
        public ObservableCollection<Device> Devices { get; private set; }
        public Device SelectedDevice { get; set; }
        public MainWindowViewModel()
        {
            OpenDeviceFileCmd = new DelegateCommand(OpenDeviceFile, CanOpenDeviceFile);
            SaveDeviceFileCmd = new DelegateCommand(SaveDeviceFile, CanSaveDeviceFile);
            GenModBusTableCmd = new DelegateCommand(GenModBusTable, CanGenModBusTable);
            OpenSettingsWindowCmd = new DelegateCommand(OpenSettingsWindow, CanOpenSettingsWindow);
            CloseWindowCommand = new DelegateCommand(CloseWindow, CanCloseWindow);

            SelectedDevice = new Device();

            //считывание текущих настроек из файла
            if (File.Exists(Properties.Settings.Default.AppSettingsFileName))
            {
                JsonFileProvider<AppSettings> settingsService = new JsonFileProvider<AppSettings>();
                AppSettings.SetSettings(settingsService.Load(Properties.Settings.Default.AppSettingsFileName)); 
            }
        }

        /// <summary>
        /// Открываем файл с перечнем устройств
        /// </summary>
        void OpenDeviceFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CSV documents (*.csv)|*.csv|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            AppSettings settings = AppSettings.GetSettings();
            if (!string.IsNullOrEmpty(settings.DefaultCsvFilePath))
            {
                dialog.InitialDirectory = settings.DefaultCsvFilePath;
            }
            if (dialog.ShowDialog() == true)
            {
                settings.DefaultCsvFilePath = dialog.FileName;
                Devices = new ObservableCollection<Device>(Device.LoadFromCsvFile(settings.DefaultCsvFilePath));
                AppSettings.SetSettings(settings);
                OnPropertyChanged("Devices");
            }
        }

        bool CanOpenDeviceFile()
        {
            return true;
        }

        /// <summary>
        /// TODO: на будующее, возможность сохранения файла с перечнем устройств
        /// </summary>
        void SaveDeviceFile()
        {
            string dev = "";
            foreach (Device device in Devices)
            {
                dev += device.ToString() + "\n";
            }
            MessageBox.Show(dev);
            OnPropertyChanged("Devices");
        }

        bool CanSaveDeviceFile()
        {
            return true;
        }

        /// <summary>
        /// Генерирование таблицы привязок
        /// </summary>
        void GenModBusTable()
        {
            try
            {
                AppSettings settings = AppSettings.GetSettings();
                List<DeviceType> typeInfos = DeviceTypeInfos.LoadFromJsonFile(settings.DefaultTypeInfosFilePath);
                Dictionary<string, int> typeOffset = new Dictionary<string, int>();
                using (StreamReader file = File.OpenText(settings.DefaultTypeOffsetFilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    typeOffset = (Dictionary<string, int>)serializer.Deserialize(file, typeof(Dictionary<string, int>));
                }

                List<Binding> bindings = new List<Binding>();
                DeviceType typeInfo;
                int addr = 0;
                // конкатенация
                foreach (Device device in Devices)
                {
                    if (device.IsIgnore)
                        continue;
                    typeInfo = typeInfos.Single(t => t.TypeName.Equals(device.Type));
                    foreach (KeyValuePair<string, string> prop in typeInfo.Propertys)
                    {
                        addr = AddNewBinding(typeOffset, bindings, addr, device, prop);
                    }

                }

                var xmlFile = new XmlDocument();
                var root = xmlFile.CreateElement("root");
                foreach (Binding binding in bindings)
                {
                    var bindingItem = xmlFile.CreateElement("item");
                    var attribute = xmlFile.CreateAttribute("Binding");     // Создаем атрибут и нужным именем.
                    attribute.InnerText = "Introduced";                     // Устанавливаем содержимое атрибута.
                    bindingItem.Attributes.Append(attribute);               // Добавляем атрибут к элементу.
                    var child_tag = xmlFile.CreateElement("node-path");
                    child_tag.InnerText = binding.Tag;
                    bindingItem.AppendChild(child_tag);
                    var child_addr = xmlFile.CreateElement("address");
                    child_addr.InnerText = binding.Address.ToString();
                    bindingItem.AppendChild(child_addr);

                    root.AppendChild(bindingItem);
                }

                xmlFile.AppendChild(root);

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML documents (*.xml)|*.xml|All files (*.*)|*.*";
                if (!string.IsNullOrEmpty(settings.DefaultTypeOffsetFilePath))
                {
                    saveFileDialog.InitialDirectory = settings.DefaultOutFilePath;
                }
                if (saveFileDialog.ShowDialog() == true)
                {
                    settings.DefaultOutFilePath = saveFileDialog.FileName;
                    xmlFile.Save(settings.DefaultOutFilePath);
                    AppSettings.SetSettings(settings);
                }
                

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            
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
        private static int AddNewBinding(Dictionary<string, int> typeOffset, List<Binding> bindings, int addr, Device device, KeyValuePair<string, string> prop)
        {
            bindings.Add(new Binding(device.Tag + "." + prop.Key, prop.Value, addr));
            if (typeOffset.ContainsKey(prop.Value))
            {
                addr += typeOffset[prop.Value];
            }
            else
            {
                MessageBox.Show($"В файле определения смещения адресса от типа данных отсутствует заданный тип данных: {prop.Value}");
            }

            return addr;
        }

        bool CanGenModBusTable()
        {
            return true;
        }

        /// <summary>
        /// Открытие окна с настройками
        /// </summary>
        void OpenSettingsWindow()
        {
            var settingsWin = new SettingsWindow();
            settingsWin.ShowDialog();
        }

        bool CanOpenSettingsWindow()
        {
            return true;
        }

        /// <summary>
        /// Закрытие окна
        /// </summary>
        void CloseWindow()
        {
            //запись текущих настроек в файл
            JsonFileProvider<AppSettings> settingsService = new JsonFileProvider<AppSettings>();
            settingsService.Save(AppSettings.GetSettings(), Properties.Settings.Default.AppSettingsFileName);
        }

        bool CanCloseWindow()
        {
            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
