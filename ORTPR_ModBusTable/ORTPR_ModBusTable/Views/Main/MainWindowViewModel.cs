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
                ModbusTable mb = new ModbusTable();
                mb.LoadSource(Devices.ToList<Device>());
                mb.Generate();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML documents (*.xml)|*.xml|All files (*.*)|*.*";
                AppSettings settings = AppSettings.GetSettings();
                if (!string.IsNullOrEmpty(settings.DefaultOutFilePath))
                {
                    saveFileDialog.InitialDirectory = settings.DefaultOutFilePath;
                }
                if (saveFileDialog.ShowDialog() == true)
                {
                    settings.DefaultOutFilePath = saveFileDialog.FileName;
                    mb.Save(settings.DefaultOutFilePath);
                    AppSettings.SetSettings(settings);
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
