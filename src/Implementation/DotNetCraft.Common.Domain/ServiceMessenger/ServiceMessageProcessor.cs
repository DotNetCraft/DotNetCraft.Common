using System;
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

        public bool SendMessage(IServiceMessage message, bool ignoreExceptions = true)
        {
            foreach (IServiceMessageHandler messageHandler in handlers)
            {
                try
                {
                    bool messageWasHandled = messageHandler.HandleMessage(message);
                    if (messageWasHandled)
                        return true;
                }
                catch (Exception ex)
                {
                    if (ignoreExceptions)
                        continue;

                    throw new ServiceMessageException("There was an exception in the message handler", ex, new Dictionary<string, string>
                    {
                        {"Type of MessageHandler", messageHandler.GetType().ToString() },
                        {"ServiceMessageHandlerId", messageHandler.ServiceMessageHandlerId.ToString() }
                    });
                }
            }

            return false;
        }

        #endregion
    }
}
