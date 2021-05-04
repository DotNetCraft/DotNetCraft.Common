using System.Collections.Generic;
using System.Linq;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.Utils.Disposal;

namespace DotNetCraft.Common.Core.DataAccessLayer.DataContexts
{
    /// <summary>
    /// Interface shows that object is a data contextWrapper.
    /// </summary>
    public interface IDataContextWrapper: IDisposableObject
    {
        /// <summary>
        /// Unikue key that was assign to this DataContext.
        /// </summary>
        IUniqueKey UniqueKey { get; }

        /// <summary>
        /// Get a set with entities.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <returns>The IQueryable instance.</returns>
        IQueryable<TEntity> Set<TEntity>()
            where TEntity : class;

        void Insert<TEntity>(TEntity entity)
            where TEntity : class;

        void Update<TEntity>(TEntity entity)
            where TEntity : class;

        void Delete<TEntity>(TEntity entity)
           where TEntity : class;

        void BeginTransaction();
        void Commit();
        void RollBack();
    }
}
