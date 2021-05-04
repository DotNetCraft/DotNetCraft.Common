using System;
using System.Collections.Generic;
using AutoMapper;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories.Smart;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.Repositories.Configs;
using DotNetCraft.Common.DataAccessLayer.Repositories.Simple;
using Microsoft.Extensions.Logging;

namespace DotNetCraft.Common.SmartDataAccessLayer.Repositories.Smart
{
    public abstract class BaseSmartRepository<TEntity> : BaseRepository<TEntity>, ISmartRepository<TEntity> 
        where TEntity : class
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BaseSmartRepository(RepositoryConfig repositoryConfig, IDataContextFactory dataContextFactory, IMapper mapper, ILogger<BaseSmartRepository<TEntity>> logger) : base(repositoryConfig, dataContextFactory, logger)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            _mapper = mapper;            
        }

        #region Implementation of ISmartRepository<TEntity,TModel>        

        /// <summary>
        /// Get model by identifier.
        /// </summary>
        /// <param name="modelId">The model's identifier.</param>
        /// <returns>The model, if it exists.</returns>
        public TModel Get<TModel, TIdentifier>(TIdentifier modelId, IUniqueKey uniqueKey = null)
        {
            try
            {
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext(uniqueKey))
                {
                    TIdentifier entityId = modelId;
                    TEntity entity = OnGet(entityId, dataContextWrapper);
                    TModel model = _mapper.Map<TEntity, TModel>(entity);
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
        public List<TModel> GetAll<TModel>(RepositorySimpleRequest request, IUniqueKey uniqueKey = null)
        {
            try
            {
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext(uniqueKey))
                {
                    List<TEntity> entities = OnGetAll(dataContextWrapper, request);

                    if (entities.Count == 0)
                        return new List<TModel>();

                    List<TModel> models = _mapper.Map<List<TEntity>, List<TModel>>(entities);
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
        public List<TModel> GetBySpecification<TModel>(RepositorySpecificationRequest<TEntity> request, IUniqueKey uniqueKey = null)
        {
            try
            {
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext(uniqueKey))
                {
                    List<TEntity> entities = OnGetBySpecification(dataContextWrapper, request);

                    if (entities.Count == 0 )
                        return new List<TModel>();

                    List<TModel> models = _mapper.Map<List<TEntity>, List<TModel>>(entities);
                    return models;
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"ModelType", typeof(TModel).ToString()},
                    {"Request", request.ToString()},
                };
                throw new DataAccessLayerException("There was a problem during retrieving models from the database", ex, errorParameters);
            }
        }

        #endregion

        #region Implementation of ISmartUnitOfWork       

        /// <summary>
        /// Insert an model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <return>The model that has been inserted.</return>
        public void Insert<TModel>(TModel model, IUniqueKey uniqueKey = null)
        {
            try
            {
                TEntity entity = _mapper.Map<TModel, TEntity>(model);
                Insert(entity, uniqueKey);
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
        public void Update<TModel>(TModel model, IUniqueKey uniqueKey = null)
        {
            try
            {
                TEntity entity = _mapper.Map<TModel, TEntity>(model);
                Update(entity, uniqueKey);
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
        public void Delete<TModel>(TModel model, IUniqueKey uniqueKey = null)
        {
            try
            {
                TEntity entity = _mapper.Map<TModel, TEntity>(model);
                Delete(entity, uniqueKey);
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
