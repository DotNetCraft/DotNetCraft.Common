using System.Collections.Generic;
using DotNetCraft.Common.Core.Utils.Tracking;

namespace DotNetCraft.Common.Utils.Tracking.Entities
{
    public abstract class BasePropertyChanged : INotifyPropertyChangedExtended
    {
        #region Implementation of INotifyPropertyChangedExtended

        public event PropertyChangedExtendedEventHandler PropertyChanged;

        #endregion

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            var oldValue = field;
            field = value;
            OnPropertyChanged(propertyName, oldValue, value);
            return true;
        }

        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedExtendedEventArgs(propertyName, oldValue, newValue));
        }
    }
}
