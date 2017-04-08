using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks
{
    public class UnitOfWork: BaseLoggerObject, IUnitOfWork
    {
        protected readonly IDataContext dataContext;

        /// <summary>
        /// Flag shows that transaction opened.
        /// </summary>
        public bool IsTransactionOpened { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger">The <see cref="ICommonLogger"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is <see langword="null" />.</exception>
        public UnitOfWork(IDataContext dataContext, ICommonLogger logger) : base(logger)
        {
            if (dataContext == null)
                throw new ArgumentNullException(nameof(dataContext));

            this.dataContext = dataContext;

            try
            {
                logger.WriteDebug("Opening transaction...");
                dataContext.BeginTransaction();
                IsTransactionOpened = true;
                logger.WriteDebug("Transaction has been opened.");
            }
            catch (Exception ex)
            {
                logger.WriteError(ex, "There was an exception during opening a transaction.");
                throw new UnitOfWorkException("There was an exception during opening a transaction.", ex);
            }
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~UnitOfWork()
        {
            OnDisposing(false);
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            OnDisposing(true);
        }

        protected virtual void OnDisposing(bool isDisposing)
        {
            if (isDisposing)
            {
                logger.WriteTrace("Disposing UnitOfWork: IsTransactionOpened = {0}...", IsTransactionOpened);
                if (IsTransactionOpened)
                {
                    try
                    {
                        logger.WriteTrace("Transaction still opened. Closing...");
                        Rollback();
                        logger.WriteTrace("Transaction has been closed.");
                    }
                    catch (UnitOfWorkException unitOfWorkException)
                    {
                        logger.WriteError(unitOfWorkException, "The was an exception during closing transaction. However, disposing will be continue...");
                    }
                }
                dataContext.Dispose();
                logger.WriteTrace("The object has been disposed.");
            }
        }

        #endregion

        #region Implementation of IUnitOfWork

        #region Virtual methods: OnInsert, OnUpdate and OnDelete

        protected virtual TEntity OnInsert<TEntity>(TEntity entity)
           where TEntity : class, IEntity
        {
            logger.WriteTrace("Inserting {0} into the data context...", entity);
            TEntity result = dataContext.Insert(entity);
            logger.WriteTrace("The entity has been inserted.");
            return result;
        }

        protected virtual void OnUpdate<TEntity>(TEntity entity)
           where TEntity : class, IEntity
        {
            logger.WriteTrace("Updating {0} in the data context...", entity);
            dataContext.Update(entity);
            logger.WriteTrace("The entity has been updated.");
        }

        protected virtual void OnDelete<TEntity>(TEntity entity)
           where TEntity : class, IEntity
        {
            logger.WriteTrace("Deliting {0} from the data context...", entity);
            dataContext.Delete(entity);
            logger.WriteTrace("The entity has been deleted.");
        }

        #endregion        

        /// <summary>
        /// Insert an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <return>The entity that has been inserted.</return>
        /// <exception cref="UnitOfWorkException">There was a problem during inserting a new entity into the database..</exception>
        public TEntity Insert<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            try
            {
                logger.WriteDebug("Inserting {0} into the data context...", entity);
                TEntity result = OnInsert(entity);
                logger.WriteDebug("The entity has been inserted.");
                return result;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Entity", entity.ToString()}
                };
                UnitOfWorkException unitOfWorkException = new UnitOfWorkException("There was a problem during inserting a new entity into the database.", ex, errorParameters);
                logger.WriteError(unitOfWorkException, unitOfWorkException.ToString());
                throw unitOfWorkException;
            }
        }

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>        
        public void Update<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            try
            {
                logger.WriteDebug("Updating {0} in the data context...", entity);
                OnUpdate(entity);
                logger.WriteDebug("The entity has been updated.");                
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Entity", entity.ToString()}
                };
                UnitOfWorkException unitOfWorkException = new UnitOfWorkException("There was a problem during updating an existing entity in the database.", ex, errorParameters);
                logger.WriteError(unitOfWorkException, unitOfWorkException.ToString());
                throw unitOfWorkException;
            }
        }

        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            try
            {
                logger.WriteDebug("Deleting {0} from the data context...", entity);
                OnDelete(entity);
                logger.WriteDebug("The entity has been deleted.");
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Entity", entity.ToString()}
                };
                UnitOfWorkException unitOfWorkException = new UnitOfWorkException("There was a problem during deleting an existing entity from the database.", ex, errorParameters);
                logger.WriteError(unitOfWorkException, unitOfWorkException.ToString());
                throw unitOfWorkException;
            }
        }

        /// <summary>
        /// Commit transaction.
        /// </summary>
        public void Commit()
        {
            if (IsTransactionOpened == false)
                throw new UnitOfWorkException("Transaction has been closed");

            try
            {
                dataContext.Commit();
                IsTransactionOpened = false;
            }
            catch (Exception ex)
            {
                logger.WriteError(ex, "There was an exception during committing changes.");
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
                dataContext.RollBack();
                IsTransactionOpened = false;
            }
            catch (Exception ex)
            {
                logger.WriteError(ex, "There was an exception during rolling back changes.");
                throw new UnitOfWorkException("There was an exception during rolling back changes.", ex);
            }
        }

        #endregion        
    }
}
