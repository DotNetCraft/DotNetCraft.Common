using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCraft.Common.Core.Utils.Caching;
using DotNetCraft.Common.Utils.Caching;
using NUnit.Framework;

namespace Utils.Tests
{
    [TestFixture]
    public class SimpleCacheTests
    {
        [Test]
        public void FullCycleSingleThreadTest()
        {
            ISimpleCache<string, int> simpleCache = new SimpleCache<string, int>();
            simpleCache.Add("key", 5);

            int actual = simpleCache.Get("key");
            Assert.AreEqual(5, actual);

            simpleCache.Update("key", 7);
            bool result = simpleCache.TryGetValue("key", out actual);
            Assert.IsTrue(result);
            Assert.AreEqual(7, actual);

            result = simpleCache.Delete("another key");
            Assert.IsFalse(result);

            result = simpleCache.Delete("key");
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase(500)]
        public void FullCycleMultiThreadTest(int threadCount)
        {
            ISimpleCache<string, int> simpleCache = new SimpleCache<string, int>();
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < threadCount; i++)
            {
                Task task = Task.Run(() =>
                {
                    int actual;
                    bool result = simpleCache.TryGetValue("key", out actual);
                    if (result)
                        Assert.AreEqual(5, actual);

                    simpleCache.AddOrUpdate("key", 5);
                    actual = simpleCache.Get("key");
                    Assert.AreEqual(5, actual);
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        [Test]
        [TestCase(500)]
        public void TryGetValueTest(int threadCount)
        {
            ISimpleCache<string, int> simpleCache = new SimpleCache<string, int>();
            int actual;
            bool result = simpleCache.TryGetValue("key", out actual);
            Assert.IsFalse(result);

            simpleCache.AddOrUpdate("key", 5);

            actual = simpleCache.Get("key");
            Assert.AreEqual(5, actual);
        }
    }
}
