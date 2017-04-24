namespace DotNetCraft.Common.Core.Domain.ServiceMessenger
{
    /// <summary>
    /// Interface shows that object is a service message processor.
    /// </summary>
    public interface IServiceMessageProcessor
    {
        /// <summary>
        /// Register a new message handler.
        /// </summary>
        /// <param name="serviceMessageHandler">The IServiceMessageHandler instance.</param>
        void RegisteredWaitHandle(IServiceMessageHandler serviceMessageHandler);

        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>True if message has been handled.</returns>
        bool SendMessage(IServiceMessage message);
    }
}
