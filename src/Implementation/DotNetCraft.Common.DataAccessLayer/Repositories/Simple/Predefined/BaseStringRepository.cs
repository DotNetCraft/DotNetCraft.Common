using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.Repositories;
using DotNetCraft.Common.DataAccessLayer.BaseEntities;

namespace DotNetCraft.Common.DataAccessLayer.Repositories.Simple
{
    public abstract class BaseStringRepository<TEntity> : BaseRepository<TEntity>, IStringRepository<TEntity>
        where TEntity : BaseEntity<string>
    {
        protected BaseStringRepository(IContextSettings contextSettings, IDataContextFactory dataContextFactory) : base(contextSettings, dataContextFactory)
        {
        }

        #region Implementation of IStringRepository<TEntity>

        /// <summary>
        /// Get entity by identifier.
        /// </summary>
        /// <param name="entityId">The entity's identifier.</param>
        /// <returns>The entity, if it exists.</returns>
        public TEntity Get(string entityId)
        {
            TEntity result = base.Get(entityId);
            return result;
        }

        #endregion
    }
}
