using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories.Smart;
using DotNetCraft.Common.Core.DataAccessLayer.Specifications;
using DotNetCraft.Common.Core.Utils.Mapping;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.Repositories.Simple;
using DotNetCraft.Common.Utils.ReflectionExtensions;

namespace DotNetCraft.Common.DataAccessLayer.Repositories.Smart
{
    public abstract class BaseSmartRepository<TEntity> : BaseRepository<TEntity>, ISmartRepository<TEntity> 
        where TEntity : class
    {
        private readonly IReflectionManager reflectionManager = ReflectionManager.Manager;
        private readonly IMapperManager mapperManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BaseSmartRepository(IDataContextFactory dataContextFactory, IMapperManager mapperManager) : base(dataContextFactory)
        {
            if (mapperManager == null)
                throw new ArgumentNullException(nameof(mapperManager));

            this.mapperManager = mapperManager;            
        }

        #region Implementation of ISmartRepository<TEntity,TModel>        

        /// <summary>
        /// Get model by identifier.
        /// </summary>
        /// <param name="modelId">The model's identifier.</param>
        /// <returns>The model, if it exists.</returns>
        public TModel Get<TModel, TIdentifier>(TIdentifier modelId)
        {
            try
            {
                Type entityType = typeof(TEntity);
                var propertyId = reflectionManager.Single(entityType, typeof(KeyAttribute));

                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext())
                {
                    //TypeConverter converter = TypeDescriptor.GetConverter(propertyId.PropertyType);
                    //TIdentifier entityId = converter.ConvertFrom(modelId);
                    TIdentifier entityId = modelId;
                    TEntity entity = OnGet(entityId, dataContextWrapper);
                    TModel model = mapperManager.Map<TEntity, TModel>(entity);
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
        public List<TModel> GetAll<TModel>(int? skip = null, int? take = null)
        {
            try
            {
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext())
                {
                    List<TEntity> entities = OnGetAll(dataContextWrapper, skip, take);

                    if (entities.Count == 0)
                        return new List<TModel>();

                    List<TModel> models = mapperManager.Map<List<TEntity>, List<TModel>>(entities);
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
        public List<TModel> GetBySpecification<TModel>(ISpecification<TEntity> specification, int? skip = null, int? take = null)
        {
            try
            {
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext())
                {
                    List<TEntity> entities = OnGetBySpecification(specification, dataContextWrapper, skip, take);

                    if (entities.Count == 0 )
                        return new List<TModel>();

                    List<TModel> models = mapperManager.Map<List<TEntity>, List<TModel>>(entities);
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
