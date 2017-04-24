using System;

namespace DotNetCraft.Common.Core.Domain.ServiceMessenger
{
    /// <summary>
    /// Interface shows that object is a service message handler.
    /// </summary>
    public interface IServiceMessageHandler
    {
        /// <summary>
        /// The object idetifier.
        /// </summary>
        Guid ServiceMessageHandlerId { get; }

        /// <summary>
        /// Handle a message.
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>True if message has been handled. Otherwise, false.</returns>
        bool HandleMessage(IServiceMessage message);
    }
}
