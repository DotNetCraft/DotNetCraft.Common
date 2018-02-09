using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Smart;
using DotNetCraft.Common.Core.Utils.Mapping;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SmartUnitOfWorks
{
    public class SmartUnitOfWork : UnitOfWork, ISmartUnitOfWork
    {
        private readonly IMapperManager mapperManager;

        /// <summary>
        /// Constructor.
        /// </summary>        
        public SmartUnitOfWork(IDataContextWrapper dataContextWrapper, IMapperManager mapperManager) : base(dataContextWrapper)
        {
            if (mapperManager == null)
                throw new ArgumentNullException(nameof(mapperManager));

            this.mapperManager = mapperManager;
        }

        #region Implementation of ISmartUnitOfWork       

        /// <summary>
        /// Insert an model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <return>The model that has been inserted.</return>
        public void Insert<TModel, TEntity>(TModel model)
            where TEntity : class
        {
            try
            {
                TEntity entity = mapperManager.Map<TModel, TEntity>(model);
                OnInsert(entity);                
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"ModelType", typeof(TModel).ToString()},
                    {"Model", model.ToString() }
                };
                throw new DataAccessLayerException("There was a problem during inserting a new model into the database", ex, errorParameters);
            }
        }

        /// <summary>
        /// Update an model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Update<TModel, TEntity>(TModel model)
            where TEntity : class
        {
            try
            {
                TEntity entity = mapperManager.Map<TModel, TEntity>(model);
                OnUpdate(entity);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"ModelType", typeof(TModel).ToString()},
                    {"Model", model.ToString() }
                };
                throw new DataAccessLayerException("There was a problem during inserting a new model into the database", ex, errorParameters);
            }
        }

        /// <summary>
        /// Delete a model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Delete<TModel, TEntity>(TModel model) where TEntity : class
        {
            try
            {
                TEntity entity = mapperManager.Map<TModel, TEntity>(model);
                OnDelete(entity);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"ModelType", typeof(TModel).ToString()},
                    {"Model", model.ToString() }
                };
                throw new DataAccessLayerException("There was a problem during inserting a new model into the database", ex, errorParameters);
            }
        }       

        #endregion
    }
}
