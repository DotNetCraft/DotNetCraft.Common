using DotNetCraft.Common.Core.BaseEntities;

namespace DotNetCraft.Common.Core.DataAccessLayer.Repositories
{
    public interface IIntRepository<TEntity> : IRepository<TEntity, int> 
        where TEntity : IEntity
    {
    }
}
