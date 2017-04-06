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
        /// Constructor.
        /// </summary>
        /// <param name="logger">The <see cref="ICommonLogger"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is <see langword="null" />.</exception>
        public UnitOfWork(IDataContext dataContext, ICommonLogger logger) : base(logger)
        {
            if (dataContext == null)
                throw new ArgumentNullException(nameof(dataContext));

            this.dataContext = dataContext;
            dataContext.BeginTransaction();
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
                dataContext.Dispose();
        }

        #endregion

        #region Implementation of IUnitOfWork

        protected virtual TEntity OnInsert<TEntity>(TEntity entity)
           where TEntity : class, IEntity
        {
            TEntity result = dataContext.Insert(entity);
            return result;
        }

        /// <summary>
        /// Insert an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <return>The entity that has been inserted.</return>
        public TEntity Insert<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            try
            {
                TEntity result = OnInsert<TEntity>(entity);
                return result;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Entity", entity.ToString()}
                };
                throw new DataAccessLayerException("There was a problem during inserting a new entity into the database", ex, errorParameters);
            }
        }

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>        
        public void Update<TEntity>(TEntity entity)
            where TEntity : IEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete<TEntity>(TEntity entity)
            where TEntity : IEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Commit transaction.
        /// </summary>
        public void Commit()
        {
            dataContext.Commit();
        }

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        public void Rollback()
        {
            dataContext.RollBack();
        }

        #endregion        
    }
}
