using System.Collections.Generic;
using DotNetCraft.Common.Core.Utils.Disposal;

namespace DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple
{
    /// <summary>
    /// Pattern Unit of Work.
    /// </summary>
    public interface IUnitOfWork : IDisposableObject
    {
        /// <summary>
        /// Flag shows that transaction opened.
        /// </summary>
        bool IsTransactionOpened { get; }

        /// <summary>
        /// Insert an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="assignIdentifier">Flag shows that identifier should be assigned</param>
        /// <return>The entity's identifier that has been inserted.</return>
        void Insert<TEntity>(TEntity entity, bool assignIdentifier = true)
            where TEntity : class;

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>        
        void Update<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// Execute query.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="query">The query</param>
        /// <param name="args">Qeury's arguments (parameters)</param>
        /// <returns>List of entities.</returns>
        ICollection<TEntity> ExecuteQuery<TEntity>(string query, params IDataBaseParameter[] args)
            where TEntity : class;

        /// <summary>
        /// Commit transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        void Rollback();
    }
}
