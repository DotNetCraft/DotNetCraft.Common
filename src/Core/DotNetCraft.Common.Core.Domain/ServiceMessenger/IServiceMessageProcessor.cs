namespace DotNetCraft.Common.Core.Domain.ServiceMessenger
{
    public interface IServiceMessageProcessor
    {
        void RegisteredWaitHandle(IServiceMessageHandler serviceMessageHandler);

        bool SendMessage(IServiceMessage message);
    }
}
