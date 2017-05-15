using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetCraft.Common.Core.Utils
{
    /// <summary>
    /// Interface shows that object is a property manager and it can be used for searching property in the object
    /// </summary>
    public interface IPropertyManager
    {
        PropertyInfo SingleOrDefault<TObjectType>(Type attributeType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public);

        PropertyInfo SingleOrDefault(Type objectType, Type attributeType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public);

        PropertyInfo Single<TObjectType>(Type attributeType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public);

        PropertyInfo Single(Type objectType, Type attributeType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public);

        ICollection<PropertyInfo> GetList<TObjectType>(Type attributeType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public);

        ICollection<PropertyInfo> GetList(Type objectType, Type attributeType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public);        
    }
}
