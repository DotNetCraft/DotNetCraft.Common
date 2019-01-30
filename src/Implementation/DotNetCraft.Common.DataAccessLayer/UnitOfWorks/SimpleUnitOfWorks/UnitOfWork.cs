using System;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.Utils.Disposal;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks
{
    public class UnitOfWork: DisposableObject, IUnitOfWork
    {
        private readonly ICommonLogger logger;

        protected readonly IDataContextWrapper dataContextWrapper;

        /// <summary>
        /// Unique key.
        /// </summary>
        public IUniqueKey UniqueKey
        {
            get { return dataContextWrapper.UniqueKey; }
        }

        /// <summary>
        /// Flag shows that transaction opened.
        /// </summary>
        public bool IsTransactionOpened { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitOfWork(IDataContextWrapper dataContextWrapper, ICommonLogger logger)
        {
            if (dataContextWrapper == null)
                throw new ArgumentNullException(nameof(dataContextWrapper));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this.dataContextWrapper = dataContextWrapper;
            this.logger = logger;

            try
            {
                logger.Debug("Opening transaction...");
                dataContextWrapper.BeginTransaction();
                IsTransactionOpened = true;
                logger.Debug("Transaction has been opened.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was an exception during opening a transaction.");
                throw new UnitOfWorkException("There was an exception during opening a transaction.", ex);
            }
        }

        #region Implementation of IUnitOfWork
       
        /// <summary>
        /// Commit transaction.
        /// </summary>
        public void Commit()
        {
            if (IsTransactionOpened == false)
                throw new UnitOfWorkException("Transaction has been closed");

            try
            {
                dataContextWrapper.Commit();
                IsTransactionOpened = false;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was an exception during committing changes.");
                throw new UnitOfWorkException("There was an exception during committing changes.", ex);
            }
        }

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        public void Rollback()
        {
            if (IsTransactionOpened == false)
                throw new UnitOfWorkException("Transaction has been closed");

            try
            {
                dataContextWrapper.RollBack();
                IsTransactionOpened = false;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was an exception during rolling back changes.");
                throw new UnitOfWorkException("There was an exception during rolling back changes.", ex);
            }
        }

        #endregion        

        #region Overrides of DisposableObject

        /// <summary>
        /// Releases resources held by the object.
        /// </summary>
        /// <param name="disposing"><c>True</c> if called manually, otherwise by GC.</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                logger.Trace("Disposing UnitOfWork: IsTransactionOpened = {0}...", IsTransactionOpened);
                if (IsTransactionOpened)
                {
                    try
                    {
                        logger.Trace("Transaction still opened. Closing...");
                        Rollback();
                        logger.Trace("Transaction has been closed.");
                    }
                    catch (UnitOfWorkException unitOfWorkException)
                    {
                        logger.Error(unitOfWorkException, "The was an exception during closing transaction. However, disposing will be continue...");
                    }
                }
                dataContextWrapper.Dispose();
                logger.Trace("The object has been disposed.");
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
