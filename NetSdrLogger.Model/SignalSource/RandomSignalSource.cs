using NetSdrLogger.Model.Helpers;
using NetSdrLoggerConsole.Models;

namespace NetSdrLogger.Model.SignalSource
{
    public class RandomSignalSource : ISignalSource, IDisposable
    {
        public event Action<Signal> OnSignalGenerated;

        private const int NEW_TRANSMITTION_CHANCE_PERCENT = 20;
        private bool _isRunning = false;
        private bool _isDisposed = false;
        private ISignalGenerator _randomSignalGenerator = new RandomSignalGenerator();

        public RandomSignalSource()
        {
        }

        public void Start()
        {
            _isRunning = true;
            Task.Run(() =>
            {
                try
                {
                    Signal previous = _randomSignalGenerator.GenerateSignal();
                    for (; ; )
                    {
                        if (!_isRunning)
                            break;

                        bool continueTransmitting = RandomService.Next(0, 100) > NEW_TRANSMITTION_CHANCE_PERCENT;

                        Signal signal;
                        if (continueTransmitting)
                        {
                            signal = _randomSignalGenerator.GenerateLinkedSignal(previous);
                        }
                        else
                        {
                            signal = _randomSignalGenerator.GenerateSignal();
                        }

                        OnSignalGenerated?.Invoke(signal);
                        previous = signal;
                        Task.Delay(RandomService.Next(100, 2000)).Wait();
                    }
                }
                catch (Exception ex)
                {
                    LoggerFactory.GetLogger().Error("Exception in TCP precessing cycle", ex);
                }
            });
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Dispose()
        {
            Stop();
            _isDisposed = true;
        }

        ~RandomSignalSource()
        {
            if (!_isDisposed)
                Dispose();
        }
    }

}
