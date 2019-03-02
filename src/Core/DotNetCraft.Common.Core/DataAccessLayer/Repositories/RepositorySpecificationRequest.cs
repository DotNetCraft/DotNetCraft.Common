using System;
using DotNetCraft.Common.Core.DataAccessLayer.Specifications;

namespace DotNetCraft.Common.Core.DataAccessLayer.Repositories
{
    public class RepositorySpecificationRequest<TEntity>: RepositorySimpleRequest
    {
        public ISpecification<TEntity> Specification { get; private set; }

        public RepositorySpecificationRequest(ISpecification<TEntity> specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));

            Specification = specification;
        }

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Specification: {0}; Base: {1}", Specification, base.ToString());
        }

        #endregion
    }
}
