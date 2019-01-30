using System.Collections.Generic;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.Specifications;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;

namespace DotNetCraft.Common.Core.DataAccessLayer.Repositories.Simple
{
    /// <summary>
    /// Interface shows, that the object is a repository for entities.
    /// </summary>
    /// <typeparam name="TEntity">Entity's type.</typeparam>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Get entity by identifier.
        /// </summary>
        /// <param name="entityId">The entity's identifier.</param>
        /// <returns>The entity, if it exists.</returns>
        TEntity Get<TIdentifier>(TIdentifier entityId, IUniqueKey uniqueKey = null);

        /// <summary>
        /// Get all entities.
        /// </summary>
        /// <returns>Collection of entities.</returns>
        List<TEntity> GetAll(RepositorySimpleRequest<TEntity> request, IUniqueKey uniqueKey = null);

        /// <summary>
        /// Get entities by specification.
        /// </summary>
        /// <param name="request">Some specification.</param>
        /// <returns>Collection of entities.</returns>
        List<TEntity> GetBySpecification(RepositorySpecificationRequest<TEntity> request, IUniqueKey uniqueKey = null);

        /// <summary>
        /// Insert an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <return>The entity's identifier that has been inserted.</return>
        void Insert(TEntity entity, IUniqueKey uniqueKey = null);

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>        
        void Update(TEntity entity, IUniqueKey uniqueKey = null);

        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity, IUniqueKey uniqueKey = null);

        /// <summary>
        /// Execute query.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="query">The query</param>
        /// <param name="args">Qeury's arguments (parameters)</param>
        /// <returns>List of entities.</returns>
        ICollection<TEntity> ExecuteQuery(string query, params IDataBaseParameter[] args);

        /// <summary>
        /// Execute query.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="query">The query</param>
        /// <param name="args">Qeury's arguments (parameters)</param>
        /// <returns>List of entities.</returns>
        ICollection<TEntity> ExecuteQuery(IUniqueKey uniqueKey, string query, params IDataBaseParameter[] args);
    }
}
