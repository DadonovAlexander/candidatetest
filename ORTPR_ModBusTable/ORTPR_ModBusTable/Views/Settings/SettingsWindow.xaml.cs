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
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = new SettingsWindowViewModel();
        }
    }
}
