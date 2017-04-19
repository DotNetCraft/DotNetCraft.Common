using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.Utils.Disposal;
using DotNetCraft.Common.Utils.Logging;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks
{
    public class UnitOfWork : DisposableObject, IUnitOfWork
    {
        private readonly ICommonLogger logger = LogManager.GetCurrentClassLogger();

        protected readonly IDataContext dataContext;

        /// <summary>
        /// Flag shows that transaction opened.
        /// </summary>
        public bool IsTransactionOpened { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitOfWork(IDataContext dataContext)
        {
            if (dataContext == null)
                throw new ArgumentNullException(nameof(dataContext));

            this.dataContext = dataContext;

            try
            {
                logger.Debug("Opening transaction...");
                dataContext.BeginTransaction();
                IsTransactionOpened = true;
                logger.Debug("Transaction has been opened.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was an exception during opening a transaction.");
                throw new UnitOfWorkException("There was an exception during opening a transaction.", ex);
            }
        }

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
                dataContext.Dispose();
                logger.Trace("The object has been disposed.");
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Implementation of IUnitOfWork

        #region Virtual methods: OnInsert, OnUpdate and OnDelete

        protected virtual void OnInsert<TEntity>(TEntity entity, bool assignIdentifier = true)
            where TEntity : class, IEntity
        {
            logger.Trace("Inserting {0} into the data context...", entity);
            dataContext.Insert(entity, assignIdentifier);
            logger.Trace("The entity has been inserted.");
        }

        protected virtual void OnUpdate<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            logger.Trace("Updating {0} in the data context...", entity);
            dataContext.Update(entity);
            logger.Trace("The entity has been updated.");
        }

        protected virtual void OnDelete<TEntity>(object entityId)
            where TEntity : class, IEntity
        {
            logger.Trace("Deliting {0} from the data context...", entityId);
            dataContext.Delete<TEntity>(entityId);
            logger.Trace("The entity has been deleted.");
        }

        protected virtual void OnDelete<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            logger.Trace("Deliting {0} from the data context...", entity);
            dataContext.Delete(entity);
            logger.Trace("The entity has been deleted.");
        }

        private ICollection<TEntity> OnExecuteQuery<TEntity>(string query, DataBaseParameter[] args)
            where TEntity : class, IEntity
        {
            logger.Trace("Executing query {0}...", query);
            ICollection<TEntity> result = dataContext.ExecuteQuery<TEntity>(query, args);
            logger.Trace("The query has been executed.");
            return result;
        }

        #endregion

        /// <summary>
        /// Insert an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <return>The entity that has been inserted.</return>
        /// <exception cref="UnitOfWorkException">There was a problem during inserting a new entity into the database..</exception>
        public void Insert<TEntity>(TEntity entity, bool assignIdentifier = true)
            where TEntity : class, IEntity
        {
            try
            {
                logger.Debug("Inserting {0} into the data context...", entity);
                OnInsert(entity, assignIdentifier);
                logger.Debug("The entity has been inserted.");
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Entity", entity.ToString()}
                };
                UnitOfWorkException unitOfWorkException = new UnitOfWorkException("There was a problem during inserting a new entity into the database.", ex, errorParameters);
                logger.Error(unitOfWorkException, unitOfWorkException.ToString());
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
                logger.Debug("Updating {0} in the data context...", entity);
                OnUpdate(entity);
                logger.Debug("The entity has been updated.");
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Entity", entity.ToString()}
                };
                UnitOfWorkException unitOfWorkException = new UnitOfWorkException("There was a problem during updating an existing entity in the database.", ex, errorParameters);
                logger.Error(unitOfWorkException, unitOfWorkException.ToString());
                throw unitOfWorkException;
            }
        }

        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <param name="entityId">The entity's identifier.</param>
        public void Delete<TEntity>(object entityId)
            where TEntity : class, IEntity
        {
            try
            {
                logger.Debug("Deleting {0} from the data context...", entityId);
                OnDelete<TEntity>(entityId);
                logger.Debug("The entity has been deleted.");
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"EntityId", entityId.ToString()}
                };
                UnitOfWorkException unitOfWorkException = new UnitOfWorkException("There was a problem during deleting an existing entity from the database.", ex, errorParameters);
                logger.Error(unitOfWorkException, unitOfWorkException.ToString());
                throw unitOfWorkException;
            }
        }

        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            try
            {
                logger.Debug("Deleting {0} from the data context...", entity);
                OnDelete(entity);
                logger.Debug("The entity has been deleted.");
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Entity", entity.ToString()}
                };
                UnitOfWorkException unitOfWorkException = new UnitOfWorkException("There was a problem during deleting an existing entity from the database.", ex, errorParameters);
                logger.Error(unitOfWorkException, unitOfWorkException.ToString());
                throw unitOfWorkException;
            }
        }

        /// <summary>
        /// Execute query.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="query">The query</param>
        /// <param name="args">Qeury's arguments (parameters)</param>
        /// <returns>List of entities.</returns>
        public ICollection<TEntity> ExecuteQuery<TEntity>(string query, params DataBaseParameter[] args)
            where TEntity : class, IEntity
        {
            try
            {
                logger.Debug("Executing query {0}...", query);
                ICollection<TEntity> result = OnExecuteQuery<TEntity>(query, args);
                logger.Debug("The query has been executed.");
                return result;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Query", query}
                };
                UnitOfWorkException unitOfWorkException = new UnitOfWorkException("There was a problem during executing query in the database.", ex, errorParameters);
                logger.Error(unitOfWorkException, unitOfWorkException.ToString());
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
                dataContext.RollBack();
                IsTransactionOpened = false;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was an exception during rolling back changes.");
                throw new UnitOfWorkException("There was an exception during rolling back changes.", ex);
            }
        }

        #endregion        
    }
}
