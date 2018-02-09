using System.Collections.Generic;

namespace DotNetCraft.Common.Core.Utils.Mapping
{
    public interface IMapperManager
    {
        bool RegisterMapping<TSource, TDestination>(IMapperInstance<TSource, TDestination> mapperInstance);

        TDestination Map<TSource, TDestination>(TSource source);

        IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);
    }
}
