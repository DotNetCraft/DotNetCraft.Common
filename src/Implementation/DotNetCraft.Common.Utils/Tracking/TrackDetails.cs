using System;

namespace DotNetCraft.Common.Utils.Tracking
{
    public class TrackDetails
    {
        public object Sender { get; }
        public string PropertyName { get; }
        public object OldValue { get; }

        public TrackDetails(object sender, string propertyName, object oldValue)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            Sender = sender;
            PropertyName = propertyName;
            OldValue = oldValue;
        }
    }
}
