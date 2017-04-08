using System.Collections.Generic;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer.Specofications;

namespace DotNetCraft.Common.Core.DataAccessLayer.Repositories
{
    /// <summary>
    /// Interface shows, that the object is a repository for entities.
    /// </summary>
    /// <typeparam name="TEntity">Entity's type.</typeparam>
    public interface IRepository<TEntity, TEntityId>
        where TEntity : IEntity
    {
        /// <summary>
        /// Get entity by identifier.
        /// </summary>
        /// <param name="entityId">The entity's identifier.</param>
        /// <returns>The entity, if it exists.</returns>
        TEntity Get(TEntityId entityId);

        /// <summary>
        /// Get all entities.
        /// </summary>
        /// <returns>Collection of entities.</returns>
        ICollection<TEntity> GetAll();

        /// <summary>
        /// Get entities by specification.
        /// </summary>
        /// <param name="specification">Some specification.</param>
        /// <returns>Collection of entities.</returns>
        ICollection<TEntity> GetBySpecification(ISpecificationRequest<TEntity> specification);        
    }
}
