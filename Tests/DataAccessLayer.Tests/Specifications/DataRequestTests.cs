using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer.Specifications;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.DataAccessLayer.Specifications;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessLayer.Tests.Specifications
{
    [TestFixture]
    public class DataRequestTests
    {
        [Test]
        public void ConstructorTest()
        {
            ISpecification<IEntity> specification = Substitute.For<ISpecification<IEntity>>();
            IDataRequest<IEntity> dataBaseParameter = new DataRequest<IEntity>(specification);
            Assert.IsNotNull(dataBaseParameter.Specification);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullParameterTest()
        {
            new DataRequest<IEntity>(null);
            Assert.Fail("ArgumentNullException expected");
        }
    }
}
