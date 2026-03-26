using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NetSdrLoggerConsole.Models
{
    public class SignalTransmittion : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignalTransmittion(Signal newSignal)
        {
            StartTime = newSignal.WinTimestamp;
            EndTime = StartTime;
            InitialFrequency = newSignal.Frequency;
            AvgFrequency = InitialFrequency;
            InitialSNR = newSignal.SNR;
            AvgSNR = InitialSNR;
            InitialBandwidth = newSignal.Bandwidth;
            Signals.Add(newSignal);
            TransmittionCount = 1;
            MaxBandwidth = newSignal.Bandwidth;
            IsActive = true;
        }

        public DateTime StartTime { private set; get; }
        public DateTime EndTime { private set; get; }
        public ulong InitialFrequency { private set; get; }
        public double AvgFrequency { private set; get; }
        public uint InitialBandwidth { private set; get; }
        public uint MaxBandwidth { private set; get; }
        public float InitialSNR { private set; get; }
        public double AvgSNR { private set; get; }
        public List<Signal> Signals { private set; get; } = new();
        public int TransmittionCount { private set; get; }

        public bool IsActive { set; get; }

        public bool UpdateActivityAndTryAttachNewSignal(Signal newSignal)
        {
            // Actually, we do not have defined logic for Bandwidth, which we should use here,
            // because it can be differ in different recordings and there is no such option in the technical requirements.
            // It looks logically to use the latest signal Bandwidth because it belongs to the signal we are testing here.
            double minFrequency = AvgFrequency - newSignal.Bandwidth / 2;
            double maxFrequency = AvgFrequency + newSignal.Bandwidth / 2;

            if (newSignal.Frequency >= minFrequency && newSignal.Frequency <= maxFrequency)
            {
                TransmittionCount++;
                AvgFrequency = (AvgFrequency * (TransmittionCount - 1) + newSignal.Frequency) / TransmittionCount;
                AvgSNR = (AvgSNR * (TransmittionCount - 1) + newSignal.SNR) / TransmittionCount;
                EndTime = newSignal.WinTimestamp;
                // Again, we do not have defined logic for Bandwidth aggregation, it looks logically to use Max(Bandwidth)
                if (newSignal.Bandwidth > MaxBandwidth)
                {
                    MaxBandwidth = newSignal.Bandwidth;
                }
                Signals.Add(newSignal);
                return true;
            }
            IsActive = false;
            return false;
        }
    }
}
