using System;
using System.Collections.Generic;
using System.ComponentModel;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories;
using DotNetCraft.Common.Core.DataAccessLayer.Specofications;
using DotNetCraft.Common.Core.Utils;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;

namespace DotNetCraft.Common.DataAccessLayer.Repositories
{
    public abstract class BaseSmartRepository<TEntity, TEntityId> : BaseRepository<TEntity, TEntityId>, ISmartRepository<TEntity, TEntityId> 
        where TEntity : class, IEntity<TEntityId>
    {
        private readonly IDotNetCraftMapper dotNetCraftMapper;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        protected BaseSmartRepository(IContextSettings contextSettings, IDataContextFactory dataContextFactory, IDotNetCraftMapper dotNetCraftMapper, ICommonLoggerFactory loggerFactory) : base(contextSettings, dataContextFactory, loggerFactory)
        {
            if (dotNetCraftMapper == null)
                throw new ArgumentNullException(nameof(dotNetCraftMapper));

            this.dotNetCraftMapper = dotNetCraftMapper;            
        }

        #region Implementation of ISmartRepository<TEntity,TModel>        

        /// <summary>
        /// Get model by identifier.
        /// </summary>
        /// <param name="modelId">The model's identifier.</param>
        /// <returns>The model, if it exists.</returns>
        public TModel Get<TModel>(int modelId)
        {
            try
            {
                using (IDataContext dataContext = dataContextFactory.CreateDataContext(contextSettings))
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(TEntityId));
                    TEntityId entityId = (TEntityId) converter.ConvertFrom(modelId);
                    TEntity entity = OnGet(entityId, dataContext);
                    TModel model = dotNetCraftMapper.Map<TEntity, TModel>(entity);
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
                    ICollection<TModel> models = dotNetCraftMapper.Map<TEntity, TModel>(entities);
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
        public ICollection<TModel> GetBySpecification<TModel>(ISpecificationRequest<TEntity> specification)
        {
            try
            {
                using (IDataContext dataContext = dataContextFactory.CreateDataContext(contextSettings))
                {
                    ICollection<TEntity> entities = OnGetBySpecification(specification, dataContext);
                    ICollection<TModel> models = dotNetCraftMapper.Map<TEntity, TModel>(entities);
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
