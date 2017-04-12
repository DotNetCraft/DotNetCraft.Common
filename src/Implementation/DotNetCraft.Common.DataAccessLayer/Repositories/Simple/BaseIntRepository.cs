using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;

namespace DotNetCraft.Common.DataAccessLayer.Repositories.Simple
{
    public abstract class BaseIntRepository<TEntity> : BaseRepository<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected BaseIntRepository(IContextSettings contextSettings, IDataContextFactory dataContextFactory) : base(contextSettings, dataContextFactory)
        {
        }
    }
}
