using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.Utils.Logging;

namespace DotNetCraft.Common.DataAccessLayer.Repositories
{
    public abstract class BaseIntRepository<TEntity> : BaseRepository<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected BaseIntRepository(IContextSettings contextSettings, IDataContextFactory dataContextFactory, ICommonLoggerFactory loggerFactory) : base(contextSettings, dataContextFactory, loggerFactory)
        {
        }
    }
}
