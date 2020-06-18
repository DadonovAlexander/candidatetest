using Microsoft.Win32;
using ORTPR_ModBusTable.Models;
using ORTPR_ModBusTable.Service;
using ORTPR_ModBusTable.Views.Main;
using System.IO;
using System.Windows;

namespace ORTPR_ModBusTable.Views.Settings
{
    
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// Настройки приложения
        /// </summary>
        internal AppSettings Settings { get; set; }
        
        public SettingsWindow()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Инициализируем параметры окна с настройками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbCsvDelimeter.Text = Settings.DefaultCsvDelimeter;
            tbDefaultTypeInfosFilePath.Text = Settings.DefaultTypeInfosFilePath;
            tbDefaultTypeOffsetFilePath.Text = Settings.DefaultTypeOffsetFilePath;
        }
        
        /// <summary>
        /// Путь до файла с определением структуры устройств
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenTypeInfosFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JSON documents (*.json)|*.json|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            if (!string.IsNullOrEmpty(Settings.DefaultTypeInfosFilePath))
            {
                dialog.InitialDirectory = Settings.DefaultTypeInfosFilePath;
            }
            if (dialog.ShowDialog() == true)
            {
                tbDefaultTypeInfosFilePath.Text = dialog.FileName;
            }
            
        }

        /// <summary>
        /// Путь до файла с определением смещения для различных типов данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenTypeOffsetFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JSON documents (*.json)|*.json|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            if (!string.IsNullOrEmpty(Settings.DefaultTypeOffsetFilePath))
            {
                dialog.InitialDirectory = Settings.DefaultTypeOffsetFilePath;
            }
            if (dialog.ShowDialog() == true)
            {
                tbDefaultTypeOffsetFilePath.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// Принимаем внесенные исправления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Settings.DefaultCsvDelimeter = tbCsvDelimeter.Text;
            Settings.DefaultTypeInfosFilePath = tbDefaultTypeInfosFilePath.Text;
            Settings.DefaultTypeOffsetFilePath = tbDefaultTypeOffsetFilePath.Text;
            this.DialogResult = true;
        }
    }
}
