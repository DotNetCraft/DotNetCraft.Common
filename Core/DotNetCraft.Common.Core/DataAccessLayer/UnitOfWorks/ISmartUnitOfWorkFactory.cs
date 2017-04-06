namespace DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks
{
    public interface ISmartUnitOfWorkFactory: IUnitOfWorkFactory
    {
        ISmartUnitOfWork CreateSmartUnitOfWork(IDataBaseSettings dataBaseSettings);
    }
}
