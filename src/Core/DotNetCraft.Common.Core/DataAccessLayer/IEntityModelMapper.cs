//using System.Collections.Generic;

//namespace DotNetCraft.Common.Core.DataAccessLayer
//{
//    /// <summary>
//    /// Interface shows that object can convert Entity into the Model and back.
//    /// </summary>
//    public interface IEntityModelMapper
//    {
//        /// <summary>
//        /// Map an entity into the model.
//        /// </summary>
//        /// <typeparam name="TEntity">Type of entity.</typeparam>
//        /// <typeparam name="TModel">Type of model</typeparam>
//        /// <param name="entity">The entity</param>
//        /// <returns>The model</returns>
//        TModel Map<TEntity, TModel>(TEntity entity);

//        /// <summary>
//        /// Map a model into the entity.
//        /// </summary>
//        /// <typeparam name="TEntity">Type of entity.</typeparam>
//        /// <typeparam name="TModel">Type of model</typeparam>        
//        /// <param name="model">The model.</param>
//        /// <returns>The entity</returns>
//        TEntity Map<TEntity, TModel>(TModel model);

//        /// <summary>
//        /// Map list of entities into the models list.
//        /// </summary>
//        /// <typeparam name="TEntity">Type of entity.</typeparam>
//        /// <typeparam name="TModel">Type of model</typeparam>
//        /// <param name="entities">List of entities.</param>
//        /// <returns>Collection of models</returns>
//        List<TModel> Map<TEntity, TModel>(ICollection<TEntity> entities);

//        /// <summary>
//        /// Map list of models into the entities list.
//        /// </summary>
//        /// <typeparam name="TEntity">Type of entity.</typeparam>
//        /// <typeparam name="TModel">Type of model</typeparam>        
//        /// <param name="models">List of models</param>
//        /// <returns>Collection of entities</returns>
//        List<TEntity> Map<TEntity, TModel>(ICollection<TModel> models);
//    }
//}
