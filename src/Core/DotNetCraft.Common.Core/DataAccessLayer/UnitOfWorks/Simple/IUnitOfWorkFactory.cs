using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;

namespace DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork(IUniqueKey uniqueKey = null);        
    }
}
