using System.Reflection;

namespace DotNetCraft.Common.Core.Utils.DependencyInjection
{
    public delegate TObject ObjectActivator<out TObject>(params object[] args);

    public interface IDependencyActivator
    {
        ObjectActivator<TObject> GetSingleActivator<TObject>();
        ObjectActivator<TObject> CreateObjectActivator<TObject>(ConstructorInfo constructorInfo, ParameterInfo[] paramsInfo);
    }
}
