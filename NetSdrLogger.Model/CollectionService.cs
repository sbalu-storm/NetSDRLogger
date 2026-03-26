using NetSdrLogger.Model.SignalSource;

namespace NetSdrLoggerConsole.Models
{
    public interface ICollectionService
    {
        public event Action<SignalTransmittion> OnSignalTransmittionAdded;
        public event Action<SignalTransmittion> OnSignalTransmittionChanged;
    }

    public class CollectionService : ICollectionService
    {
        public event Action<SignalTransmittion> OnSignalTransmittionAdded;
        public event Action<SignalTransmittion> OnSignalTransmittionChanged;

        private SignalTransmittion _lastSignalTransmittion;
        private ISignalSource _signalSource;

        public CollectionService(ISignalSource ReceiverService)
        {
            _signalSource = ReceiverService;
            _signalSource.OnSignalGenerated += OnGetSignal;
        }

        void OnGetSignal(Signal data)
        {
            LoggerFactory.GetLogger().Info($"Got data item: {data.WinTimestamp:HH:mm:ss.fff}, Freq: {data.Frequency} Hz, BW: {data.Bandwidth} Hz, SNR: {data.SNR} dB");

            bool isNewSignal = true;

            if(_lastSignalTransmittion != null)
            {
                lock(_lastSignalTransmittion)
                {
                    isNewSignal = !_lastSignalTransmittion.UpdateActivityAndTryAttachNewSignal(data);
                }
                if (isNewSignal)
                {
                    OnSignalTransmittionChanged?.Invoke(_lastSignalTransmittion);
                }
            }

            if (isNewSignal)
            {
                var currentTransmittion = new SignalTransmittion(data);
                _lastSignalTransmittion = currentTransmittion;
                OnSignalTransmittionAdded?.Invoke(_lastSignalTransmittion);
            }
            else
            {
                OnSignalTransmittionChanged?.Invoke(_lastSignalTransmittion);
            }

        }
    }
}
