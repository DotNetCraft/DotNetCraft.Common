using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories.Simple.Predefined;
using DotNetCraft.Common.DataAccessLayer.BaseEntities;

namespace DotNetCraft.Common.DataAccessLayer.Repositories.Simple.Predefined
{
    public abstract class BaseIntRepository<TEntity> : BaseRepository<TEntity>, IIntRepository<TEntity>
        where TEntity : BaseEntity<int>
    {
        protected BaseIntRepository(IContextSettings contextSettings, IDataContextFactory dataContextFactory) : base(contextSettings, dataContextFactory)
        {
        }

        #region Implementation of IIntRepository<TEntity>

        /// <summary>
        /// Get entity by identifier.
        /// </summary>
        /// <param name="entityId">The entity's identifier.</param>
        /// <returns>The entity, if it exists.</returns>
        public TEntity Get(int entityId)
        {
            TEntity result = base.Get(entityId);
            return result;
        }

        #endregion
    }
}
