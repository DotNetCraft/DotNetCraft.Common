using System;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;
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
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
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
            new UnitOfWork(dataContext);

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

            new UnitOfWork(dataContext);

            Assert.Fail("ArgumentNullException expected");
        }

        #endregion

        #region Commit...

        [Test]
        public void CommitTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
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

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
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

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
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

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
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

        #region Insert

        [Test]
        public void InsertCommitTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
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
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
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
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            FakeEntity entity = Substitute.For<FakeEntity>();

            dataContext.When(x => x.Insert(entity)).Do(
                x =>
                {
                    throw new Exception();
                });

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
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

        #endregion

        #region Update

        [Test]
        public void UpdateCommitTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
            {
                unitOfWork.Update(entity);
                unitOfWork.Commit();
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).Update(entity);
            dataContext.Received(1).Commit();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().RollBack();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateRollbackTest(bool useRollbackMethod)
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
            {
                unitOfWork.Update(entity);
                if (useRollbackMethod)
                    unitOfWork.Rollback();
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).Update(entity);
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }

        [Test]
        public void UpdateExceptionTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            FakeEntity entity = Substitute.For<FakeEntity>();

            dataContext.When(x => x.Update(entity)).Do(
                x =>
                {
                    throw new Exception();
                });

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
            {
                try
                {
                    unitOfWork.Update(entity);
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
            dataContext.Received(1).Update(entity);
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }

        #endregion

        #region Delete

        [Test]
        public void DeleteCommitTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
            {
                unitOfWork.Delete(entity);
                unitOfWork.Commit();
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).Delete(entity);
            dataContext.Received(1).Commit();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().RollBack();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteRollbackTest(bool useRollbackMethod)
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
            {
                unitOfWork.Delete(entity);
                if (useRollbackMethod)
                    unitOfWork.Rollback();
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).Delete(entity);
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }

        [Test]
        public void DeleteExceptionTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            FakeEntity entity = Substitute.For<FakeEntity>();

            dataContext.When(x => x.Delete<FakeEntity>(entity)).Do(
                x =>
                {
                    throw new Exception();
                });

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
            {
                try
                {
                    unitOfWork.Delete<FakeEntity>(entity);
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
            dataContext.Received(1).Delete(entity);
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }

        #endregion
    }
}
