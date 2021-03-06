﻿namespace DotNetCraft.Common.Core.Domain.ServiceMessenger
{
    /// <summary>
    /// Interface shows that object is a service message processor.
    /// </summary>
    public interface IServiceMessageProcessor
    {
        /// <summary>
        /// Amount of service message handlers.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Register a new message handler.
        /// </summary>
        /// <param name="serviceMessageHandler">The IServiceMessageHandler instance.</param>
        void RegisteredServiceMessageHandler(IServiceMessageHandler serviceMessageHandler);

        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ignoreExceptions">Flag shows that if there is any exception in a handler then it will be ignored.</param>
        /// <returns>True if message has been handled.</returns>
        bool SendMessage(IServiceMessage message, bool ignoreExceptions = true);
    }
}
