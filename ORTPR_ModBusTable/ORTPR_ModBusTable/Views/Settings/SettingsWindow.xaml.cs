using Microsoft.Win32;
using System.Windows;

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

        /// <summary>
        /// Инициализируем параметры окна с настройками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbCsvDelimeter.Text = Properties.Settings.Default.DefaultCsvDelimeter;
            tbDefaultTypeInfosFilePath.Text = Properties.Settings.Default.DefaultTypeInfosFilePath;
            tbDefaultTypeOffsetFilePath.Text = Properties.Settings.Default.DefaultTypeOffsetFilePath;
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
            if (!string.IsNullOrEmpty(Properties.Settings.Default.DefaultTypeInfosFilePath))
            {
                dialog.InitialDirectory = Properties.Settings.Default.DefaultTypeInfosFilePath;
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
            if (!string.IsNullOrEmpty(Properties.Settings.Default.DefaultTypeOffsetFilePath))
            {
                dialog.InitialDirectory = Properties.Settings.Default.DefaultTypeOffsetFilePath;
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
            Properties.Settings.Default.DefaultCsvDelimeter = tbCsvDelimeter.Text;
            Properties.Settings.Default.DefaultTypeInfosFilePath = tbDefaultTypeInfosFilePath.Text;
            Properties.Settings.Default.DefaultTypeOffsetFilePath = tbDefaultTypeOffsetFilePath.Text;
            this.DialogResult = true;
        }
    }
}
