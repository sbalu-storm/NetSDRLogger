using NetSdrLoggerConsole.Models;
using System.Collections.Concurrent;

namespace NetSdrLogger.Model.NetSDR
{
    internal class ByteQueueNetSdrParser
    {
        Queue<byte> _data = new();

        public ConcurrentQueue<Signal> Signals { set; get; } = new();
        public ByteQueueNetSdrParser(Queue<byte> data) 
        {
            _data = data;
        }
        public void ParseAvailableData()
        {
            try
            {
                for (; ; )
                {
                    HardwareHeader header = HardwareHeader.CreateFromQueue(_data);

                    int len = header.GetLengthValue();

                    if (header.GetMessageFromDeviceToPC() == TargetMessageType.TargetDataItemACKfromTargettoHost)
                    {

                        int messageCount = (len - HardwareSignalDataItem.LENGTH) / HardwareSignalDataItem.LENGTH;

                        //read data
                        for (int i = 0; i < messageCount; ++i)
                        {
                            var hwSignal = HardwareSignalDataItem.CreateFromQueue(_data);
                            Signal signal = hwSignal.GetUILevelSignal();
                            Signals.Enqueue(signal);
                        }
                    }
                    else
                    {
                        SkipData(len - HardwareSignalDataItem.LENGTH);
                    }
                }
            }
            catch(Exception ex)
            {
                LoggerFactory.GetLogger().Error("Error while parsing the queue", ex);
            }
            finally
            {
                _data.Clear();
            }
            HardwareSignalDataItem dataItem;
        }

        void SkipData(int count)
        {
            for(int i=0;i<count;++i)
            {
                _data.Dequeue();
            }
        }

    }
}
