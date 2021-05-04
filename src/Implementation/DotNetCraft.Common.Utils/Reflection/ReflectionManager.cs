using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions;
using Microsoft.Extensions.Logging;

namespace DotNetCraft.Common.Utils.Reflection
{
    public class ReflectionManager : IReflectionManager
    {
        private readonly object _syncObject = new object();
        private readonly ILogger<ReflectionManager> _logger;

        private readonly Dictionary<string, ObjectReflectionData> _objectReflectionDictionary;

        public ReflectionManager(ILogger<ReflectionManager> logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _objectReflectionDictionary = new Dictionary<string, ObjectReflectionData>();

            _logger = logger;
        }

        #region Implementation of IReflectionManager

        private ObjectReflectionData LoadOrCreateObjectReflectionData(Type objectType)
        {
            lock (_syncObject)
            {
                ObjectReflectionData objectReflectionData;
                if (_objectReflectionDictionary.TryGetValue(objectType.FullName, out objectReflectionData) == false)
                {
                    objectReflectionData = new ObjectReflectionData(objectType);
                    _objectReflectionDictionary.Add(objectType.FullName, objectReflectionData);
                }

                return objectReflectionData;
            }
        }

        #region FieldInfo...
        public FieldInfo GetFieldInfoByName(string fieldName, Type objectType)
        {
            ObjectReflectionData objectReflectionData = LoadOrCreateObjectReflectionData(objectType);
            return objectReflectionData.GetFieldByName(fieldName, objectType);
        }

        public FieldInfo GetFieldInfoByName<TObjectType>(string fieldName)
        {
            return GetFieldInfoByName(fieldName, typeof(TObjectType));
        }

        public List<FieldInfo> GetFieldInfosByAttributeName(string attributeName, Type objecType)
        {
            if (string.IsNullOrWhiteSpace(attributeName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(attributeName));

            if (objecType == null)
                throw new ArgumentNullException(nameof(objecType));

            ObjectReflectionData objectReflectionData = LoadOrCreateObjectReflectionData(objecType);

            return objectReflectionData.GetFieldsByAttributeName(attributeName, objecType);
        }

        public List<FieldInfo> GetFieldInfosByAttributeName<TObjectType>(string attributeName)
        {
            return GetFieldInfosByAttributeName(attributeName, typeof(TObjectType));
        }

        public List<FieldInfo> GetFieldInfosByAttribute(Type attributeType, Type objectType)
        {
            return GetFieldInfosByAttributeName(attributeType.Name, objectType);
        }

        public List<FieldInfo> GetFieldInfosByAttribute<TAttribute>(Type objectType)
            where TAttribute : Attribute
        {
            return GetFieldInfosByAttributeName(typeof(TAttribute).Name, objectType);
        }

        public List<FieldInfo> GetFieldInfosByAttribute<TAttribute, TObjectType>()
            where TAttribute : Attribute
        {
            return GetFieldInfosByAttributeName<TObjectType>(typeof(TAttribute).Name);
        }

        public FieldInfo GetFieldInfoByName(string fieldName, object obj)
        {
            return GetFieldInfoByName(fieldName, obj.GetType());
        }

        public List<FieldInfo> GetFieldInfosByAttribute(Type attributeType, object obj)
        {
            return GetFieldInfosByAttribute(attributeType, obj.GetType());
        }

        public List<FieldInfo> GetFieldInfosByAttribute<TAttribute>(object obj)
            where TAttribute : Attribute
        {
            return GetFieldInfosByAttribute<TAttribute>(obj.GetType());
        }

        #endregion

        #region PropertyInfo...

        public List<PropertyInfo> GetPropertyInfos(Type objectType)
        {
            ObjectReflectionData objectReflectionData = LoadOrCreateObjectReflectionData(objectType);
            return objectReflectionData.GetPropertyInfos(objectType);
        }

        public PropertyInfo GetPropertyInfoByName(string propertyName, Type objectType)
        {
            ObjectReflectionData objectReflectionData = LoadOrCreateObjectReflectionData(objectType);
            return objectReflectionData.GetPropertyByName(propertyName, objectType);
        }

        public PropertyInfo GetPropertyInfoByName<TObjectType>(string propertyName)
        {
            return GetPropertyInfoByName(propertyName, typeof(TObjectType));
        }

        public List<PropertyInfo> GetPropertyInfosByAttributeName(string attributeName, Type objecType)
        {
            if (string.IsNullOrWhiteSpace(attributeName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(attributeName));

            if (objecType == null)
                throw new ArgumentNullException(nameof(objecType));

            ObjectReflectionData objectReflectionData = LoadOrCreateObjectReflectionData(objecType);

            return objectReflectionData.GetPropertiesByAttributeName(attributeName, objecType);
        }

        public List<PropertyInfo> GetPropertyInfosByAttributeName<TObjectType>(string attributeName)
        {
            return GetPropertyInfosByAttributeName(attributeName, typeof(TObjectType));
        }

        public List<PropertyInfo> GetPropertyInfosByAttribute(Type attributeType, Type objectType)
        {
            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));
            if (objectType == null)
                throw new ArgumentNullException(nameof(objectType));
#if DEBUG
            if (attributeType.IsSubclassOf(typeof(Attribute)) == false)
            {
                throw new ArgumentOutOfRangeException("attributeType", "This is not attribute");
            }
#endif
                return GetPropertyInfosByAttributeName(attributeType.Name, objectType);
        }

        public List<PropertyInfo> GetPropertyInfosByAttribute<TAttribute>(Type objectType)
            where TAttribute : Attribute
        {
            return GetPropertyInfosByAttribute(typeof(TAttribute), objectType);
        }

        public List<PropertyInfo> GetPropertyInfosByAttribute<TAttribute, TObjectType>()
            where TAttribute : Attribute
        {
            return GetPropertyInfosByAttribute(typeof(TAttribute), typeof(TObjectType));
        }

        public PropertyInfo GetPropertyInfoByName(string propertyName, object obj)
        {
            return GetPropertyInfoByName(propertyName, obj.GetType());
        }

        public List<PropertyInfo> GetPropertyInfosByAttribute(Type attributeType, object obj)
        {
            return GetPropertyInfosByAttribute(attributeType, obj.GetType());
        }

        public List<PropertyInfo> GetPropertyInfosByAttribute<TAttribute>(object obj)
            where TAttribute : Attribute
        {
            return GetPropertyInfosByAttribute<TAttribute>(obj.GetType());
        }

        #endregion

        #region MethodInfo...

        public MethodInfo GetMethodInfoByName(string methodName, Type objectType)
        {
            ObjectReflectionData objectReflectionData = LoadOrCreateObjectReflectionData(objectType);
            return objectReflectionData.GetMethodByName(methodName, objectType);
        }

        public MethodInfo GetMethodInfoByName<TObjectType>(string methodName)
        {
            return GetMethodInfoByName(methodName, typeof(TObjectType));
        }

        public List<MethodInfo> GetMethodInfosByAttributeName(string attributeName, Type objecType)
        {
            if (string.IsNullOrWhiteSpace(attributeName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(attributeName));

            if (objecType == null)
                throw new ArgumentNullException(nameof(objecType));

            ObjectReflectionData objectReflectionData = LoadOrCreateObjectReflectionData(objecType);

            return objectReflectionData.GetMethodsByAttributeName(attributeName, objecType);
        }

        public List<MethodInfo> GetMethodInfosByAttributeName<TObjectType>(string attributeName)
        {
            return GetMethodInfosByAttributeName(attributeName, typeof(TObjectType));
        }

        public List<MethodInfo> GetMethodInfosByAttribute(Type attributeType, Type objectType)
        {
            return GetMethodInfosByAttributeName(attributeType.Name, objectType);
        }

        public List<MethodInfo> GetMethodInfosByAttribute<TAttribute>(Type objectType)
            where TAttribute : Attribute
        {
            return GetMethodInfosByAttributeName(typeof(TAttribute).Name, objectType);
        }

        public List<MethodInfo> GetMethodInfosByAttribute<TAttribute, TObjectType>()
            where TAttribute : Attribute
        {
            return GetMethodInfosByAttributeName<TObjectType>(typeof(TAttribute).Name);
        }

        public MethodInfo GetMethodInfoByName(string methodName, object obj)
        {
            return GetMethodInfoByName(methodName, obj.GetType());
        }

        public List<MethodInfo> GetMethodInfosByAttribute(Type attributeType, object obj)
        {
            return GetMethodInfosByAttribute(attributeType, obj.GetType());
        }

        public List<MethodInfo> GetMethodInfosByAttribute<TAttribute>(object obj)
            where TAttribute : Attribute
        {
            return GetMethodInfosByAttribute<TAttribute>(obj.GetType());
        }

        public List<MethodInfoDetails> GetMethodInfoDetailsByAttribute<TAttribute>(Type objectType)
            where TAttribute : Attribute
        {
            var items = GetMethodInfosByAttribute<TAttribute>(objectType);
            List<MethodInfoDetails> result = new List<MethodInfoDetails>();
            foreach (MethodInfo methodInfo in items)
            {
                result.Add(new MethodInfoDetails(methodInfo, objectType));
            }

            return result;
        }

        public List<MethodInfoDetails> GetMethodInfoDetailsByAttribute<TAttribute>(object secondModel)
            where TAttribute : Attribute
        {
            return GetMethodInfoDetailsByAttribute<TAttribute>(secondModel.GetType());
        }

        public TValue GetIdentifierValue<TValue>(object obj)
        {
            PropertyInfo propertyInfo = GetPropertyInfosByAttribute<KeyAttribute>(obj).SingleOrDefault();

            if (propertyInfo == null)
                propertyInfo = GetPropertyInfoByName("Id", obj);

            TValue result = (TValue)propertyInfo.GetValue(obj, null);
            return result;
        }

        public PropertyInfo GetIdentifierProperty(object obj)
        {
            PropertyInfo propertyInfo = GetPropertyInfosByAttribute<KeyAttribute>(obj).SingleOrDefault();

            if (propertyInfo == null)
                propertyInfo = GetPropertyInfoByName("Id", obj);

            return propertyInfo;
        }

        public ConstructorInfo GetSingleConstructor(Type objecType)
        {
            if (objecType == null)
                throw new ArgumentNullException(nameof(objecType));

            ObjectReflectionData objectReflectionData = LoadOrCreateObjectReflectionData(objecType);

            List<ConstructorInfo> constructorInfos = objectReflectionData.GetPublicConstructors(objecType);

            return constructorInfos.Single();
        }

        #endregion

        #endregion
    }
}
