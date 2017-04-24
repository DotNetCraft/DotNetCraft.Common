using System;
using DotNetCraft.Common.Core.Domain.ServiceMessenger;

namespace DotNetCraft.Common.Domain.ServiceMessenger
{
    public abstract class BaseServiceMessage: IServiceMessage
    {
        #region Implementation of IServiceMessage

        public object Sender { get; private set; }

        #endregion

        protected BaseServiceMessage(object sender)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));

            Sender = sender;
        }       
    }
}
