using System;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;
using DotNetCraft.Common.Utils.Logging;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessLayer.Tests.UnitOfWorks.SimpleUnitOfWorks
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        #region Constructors...                           

        [Test]
        public void ConstructorTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, new DebugLogger(GetType())))
            {
                Assert.IsTrue(unitOfWork.IsTransactionOpened);
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullParameterTest()
        {
            IDataContextWrapper dataContext = null;
            new UnitOfWork(dataContext, new DebugLogger(GetType()));

            Assert.Fail("ArgumentNullException expected");
        }

        [Test]
        [ExpectedException(typeof(UnitOfWorkException))]
        public void ConstructorExceptionTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            
            dataContext.When(x => x.BeginTransaction()).Do(
                x =>
                {
                    throw new Exception();
                });

            new UnitOfWork(dataContext, new DebugLogger(GetType()));

            Assert.Fail("ArgumentNullException expected");
        }

        #endregion

        #region Commit...

        [Test]
        public void CommitTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, new DebugLogger(GetType())))
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
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();

            dataContext.When(x => x.Commit()).Do(
                x =>
                {
                    throw new Exception();
                });

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, new DebugLogger(GetType())))
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

        #endregion

        #region Rollback

        [Test]
        public void RollbackTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, new DebugLogger(GetType())))
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
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();

            dataContext.When(x => x.RollBack()).Do(
                x =>
                {
                    throw new Exception();
                });

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext, new DebugLogger(GetType())))
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

        #endregion
    }
}
