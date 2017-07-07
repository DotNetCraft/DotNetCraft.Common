using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetCraft.Common.Core.Utils.ReflectionExtensions.Defenitions
{
    public class ClassDefenition
    {
        private readonly Dictionary<Type, List<PropertyDefinition>> attributesDictionary;

        public string Name { get; private set; }
        public string FullName { get; private set; }
        public Type ClassType { get; private set; }

        public List<PropertyDefinition> Properties { get; private set; }

        public ClassDefenition(Type classType)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            Name = classType.Name;
            FullName = classType.FullName;
            ClassType = classType;
            Properties = new List<PropertyDefinition>();

            attributesDictionary = new Dictionary<Type, List<PropertyDefinition>>();

            PropertyInfo[] propertyInfos = classType.GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                PropertyDefinition propertyDefinition = new PropertyDefinition(propertyInfo);
                Properties.Add(propertyDefinition);
                var propertyAttributes = propertyDefinition.GetAttributes();
                for (int i = 0; i < propertyAttributes.Count; i++)
                {
                    var attribute = propertyAttributes[i];
                    Type attributeType = attribute.GetType();
                    List<PropertyDefinition> cachedProperties;
                    if (attributesDictionary.TryGetValue(attributeType, out cachedProperties) == false)
                    {
                        cachedProperties = new List<PropertyDefinition>();
                        attributesDictionary.Add(attributeType, cachedProperties);
                    }
                    cachedProperties.Add(propertyDefinition);
                }
            }
        }

        #region GetProperty by name...

        private PropertyDefinition GetProperty(string propertyName, bool raiseException)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(propertyName));

            for (int i = 0; i < Properties.Count; i++)
            {
                PropertyDefinition currentProperty = Properties[i];
                if (currentProperty.Name == propertyName)
                    return currentProperty;
            }

            if (raiseException)
                throw new ArgumentOutOfRangeException("propertyName", "There is no such property: " + propertyName);

            return null;
        }

        public PropertyDefinition GetProperty(string propertyName)
        {
            var result = GetProperty(propertyName, true);
            return result;
        }

        public bool TryGetProperty(string propertyName, out PropertyDefinition propertyDefinition)
        {
            propertyDefinition = GetProperty(propertyName, false);
            return propertyDefinition != null;
        }

        #endregion

        #region Get properties by attribute/attributes...

        #region Slow version...
        public List<PropertyDefinition> GetProperties(Type attributeType)
        {
            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));

            List<PropertyDefinition> result = new List<PropertyDefinition>();
            for (int i = 0; i < Properties.Count; i++)
            {
                PropertyDefinition currentProperty = Properties[i];
                bool isPropertyContainsAttribute = currentProperty.ContainsAttribute(attributeType);
                if (isPropertyContainsAttribute)
                    result.Add(currentProperty);
            }

            return result;
        }

        public List<PropertyDefinition> GetProperties(List<Type> attributeTypes)
        {
            if (attributeTypes == null)
                throw new ArgumentNullException(nameof(attributeTypes));
            if (attributeTypes.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(attributeTypes), "Yuo should define at minimun one attribute's type.");

            List<PropertyDefinition> result = new List<PropertyDefinition>();
            for (int i = 0; i < Properties.Count; i++)
            {
                PropertyDefinition currentProperty = Properties[i];
                bool isPropertyContainsAttribute = currentProperty.ContainOneOfAttributes(attributeTypes);
                if (isPropertyContainsAttribute)
                    result.Add(currentProperty);
            }

            return result;
        }
        #endregion

        #region Fast version...
        public List<PropertyDefinition> GetPropertiesFast(List<Type> attributeTypes)
        {
            List<PropertyDefinition> result = new List<PropertyDefinition>();

            foreach (Type attributeType in attributeTypes)
            {
                List<PropertyDefinition> temp = GetPropertiesFast(attributeType);

                foreach (PropertyDefinition propertyDefinition in temp)
                {
                    //TODO: Remove LINQ => it will be faster.
                    if (result.Any(x => x.Name == propertyDefinition.Name) == false)
                        result.Add(propertyDefinition);
                }
            }

            return result;
        }

        public List<PropertyDefinition> GetPropertiesFast(Type attributeType)
        {
            List<PropertyDefinition> result;
            if (attributesDictionary.TryGetValue(attributeType, out result))
                return result;
            return new List<PropertyDefinition>();
        }
        #endregion

        #endregion
    }
}
