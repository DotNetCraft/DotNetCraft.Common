using DotNetCraft.Common.Core.BaseEntities;

namespace DotNetCraft.Common.Core.DataAccessLayer.Specofications
{
    public class SimpleSpecificationRequest<TEntity> : ISpecificationRequest<TEntity> 
        where TEntity : IEntity
    {
        #region Implementation of ISpecificationRequest<TEntity>

        /// <summary>
        /// The specification.
        /// </summary>
        public ISpecification<TEntity> Specification { get; set; }

        #endregion
    }
}
