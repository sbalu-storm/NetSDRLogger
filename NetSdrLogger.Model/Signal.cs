namespace NetSdrLoggerConsole.Models
{
    public struct Signal
    {
        public Signal(ulong timestamp, ulong frequency, uint bandwidth, float snr)
        {
            Timestamp = timestamp;
            Frequency = frequency;
            Bandwidth = bandwidth;
            SNR = snr;
        }

        public DateTime WinTimestamp => DateTimeOffset.FromUnixTimeSeconds((long)Timestamp).UtcDateTime;
        public ulong Timestamp;
        public ulong Frequency;
        public uint Bandwidth;
        public float SNR;
    }
}