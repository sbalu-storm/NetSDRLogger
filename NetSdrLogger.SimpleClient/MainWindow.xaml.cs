using NetSdrLogger.Model.SignalSource;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        ICollectionService CollectionService { set; get; }
        ISignalSource SignalSource { set; get; }

        ObservableCollection<ViewModelSignalTransmittion> _signalTransmittions = new();
        public ObservableCollection<ViewModelSignalTransmittion> SignalTransmittions
        {
            get => _signalTransmittions;
            private set => SetProperty(ref _signalTransmittions, value);
        }

        public MainWindow(ISignalSource signalSource, ICollectionService collectionService)
        {
            InitializeComponent();

            DataContext = this;

            SignalSource = signalSource;
            CollectionService = collectionService;

            CollectionService.OnSignalTransmittionAdded += _collectionService_OnSignalTransmittionAdded;
            CollectionService.OnSignalTransmittionChanged += _collectionService_OnSignalTransmittionChanged;

            signalSource.Start();
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T propertyField, T newPropertyValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(propertyField, newPropertyValue))
            {
                return false;
            }

            propertyField = newPropertyValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        private void _collectionService_OnSignalTransmittionAdded(SignalTransmittion latestTransmittion)
        {
            LoggerFactory.GetLogger().Info($"Transmittion Added: Time: {latestTransmittion.StartTime:HH:mm:ss.fff} to {latestTransmittion.EndTime:ss.fff}, IsActive: {latestTransmittion.IsActive}, Freq: {(latestTransmittion.AvgFrequency / 1000000):F3} mHz, BW: {latestTransmittion.MaxBandwidth / 1000:F2} kHz, SNR: {latestTransmittion.AvgSNR:F2} dB, Signals:{latestTransmittion.TransmittionCount}");
            var latestVmSignalTransmittion = new ViewModelSignalTransmittion(latestTransmittion);

            Dispatcher.Invoke(() =>
            {
                SignalTransmittions.Add(latestVmSignalTransmittion);
            });
        }

        private void _collectionService_OnSignalTransmittionChanged(SignalTransmittion latestTransmittion)
        {
            LoggerFactory.GetLogger().Info($"Transmittion Changed: Time: {latestTransmittion.StartTime:HH:mm:ss.fff} to {latestTransmittion.EndTime:ss.fff}, IsActive: {latestTransmittion.IsActive}, Freq: {(latestTransmittion.AvgFrequency / 1000000):F3} mHz, BW: {latestTransmittion.MaxBandwidth / 1000:F2} kHz, SNR: {latestTransmittion.AvgSNR:F2} dB, Signals:{latestTransmittion.TransmittionCount}");
            Dispatcher.Invoke(() =>
            {
                lock (latestTransmittion)
                {
                    SignalTransmittions.Last().Update(latestTransmittion);
                }
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SignalSource.Dispose();
        }
    }
}