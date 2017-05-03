using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories.Smart;
using DotNetCraft.Common.Core.DataAccessLayer.Specifications;
using DotNetCraft.Common.Core.Utils;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.Repositories.Simple;

namespace DotNetCraft.Common.DataAccessLayer.Repositories.Smart
{
    public abstract class BaseSmartRepository<TEntity> : BaseRepository<TEntity>, ISmartRepository<TEntity> 
        where TEntity : class, IEntity
    {
        private readonly IPropertyManager propertyManager;
        private readonly IEntityModelMapper entityModelMapper;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BaseSmartRepository(IPropertyManager propertyManager, IContextSettings contextSettings, IDataContextFactory dataContextFactory, IEntityModelMapper entityModelMapper) : base(contextSettings, dataContextFactory)
        {
            if (propertyManager == null)
                throw new ArgumentNullException(nameof(propertyManager));
            if (entityModelMapper == null)
                throw new ArgumentNullException(nameof(entityModelMapper));

            this.propertyManager = propertyManager;
            this.entityModelMapper = entityModelMapper;            
        }

        #region Implementation of ISmartRepository<TEntity,TModel>        

        /// <summary>
        /// Get model by identifier.
        /// </summary>
        /// <param name="modelId">The model's identifier.</param>
        /// <returns>The model, if it exists.</returns>
        public TModel Get<TModel>(object modelId)
        {
            try
            {
                Type entityType = typeof(TEntity);
                PropertyInfo propertyId = propertyManager.Single(entityType, typeof(KeyAttribute));

                using (IDataContext dataContext = dataContextFactory.CreateDataContext(contextSettings))
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(propertyId.PropertyType);
                    object entityId = converter.ConvertFrom(modelId);
                    TEntity entity = OnGet(entityId, dataContext);
                    TModel model = entityModelMapper.Map<TEntity, TModel>(entity);
                    return model;
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"ModelType", typeof(TModel).ToString()},
                    {"ModelId", modelId.ToString()}
                };
                throw new DataAccessLayerException("There was a problem during retrieving a model from the database", ex, errorParameters);
            }
        }

        /// <summary>
        /// Get all models.
        /// </summary>
        /// <returns>Collection of models.</returns>
        public ICollection<TModel> GetAll<TModel>()
        {
            try
            {
                using (IDataContext dataContext = dataContextFactory.CreateDataContext(contextSettings))
                {
                    ICollection<TEntity> entities = OnGetAll(dataContext);
                    ICollection<TModel> models = entityModelMapper.Map<TEntity, TModel>(entities);
                    return models;
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"ModelType", typeof(TModel).ToString()}
                };
                throw new DataAccessLayerException("There was a problem during retrieving models from the database", ex, errorParameters);
            }
        }

        /// <summary>
        /// Get models by specification.
        /// </summary>
        /// <param name="specification">Some specification.</param>
        /// <returns>Collection of models.</returns>
        public ICollection<TModel> GetBySpecification<TModel>(IDataRequest<TEntity> specification)
        {
            try
            {
                using (IDataContext dataContext = dataContextFactory.CreateDataContext(contextSettings))
                {
                    ICollection<TEntity> entities = OnGetBySpecification(specification, dataContext);
                    ICollection<TModel> models = entityModelMapper.Map<TEntity, TModel>(entities);
                    return models;
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"ModelType", typeof(TModel).ToString()},
                    {"Specification", specification.ToString()},
                };
                throw new DataAccessLayerException("There was a problem during retrieving models from the database", ex, errorParameters);
            }
        }

        #endregion
    }
}
