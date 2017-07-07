using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetCraft.Common.Core.Utils.ReflectionExtensions.Defenitions
{
    public class PropertyDefinition
    {
        private readonly PropertyInfo propertyInfo;

        private readonly List<Attribute> attributes;

        public string Name { get; private set; }
        public Type PropertyType { get; private set; }

        public PropertyInfo PropertyInfo
        {
            get { return propertyInfo; }
        }

        public PropertyDefinition(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            this.propertyInfo = propertyInfo;
            Name = propertyInfo.Name;
            PropertyType = propertyInfo.PropertyType;

            Attribute[] attributes = Attribute.GetCustomAttributes(propertyInfo);
            this.attributes = new List<Attribute>();
            this.attributes.AddRange(attributes);
        }

        public object GetValue(object instance)
        {
            //TODO: >= .NET 4.5 PropertyInfo.GetValue(instance);
            var result = propertyInfo.GetValue(instance, null);
            return result;
        }

        public TValueType GetValue<TValueType>(object instance)
            where TValueType : Type
        {
            //TODO: >= .NET 4.5 PropertyInfo.GetValue(instance);
            TValueType result = (TValueType)propertyInfo.GetValue(instance, null);
            return result;
        }

        public PropertyDefinition Clone()
        {
            PropertyDefinition result = new PropertyDefinition(propertyInfo);
            return result;
        }

        #region Overrides of Object

        public override string ToString()
        {
            return Name;
        }

        #endregion

        public bool ContainsAttribute(Type attributeType)
        {
            for (int i = 0; i < attributes.Count; i++)
            {
                var currentAttribute = attributes[i];
                Type currentAttributeType = currentAttribute.GetType();
                if (currentAttributeType == attributeType)
                    return true;
            }
            return false;
        }

        public bool ContainOneOfAttributes(List<Type> attributeTypes)
        {
            for (int i = 0; i < attributeTypes.Count; i++)
            {
                Type attributeType = attributeTypes[i];
                bool currentResult = ContainsAttribute(attributeType);
                if (currentResult)
                    return true;
            }
            return false;
        }

        public List<Attribute> GetAttributes()
        {
            return attributes;
        }
    }
}
