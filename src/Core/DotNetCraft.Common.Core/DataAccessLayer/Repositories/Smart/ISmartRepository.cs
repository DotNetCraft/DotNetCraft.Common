using System.Collections.Generic;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories.Simple;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;

namespace DotNetCraft.Common.Core.DataAccessLayer.Repositories.Smart
{
    public interface ISmartRepository<TEntity> : IRepository<TEntity>
    {
        /// <summary>
        /// Get model by identifier.
        /// </summary>
        /// <param name="modelId">The model's identifier.</param>
        /// <returns>The model, if it exists.</returns>
        TModel Get<TModel, TIdentifier>(TIdentifier modelId, IUniqueKey uniqueKey = null);

        /// <summary>
        /// Get all models.
        /// </summary>
        /// <returns>Collection of models.</returns>
        List<TModel> GetAll<TModel>(RepositorySimpleRequest request, IUniqueKey uniqueKey = null);

        /// <summary>
        /// Get models by specification.
        /// </summary>
        /// <param name="specification">Some specification.</param>
        /// <returns>Collection of models.</returns>
        List<TModel> GetBySpecification<TModel>(RepositorySpecificationRequest<TEntity> request, IUniqueKey uniqueKey = null);

        /// <summary>
        /// Insert an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="assignIdentifier">Flag shows that identifier should be assigned</param>
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
