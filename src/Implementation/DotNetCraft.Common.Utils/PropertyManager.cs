using System;
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
        #region Implementation of IPropertyManager

        public PropertyInfo SingleOrDefault(Type objectType, Type attributeType)
        {
            PropertyInfo result = null;
            PropertyInfo[] propertyInfos = objectType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
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

        public PropertyInfo Single(Type objectType, Type attributeType)
        {
            PropertyInfo propertyInfo = SingleOrDefault(objectType, attributeType);
            if (propertyInfo == null)
                throw new Exception("TODO");

            return propertyInfo;
        }

        #endregion
    }
}
