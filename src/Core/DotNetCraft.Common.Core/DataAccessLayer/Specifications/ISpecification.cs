using System;
using System.Linq.Expressions;

namespace DotNetCraft.Common.Core.DataAccessLayer.Specifications
{
    /// <summary>
    /// Interface shows that object is specification.
    /// </summary>
    /// <typeparam name="TEntity">Entity's type</typeparam>
    public interface ISpecification<TEntity>
    {
        /// <summary>
        /// Prepare expression for specification.
        /// </summary>
        /// <returns>Expression.</returns>
        Expression<Func<TEntity, bool>> IsSatisfiedBy();
    }
}
