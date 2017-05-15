using System;
using System.Collections.Generic;
using System.Reflection;
using DotNetCraft.Common.Core.Utils;

namespace DotNetCraft.Common.Utils
{
    /// <summary>
    /// Simple property manager.
    /// </summary>
    /// <remarks>
    /// Perfect searching logic has not been implemented yet.
    /// Just for now - very simple stuff.
    /// </remarks>
    public class PropertyManager: IPropertyManager
    {
        /// <summary>
        /// The property manager.
        /// </summary>
        private static IPropertyManager propertyManager;

        /// <summary>
        /// Gets or sets the log factory.
        /// Use this to override the factory that is used to create loggers
        /// </summary>
        public static IPropertyManager Manager
        {
            get { return propertyManager ?? new PropertyManager(); }
            set { propertyManager = value; }
        }

        #region Implementation of IPropertyManager

        public PropertyInfo SingleOrDefault<TObjectType>(Type attributeType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return SingleOrDefault(typeof(TObjectType), attributeType, bindingFlags);
        }

        public PropertyInfo SingleOrDefault(Type objectType, Type attributeType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (objectType == null)
                throw new ArgumentNullException(nameof(objectType));
            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));

            PropertyInfo result = null;
            PropertyInfo[] propertyInfos = objectType.GetProperties(bindingFlags);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                var attributes = Attribute.GetCustomAttributes(propertyInfo, attributeType);
                if (attributes.Length > 0)
                {
                    result = propertyInfo;
                    break;
                }
            }

            return result;
        }

        public PropertyInfo Single<TObjectType>(Type attributeType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return Single(typeof(TObjectType), attributeType, bindingFlags);
        }

        public PropertyInfo Single(Type objectType, Type attributeType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (objectType == null)
                throw new ArgumentNullException(nameof(objectType));
            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));


            PropertyInfo result = null;
            PropertyInfo[] propertyInfos = objectType.GetProperties(bindingFlags);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                var attributes = Attribute.GetCustomAttributes(propertyInfo, attributeType);
                if (attributes.Length > 0)
                {
                    if (result != null)
                    {
                        throw new PropertyManagerException("Sequence contains more than one element", new Dictionary<string, string>
                        {
                            {"objectType", objectType.ToString()},
                            {"attributeType", attributeType.ToString()},
                        });
                    }
                    result = propertyInfo;
                }
            }

            if (result == null)
                throw new PropertyManagerException("There is no property with such attribute", new Dictionary<string, string>
                {
                    { "objectType",objectType.ToString()},
                    { "attributeType",attributeType.ToString()},
                });

            return result;
        }

        public ICollection<PropertyInfo> GetList<TObjectType>(Type attributeType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            Type objectType = typeof(TObjectType);
            ICollection<PropertyInfo> result = GetList(objectType, attributeType, bindingFlags);
            return result;
        }

        public ICollection<PropertyInfo> GetList(Type objectType, Type attributeType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (objectType == null)
                throw new ArgumentNullException(nameof(objectType));
            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));


            List<PropertyInfo> result = new List<PropertyInfo>();
            PropertyInfo[] propertyInfos = objectType.GetProperties(bindingFlags);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                var attributes = Attribute.GetCustomAttributes(propertyInfo, attributeType);
                if (attributes.Length > 0)
                {
                    result.Add(propertyInfo);
                }
            }            

            return result;
        }

        #endregion
    }
}
