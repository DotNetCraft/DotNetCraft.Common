namespace DotNetCraft.Common.Core.BaseEntities
{
    public interface IEntity
    {
        object EntityId { get; }
    }

    public interface IEntity<TEntityId>: IEntity
    {
        new TEntityId Id { get; set; }
    }
}
