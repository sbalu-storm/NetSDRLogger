using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NetSdrLoggerConsole.Models
{
    public class ViewModelSignalTransmittion : INotifyPropertyChanged
    {
        public ViewModelSignalTransmittion(SignalTransmittion latestTransmittion)
        {
            Update(latestTransmittion);
        }

        public void Update(SignalTransmittion latestTransmittion)
        {
            StartTime = latestTransmittion.StartTime;
            EndTime = latestTransmittion.EndTime;
            InitialFrequency = latestTransmittion.InitialFrequency;
            AvgFrequency = latestTransmittion.AvgFrequency / 1000000;
            InitialSNR = latestTransmittion.InitialSNR;
            AvgSNR = latestTransmittion.AvgSNR;
            InitialBandwidth = latestTransmittion.InitialBandwidth;
            TransmittionCount = latestTransmittion.TransmittionCount;
            MaxBandwidth = (float)latestTransmittion.MaxBandwidth / 1000;
            IsActive = latestTransmittion.IsActive;

            Signals = latestTransmittion.Signals; // todo: need show it as a hint on listview item
        }


        public DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            private set => SetProperty(ref _startTime, value);
        }

        public DateTime _endTime;
        public DateTime EndTime
        {
            get
            {
                return _endTime;
            }
            private set
            {
                var duration = EndTime - StartTime;
                if (duration.Ticks < 0)
                    duration = new TimeSpan();
                Duration = duration;
                SetProperty(ref _endTime, value);
            }
        }

        TimeSpan _duration;
        public TimeSpan Duration 
        {
            get => _duration;
            private set => SetProperty(ref _duration, value);
        }

        public ulong _initialFrequency;
        public ulong InitialFrequency
        {
            get => _initialFrequency;
            private set => SetProperty(ref _initialFrequency, value);
        }

        public double _avgFrequency;
        public double AvgFrequency
        {
            get => _avgFrequency;
            private set => SetProperty(ref _avgFrequency, value);
        }

        public uint _initialBandwidth;
        public uint InitialBandwidth
        {
            get => _initialBandwidth;
            private set => SetProperty(ref _initialBandwidth, value);
        }

        public float _maxBandwidth;
        public float MaxBandwidth
        {
            get => _maxBandwidth;
            private set => SetProperty(ref _maxBandwidth, value);
        }

        public float _initialSNR;
        public float InitialSNR
        {
            get => _initialSNR;
            private set => SetProperty(ref _initialSNR, value);
        }

        public double _avgSNR;
        public double AvgSNR
        {
            get => _avgSNR;
            private set => SetProperty(ref _avgSNR, value);
        }

        public bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            private set => SetProperty(ref _isActive, value);
        }

        private int _transmittionCount = 0;
        public int TransmittionCount
        {
            get => _transmittionCount;
            private set => SetProperty(ref _transmittionCount, value);
        }
        public List<Signal> Signals { private set; get; } = new();


        public event PropertyChangedEventHandler PropertyChanged;
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
    }
}
