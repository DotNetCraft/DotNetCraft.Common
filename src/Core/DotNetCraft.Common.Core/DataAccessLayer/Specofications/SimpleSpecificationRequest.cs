using DotNetCraft.Common.Core.BaseEntities;

namespace DotNetCraft.Common.Core.DataAccessLayer.Specofications
{
    public class SimpleSpecificationRequest<TEntity> : ISpecificationRequest<TEntity> 
        where TEntity : IEntity
    {
        #region Implementation of ISpecificationRequest<TEntity>

        /// <summary>
        /// Amount of elements that will be skipped.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Amount of elements that will be taken.
        /// </summary>
        public int? Take { get; set; }

        /// <summary>
        /// The specification.
        /// </summary>
        public ISpecification<TEntity> Specification { get; set; }

        #endregion
    }
}
