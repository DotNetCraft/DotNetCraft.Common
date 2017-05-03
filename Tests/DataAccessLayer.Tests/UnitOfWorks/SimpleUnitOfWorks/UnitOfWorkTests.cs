using System;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
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
            IDataContext dataContext = Substitute.For<IDataContext>();
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
            IDataContext dataContext = null;
            new UnitOfWork(dataContext);

            Assert.Fail("ArgumentNullException expected");
        }

        [Test]
        [ExpectedException(typeof(UnitOfWorkException))]
        public void ConstructorExceptionTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            
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
            IDataContext dataContext = Substitute.For<IDataContext>();
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
            IDataContext dataContext = Substitute.For<IDataContext>();

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
            IDataContext dataContext = Substitute.For<IDataContext>();

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
            IDataContext dataContext = Substitute.For<IDataContext>();

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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IEntity entity = Substitute.For<IEntity>();
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IEntity entity = Substitute.For<IEntity>();
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IEntity entity = Substitute.For<IEntity>();

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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IEntity entity = Substitute.For<IEntity>();
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IEntity entity = Substitute.For<IEntity>();
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IEntity entity = Substitute.For<IEntity>();

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
            IDataContext dataContext = Substitute.For<IDataContext>();
            int entityId = 5;
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
            {
                unitOfWork.Delete<IEntity>(entityId);
                unitOfWork.Commit();
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).Delete<IEntity>(entityId);
            dataContext.Received(1).Commit();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().RollBack();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteRollbackTest(bool useRollbackMethod)
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            int entityId = 5;
            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
            {
                unitOfWork.Delete<IEntity>(entityId);
                if (useRollbackMethod)
                    unitOfWork.Rollback();
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).Delete<IEntity>(entityId);
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }

        [Test]
        public void DeleteExceptionTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            IEntity entity = Substitute.For<IEntity>();

            dataContext.When(x => x.Delete<IEntity>(5)).Do(
                x =>
                {
                    throw new Exception();
                });

            using (IUnitOfWork unitOfWork = new UnitOfWork(dataContext))
            {
                try
                {
                    unitOfWork.Delete<IEntity>(5);
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
            dataContext.Received(1).Delete<IEntity>(5);
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }

        #endregion
    }
}
