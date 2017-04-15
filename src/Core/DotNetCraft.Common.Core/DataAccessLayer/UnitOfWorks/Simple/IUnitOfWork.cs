using DotNetCraft.Common.Core.BaseEntities;
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
            where TEntity : class, IEntity;

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>        
        void Update<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        /// <summary>
        /// Delete an entity by it's identifier.
        /// </summary>
        /// <param name="entityId">The entity's identifier.</param>
        void Delete<TEntity>(object entityId)
            where TEntity : class, IEntity;

        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

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
