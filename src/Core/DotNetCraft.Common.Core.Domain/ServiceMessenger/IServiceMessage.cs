namespace DotNetCraft.Common.Core.Domain.ServiceMessenger
{
    /// <summary>
    /// Interface shows that object is a service message.
    /// </summary>
    public interface IServiceMessage
    {
        /// <summary>
        /// The sender.
        /// </summary>
        object Sender { get; }
    }
}
