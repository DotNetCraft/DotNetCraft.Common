using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;

namespace DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Smart
{
    public interface ISmartUnitOfWorkFactory: IUnitOfWorkFactory
    {
        ISmartUnitOfWork CreateSmartUnitOfWork(IContextSettings contextSettings);
    }
}
