using NetSdrLoggerConsole.Models;

namespace NetSdrLogger.Model.SignalSource
{
    public interface ISignalSource : IDisposable
    {
        public void Start();
        public void Stop();

        public event Action<Signal> OnSignalGenerated;
    }

}
