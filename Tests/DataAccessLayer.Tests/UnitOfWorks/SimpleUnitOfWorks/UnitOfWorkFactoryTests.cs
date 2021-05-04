using System;
using DotNetCraft.Common.CommonClasses.Tests;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
            IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(serviceProvider, dataContextFactory, new NullLogger<UnitOfWorkFactory>());
            Assert.IsNotNull(unitOfWorkFactory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullParameterTest()
        {
            IDataContextFactory dataContextFactoryt = null;
            IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
            new UnitOfWorkFactory(serviceProvider, dataContextFactoryt, new NullLogger<UnitOfWorkFactory>());

            Assert.Fail("ArgumentNullException expected");
        }

        #endregion

        #region CreateDataContextTest

        [Test]
        public void CreateDataContextTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<UnitOfWork>();
            serviceCollection.AddTransient<ILogger<UnitOfWork>, NullLogger<UnitOfWork>>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(serviceProvider, dataContextFactory, new NullLogger<UnitOfWorkFactory>());

            dataContextFactory.CreateDataContext().Returns(dataContext);

            IUnitOfWork unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
            Assert.IsNotNull(unitOfWork);
            dataContextFactory.Received(1).CreateDataContext();
        }

        [Test]
        public void CreateDataContextExceptionTest()
        {
            IDataContextFactory dataContextFactory = Substitute.For<IDataContextFactory>();
            IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(serviceProvider, dataContextFactory, new NullLogger<UnitOfWorkFactory>());

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
            IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(serviceProvider, dataContextFactory, new NullLogger<UnitOfWorkFactory>());

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
