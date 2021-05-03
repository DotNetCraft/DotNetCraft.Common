using DotNetCraft.Common.Utils.Disposal;
using NUnit.Framework;

namespace DotNetCraft.Common.Utils.Tests
{
    [TestFixture]
    public class DisposableObjectTests
    {
        private class Sample: DisposableObject
        {            
        }

        [Test]
        public void TestUsingScenario()
        {
            Sample sample;
            using (sample = new Sample())
            {
                Assert.IsFalse(sample.IsDisposed);
            }
            Assert.IsTrue(sample.IsDisposed);
        }

        [Test]
        public void TestNotifyWhenDisposed()
        {
            bool isEventRaised = false;
            Sample sample;
            using (sample = new Sample())
            {
                sample.Disposed += (sender, args) =>
                {
                    isEventRaised = true;
                    DisposableObject disposableObject = (DisposableObject) sender;
                    Assert.IsTrue(disposableObject.IsDisposed);
                };
            }

            Assert.IsTrue(sample.IsDisposed);
        }
    }
}
