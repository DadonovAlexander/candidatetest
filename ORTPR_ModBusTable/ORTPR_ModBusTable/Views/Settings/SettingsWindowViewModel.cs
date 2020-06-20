using Microsoft.Win32;
using ORTPR_ModBusTable.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ORTPR_ModBusTable.Views.Settings
{
    class SettingsWindowViewModel : INotifyPropertyChanged
    {
        public DelegateCommand OpenTypeInfosFileCmd { get; protected set; }
        public DelegateCommand OpenTypeOffsetFileCmd { get; protected set; }
        public DelegateCommand<object> AcceptCmd { get; protected set; }
        public DelegateCommand CancelCmd { get; protected set; }

        public string CsvDelimeter { get; set; }
        public string TypeInfosFilePath { get; set; }
        public string TypeOffsetFilePath { get; set; }

        public SettingsWindowViewModel()
        {
            OpenTypeInfosFileCmd = new DelegateCommand(OpenTypeInfosFile, CanOpenTypeInfosFile);
            OpenTypeOffsetFileCmd = new DelegateCommand(OpenTypeOffsetFile, CanOpenTypeOffsetFile);
            AcceptCmd = new DelegateCommand<object>(Accept, CanAccept);
            //CancelCmd = new DelegateCommand(Cancel, CanCancel);

            //Загружаем пути к файлам из текущих настроек
            AppSettings settings = AppSettings.GetSettings();
            CsvDelimeter = settings.DefaultCsvDelimeter;
            TypeInfosFilePath = settings.DefaultTypeInfosFilePath;
            TypeOffsetFilePath = settings.DefaultTypeOffsetFilePath;
        }

        /// <summary>
        /// Путь до файла с определением структуры устройств
        /// </summary>
        void OpenTypeInfosFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JSON documents (*.json)|*.json|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            if (!string.IsNullOrEmpty(TypeInfosFilePath))
            {
                dialog.InitialDirectory = TypeInfosFilePath;
            }
            if (dialog.ShowDialog() == true)
            {
                TypeInfosFilePath = dialog.FileName;
                OnPropertyChanged("TypeInfosFilePath");
            }
        }

        bool CanOpenTypeInfosFile()
        {
            return true;
        }

        /// <summary>
        /// Путь до файла с определением смещения для различных типов данных
        /// </summary>
        void OpenTypeOffsetFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JSON documents (*.json)|*.json|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            if (!string.IsNullOrEmpty(TypeOffsetFilePath))
            {
                dialog.InitialDirectory = TypeOffsetFilePath;
            }
            if (dialog.ShowDialog() == true)
            {
                TypeOffsetFilePath = dialog.FileName;
                OnPropertyChanged("TypeOffsetFilePath");
            }
        }

        bool CanOpenTypeOffsetFile()
        {
            return true;
        }

        /// <summary>
        /// Принимаем внесенные исправления
        /// </summary>
        void Accept(object param)
        {
            AppSettings settings = AppSettings.GetSettings();
            settings.DefaultCsvDelimeter = CsvDelimeter;
            settings.DefaultTypeInfosFilePath = TypeInfosFilePath;
            settings.DefaultTypeOffsetFilePath = TypeOffsetFilePath;

            if(param is SettingsWindow)
            {
                ((SettingsWindow)param).DialogResult = true;
            }
        }

        bool CanAccept(object param)
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
