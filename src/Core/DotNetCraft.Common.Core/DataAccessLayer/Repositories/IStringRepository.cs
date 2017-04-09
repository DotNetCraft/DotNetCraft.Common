using DotNetCraft.Common.Core.BaseEntities;

namespace DotNetCraft.Common.Core.DataAccessLayer.Repositories
{
    public interface IStringRepository<TEntity> : IRepository<TEntity, string>
           where TEntity : IEntity
    {
    }
}
