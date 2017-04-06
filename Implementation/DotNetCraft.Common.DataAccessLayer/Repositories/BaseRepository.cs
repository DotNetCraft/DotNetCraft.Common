using System;
using System.Collections.Generic;
using System.Linq;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories;
using DotNetCraft.Common.Core.DataAccessLayer.Specofications;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;

namespace DotNetCraft.Common.DataAccessLayer.Repositories
{
    public abstract class BaseRepository<TEntity, TEntityId> : BaseLoggerObject, IRepository<TEntity, TEntityId>
        where TEntity : class, IEntity<TEntityId>
    {
        protected readonly IDataBaseSettings dataBaseSettings;

        protected readonly IDataContextFactory dataContextFactory;

        protected BaseRepository(IDataBaseSettings dataBaseSettings, IDataContextFactory dataContextFactory, ICommonLoggerFactory loggerFactory): base(loggerFactory)
        {
            if (dataBaseSettings == null)
                throw new ArgumentNullException(nameof(dataBaseSettings));
            if (dataContextFactory == null)
                throw new ArgumentNullException(nameof(dataContextFactory));

            this.dataBaseSettings = dataBaseSettings;
            this.dataContextFactory = dataContextFactory;
        }

        #region Virtual methods...

        /// <summary>
        /// Occurs when model by identifier from the repository is needed.
        /// </summary>
        /// <param name="entityId">The model's identifier.</param>
        /// <param name="dataContext">The IDataContext instance</param>
        /// <returns>The model, if it exists.</returns>
        protected virtual TEntity OnGet(TEntityId entityId, IDataContext dataContext)
        {
            IQueryable<TEntity> collection = dataContext.GetCollectionSet<TEntity>();
            var result = collection.Single(x => x.Id.Equals(entityId));
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
        /// <param name="specificationRequest">Some specification.</param>
        /// <param name="dataContext">The IDataContext instance</param>
        /// <returns>Collection of models.</returns>
        protected virtual ICollection<TEntity> OnGetBySpecification(ISpecificationRequest<TEntity> specificationRequest, IDataContext dataContext)
        {
            IQueryable<TEntity> collection = dataContext.GetCollectionSet<TEntity>();
            ICollection<TEntity> result = collection.Where(specificationRequest.Specification.IsSatisfiedBy()).ToList();
            return result;
        }
        #endregion

        #region Implementation of IRepository<TEntity>

        /// <summary>
        /// Get entity by identifier.
        /// </summary>
        /// <param name="entityId">The entity's identifier.</param>
        /// <returns>The entity, if it exists.</returns>
        public TEntity Get(TEntityId entityId)
        {
            try
            {
                using (IDataContext dataContext = dataContextFactory.CreateDataContext(dataBaseSettings))
                {
                    return OnGet(entityId, dataContext);
                }
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
                using (IDataContext dataContext = dataContextFactory.CreateDataContext(dataBaseSettings))
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
                using (IDataContext dataContext = dataContextFactory.CreateDataContext(dataBaseSettings))
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
