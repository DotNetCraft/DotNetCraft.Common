using System.ComponentModel;
using System.Text;

namespace DotNetCraft.Common.Core.Utils.Tracking
{
    public class PropertyChangedExtendedEventArgs : PropertyChangedEventArgs
    {
        public virtual object OldValue { get; private set; }
        public virtual object NewValue { get; private set; }

        public PropertyChangedExtendedEventArgs(string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        #region Overrides of Object

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("PropertyName: " + PropertyName);
            stringBuilder.AppendLine("OldValue: " + OldValue);
            stringBuilder.AppendLine("NewValue: " + NewValue);
            return stringBuilder.ToString();
        }

        #endregion
    }

    public interface INotifyPropertyChangedExtended
    {
        event PropertyChangedExtendedEventHandler PropertyChanged;
    }

    public delegate void PropertyChangedExtendedEventHandler(object sender, PropertyChangedExtendedEventArgs e);
}
