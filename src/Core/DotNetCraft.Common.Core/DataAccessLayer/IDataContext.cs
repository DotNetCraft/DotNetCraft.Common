using System.Collections.Generic;
using System.Linq;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.Utils.Disposal;

namespace DotNetCraft.Common.Core.DataAccessLayer
{
    /// <summary>
    /// Interface shows that object is a data context.
    /// </summary>
    public interface IDataContext : IDisposableObject
    {
        /// <summary>
        /// Get a set with entities.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <returns>The IQueryable instance.</returns>
        IQueryable<TEntity> GetCollectionSet<TEntity>()
            where TEntity : class, IEntity;

        void Insert<TEntity>(TEntity entity, bool assignIdentifier = true)
            where TEntity : class, IEntity;

        void Update<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        void Delete<TEntity>(object entityId)
            where TEntity : class, IEntity;

        void Delete<TEntity>(TEntity entity)
           where TEntity : class, IEntity;

        void BeginTransaction();
        void Commit();
        void RollBack();

        ICollection<TEntity> ExecuteQuery<TEntity>(string query, DataBaseParameter[] args) 
            where TEntity : class, IEntity;
    }
}
