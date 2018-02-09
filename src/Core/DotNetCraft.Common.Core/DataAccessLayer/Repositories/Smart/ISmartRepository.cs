using System.Collections.Generic;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories.Simple;
using DotNetCraft.Common.Core.DataAccessLayer.Specifications;

namespace DotNetCraft.Common.Core.DataAccessLayer.Repositories.Smart
{
    public interface ISmartRepository<TEntity> : IRepository<TEntity>
    {
        /// <summary>
        /// Get model by identifier.
        /// </summary>
        /// <param name="modelId">The model's identifier.</param>
        /// <returns>The model, if it exists.</returns>
        TModel Get<TModel, TIdentifier>(TIdentifier modelId);

        /// <summary>
        /// Get all models.
        /// </summary>
        /// <returns>Collection of models.</returns>
        List<TModel> GetAll<TModel>(int? skip = null, int? take = null);

        /// <summary>
        /// Get models by specification.
        /// </summary>
        /// <param name="specification">Some specification.</param>
        /// <returns>Collection of models.</returns>
        List<TModel> GetBySpecification<TModel>(ISpecification<TEntity> specification, int? skip = null, int? take = null);
    }
}
