using System;
using System.Linq.Expressions;
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
        ISpecification<TEntity> Specification { get; set; }
    }
}
