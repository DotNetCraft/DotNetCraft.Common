using System.Collections.Generic;
using DotNetCraft.Common.Core.DataAccessLayer.Specifications;

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
        TEntity Get(object entityId);

        /// <summary>
        /// Get all entities.
        /// </summary>
        /// <returns>Collection of entities.</returns>
        List<TEntity> GetAll(int? skip = null, int? take = null);

        /// <summary>
        /// Get entities by specification.
        /// </summary>
        /// <param name="specification">Some specification.</param>
        /// <returns>Collection of entities.</returns>
        List<TEntity> GetBySpecification(ISpecification<TEntity> specification, int? skip = null, int? take = null);        
    }
}
