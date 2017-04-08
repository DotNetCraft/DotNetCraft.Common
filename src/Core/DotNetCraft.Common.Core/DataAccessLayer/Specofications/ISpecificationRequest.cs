using DotNetCraft.Common.Core.BaseEntities;

namespace DotNetCraft.Common.Core.DataAccessLayer.Specofications
{
    /// <summary>
    /// Interface shows that object contains all data for retrieving entities
    /// </summary>
    public interface ISpecificationRequest<TEntity>
        where TEntity: IEntity
    {
        /// <summary>
        /// The specification.
        /// </summary>
        ISpecification<TEntity> Specification { get; }
    }
}
