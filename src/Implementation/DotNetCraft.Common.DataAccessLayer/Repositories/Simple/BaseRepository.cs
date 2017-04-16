﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetCraft.Common.Core.Attributes;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories;
using DotNetCraft.Common.Core.DataAccessLayer.Specofications;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.Extentions;
using DotNetCraft.Common.Utils.Logging;

namespace DotNetCraft.Common.DataAccessLayer.Repositories.Simple
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected readonly IContextSettings contextSettings;

        protected readonly IDataContextFactory dataContextFactory;

        protected ICommonLogger logger = LogManager.GetCurrentClassLogger();

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

        /// <summary>
        /// Occurs when model by identifier from the repository is needed.
        /// </summary>
        /// <param name="entityId">The model's identifier.</param>
        /// <param name="dataContext">The IDataContext instance</param>
        /// <returns>The model, if it exists.</returns>
        protected virtual TEntity OnGet(object entityId, IDataContext dataContext)
        {
            PropertyInfo propertyId = GeIdentifiertPropertyInfo();

            IQueryable<TEntity> collection = dataContext.GetCollectionSet<TEntity>();
            collection = collection.Simplified(propertyId, entityId);
            TEntity result = collection.SingleOrDefault();

            if (result == null)
                throw new EntityNotFoundException("There is no element by " + entityId);

            return result;
        }

        private static PropertyInfo GeIdentifiertPropertyInfo()
        {
            Type entityType = typeof(TEntity);
            PropertyInfo[] propertyInfos = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo propertyId = null;
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                var attributes = Attribute.GetCustomAttributes(propertyInfo, typeof(IdentifierAttribute));
                if (attributes.Length > 0)
                {
                    propertyId = propertyInfo;
                    break;
                }
            }

            if (propertyId == null)
                throw new DataAccessLayerException("There is no identifier for " + entityType);
            return propertyId;
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
        /// <param name="specificationRequest">Some specification.</param>
        /// <param name="dataContext">The IDataContext instance</param>
        /// <returns>Collection of models.</returns>
        protected virtual ICollection<TEntity> OnGetBySpecification(ISpecificationRequest<TEntity> specificationRequest, IDataContext dataContext)
        {
            IQueryable<TEntity> collection = dataContext.GetCollectionSet<TEntity>();
            var specification = specificationRequest.Specification;
            collection = collection.Where(specification.IsSatisfiedBy());

            if (specificationRequest.Skip.HasValue || specificationRequest.Take.HasValue)
            {
                PropertyInfo propertyId = GeIdentifiertPropertyInfo();
                collection = collection.OrderByProperty(propertyId.Name);
            }

            if (specificationRequest.Skip.HasValue)
                collection = collection.Skip(specificationRequest.Skip.Value);

            if (specificationRequest.Take.HasValue)
                collection = collection.Take(specificationRequest.Take.Value);

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
        public ICollection<TEntity> GetBySpecification(ISpecificationRequest<TEntity> specification)
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
