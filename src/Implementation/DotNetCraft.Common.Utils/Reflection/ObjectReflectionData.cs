using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetCraft.Common.Utils.Reflection
{
    public class ObjectReflectionData
    {
        private BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public Type ObjectType { get; }

        private readonly Dictionary<string, List<FieldInfo>> _attributeAndFieldsDictionary;

        private readonly Dictionary<string, FieldInfo> _fieldInfoDictionary;

        private readonly Dictionary<string, List<PropertyInfo>> _attributeAndPropertiesDictionary;

        private readonly List<PropertyInfo> _fullPropertyInfos;
        private readonly Dictionary<string, PropertyInfo> _propertyInfoDictionary;

        private readonly Dictionary<string, List<MethodInfo>> _attributeAndMethodsDictionary;

        private readonly Dictionary<string, MethodInfo> _methodInfoDictionary;

        private readonly List<ConstructorInfo> _constructors;

        public ObjectReflectionData(Type objectType)
        {
            ObjectType = objectType;
            //TODO: Load all data from the objectType

            _attributeAndFieldsDictionary = new Dictionary<string, List<FieldInfo>>();
            _fieldInfoDictionary = new Dictionary<string, FieldInfo>();
            _attributeAndPropertiesDictionary = new Dictionary<string, List<PropertyInfo>>();
            _fullPropertyInfos = new List<PropertyInfo>();
            _propertyInfoDictionary = new Dictionary<string, PropertyInfo>();
            _attributeAndMethodsDictionary = new Dictionary<string, List<MethodInfo>>();
            _methodInfoDictionary = new Dictionary<string, MethodInfo>();
            _constructors = new List<ConstructorInfo>();

            CreateFieldInformation();
            CreatePropertyInformation();
            CreateMethodInformation();
            CreateConstructorInformation();
        }

        private void CreateConstructorInformation()
        {
            Type currentType = ObjectType;
            ConstructorInfo[] constructorInfos = currentType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            _constructors.AddRange(constructorInfos);
        }

        private void CreateFieldInformation()
        {
            Type currentType = ObjectType;
            do
            {
                FieldInfo[] fieldInfos = currentType.GetFields(bindingFlags);

                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    if (_fieldInfoDictionary.ContainsKey(fieldInfo.Name) == false)
                        _fieldInfoDictionary.Add(fieldInfo.Name, fieldInfo);

                    object[] attributes = fieldInfo.GetCustomAttributes(true);
                    foreach (Attribute attribute in attributes)
                    {
                        string attrName = attribute.GetType().Name;

                        List<FieldInfo> fieldList;
                        if (_attributeAndFieldsDictionary.TryGetValue(attrName, out fieldList) == false)
                        {
                            fieldList = new List<FieldInfo>();
                            _attributeAndFieldsDictionary.Add(attrName, fieldList);
                        }

                        if (fieldList.Any(x => x.Name == fieldInfo.Name) == false)
                        {
                            fieldList.Add(fieldInfo);
                        }
                    }
                }

                currentType = currentType.BaseType;
            } while (currentType != null);
        }

        private void CreatePropertyInformation()
        {
            Type currentType = ObjectType;
            do
            {
                PropertyInfo[] properties = currentType.GetProperties(bindingFlags);

                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (_propertyInfoDictionary.ContainsKey(propertyInfo.Name) == false)
                        _propertyInfoDictionary.Add(propertyInfo.Name, propertyInfo);

                    object[] attributes = propertyInfo.GetCustomAttributes(true);
                    foreach (Attribute attribute in attributes)
                    {
                        string attrName = attribute.GetType().Name;

                        List<PropertyInfo> propertyInfos;
                        if (_attributeAndPropertiesDictionary.TryGetValue(attrName, out propertyInfos) == false)
                        {
                            propertyInfos = new List<PropertyInfo>();
                            _attributeAndPropertiesDictionary.Add(attrName, propertyInfos);
                        }

                        if (propertyInfos.Any(x => x.Name == propertyInfo.Name) == false)
                        {
                            propertyInfos.Add(propertyInfo);
                        }
                    }

                    _fullPropertyInfos.Add(propertyInfo);
                }

                currentType = currentType.BaseType;
            } while (currentType != null);
        }

        private void CreateMethodInformation()
        {
            Type currentType = ObjectType;
            do
            {
                MethodInfo[] methodInfos = currentType.GetMethods(bindingFlags);

                foreach (MethodInfo methodInfo in methodInfos)
                {
                    if (_methodInfoDictionary.ContainsKey(methodInfo.Name) == false)
                        _methodInfoDictionary.Add(methodInfo.Name, methodInfo);

                    object[] attributes = methodInfo.GetCustomAttributes(true);
                    foreach (var attribute in attributes)
                    {
                        string attrName = attribute.GetType().Name;

                        List<MethodInfo> methodInfoList;
                        if (_attributeAndMethodsDictionary.TryGetValue(attrName, out methodInfoList) == false)
                        {
                            methodInfoList = new List<MethodInfo>();
                            _attributeAndMethodsDictionary.Add(attrName, methodInfoList);
                        }
                        if (methodInfoList.Any(x => x.Name == methodInfo.Name) == false)
                            methodInfoList.Add(methodInfo);
                    }
                }

                currentType = currentType.BaseType;
            } while (currentType != null);
        }

        public List<FieldInfo> GetFieldsByAttributeName(string attributeName, Type objectType)
        {
            List<FieldInfo> result;

            if (_attributeAndFieldsDictionary.TryGetValue(attributeName, out result) == false)
                result = new List<FieldInfo>();

            return result;
        }

        public FieldInfo GetFieldByName(string fieldName, Type objectType)
        {
            return _fieldInfoDictionary[fieldName];
        }

        public List<PropertyInfo> GetPropertyInfos(Type objectType)
        {
            return _fullPropertyInfos;
        }

        public List<PropertyInfo> GetPropertiesByAttributeName(string attributeName, Type objecType)
        {
            List<PropertyInfo> result;

            if (_attributeAndPropertiesDictionary.TryGetValue(attributeName, out result) == false)
                result = new List<PropertyInfo>();

            return result;
        }

        public PropertyInfo GetPropertyByName(string propertyName, Type objectType)
        {
            return _propertyInfoDictionary[propertyName];
        }

        public MethodInfo GetMethodByName(string methodName, Type objectType)
        {
            return _methodInfoDictionary[methodName];
        }

        public List<MethodInfo> GetMethodsByAttributeName(string attributeName, Type objecType)
        {
            List<MethodInfo> result;

            if (_attributeAndMethodsDictionary.TryGetValue(attributeName, out result) == false)
                result = new List<MethodInfo>();

            return result;
        }

        public List<ConstructorInfo> GetPublicConstructors(Type objecType)
        {
            return _constructors;
        }
    }
}
