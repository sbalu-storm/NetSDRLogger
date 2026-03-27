using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NetSdrLogger.SimpleClient
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
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
