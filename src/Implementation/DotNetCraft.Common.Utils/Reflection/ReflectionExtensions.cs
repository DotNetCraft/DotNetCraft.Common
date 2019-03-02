using System;
using System.Reflection;

namespace DotNetCraft.Common.Utils.Reflection
{
    public static class ReflectionExtensions
    {
        public static Func<TObject, TResult> GetGetter<TObject, TResult>(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanRead == false)
                return null;

            Type objectType = typeof(TObject);
            Type resultType = typeof(TResult);
            Type openGetterType = typeof(Func<,>);
            Type concreteGetterType = openGetterType.MakeGenericType(objectType, resultType);
            MethodInfo getMethod = propertyInfo.GetGetMethod(true);
            Delegate getterInvocation = Delegate.CreateDelegate(concreteGetterType, null, getMethod);

            Func<TObject, TResult> result = (Func<TObject, TResult>)getterInvocation;
            return result;
        }

        public static Action<TObject, TResult> GetSetter<TObject, TResult>(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanWrite == false)
                return null;

            Type objectType = typeof(TObject);
            Type resultType = typeof(TResult);

            Type openGetterType = typeof(Action<,>);
            Type concreteGetterType = openGetterType.MakeGenericType(objectType, resultType);
            MethodInfo setMethod = propertyInfo.GetSetMethod(true);
            Delegate setterInvocation = Delegate.CreateDelegate(concreteGetterType, null, setMethod);

            Action<TObject, TResult> result = (Action<TObject, TResult>)setterInvocation;
            return result;
        }

        #region Get Functions...
        private static Delegate GetDelegate<TObject, TResult>(Type funcType, MethodInfo methodInfo)
        {
            Type objectType = typeof(TObject);
            Type resultType = typeof(TResult);

            ParameterInfo[] parameters = methodInfo.GetParameters();
            Type[] genericArguments = new Type[2 + parameters.Length];
            int index = 0;
            genericArguments[index++] = objectType;
            foreach (ParameterInfo parameterInfo in parameters)
            {
                genericArguments[index++] = parameterInfo.ParameterType;
            }
            genericArguments[index] = resultType;
            
            Type concreteGetterType = funcType.MakeGenericType(genericArguments);
            Delegate func = Delegate.CreateDelegate(concreteGetterType, null, methodInfo);
            return func;
        }

        public static Func<TObject, TResult> GetFunction<TObject, TResult>(this MethodInfo methodInfo)
        {
            Delegate func = GetDelegate<TObject, TResult>(typeof(Func<,>), methodInfo);
            Func<TObject, TResult> result = (Func<TObject, TResult>)func;
            return result;
        }

        public static Func<TObject, TArg, TResult> GetFunction<TObject, TArg, TResult>(this MethodInfo methodInfo)
        {
            Delegate func = GetDelegate<TObject, TResult>(typeof(Func<,,>), methodInfo);
            Func<TObject, TArg, TResult> result = (Func<TObject, TArg,TResult>)func;
            return result;
        }

        public static Func<TObject, TArg1, TArg2, TResult> GetFunction<TObject, TArg1, TArg2, TResult>(this MethodInfo methodInfo)
        {
            Delegate func = GetDelegate<TObject, TResult>(typeof(Func<,,,>), methodInfo);
            Func<TObject, TArg1, TArg2, TResult > result = (Func<TObject, TArg1, TArg2, TResult>)func;
            return result;
        }

        public static Func<TObject, TArg1, TArg2, TArg3, TResult> GetFunction<TObject, TArg1, TArg2, TArg3, TResult>(this MethodInfo methodInfo)
        {
            Delegate func = GetDelegate<TObject, TResult>(typeof(Func<,,,,>), methodInfo);
            Func<TObject, TArg1, TArg2, TArg3, TResult> result = (Func<TObject, TArg1, TArg2, TArg3, TResult>)func;
            return result;
        }
        #endregion
    }
}
