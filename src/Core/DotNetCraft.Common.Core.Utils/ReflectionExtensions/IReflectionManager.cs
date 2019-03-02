using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetCraft.Common.Core.Utils.ReflectionExtensions
{
    public interface IReflectionManager
    {
        #region FieldInfo...

        #region Get FieldInfo by name...
        FieldInfo GetFieldInfoByName<TObjectType>(string fieldName);
        FieldInfo GetFieldInfoByName(string fieldName, Type objectType);
        FieldInfo GetFieldInfoByName(string fieldName, object obj);
        #endregion

        #region Get FieldInfo by attribute...
        List<FieldInfo> GetFieldInfosByAttributeName(string attributeName, Type objecType);
        List<FieldInfo> GetFieldInfosByAttributeName<TObjectType>(string attributeName);
        List<FieldInfo> GetFieldInfosByAttribute(Type attributeType, Type objectType);
        List<FieldInfo> GetFieldInfosByAttribute<TAttribute>(Type objectType)
            where TAttribute : Attribute;
        List<FieldInfo> GetFieldInfosByAttribute<TAttribute, TObjectType>()
            where TAttribute : Attribute;
        List<FieldInfo> GetFieldInfosByAttribute(Type attributeType, object obj);
        List<FieldInfo> GetFieldInfosByAttribute<TAttribute>(object obj)
            where TAttribute : Attribute;
        #endregion

        #endregion        

        #region PropertyInfo...

        List<PropertyInfo> GetPropertyInfos(Type objectType);

        #region Get PropertyInfo by name...
        PropertyInfo GetPropertyInfoByName(string propertyName, Type objectType);
        PropertyInfo GetPropertyInfoByName<TObjectType>(string propertyName);
        PropertyInfo GetPropertyInfoByName(string propertyName, object obj);
        #endregion

        #region Get PropertyInfo by attribute...
        List<PropertyInfo> GetPropertyInfosByAttributeName(string attributeName, Type objecType);
        List<PropertyInfo> GetPropertyInfosByAttributeName<TObjectType>(string attributeName);
        List<PropertyInfo> GetPropertyInfosByAttribute(Type attributeType, Type objectType);
        List<PropertyInfo> GetPropertyInfosByAttribute<TAttribute>(Type objectType)
            where TAttribute : Attribute;
        List<PropertyInfo> GetPropertyInfosByAttribute<TAttribute, TObjectType>()
            where TAttribute : Attribute;

        List<PropertyInfo> GetPropertyInfosByAttribute(Type attributeType, object obj);
        List<PropertyInfo> GetPropertyInfosByAttribute<TAttribute>(object obj)
            where TAttribute : Attribute;
        #endregion

        #endregion        

        #region MethodInfo...

        #region Get MethodInfo by name...
        MethodInfo GetMethodInfoByName(string methodName, Type objectType);
        MethodInfo GetMethodInfoByName<TObjectType>(string methodName);
        MethodInfo GetMethodInfoByName(string methodName, object obj);
        #endregion

        #region Get MethodInfo by attribute...
        List<MethodInfo> GetMethodInfosByAttributeName(string attributeName, Type objecType);
        List<MethodInfo> GetMethodInfosByAttributeName<TObjectType>(string attributeName);
        List<MethodInfo> GetMethodInfosByAttribute(Type attributeType, Type objectType);
        List<MethodInfo> GetMethodInfosByAttribute<TAttribute>(Type objectType)
            where TAttribute : Attribute;
        List<MethodInfo> GetMethodInfosByAttribute<TAttribute, TObjectType>()
            where TAttribute : Attribute;

        List<MethodInfo> GetMethodInfosByAttribute(Type attributeType, object obj);
        List<MethodInfo> GetMethodInfosByAttribute<TAttribute>(object obj)
            where TAttribute : Attribute;

        List<MethodInfoDetails> GetMethodInfoDetailsByAttribute<TAttribute>(Type objectType)
            where TAttribute : Attribute;
        List<MethodInfoDetails> GetMethodInfoDetailsByAttribute<TAttribute>(object secondModel)
            where TAttribute : Attribute;

        #endregion

        #endregion

        TValue GetIdentifierValue<TValue>(object obj);
        PropertyInfo GetIdentifierProperty(object obj);

        #region Constructors...
        ConstructorInfo GetSingleConstructor(Type type);
        #endregion        
    }
}
