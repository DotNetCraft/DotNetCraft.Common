using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.Utils.DependencyInjection;
using DotNetCraft.Common.Core.Utils.DependencyInjection.Enums;
using DotNetCraft.Common.Core.Utils.Logging;

namespace DotNetCraft.Common.Utils.DependencyInjection
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly IDependencyActivator _dependencyActivator;
        private readonly Dictionary<Type, IDependencyItem> _dependencyDictionary;
        private readonly Dictionary<Type, IDependencyItem> _createDependencyDictionary;
        private readonly ICommonLogger _simpleLogger;

        public DependencyContainer(IDependencyActivator dependencyActivator, ICommonLoggerFactory simpleLoggerFactory)
        {
            if (dependencyActivator == null)
                throw new ArgumentNullException(nameof(dependencyActivator));
            if (simpleLoggerFactory == null)
                throw new ArgumentNullException(nameof(simpleLoggerFactory));

            _dependencyActivator = dependencyActivator;
            _dependencyDictionary = new Dictionary<Type, IDependencyItem>();
            _createDependencyDictionary = new Dictionary<Type, IDependencyItem>();

            _simpleLogger = simpleLoggerFactory.Create<DependencyContainer>();

            _simpleLogger.Info("Registering container...");

            Type key = typeof(IDependencyContainer);
            Type value = GetType();
            IDependencyItem dependencyItem = CreateObjectDependencyItem(value, this);
            _dependencyDictionary.Add(key, dependencyItem);

            key = typeof(ICommonLoggerFactory);
            value = simpleLoggerFactory.GetType();
            dependencyItem = CreateObjectDependencyItem(value, simpleLoggerFactory);
            _dependencyDictionary.Add(key, dependencyItem);

            key = typeof(IDependencyActivator);
            value = dependencyActivator.GetType();
            dependencyItem = CreateObjectDependencyItem(value, dependencyActivator);
            _dependencyDictionary.Add(key, dependencyItem);

            _simpleLogger.Info("The container has been registered.");
        }

        private IDependencyItem CreateObjectDependencyItem(Type genericParameterType, object input)
        {
            Type dependencyItemType = typeof(ObjectDependencyItem<>);
            dependencyItemType = dependencyItemType.MakeGenericType(genericParameterType);
            IDependencyItem result = (IDependencyItem) Activator.CreateInstance(dependencyItemType, new object[] {input});
            return result;
        }

        private IDependencyItem CreateDependencyItem(Type genericParameterType, DependencyStrategy dependencyStrategy)
        {
            //DependencyItem(DependencyStrategy dependencyStrategy, IDependencyContainer dependencyContainer, IDependencyActivator dependencyActivator)
            Type dependencyItemType = typeof(DependencyItem<>);
            dependencyItemType = dependencyItemType.MakeGenericType(genericParameterType);
            IDependencyItem result = (IDependencyItem)Activator.CreateInstance(dependencyItemType, new object[] { dependencyStrategy, this, _dependencyActivator });
            return result;
        }

        #region Implementation of IDependencyContainer

        public void Register<TInterface, TClass>(DependencyStrategy dependencyStrategy = DependencyStrategy.Singleton)
            where TClass: TInterface
        {
            lock (_dependencyDictionary)
            {                
                Type key = typeof(TInterface);
                Type value = typeof(TClass);

                _simpleLogger.Debug("Registering dependency {0} and {1}...", key, value);
                IDependencyItem dependencyItem = CreateDependencyItem(value, dependencyStrategy);
                _dependencyDictionary.Add(key, dependencyItem);
                _simpleLogger.Debug("The dependency has been registered.");
            }
        }

        public void Register<TClass>(DependencyStrategy dependencyStrategy = DependencyStrategy.Singleton)
        {
            lock (_dependencyDictionary)
            {
                Type key = typeof(TClass);
                Type value = typeof(TClass);
                IDependencyItem dependencyItem = new DependencyItem<TClass>(dependencyStrategy, this, _dependencyActivator);
                _dependencyDictionary.Add(key, dependencyItem);
            }
        }

        public void Register<TObject>(TObject instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            lock (_dependencyDictionary)
            {
                Type key = typeof(TObject);
                IDependencyItem dependencyItem = new ObjectDependencyItem<TObject>(instance);
                _dependencyDictionary.Add(key, dependencyItem);
            }
        }

        public TInterface Resolve<TInterface>(params object[] args)
        {
            Type key = typeof(TInterface);           
            return (TInterface) Resolve(key, args);
        }

        public object Resolve(Type interfaceType, params object[] args)
        {
            lock (_dependencyDictionary)
            {
                IDependencyItem dependencyItem;
                if (_dependencyDictionary.TryGetValue(interfaceType, out dependencyItem) == false)
                    throw new ArgumentOutOfRangeException("interfaceType", "Type was not registered: " + interfaceType);

                var instance = dependencyItem.GetOrCreateObject(args);
                return instance;
            }
        }

        public object Create(Type objectType, params object[] args)
        {
            lock (_createDependencyDictionary)
            {
                IDependencyItem dependencyItem;
                if (_createDependencyDictionary.TryGetValue(objectType, out dependencyItem) == false)
                {
                    dependencyItem = CreateDependencyItem(objectType, DependencyStrategy.Instance);
                    _createDependencyDictionary[objectType] = dependencyItem;
                }

                var instance = dependencyItem.GetOrCreateObject(args);
                return instance;
            }
        }

        public TInterface Create<TInterface>(Type objectType, params object[] args)
        {
            var result = Create(objectType, args);
            return (TInterface)result;
        }

        public TClass Create<TClass>(params object[] args)
        {
            Type type = typeof(TClass);
            return (TClass) Create(type, args);
        }

        #endregion
    }
}
