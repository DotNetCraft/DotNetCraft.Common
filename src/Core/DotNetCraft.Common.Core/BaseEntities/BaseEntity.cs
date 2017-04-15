using System;
using System.Reflection;
using System.Text;
using DotNetCraft.Common.Core.Attributes;

namespace DotNetCraft.Common.Core.BaseEntities
{
    /// <summary>
    /// Base class for all entities.
    /// </summary>
    public abstract class BaseEntity<TEntityId> : IEntity
    { 
        /// <summary>
        /// The identifier.
        /// </summary>
        [Identifier]
        [FieldToString]
        public TEntityId Id { get; set; }       

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                PropertyInfo[] propertyInfos = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    var attributes = Attribute.GetCustomAttributes(propertyInfo, typeof(FieldToStringAttribute));
                    if (attributes.Length > 0)
                    {
                        var propValue = propertyInfo.GetValue(this);
                        stringBuilder.AppendFormat("{0}: {1}", propertyInfo.Name, propValue);
                    }
                }
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                return string.Format("Impossible to show details for the object {0} (Id: {1}): {2}", GetType(), Id, ex);
            }
        }

        #endregion
    }
}
