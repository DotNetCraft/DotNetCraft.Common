using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.DataAccessLayer;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessLayer.Tests
{
    [TestFixture]
    public class DataContextPoolItemTests
    {
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(20)]
        public void IncreseRefTest(int referenceCount)
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
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
            IDataContext dataContext = Substitute.For<IDataContext>();
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IDataContextPoolItem dataContextPoolItem = new DataContextPoolItem(dataContext);
            dataContextPoolItem.DecreaseRef();

            dataContextPoolItem.DecreaseRef();
            Assert.Fail("Exception expected");
        }
    }
}
