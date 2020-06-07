using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ORTPR_ModBusTable.Views.Settings
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public string CsvDelimeter { get; private set; }
        public string DefaultTypeInfosFilePath { get; private set; }
        public string DefaultTypeOffsetFilePath { get; private set; }
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CsvDelimeter = Properties.Settings.Default.DefaultCsvDelimeter;
            DefaultTypeInfosFilePath = Properties.Settings.Default.DefaultTypeInfosFilePath;
            DefaultTypeOffsetFilePath = Properties.Settings.Default.DefaultTypeOffsetFilePath;
        }

        

        private void OpenTypeInfosFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JSON documents (*.json)|*.json|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.DefaultTypeInfosFilePath))
            {
                dialog.InitialDirectory = Properties.Settings.Default.DefaultTypeInfosFilePath;
            }
            if (dialog.ShowDialog() == true)
            {
                DefaultTypeInfosFilePath = dialog.FileName;
            }
        }

        private void OpenTypeOffsetFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JSON documents (*.json)|*.json|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.DefaultTypeOffsetFilePath))
            {
                dialog.InitialDirectory = Properties.Settings.Default.DefaultTypeOffsetFilePath;
            }
            if (dialog.ShowDialog() == true)
            {
                DefaultTypeOffsetFilePath = dialog.FileName;
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DefaultCsvDelimeter = CsvDelimeter;
            Properties.Settings.Default.DefaultTypeInfosFilePath = DefaultTypeInfosFilePath;
            Properties.Settings.Default.DefaultTypeOffsetFilePath = DefaultTypeOffsetFilePath;
        }
    }
}
