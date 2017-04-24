using System;
using System.Reflection;

namespace DotNetCraft.Common.Core.Utils
{
    /// <summary>
    /// Interface shows that object is a property manager and it can be used for searching property in the object
    /// </summary>
    public interface IPropertyManager
    {
        PropertyInfo SingleOrDefault<TObjectType>(Type attributeType);

        PropertyInfo SingleOrDefault(Type objectType, Type attributeType);

        PropertyInfo Single<TObjectType>(Type attributeType);

        PropertyInfo Single(Type objectType, Type attributeType);
    }
}
