using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories.Simple;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.Extentions;
using DotNetCraft.Common.DataAccessLayer.Repositories.Configs;
using Microsoft.Extensions.Logging;

namespace DotNetCraft.Common.DataAccessLayer.Repositories.Simple
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        #region Fields...
        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<BaseRepository<TEntity>> _logger;

        private readonly RepositoryConfig repositoryConfig;

        /// <summary>
        /// The data contextWrapper factory.
        /// </summary>
        protected readonly IDataContextFactory dataContextFactory;

        private readonly PropertyInfo propertyId;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataContextFactory">The data contextWrapper factory.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataContextFactory"/> is <see langword="null"/></exception>
        protected BaseRepository(RepositoryConfig repositoryConfig, IDataContextFactory dataContextFactory, ILogger<BaseRepository<TEntity>> logger)
        {
            if (repositoryConfig == null)
                throw new ArgumentNullException(nameof(repositoryConfig));
            if (dataContextFactory == null)
                throw new ArgumentNullException(nameof(dataContextFactory));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this.repositoryConfig = repositoryConfig;
            this.dataContextFactory = dataContextFactory;
            _logger = logger;

            propertyId = GetIdentifiertPropertyInfo();
        }

        #region Virtual methods...

        private PropertyInfo GetIdentifiertPropertyInfo()
        {
            _logger.LogTrace("Searching for identifier's property (Config: {0})...", repositoryConfig);
            PropertyInfo[] propertyInfos = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (repositoryConfig.UseKeyAttribute)
                {
                    var attr = Attribute.GetCustomAttribute(propertyInfo, typeof(KeyAttribute));
                    if (attr != null)
                    {
                        _logger.LogTrace("The property has been found by KeyAttribute.");
                        return propertyInfo;
                    }
                }

                if (propertyInfo.Name.ToLower() == repositoryConfig.IdentifierPropertyName)
                {
                    _logger.LogTrace("The property has been found by IdentifierPropertyName.");
                    return propertyInfo;
                }
            }

            _logger.LogError("Entity doesn't have any identifiers.");
            throw new DataAccessLayerException("There is no identifier for " + typeof(TEntity));
        }

        private List<TEntity> LoadCollection(IQueryable<TEntity> collection, RepositorySimpleRequest request = null)
        {
            if (request != null)
            {
                _logger.LogTrace("Preparing query ({0})...", request);
                int? skip = request.Skip;
                int? take = request.Take;
                if (skip.HasValue || take.HasValue)
                {
                    string orderBy;
                    if (string.IsNullOrWhiteSpace(request.OrderBy))
                        orderBy = propertyId.Name;
                    else
                        orderBy = request.OrderBy;
                    collection = collection.OrderByProperty(orderBy);
                }               

                if (skip.HasValue)
                    collection = collection.Skip(skip.Value);

                if (take.HasValue)
                    collection = collection.Take(take.Value);
                _logger.LogTrace("The query has been created.");
            }

            List<TEntity> result = collection.ToList();
            _logger.LogTrace("Collection has been loaded");
            return result;
        }

        /// <summary>
        /// Occurs when model by identifier from the repository is needed.
        /// </summary>
        /// <param name="entityId">The model's identifier.</param>
        /// <param name="dataContextWrapper">The IDataContextWrapper instance</param>
        /// <returns>The model, if it exists.</returns>
        /// <exception cref="EntityNotFoundException">Entity has not been found by the entity identifier.</exception>
        protected virtual TEntity OnGet<TIdentifier>(TIdentifier entityId, IDataContextWrapper dataContextWrapper)
        {
            IQueryable<TEntity> collection = dataContextWrapper.Set<TEntity>();
            collection = collection.Simplified(propertyId, entityId);
            TEntity result = collection.SingleOrDefault();

            if (result == null)
                throw new EntityNotFoundException("There is no entity by " + entityId);

            return result;
        }

        /// <summary>
        /// Occurs when all models are required.
        /// </summary>
        /// <param name="dataContextWrapper">The IDataContextWrapper instance</param>
        /// <returns>Collection of models.</returns>
        protected virtual List<TEntity> OnGetAll(IDataContextWrapper dataContextWrapper, RepositorySimpleRequest request)
        {
            IQueryable<TEntity> collection = dataContextWrapper.Set<TEntity>();
            List<TEntity> result = LoadCollection(collection, request);
            return result;
        }

        /// <summary>
        /// Occurs when all models according to specification are required.
        /// </summary>
        /// <param name="specification">Some specification.</param>
        /// <param name="dataContextWrapper">The IDataContextWrapper instance</param>
        /// <returns>Collection of models.</returns>
        protected virtual List<TEntity> OnGetBySpecification(IDataContextWrapper dataContextWrapper, RepositorySpecificationRequest<TEntity> request)
        {
            IQueryable<TEntity> collection = dataContextWrapper.Set<TEntity>();
            var specification = request.Specification;
            collection = collection.Where(specification.IsSatisfiedBy());
            List<TEntity> result = LoadCollection(collection, request);
            return result;
        }

        #endregion

        #region Implementation of IRepository<TEntity>

        #region Get methods...
        /// <summary>
        /// Get entity by identifier.
        /// </summary>
        /// <param name="entityId">The entity's identifier.</param>
        /// <returns>The entity, if it exists.</returns>
        public TEntity Get<TIdentifier>(TIdentifier entityId, IUniqueKey uniqueKey = null)
        {
            try
            {
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext(uniqueKey))
                {
                    return OnGet(entityId, dataContextWrapper);
                }
            }
            catch (EntityNotFoundException entityNotFoundException)
            {
                _logger.LogError(entityNotFoundException, "Entity not found");
                throw;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"EntityId", entityId.ToString()}
                };
                throw new DataAccessLayerException("There was a problem during retrieving a entity from the database", ex, errorParameters);
            }
        }

        /// <summary>
        /// Get all entities.
        /// </summary>
        /// <returns>Collection of entities.</returns>
        public List<TEntity> GetAll(RepositorySimpleRequest request, IUniqueKey uniqueKey = null)
        {
            try
            {
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext(uniqueKey))
                {
                    return OnGetAll(dataContextWrapper, request);
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                };
                throw new DataAccessLayerException("There was a problem during retrieving entities from the database", ex, errorParameters);
            }
        }

        /// <summary>
        /// Get entities by specification.
        /// </summary>
        /// <param name="specification">Some specification.</param>
        /// <returns>Collection of entities.</returns>
        public List<TEntity> GetBySpecification(RepositorySpecificationRequest<TEntity> request, IUniqueKey uniqueKey = null)
        {
            try
            {
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext())
                {
                    return OnGetBySpecification(dataContextWrapper, request);
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Request", request.ToString()},
                };
                throw new DataAccessLayerException("There was a problem during retrieving entities from the database by specification", ex, errorParameters);
            }
        }
        #endregion

        #region Insert/Update/Delete methods...

        #region Virtual methods: OnInsert, OnUpdate and OnDelete

        protected virtual void OnInsert(IDataContextWrapper dataContextWrapper, TEntity entity)
        {
            _logger.LogTrace("Inserting {0} into the data contextWrapper...", entity);
            dataContextWrapper.Insert(entity);
            _logger.LogTrace("The entity has been inserted.");
        }

        protected virtual void OnUpdate(IDataContextWrapper dataContextWrapper, TEntity entity)
        {
            _logger.LogTrace("Updating {0} in the data contextWrapper...", entity);
            dataContextWrapper.Update(entity);
            _logger.LogTrace("The entity has been updated.");
        }

        protected virtual void OnDelete(IDataContextWrapper dataContextWrapper, TEntity entity)
        {
            _logger.LogTrace("Deliting {0} from the data contextWrapper...", entity);
            dataContextWrapper.Delete(entity);
            _logger.LogTrace("The entity has been deleted.");
        }

        #endregion

        /// <summary>
        /// Insert an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <return>The entity that has been inserted.</return>
        /// <exception cref="UnitOfWorkException">There was a problem during inserting a new entity into the database..</exception>
        public void Insert(TEntity entity, IUniqueKey uniqueKey = null)
        {
            try
            {
                _logger.LogDebug("Inserting {0} into the data contextWrapper...", entity);
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext(uniqueKey))
                {
                    OnInsert(dataContextWrapper, entity);
                }
                _logger.LogDebug("The entity has been inserted.");
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Entity", entity.ToString()}
                };
                UnitOfWorkException unitOfWorkException = new UnitOfWorkException("There was a problem during inserting a new entity into the database.", ex, errorParameters);
                _logger.LogError(unitOfWorkException, unitOfWorkException.ToString());
                throw unitOfWorkException;
            }
        }

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>        
        public void Update(TEntity entity, IUniqueKey uniqueKey = null)
        {
            try
            {
                _logger.LogDebug("Updating {0} in the data contextWrapper...", entity);
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext(uniqueKey))
                {
                    OnUpdate(dataContextWrapper, entity);
                }
                _logger.LogDebug("The entity has been updated.");
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Entity", entity.ToString()}
                };
                UnitOfWorkException unitOfWorkException = new UnitOfWorkException("There was a problem during updating an existing entity in the database.", ex, errorParameters);
                _logger.LogError(unitOfWorkException, unitOfWorkException.ToString());
                throw unitOfWorkException;
            }
        }

        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(TEntity entity, IUniqueKey uniqueKey = null)
        {
            try
            {
                _logger.LogDebug("Deleting {0} from the data contextWrapper...", entity);
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext(uniqueKey))
                {
                    OnDelete(dataContextWrapper, entity);
                }
                _logger.LogDebug("The entity has been deleted.");
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Entity", entity.ToString()}
                };
                UnitOfWorkException unitOfWorkException = new UnitOfWorkException("There was a problem during deleting an existing entity from the database.", ex, errorParameters);
                _logger.LogError(unitOfWorkException, unitOfWorkException.ToString());
                throw unitOfWorkException;
            }
        }

        #endregion

        #endregion
    }
}
