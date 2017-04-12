﻿using System;
using DotNetCraft.Common.Core.DataAccessLayer;
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
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            IUnitOfWorkFactory unitOfWorkFactory = new SmartUnitOfWorkFactory(dataContextFactory, dotNetCraftMapper);
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
            IDotNetCraftMapper dotNetCraftMapper = correctDotNetCraft ? Substitute.For<IDotNetCraftMapper>(): null;
            new SmartUnitOfWorkFactory(dataContextFactoryt, dotNetCraftMapper);

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
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            IUnitOfWorkFactory unitOfWorkFactory = new SmartUnitOfWorkFactory(dataContextFactory, dotNetCraftMapper);

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
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            IUnitOfWorkFactory unitOfWorkFactory = new SmartUnitOfWorkFactory(dataContextFactory, dotNetCraftMapper);

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
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            IUnitOfWorkFactory unitOfWorkFactory = new SmartUnitOfWorkFactory(dataContextFactory, dotNetCraftMapper);

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
