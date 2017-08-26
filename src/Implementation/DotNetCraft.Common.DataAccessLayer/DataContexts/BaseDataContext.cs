using System;
using System.Collections.Generic;
using System.Linq;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.Utils.Disposal;

namespace DotNetCraft.Common.DataAccessLayer.DataContexts
{
    public abstract class BaseDataContext : DisposableObject, IDataContext
    {
        private readonly IDataContextFactory owner;
        private bool isDisposed;

        protected BaseDataContext(IDataContextFactory owner)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            this.owner = owner;
        }


        #region Implementation of DisposableObject
        
        /// <summary>
        /// Occurs when object is disposing.
        /// </summary>
        /// <param name="isDisposing">Flag shows that object is disposing or not.</param>
        protected abstract void OnDataContextDisposing(bool isDisposing);

        /// <summary>
        /// Releases resources held by the object.
        /// </summary>
        /// <param name="disposing"><c>True</c> if called manually, otherwise by GC.</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bool canDispose = owner.ReleaseDataContext(this);
                if (canDispose == false)
                    return;
                OnDataContextDisposing(true);
            }
            else
            {
                OnDataContextDisposing(false);
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Implementation of IDataContext

        protected abstract IQueryable<TEntity> OnGetCollectionSet<TEntity>()
            where TEntity : class;

        protected abstract void OnInsert<TEntity>(TEntity entity, bool assignIdentifier = true)
            where TEntity : class;

        protected abstract void OnUpdate<TEntity>(TEntity entity)
            where TEntity : class;

        protected abstract void OnDelete<TEntity>(object entityId)
            where TEntity : class;

        protected abstract void OnDelete<TEntity>(TEntity entity)
            where TEntity : class;

        protected abstract ICollection<TEntity> OnExecuteQuery<TEntity>(string query, IDataBaseParameter[] args)
            where TEntity : class;

        /// <summary>
        /// Get a set with entities.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <returns>The IQueryable instance.</returns>
        public IQueryable<TEntity> GetCollectionSet<TEntity>() 
            where TEntity : class
        {
            try
            {
                IQueryable<TEntity> result = OnGetCollectionSet<TEntity>();
                return result;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()}
                };
                throw new DataAccessLayerException("There was a problem during retrieving a collection set", ex, errorParameters);
            }
        }        

        public void Insert<TEntity>(TEntity entity, bool assignIdentifier = true)
            where TEntity : class
        {
            try
            {
                OnInsert(entity, assignIdentifier);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"entity", entity.ToString()}
                };
                throw new DataAccessLayerException("There was a problem during inserting a new entity into the database", ex, errorParameters);
            }
        }        

        public void Update<TEntity>(TEntity entity) 
            where TEntity : class
        {
            try
            {
                OnUpdate(entity);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"entity", entity.ToString()}
                };
                throw new DataAccessLayerException("There was a problem during updating an existing entity in the database", ex, errorParameters);
            }
        }

        public void Delete<TEntity>(object entityId) 
            where TEntity : class
        {
            try
            {
                OnDelete<TEntity>(entityId);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"entityId", entityId.ToString()}
                };
                throw new DataAccessLayerException("There was a problem during deleting an existing entity from the database", ex, errorParameters);
            }
        }

        public void Delete<TEntity>(TEntity entity) 
            where TEntity : class
        {
            try
            {
                OnDelete(entity);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"entity", entity.ToString()}
                };
                throw new DataAccessLayerException("There was a problem during deleting an existing entity from the database", ex, errorParameters);
            }
        }


        protected abstract void OnBeginTransaction();
        protected abstract void OnCommit();
        protected abstract void OnRollBack();

        public void BeginTransaction()
        {
            try
            {
                OnBeginTransaction();
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException("There was a problem during opening transaction", ex);
            }
        }        

        public void Commit()
        {
            try
            {
                OnCommit();
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException("There was a problem during commiting changes", ex);
            }
        }        

        public void RollBack()
        {
            try
            {
                OnRollBack();
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException("There was a problem during rolling back changes", ex);
            }
        }

        public ICollection<TEntity> ExecuteQuery<TEntity>(string query, IDataBaseParameter[] args) 
            where TEntity : class
        {
            try
            {
                var result = OnExecuteQuery<TEntity>(query, args);
                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException("There was a problem during rolling back changes", ex);
            }
        }

        #endregion
    }
}
