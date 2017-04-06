namespace DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork(IDataBaseSettings dataBaseSettings);        
    }
}
