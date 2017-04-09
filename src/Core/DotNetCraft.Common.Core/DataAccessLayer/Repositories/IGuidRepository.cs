using System;
using DotNetCraft.Common.Core.BaseEntities;

namespace DotNetCraft.Common.Core.DataAccessLayer.Repositories
{
    public interface IGuidRepository<TEntity> : IRepository<TEntity, Guid>
       where TEntity : IEntity
    {
    }
}
