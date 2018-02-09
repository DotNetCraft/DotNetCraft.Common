using System;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Smart;
using DotNetCraft.Common.Core.Utils.Mapping;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SmartUnitOfWorks;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessLayer.Tests.UnitOfWorks.SmartUnitOfWorks
{
    [TestFixture]
    public class SmartUnitOfWorkTests
    {
        #region Constructors...

        [Test]
        public void ConstructorTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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
        public void ConstructorNullParameterTest(bool correctContext, bool correctDotNetCraft)
        {
            IDataContextWrapper dataContext = correctContext ? Substitute.For<IDataContextWrapper>() : null;
            IMapperManager entityModelMapper = correctDotNetCraft ? Substitute.For<IMapperManager>() : null;
            new SmartUnitOfWork(dataContext, entityModelMapper);

            Assert.Fail("ArgumentNullException expected");
        }

        [Test]
        [ExpectedException(typeof(UnitOfWorkException))]
        public void ConstructorExceptionTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();

            dataContext.When(x => x.BeginTransaction()).Do(
                x =>
                {
                    throw new Exception();
                });

            new SmartUnitOfWork(dataContext, entityModelMapper);

            Assert.Fail("ArgumentNullException expected");
        }

        #endregion

        #region Commit...

        [Test]
        public void CommitTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();

            dataContext.When(x => x.Commit()).Do(
                x =>
                {
                    throw new Exception();
                });

            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();

            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();

            dataContext.When(x => x.RollBack()).Do(
                x =>
                {
                    throw new Exception();
                });

            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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

        #region Insert Entity
        [Test]
        public void InsertEntityCommitTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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
        public void InsertEntityRollbackTest(bool useRollbackMethod)
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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
        public void InsertEntityExceptionTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            FakeEntity entity = Substitute.For<FakeEntity>();

            dataContext.When(x => x.Insert(entity)).Do(
                x =>
                {
                    throw new Exception();
                });

            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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

        #region Insert Model
        //TODO: Add tests
        #endregion

        #endregion

        #region Update

        #region Update Entity

        [Test]
        public void UpdateEntityCommitTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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
        public void UpdateEntityRollbackTest(bool useRollbackMethod)
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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
        public void UpdateEntityExceptionTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            FakeEntity entity = Substitute.For<FakeEntity>();

            dataContext.When(x => x.Update(entity)).Do(
                x =>
                {
                    throw new Exception();
                });

            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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

        #region Update Model
        //TODO: Add tests
        #endregion

        #endregion

        #region Delete

        #region Delete Entity

        [Test]
        public void DeleteEntityCommitTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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
        public void DeleteEntityRollbackTest(bool useRollbackMethod)
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
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
        public void DeleteEntityExceptionTest()
        {
            IDataContextWrapper dataContext = Substitute.For<IDataContextWrapper>();
            IMapperManager entityModelMapper = Substitute.For<IMapperManager>();
            FakeEntity entity = Substitute.For<FakeEntity>();
            dataContext.When(x => x.Delete(entity)).Do(
                x =>
                {
                    throw new Exception();
                });

            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, entityModelMapper))
            {
                try
                {
                    unitOfWork.Delete(entity);
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

        #region Delete Model
        //TODO: Add tests
        #endregion

        #endregion
    }
}
