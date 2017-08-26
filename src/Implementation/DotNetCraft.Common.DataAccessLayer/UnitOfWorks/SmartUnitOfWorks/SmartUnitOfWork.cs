using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Smart;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SmartUnitOfWorks
{
    public class SmartUnitOfWork : UnitOfWork, ISmartUnitOfWork
    {
        private readonly IEntityModelMapper entityModelMapper;

        /// <summary>
        /// Constructor.
        /// </summary>        
        public SmartUnitOfWork(IDataContext dataContext, IEntityModelMapper entityModelMapper) : base(dataContext)
        {
            if (entityModelMapper == null)
                throw new ArgumentNullException(nameof(entityModelMapper));

            this.entityModelMapper = entityModelMapper;
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
                TEntity entity = entityModelMapper.Map<TEntity, TModel>(model);
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete an model.
        /// </summary>
        /// <param name="modelId">The model's identifier.</param>
        public void Delete<TModel, TEntity>(object modelId)
            where TEntity : class
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
