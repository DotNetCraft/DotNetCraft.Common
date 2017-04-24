using System;
using DotNetCraft.Common.Domain.ServiceMessenger;
using NUnit.Framework;

namespace Domain.Tests
{
    [TestFixture]
    public class BaseServiceMessageTests
    {
        private class Sample: BaseServiceMessage
        {
            public Sample(object sender) : base(sender)
            {
            }
        }

        [Test]
        public void BaseServiceMessageConstructorTest()
        {
            BaseServiceMessage sample = new Sample(new object());
            Assert.IsNotNull(sample.Sender);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BaseServiceMessageConstructorWithNullParamsTest()
        {
            BaseServiceMessage sample = new Sample(null);
            Assert.Fail("ArgumentNullException expected");
        }
    }
}
