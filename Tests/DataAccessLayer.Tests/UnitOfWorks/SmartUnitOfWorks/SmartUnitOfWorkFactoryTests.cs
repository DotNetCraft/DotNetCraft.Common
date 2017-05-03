using System;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;
using DotNetCraft.Common.Core.Utils;
using DotNetCraft.Common.Core.Utils.Logging;
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
            IEntityModelMapper entityModelMapper = Substitute.For<IEntityModelMapper>();
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
            IEntityModelMapper entityModelMapper = correctDotNetCraft ? Substitute.For<IEntityModelMapper>(): null;
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IEntityModelMapper entityModelMapper = Substitute.For<IEntityModelMapper>();
            IUnitOfWorkFactory unitOfWorkFactory = new SmartUnitOfWorkFactory(dataContextFactory, entityModelMapper);

            dataContextFactory.CreateDataContext(contextSettings).Returns(dataContext);

            IUnitOfWork unitOfWork = unitOfWorkFactory.CreateUnitOfWork(contextSettings);
            Assert.IsNotNull(unitOfWork);
            dataContextFactory.Received(1).CreateDataContext(contextSettings);
        }

        [Test]
        public void CreateDataContextExceptionTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            IContextSettings contextSettings = Substitute.For<IContextSettings>();
            IEntityModelMapper entityModelMapper = Substitute.For<IEntityModelMapper>();
            IUnitOfWorkFactory unitOfWorkFactory = new SmartUnitOfWorkFactory(dataContextFactory, entityModelMapper);

            dataContextFactory.When(x => x.CreateDataContext(contextSettings)).Do(x =>
            {
                throw new Exception();
            });

            try
            {
                unitOfWorkFactory.CreateUnitOfWork(contextSettings);
                Assert.Fail("UnitOfWorkException expected");
            }
            catch (UnitOfWorkException)
            {
            }
            catch (Exception)
            {
                Assert.Fail("UnitOfWorkException expected");
            }

            dataContextFactory.Received(1).CreateDataContext(contextSettings);
        }

        [Test]
        public void CreateDataContextBadContextTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            IContextSettings contextSettings = Substitute.For<IContextSettings>();
            IEntityModelMapper entityModelMapper = Substitute.For<IEntityModelMapper>();
            IUnitOfWorkFactory unitOfWorkFactory = new SmartUnitOfWorkFactory(dataContextFactory, entityModelMapper);

            IDataContext dataContext = null;
            dataContextFactory.CreateDataContext(contextSettings).Returns(dataContext);

            try
            {
                unitOfWorkFactory.CreateUnitOfWork(contextSettings);
                Assert.Fail("UnitOfWorkException expected");
            }
            catch (UnitOfWorkException)
            {
            }
            catch (Exception)
            {
                Assert.Fail("UnitOfWorkException expected");
            }

            dataContextFactory.Received(1).CreateDataContext(contextSettings);
        }

        #endregion
    }
}
