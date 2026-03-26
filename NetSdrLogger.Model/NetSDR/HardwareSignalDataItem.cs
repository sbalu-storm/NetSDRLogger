using NetSdrLoggerConsole.Models;

namespace NetSdrLogger.Model.NetSDR
{
    internal struct HardwareSignalDataItem
    {
        public const int LENGTH = 28;

        public Signal GetUILevelSignal()
        {
            return new Signal(timestamp, frequency, bandwidth, snr);
        }


        public static HardwareSignalDataItem CreateFromQueue(Queue<byte> data)
        {
            if (data.Count < LENGTH)
            {
                throw new FormatException();
            }

            HardwareSignalDataItem hwSignal = new HardwareSignalDataItem();
            hwSignal.timestamp = BitUtils.GetLittleEndianUInt64FromQueue(data);
            hwSignal.frequency = BitUtils.GetLittleEndianUInt64FromQueue(data);
            hwSignal.bandwidth = BitUtils.GetLittleEndianUInt32FromQueue(data);
            hwSignal.snr = BitUtils.GetLittleEndianFloatFromQueue(data);

            return hwSignal;
        }

        public UInt64 timestamp;
        public UInt64 frequency;
        public UInt32 bandwidth;
        public float snr;

    }
}
