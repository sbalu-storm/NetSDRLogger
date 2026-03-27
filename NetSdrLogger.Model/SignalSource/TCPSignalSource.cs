using NetSdrLogger.Model.NetSDR;
using NetSdrLoggerConsole.Models;
using System.Net.Sockets;

namespace NetSdrLogger.Model.SignalSource
{
    public class TCPSignalSource : ISignalSource
    {
        public event Action<Signal> OnSignalGenerated;

        private bool _isRunning = false;
        private bool _isDisposed = false;
        private string _serverIp = "127.0.0.1";
        private int _port = 50000;

        public TCPSignalSource(string serverIp, int port)
        {
            _serverIp = serverIp;
            _port = port;
        }

        public void Start()
        {
            _isRunning = true;
            Task.Run(() =>
            {
                try
                {
                    Queue<byte> data = new Queue<byte>();
                    ByteQueueNetSdrParser byteQueueNetSdrParser = new ByteQueueNetSdrParser(data);
                    //get message
                    using (TcpClient client = new TcpClient(_serverIp, _port))
                    {
                        byte[] buffer = new byte[8192];
                        for (; ; )
                        {
                            if (!_isRunning)
                                break;

                            GetDataQueue(data, client, buffer);
                            byteQueueNetSdrParser.ParseAvailableData();
                            foreach(Signal signal in byteQueueNetSdrParser.Signals)
                            {
                                OnSignalGenerated(signal);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerFactory.GetLogger().Error("Exception in TCP precessing cycle", ex);
                }
            });
        }

        private bool GetDataQueue(Queue<byte> data, TcpClient client, byte[] buffer)
        {
            using (NetworkStream stream = client.GetStream())
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                { 
                    return false;
                }

                for (int i = 0; i < bytesRead; ++i)
                {
                    data.Enqueue(buffer[i]);
                }
            }
            return true;
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

        ~TCPSignalSource()
        {
            if (!_isDisposed)
                Dispose();
        }
    }

}
