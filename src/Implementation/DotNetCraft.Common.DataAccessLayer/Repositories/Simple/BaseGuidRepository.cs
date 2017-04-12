using System;
using DotNetCraft.Common.Core.BaseEntities;
using DotNetCraft.Common.Core.DataAccessLayer;

namespace DotNetCraft.Common.DataAccessLayer.Repositories.Simple
{
    public abstract class BaseGuidRepository<TEntity>: BaseRepository<TEntity, Guid> 
        where TEntity : class, IEntity<Guid>
    {
        protected BaseGuidRepository(IContextSettings contextSettings, IDataContextFactory dataContextFactory) : base(contextSettings, dataContextFactory)
        {
        }
    }
}
