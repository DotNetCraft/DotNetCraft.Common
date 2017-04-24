using DotNetCraft.Common.Core.BaseEntities;

namespace DotNetCraft.Common.Core.DataAccessLayer.Specifications
{
    /// <summary>
    /// Interface shows that object contains all data for retrieving entities
    /// </summary>
    public interface IDataRequest<TEntity>
        where TEntity: IEntity
    {
        /// <summary>
        /// Amount of elements that will be skipped.
        /// </summary>
        int? Skip { get; set; }

        /// <summary>
        /// Amount of elements that will be taken.
        /// </summary>
        int? Take { get; set; }

        /// <summary>
        /// The specification.
        /// </summary>
        ISpecification<TEntity> Specification { get; }
    }
}
