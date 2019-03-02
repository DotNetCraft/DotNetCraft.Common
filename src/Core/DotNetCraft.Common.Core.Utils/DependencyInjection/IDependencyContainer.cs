using System;
using DotNetCraft.Common.Core.Utils.DependencyInjection.Enums;

namespace DotNetCraft.Common.Core.Utils.DependencyInjection
{
    public interface IDependencyContainer
    {
        void Register<TInterface, TClass>(DependencyStrategy dependencyStrategy = DependencyStrategy.Singleton)
            where TClass: TInterface;
        void Register<TClass>(DependencyStrategy dependencyStrategy = DependencyStrategy.Singleton);
        void Register<TObject>(TObject instance);

        TInterface Resolve<TInterface>(params object[] args);
        object Resolve(Type interfaceType, params object[] args);

        TInterface Create<TInterface>(Type objectType, params object[] args);
        object Create(Type objectType, params object[] args);
        TClass Create<TClass>(params object[] args);
    }
}
