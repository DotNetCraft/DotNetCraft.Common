namespace DotNetCraft.Common.Core.Utils.DependencyInjection
{
    public interface IDependencyItem
    {
        object GetOrCreateObject(params object[] args);
    }
}
