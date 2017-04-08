using DotNetCraft.Common.Core.BaseEntities;

namespace DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks
{
    /// <summary>
    /// Pattern Unit of Work.
    /// </summary>
    public interface ISmartUnitOfWork: IUnitOfWork
    {
        /// <summary>
        /// Insert an model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <return>The model that has been inserted.</return>
        TModel Insert<TModel, TEntity>(TModel model)
            where TEntity : class, IEntity; //TODO: Return ID

        /// <summary>
        /// Update an model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Update<TModel, TEntity>(TModel model)
            where TEntity : IEntity;

        /// <summary>
        /// Delete an model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Delete<TModel, TEntity>(TModel model)
            where TEntity : IEntity;

        /// <summary>
        /// Commit transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        void Rollback();
    }
}
