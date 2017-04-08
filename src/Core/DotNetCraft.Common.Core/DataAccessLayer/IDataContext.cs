using System;
using System.Linq;
using DotNetCraft.Common.Core.BaseEntities;

namespace DotNetCraft.Common.Core.DataAccessLayer
{
    /// <summary>
    /// Interface shows that object is a data context.
    /// </summary>
    public interface IDataContext : IDisposable
    {
        /// <summary>
        /// Get a set with entities.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <returns>The IQueryable instance.</returns>
        IQueryable<TEntity> GetCollectionSet<TEntity>()
            where TEntity : class, IEntity;

        TEntity Insert<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        TEntity Update<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        void BeginTransaction();
        void Commit();
        void RollBack();
    }
}
