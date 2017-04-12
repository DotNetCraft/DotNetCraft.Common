using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Smart;
using DotNetCraft.Common.Core.Utils;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SmartUnitOfWorks
{
    public class SmartUnitOfWork : UnitOfWork, ISmartUnitOfWork
    {
        private readonly IDotNetCraftMapper dotNetCraftMapper;

        /// <summary>
        /// Constructor.
        /// </summary>        
        public SmartUnitOfWork(IDataContext dataContext, IDotNetCraftMapper dotNetCraftMapper) : base(dataContext)
        {
            if (dotNetCraftMapper == null)
                throw new ArgumentNullException(nameof(dotNetCraftMapper));

            this.dotNetCraftMapper = dotNetCraftMapper;
        }

        #region Implementation of ISmartUnitOfWork       

        /// <summary>
        /// Insert an model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <return>The model that has been inserted.</return>
        public void Insert<TModel, TEntity>(TModel model)
            where TEntity : class, IEntity
        {
            try
            {
                TEntity entity = dotNetCraftMapper.Map<TModel, TEntity>(model);
                OnInsert(entity);                
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"EntityType", typeof(TEntity).ToString()},
                    {"ModelType", typeof(TModel).ToString()},
                    {"Model", model.ToString() }
                };
                throw new DataAccessLayerException("There was a problem during inserting a new model into the database", ex, errorParameters);
            }
        }

        /// <summary>
        /// Update an model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Update<TModel, TEntity>(TModel model)
            where TEntity : class, IEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete an model.
        /// </summary>
        /// <param name="modelId">The model's identifier.</param>
        public void Delete<TModel, TEntity>(object modelId)
            where TEntity : class, IEntity
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
