using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions.Defenitions;

namespace DotNetCraft.Common.Core.Utils.ReflectionExtensions
{
    public interface IReflectionManager
    {
        /// <summary>
        /// Register class.
        /// </summary>
        /// <typeparam name="TObject">Type of the class.</typeparam>
        void RegisterClass<TObject>();

        /// <summary>
        /// Register class
        /// </summary>
        /// <param name="objectType">Type of the class</param>
        void RegisterClass(Type objectType);

        /// <summary>
        /// Get class definition
        /// </summary>
        /// <typeparam name="TObject">Type of the class</typeparam>
        /// <returns>The class definition.</returns>
        ClassDefenition GetClassDefenition<TObject>();

        /// <summary>
        /// Get class definition
        /// </summary>
        /// <param name="objectType">Type of the class</param>
        /// <returns>The class definition.</returns>
        ClassDefenition GetClassDefenition(Type objectType);

        #region Retreiving properties...

        PropertyDefinition SingleOrDefault<TObject>(Type attributeType);

        PropertyDefinition SingleOrDefault(Type objectType, Type attributeType);

        PropertyDefinition Single<TObject>(Type attributeType);

        PropertyDefinition Single(Type objectType, Type attributeType);

        ICollection<PropertyDefinition> SelectAll<TObject>(Type attributeType);

        ICollection<PropertyDefinition> SelectAll(Type objectType, Type attributeType);

        #endregion
    }
}
