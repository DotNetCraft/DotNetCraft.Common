using System;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.Utils.Disposal;

namespace DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks
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
        /// <return>The entity that has been inserted.</return>
        TEntity Insert<TEntity>(TEntity entity)
            where TEntity : class, IEntity; //TODO: Return ID

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>        
        void Update<TEntity>(TEntity entity)
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
