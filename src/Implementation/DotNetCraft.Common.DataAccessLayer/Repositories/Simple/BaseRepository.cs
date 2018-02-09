using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories.Simple;
using DotNetCraft.Common.Core.DataAccessLayer.Specifications;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions.Defenitions;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.Extentions;
using DotNetCraft.Common.Utils.Logging;
using DotNetCraft.Common.Utils.ReflectionExtensions;

namespace DotNetCraft.Common.DataAccessLayer.Repositories.Simple
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        #region Fields...
        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ICommonLogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The IPropertyManager instance.
        /// </summary>
        protected readonly IReflectionManager reflectionManager = ReflectionManager.Manager;

        /// <summary>
        /// The data contextWrapper factory.
        /// </summary>
        protected readonly IDataContextFactory dataContextFactory;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataContextFactory">The data contextWrapper factory.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataContextFactory"/> is <see langword="null"/></exception>
        protected BaseRepository(IDataContextFactory dataContextFactory)
        {
            if (dataContextFactory == null)
                throw new ArgumentNullException(nameof(dataContextFactory));

            this.dataContextFactory = dataContextFactory;
        }

        #region Virtual methods...

        private PropertyInfo GeIdentifiertPropertyInfo()
        {
            PropertyDefinition propertyId = reflectionManager.SingleOrDefault<TEntity>(typeof(KeyAttribute));
            if (propertyId == null)
                throw new DataAccessLayerException("There is no identifier for " + typeof(TEntity));

            PropertyInfo result = propertyId.PropertyInfo;
            return result;
        }

        private List<TEntity> LoadCollection(int? skip, int? take, IQueryable<TEntity> collection)
        {
            if (skip.HasValue || take.HasValue)
            {
                PropertyInfo propertyId = GeIdentifiertPropertyInfo();
                collection = collection.OrderByProperty(propertyId.Name);
            }

            if (skip.HasValue)
                collection = collection.Skip(skip.Value);

            if (take.HasValue)
                collection = collection.Take(take.Value);

            List<TEntity> result = collection.ToList();
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
            PropertyInfo propertyId = GeIdentifiertPropertyInfo();

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
        protected virtual List<TEntity> OnGetAll(IDataContextWrapper dataContextWrapper, int? skip, int? take)
        {
            IQueryable<TEntity> collection = dataContextWrapper.Set<TEntity>();
            List<TEntity> result = LoadCollection(skip, take, collection);
            return result;
        }

        /// <summary>
        /// Occurs when all models according to specification are required.
        /// </summary>
        /// <param name="specification">Some specification.</param>
        /// <param name="dataContextWrapper">The IDataContextWrapper instance</param>
        /// <returns>Collection of models.</returns>
        protected virtual List<TEntity> OnGetBySpecification(ISpecification<TEntity> specification, IDataContextWrapper dataContextWrapper, int? skip ,int? take)
        {
            IQueryable<TEntity> collection = dataContextWrapper.Set<TEntity>();
            collection = collection.Where(specification.IsSatisfiedBy());

            List<TEntity> result = LoadCollection(skip, take, collection);
            return result;
        }        

        #endregion

        #region Implementation of IRepository<TEntity>

        /// <summary>
        /// Get entity by identifier.
        /// </summary>
        /// <param name="entityId">The entity's identifier.</param>
        /// <returns>The entity, if it exists.</returns>
        public TEntity Get<TIdentifier>(TIdentifier entityId)
        {
            try
            {
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext())
                {
                    return OnGet(entityId, dataContextWrapper);
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
        public List<TEntity> GetAll(int? skip = null, int? take = null)
        {
            try
            {
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext())
                {
                    return OnGetAll(dataContextWrapper, skip, take);
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
        public List<TEntity> GetBySpecification(ISpecification<TEntity> specification, int? skip = null, int? take = null)
        {
            try
            {
                using (IDataContextWrapper dataContextWrapper = dataContextFactory.CreateDataContext())
                {
                    return OnGetBySpecification(specification, dataContextWrapper, skip, take);
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
