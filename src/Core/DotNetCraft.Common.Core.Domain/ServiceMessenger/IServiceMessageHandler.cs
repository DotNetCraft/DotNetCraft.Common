using System;

namespace DotNetCraft.Common.Core.Domain.ServiceMessenger
{
    public interface IServiceMessageHandler
    {
        Guid ServiceMessageHandlerId { get; }

        bool HandleMessage(IServiceMessage message);
    }
}
