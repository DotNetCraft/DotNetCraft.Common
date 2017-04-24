using System.Collections.Generic;
using DotNetCraft.Common.Core.Domain.ServiceMessenger;
using DotNetCraft.Common.Domain.ServiceMessenger.Exceptions;

namespace DotNetCraft.Common.Domain.ServiceMessenger
{
    public class ServiceMessageProcessor: IServiceMessageProcessor
    {
        private readonly List<IServiceMessageHandler> handlers;

        /// <summary>
        /// Amount of service message handlers.
        /// </summary>
        public int Count
        {
            get { return handlers.Count; }
        }

        public ServiceMessageProcessor()
        {
            handlers = new List<IServiceMessageHandler>();

            //TODO: Load service message handlers by attribute
        }

        #region Implementation of IServiceMessageProcessor

        public void RegisteredServiceMessageHandler(IServiceMessageHandler serviceMessageHandler)
        {
            if (handlers.Exists(x => x.ServiceMessageHandlerId == serviceMessageHandler.ServiceMessageHandlerId))
                throw new ServiceMessageException("This service message handler has been already registered");

            handlers.Add(serviceMessageHandler);
        }

        public bool SendMessage(IServiceMessage message)
        {
            foreach (IServiceMessageHandler messageHandler in handlers)
            {
                bool messageWasHandled = messageHandler.HandleMessage(message);
                if (messageWasHandled)
                    return true;
            }

            return false;
        }

        #endregion
    }
}
