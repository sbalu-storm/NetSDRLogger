using NetSdrLogger.Model.SignalSource;
using NetSdrLoggerConsole.Models;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace NetSdrLogger.SimpleClient.ViewModel
{
    public class MainViewModel : BaseViewModel, IDisposable
    {
        private ICollectionService CollectionService { set; get; }
        private ISignalSource SignalSource { set; get; }
        
        private readonly Dispatcher _dispatcher;
        private bool _disposed;


        ObservableCollection<ViewModelSignalTransmittion> _signalTransmittions = new();
        public ObservableCollection<ViewModelSignalTransmittion> SignalTransmittions
        {
            get => _signalTransmittions;
            private set => SetProperty(ref _signalTransmittions, value);
        }

        public MainViewModel(ISignalSource signalSource, ICollectionService collectionService)
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            SignalSource = signalSource;
            CollectionService = collectionService;

            CollectionService.OnSignalTransmittionAdded += _collectionService_OnSignalTransmittionAdded;
            CollectionService.OnSignalTransmittionChanged += _collectionService_OnSignalTransmittionChanged;

            signalSource.Start();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                SignalSource.Dispose();
            }
        }

        private void _collectionService_OnSignalTransmittionAdded(SignalTransmittion latestTransmittion)
        {
            LoggerFactory.GetLogger().Info($"Transmittion Added: Time: {latestTransmittion.StartTime:HH:mm:ss.fff} to {latestTransmittion.EndTime:ss.fff}, IsActive: {latestTransmittion.IsActive}, Freq: {latestTransmittion.AvgFrequency / 1000000:F3} mHz, BW: {latestTransmittion.MaxBandwidth / 1000:F2} kHz, SNR: {latestTransmittion.AvgSNR:F2} dB, Signals:{latestTransmittion.TransmittionCount}");
            var latestVmSignalTransmittion = new ViewModelSignalTransmittion(latestTransmittion);

            _dispatcher.Invoke(() =>
            {
                SignalTransmittions.Add(latestVmSignalTransmittion);
            });
        }

        private void _collectionService_OnSignalTransmittionChanged(SignalTransmittion latestTransmittion)
        {
            LoggerFactory.GetLogger().Info($"Transmittion Changed: Time: {latestTransmittion.StartTime:HH:mm:ss.fff} to {latestTransmittion.EndTime:ss.fff}, IsActive: {latestTransmittion.IsActive}, Freq: {latestTransmittion.AvgFrequency / 1000000:F3} mHz, BW: {latestTransmittion.MaxBandwidth / 1000:F2} kHz, SNR: {latestTransmittion.AvgSNR:F2} dB, Signals:{latestTransmittion.TransmittionCount}");
            _dispatcher.Invoke(() =>
            {
                lock (latestTransmittion)
                {
                    SignalTransmittions.Last().Update(latestTransmittion);
                }
            });
        }

    }
}
