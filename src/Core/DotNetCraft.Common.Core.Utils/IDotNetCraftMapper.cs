using System.Collections.Generic;

namespace DotNetCraft.Common.Core.Utils
{
    public interface IDotNetCraftMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);

        ICollection<TDestination> Map<TSource, TDestination>(ICollection<TSource> source);
    }
}
