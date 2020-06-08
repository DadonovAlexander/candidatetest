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
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbCsvDelimeter.Text = Properties.Settings.Default.DefaultCsvDelimeter;
            tbDefaultTypeInfosFilePath.Text = Properties.Settings.Default.DefaultTypeInfosFilePath;
            tbDefaultTypeOffsetFilePath.Text = Properties.Settings.Default.DefaultTypeOffsetFilePath;
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
                tbDefaultTypeInfosFilePath.Text = dialog.FileName;
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
                tbDefaultTypeOffsetFilePath.Text = dialog.FileName;
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DefaultCsvDelimeter = tbCsvDelimeter.Text;
            Properties.Settings.Default.DefaultTypeInfosFilePath = tbDefaultTypeInfosFilePath.Text;
            Properties.Settings.Default.DefaultTypeOffsetFilePath = tbDefaultTypeOffsetFilePath.Text;
            this.DialogResult = true;
        }
    }
}
