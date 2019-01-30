using System.Text;
using DotNetCraft.Common.Core.DataAccessLayer.Specifications;

namespace DotNetCraft.Common.Core.DataAccessLayer.Repositories
{
    public class RepositorySimpleRequest<TEntity>
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string OrderBy { get; set; }

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Skip: {0}; Take: {1}; OrderBy: {2}", Skip, Take, OrderBy);
        }

        #endregion
    }

    public class RepositorySpecificationRequest<TEntity>: RepositorySimpleRequest<TEntity>
    {
        public ISpecification<TEntity> Specification { get; set; }

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Specification: {0}", base.ToString());
        }

        #endregion
    }
}
