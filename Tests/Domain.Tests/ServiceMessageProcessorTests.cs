using System;
using DotNetCraft.Common.Core.Domain.ServiceMessenger;
using DotNetCraft.Common.Domain.ServiceMessenger;
using DotNetCraft.Common.Domain.ServiceMessenger.Exceptions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Domain.Tests
{
    [TestFixture]
    public class ServiceMessageProcessorTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SendMessageTest(bool isHandled)
        {
            IServiceMessageProcessor serviceMessageProcessor = new ServiceMessageProcessor();
            IServiceMessage message = Substitute.For<IServiceMessage>();

            IServiceMessageHandler serviceMessageHandler = Substitute.For<IServiceMessageHandler>();
            serviceMessageHandler.HandleMessage(message).Returns(isHandled);
            serviceMessageProcessor.RegisteredServiceMessageHandler(serviceMessageHandler);

            bool result = serviceMessageProcessor.SendMessage(message);

            Assert.AreEqual(isHandled, result);
            serviceMessageHandler.Received(1).HandleMessage(message);            
        }

        [Test]
        public void RegisteredServiceMessageHandlerTest()
        {
            IServiceMessageProcessor serviceMessageProcessor = new ServiceMessageProcessor();
            IServiceMessageHandler serviceMessageHandler = Substitute.For<IServiceMessageHandler>();
            serviceMessageProcessor.RegisteredServiceMessageHandler(serviceMessageHandler);
            Assert.AreEqual(1, serviceMessageProcessor.Count);
        }

        [Test]
        [ExpectedException(typeof(ServiceMessageException))]
        public void RegisteredServiceMessageHandlerDublicateTest()
        {
            IServiceMessageProcessor serviceMessageProcessor = new ServiceMessageProcessor();
            IServiceMessageHandler serviceMessageHandler = Substitute.For<IServiceMessageHandler>();
            Guid guid = Guid.NewGuid();
            serviceMessageHandler.ServiceMessageHandlerId.Returns(guid);
            serviceMessageProcessor.RegisteredServiceMessageHandler(serviceMessageHandler);
            serviceMessageProcessor.RegisteredServiceMessageHandler(serviceMessageHandler);

            Assert.Fail("ServiceMessageException expected");
        }

        [Test]
        public void RegisteredServiceMessageHandlerSeveralTimesTest()
        {
            IServiceMessageProcessor serviceMessageProcessor = new ServiceMessageProcessor();
            IServiceMessageHandler serviceMessageHandler1 = Substitute.For<IServiceMessageHandler>();
            IServiceMessageHandler serviceMessageHandler2 = Substitute.For<IServiceMessageHandler>();
            serviceMessageHandler1.ServiceMessageHandlerId.Returns(Guid.NewGuid());
            serviceMessageHandler2.ServiceMessageHandlerId.Returns(Guid.NewGuid());

            serviceMessageProcessor.RegisteredServiceMessageHandler(serviceMessageHandler1);
            serviceMessageProcessor.RegisteredServiceMessageHandler(serviceMessageHandler2);

            Assert.AreEqual(2, serviceMessageProcessor.Count);
        }
        
        [Test]
        public void ExceptionInServiceMessageHandlerTest()
        {
            IServiceMessageProcessor serviceMessageProcessor = new ServiceMessageProcessor();
            IServiceMessageHandler serviceMessageHandler = Substitute.For<IServiceMessageHandler>();
            IServiceMessage message = Substitute.For<IServiceMessage>();

            serviceMessageHandler.ServiceMessageHandlerId.Returns(Guid.NewGuid());
            serviceMessageHandler.HandleMessage(Arg.Any<IServiceMessage>()).ThrowsForAnyArgs(new Exception());

            serviceMessageProcessor.RegisteredServiceMessageHandler(serviceMessageHandler);
            
            var actual = serviceMessageProcessor.SendMessage(message);
            Assert.IsFalse(actual);            
        }

        [Test]
        [ExpectedException(typeof(ServiceMessageException))]
        public void RethrowExceptionInServiceMessageHandlerTest()
        {
            IServiceMessageProcessor serviceMessageProcessor = new ServiceMessageProcessor();
            IServiceMessageHandler serviceMessageHandler = Substitute.For<IServiceMessageHandler>();
            IServiceMessage message = Substitute.For<IServiceMessage>();

            serviceMessageHandler.ServiceMessageHandlerId.Returns(Guid.NewGuid());
            serviceMessageHandler.HandleMessage(Arg.Any<IServiceMessage>()).ThrowsForAnyArgs(new Exception());

            serviceMessageProcessor.RegisteredServiceMessageHandler(serviceMessageHandler);

            serviceMessageProcessor.SendMessage(message, false);
            Assert.Fail("ServiceMessageException expected");
        }
    }
}
