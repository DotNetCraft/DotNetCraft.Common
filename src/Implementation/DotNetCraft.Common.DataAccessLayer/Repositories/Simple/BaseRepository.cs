using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories.Simple;
using DotNetCraft.Common.Core.DataAccessLayer.Specifications;
using DotNetCraft.Common.Core.Utils;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions.Defenitions;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.Extentions;
using DotNetCraft.Common.Utils;
using DotNetCraft.Common.Utils.Logging;
using DotNetCraft.Common.Utils.ReflectionExtensions;

namespace DotNetCraft.Common.DataAccessLayer.Repositories.Simple
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        #region Fields...
        /// <summary>
        /// The logger.
        /// </summary>
        protected static ICommonLogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The IPropertyManager instance.
        /// </summary>
        protected static IReflectionManager reflectionManager = ReflectionManager.Manager;

        /// <summary>
        /// Context settings.
        /// </summary>
        protected readonly IContextSettings contextSettings;

        /// <summary>
        /// The data context factory.
        /// </summary>
        protected readonly IDataContextFactory dataContextFactory;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contextSettings">Context settings.</param>
        /// <param name="dataContextFactory">The data context factory.</param>
        /// <exception cref="ArgumentNullException"><paramref name="contextSettings"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentNullException"><paramref name="dataContextFactory"/> is <see langword="null"/></exception>
        protected BaseRepository(IContextSettings contextSettings, IDataContextFactory dataContextFactory)
        {
            if (contextSettings == null)
                throw new ArgumentNullException(nameof(contextSettings));
            if (dataContextFactory == null)
                throw new ArgumentNullException(nameof(dataContextFactory));

            this.contextSettings = contextSettings;
            this.dataContextFactory = dataContextFactory;
        }

        #region Virtual methods...

        private static PropertyInfo GeIdentifiertPropertyInfo()
        {
            PropertyDefinition propertyId = reflectionManager.SingleOrDefault<TEntity>(typeof(KeyAttribute));
            if (propertyId == null)
                throw new DataAccessLayerException("There is no identifier for " + typeof(TEntity));

            PropertyInfo result = propertyId.PropertyInfo;
            return result;
        }

        /// <summary>
        /// Occurs when model by identifier from the repository is needed.
        /// </summary>
        /// <param name="entityId">The model's identifier.</param>
        /// <param name="dataContext">The IDataContext instance</param>
        /// <returns>The model, if it exists.</returns>
        /// <exception cref="EntityNotFoundException">Entity has not been found by the entity identifier.</exception>
        protected virtual TEntity OnGet(object entityId, IDataContext dataContext)
        {
            PropertyInfo propertyId = GeIdentifiertPropertyInfo();

            IQueryable<TEntity> collection = dataContext.GetCollectionSet<TEntity>();
            collection = collection.Simplified(propertyId, entityId);
            TEntity result = collection.SingleOrDefault();

            if (result == null)
                throw new EntityNotFoundException("There is no entity by " + entityId);

            return result;
        }

        /// <summary>
        /// Occurs when all models are required.
        /// </summary>
        /// <param name="dataContext">The IDataContext instance</param>
        /// <returns>Collection of models.</returns>
        protected virtual ICollection<TEntity> OnGetAll(IDataContext dataContext)
        {
            IQueryable<TEntity> collection = dataContext.GetCollectionSet<TEntity>();
            ICollection<TEntity> result = collection.ToList();
            return result;
        }

        /// <summary>
        /// Occurs when all models according to specification are required.
        /// </summary>
        /// <param name="dataRequest">Some specification.</param>
        /// <param name="dataContext">The IDataContext instance</param>
        /// <returns>Collection of models.</returns>
        protected virtual ICollection<TEntity> OnGetBySpecification(IDataRequest<TEntity> dataRequest, IDataContext dataContext)
        {
            IQueryable<TEntity> collection = dataContext.GetCollectionSet<TEntity>();
            var specification = dataRequest.Specification;
            collection = collection.Where(specification.IsSatisfiedBy());

            if (dataRequest.Skip.HasValue || dataRequest.Take.HasValue)
            {
                PropertyInfo propertyId = GeIdentifiertPropertyInfo();
                collection = collection.OrderByProperty(propertyId.Name);
            }

            if (dataRequest.Skip.HasValue)
                collection = collection.Skip(dataRequest.Skip.Value);

            if (dataRequest.Take.HasValue)
                collection = collection.Take(dataRequest.Take.Value);

            ICollection<TEntity> result = collection.ToList();
            return result;
        }
        #endregion

        #region Implementation of IRepository<TEntity>

        /// <summary>
        /// Get entity by identifier.
        /// </summary>
        /// <param name="entityId">The entity's identifier.</param>
        /// <returns>The entity, if it exists.</returns>
        public TEntity Get(object entityId)
        {
            try
            {
                using (IDataContext dataContext = dataContextFactory.CreateDataContext(contextSettings))
                {
                    return OnGet(entityId, dataContext);
                }
            }
            catch (EntityNotFoundException entityNotFoundException)
            {
                logger.Error(entityNotFoundException, "Entity not found");
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
        public ICollection<TEntity> GetAll()
        {
            try
            {
                using (IDataContext dataContext = dataContextFactory.CreateDataContext(contextSettings))
                {
                    return OnGetAll(dataContext);
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
        public ICollection<TEntity> GetBySpecification(IDataRequest<TEntity> specification)
        {
            try
            {
                using (IDataContext dataContext = dataContextFactory.CreateDataContext(contextSettings))
                {
                    return OnGetBySpecification(specification, dataContext);
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"Specification", specification.ToString()},
                };
                throw new DataAccessLayerException("There was a problem during retrieving entities from the database by specification", ex, errorParameters);
            }
        }

        #endregion        
    }
}
