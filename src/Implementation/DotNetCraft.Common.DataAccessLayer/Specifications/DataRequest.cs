using System;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer.Specifications;

namespace DotNetCraft.Common.DataAccessLayer.Specifications
{
    /// <summary>
    /// Simple data request.
    /// </summary>
    /// <typeparam name="TEntity">Type of Entity</typeparam>
    public class DataRequest<TEntity> : IDataRequest<TEntity> 
        where TEntity : IEntity
    {
        #region Implementation of IDataRequest<TEntity>

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
        public ISpecification<TEntity> Specification { get; private set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="specification">The specification</param>
        public DataRequest(ISpecification<TEntity> specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));

            Specification = specification;
        }
    }
}
