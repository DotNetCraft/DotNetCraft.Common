using System;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.Utils.Logging;

namespace DotNetCraft.Common.DataAccessLayer.Repositories
{
    public abstract class BaseGuidRepository<TEntity>: BaseRepository<TEntity, Guid> 
        where TEntity : class, IEntity<Guid>
    {
        protected BaseGuidRepository(IContextSettings contextSettings, IDataContextFactory dataContextFactory) : base(contextSettings, dataContextFactory)
        {
        }
    }
}
