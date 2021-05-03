using System;
using DotNetCraft.Common.CommonClasses.Tests;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;
using DotNetCraft.Common.Utils.Logging;
using NSubstitute;
using NUnit.Framework;

namespace DotNetCraft.Common.DataAccessLayer.Tests.UnitOfWorks.SimpleUnitOfWorks
{
    [TestFixture]
    public class UnitOfWorkFactoryTests
    {
        #region Constructors...

        [Test]
        public void ConstructorTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(dataContextFactory, new DebugLoggerFactory());
            Assert.IsNotNull(unitOfWorkFactory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullParameterTest()
        {
            IDataContextFactory dataContextFactoryt = null;
            new UnitOfWorkFactory(dataContextFactoryt, new DebugLoggerFactory());

            Assert.Fail("ArgumentNullException expected");
        }

        #endregion

        #region CreateDataContextTest

        [Test]
        public void CreateDataContextTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(dataContextFactory, new DebugLoggerFactory());

            dataContextFactory.CreateDataContext().Returns(dataContext);

            IUnitOfWork unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
            Assert.IsNotNull(unitOfWork);
            dataContextFactory.Received(1).CreateDataContext();
        }

        [Test]
        public void CreateDataContextExceptionTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(dataContextFactory, new DebugLoggerFactory());

            dataContextFactory.When(x=>x.CreateDataContext()).Do(x =>
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
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(dataContextFactory, new DebugLoggerFactory());

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
