using System;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessLayer.Tests.UnitOfWorks.SimpleUnitOfWorks
{
    [TestFixture]
    public class UnitOfWorkFactoryTests
    {
        #region Constructors...

        [Test]
        public void ConstructorTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            ICommonLoggerFactory loggerFactory = Substitute.For<ICommonLoggerFactory>();
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(dataContextFactory, loggerFactory);
            Assert.IsNotNull(unitOfWorkFactory);
        }

        [Test]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullParameterTest(bool correctContext, bool correctLogger)
        {
            IDataContextFactory dataContextFactoryt = correctContext ? Substitute.For<IDataContextFactory>() : null;
            ICommonLoggerFactory loggerFactory = correctLogger ? Substitute.For<ICommonLoggerFactory>() : null;
            new UnitOfWorkFactory(dataContextFactoryt, loggerFactory);

            Assert.Fail("ArgumentNullException expected");
        }

        #endregion

        #region CreateDataContextTest

        [Test]
        public void CreateDataContextTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            ICommonLoggerFactory loggerFactory = Substitute.For<ICommonLoggerFactory>();
            IContextSettings contextSettings = Substitute.For<IContextSettings>();
            IDataContext dataContext = Substitute.For<IDataContext>();
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(dataContextFactory, loggerFactory);

            dataContextFactory.CreateDataContext(contextSettings).Returns(dataContext);

            IUnitOfWork unitOfWork = unitOfWorkFactory.CreateUnitOfWork(contextSettings);
            Assert.IsNotNull(unitOfWork);
            dataContextFactory.Received(1).CreateDataContext(contextSettings);
        }

        [Test]
        public void CreateDataContextExceptionTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            ICommonLoggerFactory loggerFactory = Substitute.For<ICommonLoggerFactory>();
            IContextSettings contextSettings = Substitute.For<IContextSettings>();
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(dataContextFactory, loggerFactory);

            dataContextFactory.When(x=>x.CreateDataContext(contextSettings)).Do(x =>
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
            ICommonLoggerFactory loggerFactory = Substitute.For<ICommonLoggerFactory>();
            IContextSettings contextSettings = Substitute.For<IContextSettings>();
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(dataContextFactory, loggerFactory);

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
