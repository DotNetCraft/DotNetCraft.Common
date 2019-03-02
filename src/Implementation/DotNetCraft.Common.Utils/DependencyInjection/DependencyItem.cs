using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetCraft.Common.Core.Utils.DependencyInjection;
using DotNetCraft.Common.Core.Utils.DependencyInjection.Enums;

namespace DotNetCraft.Common.Utils.DependencyInjection
{    
    public class DependencyItem<TObject>: IDependencyItem
    {
        private readonly IDependencyContainer _dependencyContainer;

        #region Fields...

        private TObject _instance;

        private readonly ParameterInfo[] _inputParameters;
        private readonly ObjectActivator<TObject> _activator;

        #endregion

        #region Properties...

        public Type InstanceType { get; }

        public DependencyStrategy DependencyStrategy { get; }
        #endregion

        #region Constructors...       

        public DependencyItem(DependencyStrategy dependencyStrategy, IDependencyContainer dependencyContainer, IDependencyActivator dependencyActivator)
        {
            if (dependencyContainer == null)
                throw new ArgumentNullException(nameof(dependencyContainer));
            if (dependencyActivator == null)
                throw new ArgumentNullException(nameof(dependencyActivator));
            _dependencyContainer = dependencyContainer;

            InstanceType = typeof(TObject);
            DependencyStrategy = dependencyStrategy;

            var constructor = InstanceType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
            _inputParameters = constructor.GetParameters();
            _activator = dependencyActivator.CreateObjectActivator<TObject>(constructor, _inputParameters);
        }

        #endregion

        #region Get or Create instance...
        private TObject CreateInstance(params object[] args)
        {
            List<object> inputArgs = null;
            if (args != null && args.Length > 0)
                inputArgs = new List<object>(args);

            object[] constructorParameters = new object[_inputParameters.Length];
            for (var index = 0; index < _inputParameters.Length; index++)
            {
                ParameterInfo inputParameter = _inputParameters[index];

                object parameterValue = null;
                if (inputArgs != null)
                {
                    for (var i = 0; i < inputArgs.Count; i++)
                    {
                        var arg = inputArgs[i];
                        Type argType = arg.GetType();
                        if (argType == inputParameter.ParameterType || argType.BaseType == inputParameter.ParameterType)
                        {
                            parameterValue = arg;
                            inputArgs.RemoveAt(i);
                            break;
                        }
                    }
                }

                if (parameterValue == null)
                    parameterValue = _dependencyContainer.Resolve(inputParameter.ParameterType);

                constructorParameters[index] = parameterValue;
            }

            //var result = _constructor.Invoke(constructorParameters);
            var result = _activator(constructorParameters);
            return result;
        }

        public TObject GetOrCreateInstance(params object[] args)
        {
            if (DependencyStrategy == DependencyStrategy.Singleton)
            {
                if (_instance != null)
                    return _instance;

                _instance = CreateInstance(args);
                return _instance;
            }

            var instance = CreateInstance(args);
            return instance;
        }

        public object GetOrCreateObject(params object[] args)
        {
            return GetOrCreateInstance(args);
        }
        #endregion
    }
}
