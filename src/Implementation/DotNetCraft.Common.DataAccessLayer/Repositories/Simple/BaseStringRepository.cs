using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;

namespace DotNetCraft.Common.DataAccessLayer.Repositories.Simple
{
    public abstract class BaseStringRepository<TEntity> : BaseRepository<TEntity, string>
        where TEntity : class, IEntity<string>
    {
        protected BaseStringRepository(IContextSettings contextSettings, IDataContextFactory dataContextFactory) : base(contextSettings, dataContextFactory)
        {
        }
    }
}
