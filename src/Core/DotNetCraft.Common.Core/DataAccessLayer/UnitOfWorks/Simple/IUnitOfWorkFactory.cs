namespace DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork();        
    }
}
