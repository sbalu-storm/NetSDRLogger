using NetSdrLogger.Model.SignalSource;
using NetSdrLogger.SimpleClient.ViewModel;
using NetSdrLoggerConsole.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace NetSdrLogger.SimpleClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel ViewModel {  get; set; }

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = viewModel;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ViewModel.Dispose();
        }
    }
}