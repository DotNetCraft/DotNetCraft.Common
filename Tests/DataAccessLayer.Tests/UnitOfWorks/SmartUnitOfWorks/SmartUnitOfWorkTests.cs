using System;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.Utils;
using DotNetCraft.Common.Core.Utils.Logging;
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
            {
                Assert.IsTrue(unitOfWork.IsTransactionOpened);
            }
            dataContext.Received(1).BeginTransaction();
            dataContext.Received(1).RollBack();
            dataContext.Received(1).Dispose();
            dataContext.DidNotReceive().Commit();
        }

        [Test]
        [TestCase(false, true, true)]
        [TestCase(true, false, true)]
        [TestCase(true, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, true, false)]
        [TestCase(true, false, false)]
        [TestCase(false, false, false)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullParameterTest(bool correctContext, bool correctDotNetCraft, bool correctLogger)
        {
            IDataContext dataContext = correctContext ? Substitute.For<IDataContext>() : null;
            ICommonLogger logger = correctLogger ? Substitute.For<ICommonLogger>() : null;
            IDotNetCraftMapper dotNetCraftMapper = correctDotNetCraft ? Substitute.For<IDotNetCraftMapper>() : null;
            new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger);

            Assert.Fail("ArgumentNullException expected");
        }

        [Test]
        [ExpectedException(typeof(UnitOfWorkException))]
        public void ConstructorExceptionTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();

            dataContext.When(x => x.BeginTransaction()).Do(
                x =>
                {
                    throw new Exception();
                });

            new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger);

            Assert.Fail("ArgumentNullException expected");
        }

        #endregion

        #region Commit...

        [Test]
        public void CommitTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();

            dataContext.When(x => x.Commit()).Do(
                x =>
                {
                    throw new Exception();
                });

            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();

            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();

            dataContext.When(x => x.RollBack()).Do(
                x =>
                {
                    throw new Exception();
                });

            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IEntity entity = Substitute.For<IEntity>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IEntity entity = Substitute.For<IEntity>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IEntity entity = Substitute.For<IEntity>();

            dataContext.When(x => x.Insert(entity)).Do(
                x =>
                {
                    throw new Exception();
                });

            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IEntity entity = Substitute.For<IEntity>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IEntity entity = Substitute.For<IEntity>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IEntity entity = Substitute.For<IEntity>();

            dataContext.When(x => x.Update(entity)).Do(
                x =>
                {
                    throw new Exception();
                });

            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IEntity entity = Substitute.For<IEntity>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IEntity entity = Substitute.For<IEntity>();
            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
            IDataContext dataContext = Substitute.For<IDataContext>();
            IDotNetCraftMapper dotNetCraftMapper = Substitute.For<IDotNetCraftMapper>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IEntity entity = Substitute.For<IEntity>();

            dataContext.When(x => x.Delete(entity)).Do(
                x =>
                {
                    throw new Exception();
                });

            using (ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(dataContext, dotNetCraftMapper, logger))
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
