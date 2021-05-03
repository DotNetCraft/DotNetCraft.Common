using System;
using System.Collections.Generic;
using CommonClasses;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.DataAccessLayer.DataContexts;
using DotNetCraft.Common.DataAccessLayer.DataContexts.UniqueKeys;
using NSubstitute;
using NUnit.Framework;

namespace DotNetCraft.Common.DataAccessLayer.Tests
{
    [TestFixture]
    public class DataContextPoolItemTests
    {
        [Test]
        public void ConstructorTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IDataContextPoolItem dataContextPoolItem = new DataContextPoolItem(dataContext);
            Assert.AreEqual(1, dataContextPoolItem.ReferenceCount);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullParameterTest()
        {
            new DataContextPoolItem(null);
            Assert.Fail("ArgumentNullException expected");
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(20)]
        public void IncreseRefTest(int referenceCount)
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IDataContextPoolItem dataContextPoolItem =new DataContextPoolItem(dataContext);
            Assert.AreEqual(1, dataContextPoolItem.ReferenceCount);

            for (int i = 0; i < referenceCount; i++)
            {
                dataContextPoolItem.IncreaseRef();
                int expected = 1 + 1 + i;
                Assert.AreEqual(expected, dataContextPoolItem.ReferenceCount);
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(20)]
        public void DecreaseRefTest(int referenceCount)
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IDataContextPoolItem dataContextPoolItem = new DataContextPoolItem(dataContext);
            
            for (int i = 0; i < referenceCount; i++)
                dataContextPoolItem.IncreaseRef();

            dataContextPoolItem.DecreaseRef();
            Assert.AreEqual(referenceCount, dataContextPoolItem.ReferenceCount);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void DecreaseRefNegativeTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IDataContextPoolItem dataContextPoolItem = new DataContextPoolItem(dataContext);
            dataContextPoolItem.DecreaseRef();

            dataContextPoolItem.DecreaseRef();
            Assert.Fail("Exception expected");
        }

        [Test]
        public void UniqueKeyDataContextPoolTest()
        {
            Dictionary<string, IDataContextPoolItem> dataContextPool = new Dictionary<string, IDataContextPoolItem>();
            IUniqueKey uniqueKey1 = new ThreadUniqueKey();

            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IDataContextPoolItem dataContextPoolItem = new DataContextPoolItem(dataContext);

            dataContextPool.Add(uniqueKey1.Key, dataContextPoolItem);

            IUniqueKey uniqueKey2 = new ThreadUniqueKey();
            IDataContextPoolItem actual = dataContextPool[uniqueKey2.Key];
            Assert.AreEqual(dataContextPoolItem.GetHashCode(), actual.GetHashCode());
        }
    }
}
