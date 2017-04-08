using System;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessLayer.Tests
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        [Test]
        public void ConstructorTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, logger))
            {
                Assert.IsTrue(unitOfWork.IsTransactionOpened);
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }

        [Test]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullParameterTest(bool correctContext, bool correctLogger)
        {
            IDataContext dataContext = correctContext ? Substitute.For<IDataContext>() : null;
            ICommonLogger logger = correctLogger ? Substitute.For<ICommonLogger>() : null;
            new UnitOfWork(dataContext, logger);

            Assert.Fail("ArgumentNullException expected");
        }

        [Test]
        [ExpectedException(typeof(UnitOfWorkException))]
        public void ConstructorExceptionTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();

            dataContext.When(x => x.BeginTransaction()).Do(
                x =>
                {
                    throw new Exception();
                });

            new UnitOfWork(dataContext, logger);

            Assert.Fail("ArgumentNullException expected");
        }

        [Test]
        public void CommitTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, logger))
            {
                unitOfWork.Commit();
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).Commit();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().RollBack();
        }

        [Test]
        public void CommitExceptionTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();

            dataContext.When(x => x.Commit()).Do(
                x =>
                {
                    throw new Exception();
                });

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, logger))
            {
                try
                {
                    unitOfWork.Commit();
                }
                catch (UnitOfWorkException)
                {
                }
                catch (Exception)
                {
                    Assert.Fail("UnitOfWorkException expected");
                }                
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).Commit();
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
        }

        [Test]
        public void RollbackTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, logger))
            {
                unitOfWork.Rollback();
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }

        [Test]
        public void RollbackExceptionTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();

            dataContext.When(x => x.RollBack()).Do(
                x =>
                {
                    throw new Exception();
                });

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, logger))
            {
                try
                {
                    unitOfWork.Rollback();
                }
                catch (UnitOfWorkException)
                {
                }
                catch (Exception)
                {
                    Assert.Fail("UnitOfWorkException expected");
                }
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(2).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }

        [Test]
        public void InsertCommitTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IEntity entity = Substitute.For<IEntity>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, logger))
            {
                unitOfWork.Insert(entity);
                unitOfWork.Commit();
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).Insert(entity);
            dataContext.Received(1).Commit();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().RollBack();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void InsertRollbackTest(bool useRollbackMethod)
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IEntity entity = Substitute.For<IEntity>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, logger))
            {                
                unitOfWork.Insert(entity);
                if (useRollbackMethod)
                    unitOfWork.Rollback();
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).Insert(entity);
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }

        [Test]
        public void InsertExceptionTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IEntity entity = Substitute.For<IEntity>();

            dataContext.When(x => x.Insert(entity)).Do(
                x =>
                {
                    throw new Exception();
                });

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, logger))
            {                
                try
                {
                    unitOfWork.Insert(entity);
                    unitOfWork.Commit();
                }
                catch (UnitOfWorkException)
                {
                }
                catch (Exception)
                {
                    Assert.Fail("UnitOfWorkException expected");
                }
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).Insert(entity);
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }
    }
}
