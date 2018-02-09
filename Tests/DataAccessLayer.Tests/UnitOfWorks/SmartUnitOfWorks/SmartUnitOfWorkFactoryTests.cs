using System;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;
using DotNetCraft.Common.Core.Utils;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.Core.Utils.Mapping;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SmartUnitOfWorks;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessLayer.Tests.UnitOfWorks.SmartUnitOfWorks
{
    [TestFixture]
    public class SmartUnitOfWorkFactoryTests
    {
        #region Constructors...

        [Test]
        public void ConstructorTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            IUnitOfWorkFactory unitOfWorkFactory = new SmartUnitOfWorkFactory(dataContextFactory, entityModelMapper);
            Assert.IsNotNull(unitOfWorkFactory);
        }

        [Test]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullParameterTest(bool correctContext, bool correctDotNetCraft)
        {
            IDataContextFactory dataContextFactoryt = correctContext ? Substitute.For<IDataContextFactory>() : null;
            IMapperManager entityModelMapper = correctDotNetCraft ? Substitute.For<IMapperManager>(): null;
            new SmartUnitOfWorkFactory(dataContextFactoryt, entityModelMapper);

            Assert.Fail("ArgumentNullException expected");
        }

        #endregion

        #region CreateDataContextTest

        [Test]
        public void CreateDataContextTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            IContextSettings contextSettings = Substitute.For<IContextSettings>();
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            IUnitOfWorkFactory unitOfWorkFactory = new SmartUnitOfWorkFactory(dataContextFactory, entityModelMapper);

            dataContextFactory.CreateDataContext().Returns(dataContext);

            IUnitOfWork unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
            Assert.IsNotNull(unitOfWork);
            dataContextFactory.Received(1).CreateDataContext();
        }

        [Test]
        public void CreateDataContextExceptionTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            IContextSettings contextSettings = Substitute.For<IContextSettings>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            IUnitOfWorkFactory unitOfWorkFactory = new SmartUnitOfWorkFactory(dataContextFactory, entityModelMapper);

            dataContextFactory.When(x => x.CreateDataContext()).Do(x =>
            {
                throw new Exception();
            });

            try
            {
                unitOfWorkFactory.CreateUnitOfWork();
                Assert.Fail("UnitOfWorkException expected");
            }
            catch (UnitOfWorkException)
            {
            }
            catch (Exception)
            {
                Assert.Fail("UnitOfWorkException expected");
            }

            dataContextFactory.Received(1).CreateDataContext();
        }

        [Test]
        public void CreateDataContextBadContextTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            IContextSettings contextSettings = Substitute.For<IContextSettings>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            IUnitOfWorkFactory unitOfWorkFactory = new SmartUnitOfWorkFactory(dataContextFactory, entityModelMapper);

            IDataContextWrapper dataContext = null;
            dataContextFactory.CreateDataContext().Returns(dataContext);

            try
            {
                unitOfWorkFactory.CreateUnitOfWork();
                Assert.Fail("UnitOfWorkException expected");
            }
            catch (UnitOfWorkException)
            {
            }
            catch (Exception)
            {
                Assert.Fail("UnitOfWorkException expected");
            }

            dataContextFactory.Received(1).CreateDataContext();
        }

        #endregion
    }
}
