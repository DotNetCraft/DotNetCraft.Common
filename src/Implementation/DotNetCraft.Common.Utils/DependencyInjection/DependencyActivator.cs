using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotNetCraft.Common.Core.Utils.DependencyInjection;

namespace DotNetCraft.Common.Utils.DependencyInjection
{
    public class DependencyActivator: IDependencyActivator
    {
        private readonly Dictionary<string, object> _objectActivators;
        private readonly object _syncObject = new object();

        public DependencyActivator()
        {
            _objectActivators = new Dictionary<string, object>();
        }

        #region Implementation of IDependencyActivator

        public ObjectActivator<TObject> GetSingleActivator<TObject>()
        {
            Type objectType = typeof(TObject);
            string key = objectType.FullName;
            object obj;
            ObjectActivator<TObject> objectActivator;
            if (_objectActivators.TryGetValue(key, out obj) == false)
            {
                lock (_syncObject)
                {
                    if (_objectActivators.TryGetValue(key, out obj) == false)
                    {
                        ConstructorInfo constructorInfo = objectType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
                        objectActivator = CreateObjectActivator<TObject>(constructorInfo, new ParameterInfo[0]);
                        _objectActivators[key] = objectActivator;
                    }
                    else
                    {
                        objectActivator = (ObjectActivator<TObject>)obj;
                    }
                }                
            }
            else
            {
                objectActivator = (ObjectActivator<TObject>) obj;
            }

            return objectActivator;
        }

        public ObjectActivator<TObject> CreateObjectActivator<TObject>(ConstructorInfo constructorInfo, ParameterInfo[] paramsInfo)
        {
            //create a single param of type object[]
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp = new Expression[paramsInfo.Length];

            //pick each arg from the params array 
            //and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp = Expression.ArrayIndex(param, index);

                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(constructorInfo, argsExp);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator<TObject>), newExp, param);

            //compile it
            ObjectActivator<TObject> compiled = (ObjectActivator<TObject>)lambda.Compile();
            return compiled;
        }        

        #endregion
    }
}
