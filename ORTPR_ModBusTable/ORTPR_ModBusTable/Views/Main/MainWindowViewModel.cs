using Microsoft.Win32;
using Newtonsoft.Json;
using ORTPR_ModBusTable.Models;
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

        public ObservableCollection<Device> Devices { get; private set; }
        public Device SelectedDevice { get; set; }
        public MainWindowViewModel()
        {
            OpenDeviceFileCmd = new DelegateCommand(OpenDeviceFile, CanOpenDeviceFile);
            SaveDeviceFileCmd = new DelegateCommand(SaveDeviceFile, CanSaveDeviceFile);
            GenModBusTableCmd = new DelegateCommand(GenModBusTable, CanGenModBusTable);
            OpenSettingsWindowCmd = new DelegateCommand(OpenSettingsWindow, CanOpenSettingsWindow);

            SelectedDevice = new Device();
        }

        void OpenDeviceFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CSV documents (*.csv)|*.csv|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.DefaultCsvFilePath))
            {
                dialog.InitialDirectory = Properties.Settings.Default.DefaultCsvFilePath;
            }
            if (dialog.ShowDialog() == true)
            {
                string fileName = dialog.FileName;
                Properties.Settings.Default.DefaultCsvFilePath = fileName;
                Devices = new ObservableCollection<Device>(Device.LoadFromCsvFile(fileName));
                OnPropertyChanged("Devices");
            }
        }

        bool CanOpenDeviceFile()
        {
            return true;
        }

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

        void GenModBusTable()
        {
            try
            {
                List<DeviceType> typeInfos = DeviceTypeInfos.LoadFromJsonFile(Properties.Settings.Default.DefaultTypeInfosFilePath);
                Dictionary<string, int> typeOffset = new Dictionary<string, int>();
                using (StreamReader file = File.OpenText(Properties.Settings.Default.DefaultTypeOffsetFilePath))
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
                    if (!device.IsIgnore)
                    {
                        typeInfo = typeInfos.Single(t => t.TypeName.Equals(device.Type));
                        foreach (KeyValuePair<string, string> prop in typeInfo.Propertys)
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
                        }
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
                if (!string.IsNullOrEmpty(Properties.Settings.Default.DefaultTypeOffsetFilePath))
                {
                    saveFileDialog.InitialDirectory = Properties.Settings.Default.DefaultOutFilePath;
                }
                if (saveFileDialog.ShowDialog() == true)
                {
                    Properties.Settings.Default.DefaultOutFilePath = saveFileDialog.FileName;
                    xmlFile.Save(Properties.Settings.Default.DefaultOutFilePath);
                }
                

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            
        }

        bool CanGenModBusTable()
        {
            return true;
        }


        void OpenSettingsWindow()
        {
            var settings = new SettingsWindow();
            settings.ShowDialog();
        }

        bool CanOpenSettingsWindow()
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
