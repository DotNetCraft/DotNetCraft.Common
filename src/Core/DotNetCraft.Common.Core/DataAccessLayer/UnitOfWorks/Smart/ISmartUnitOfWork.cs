using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;

namespace DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Smart
{
    /// <summary>
    /// Pattern Unit of Work.
    /// </summary>
    public interface ISmartUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Insert an model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <return>The model's identifier that has been inserted.</return>
        void Insert<TModel, TEntity>(TModel model)
            where TEntity : class;

        /// <summary>
        /// Update an model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Update<TModel, TEntity>(TModel model)
            where TEntity : class;

        /// <summary>
        /// Delete a model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Delete<TModel, TEntity>(TModel model)
            where TEntity : class;
    }
}
