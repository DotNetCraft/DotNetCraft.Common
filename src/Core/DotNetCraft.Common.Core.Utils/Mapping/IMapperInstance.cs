namespace DotNetCraft.Common.Core.Utils.Mapping
{
    public interface IMapperInstance<TSource, TDestination>
    {
        TDestination Map(TSource source);
    }
}
