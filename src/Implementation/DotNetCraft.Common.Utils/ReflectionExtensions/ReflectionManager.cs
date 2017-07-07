using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions.Defenitions;

namespace DotNetCraft.Common.Utils.ReflectionExtensions
{
    public class ReflectionManager : IReflectionManager
    {
        private readonly Dictionary<Type, ClassDefenition> classDefenitions;
        private readonly object syncObject = new object();

        #region Singleton if need

        private static readonly object instanceSyncObject = new object();

        private static IReflectionManager instance;

        public static IReflectionManager Manager
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceSyncObject)
                    {
                        if (instance == null)
                        {
                            instance = new ReflectionManager();
                        }
                    }
                }
                return instance;
            }
        }


        #endregion

        #region Constructor...
        /// <summary>
        /// Constructor.
        /// </summary>
        public ReflectionManager()
        {
            classDefenitions = new Dictionary<Type, ClassDefenition>();
        }
        #endregion

        #region Registration...
        /// <summary>
        /// Register class.
        /// </summary>
        /// <typeparam name="TObject">Type of the class.</typeparam>
        public void RegisterClass<TObject>()
        {
            Type classType = typeof(TObject);
            LoadOrCreateClassDefenition(classType);
        }

        /// <summary>
        /// Register class
        /// </summary>
        /// <param name="objectType">Type of the class</param>
        public void RegisterClass(Type objectType)
        {
            LoadOrCreateClassDefenition(objectType);
        }
        #endregion

        #region Retreiving class definitions...

        private ClassDefenition LoadOrCreateClassDefenition(Type objectType)
        {
            lock (syncObject)
            {
                ClassDefenition classDefenition;
                if (classDefenitions.TryGetValue(objectType, out classDefenition))
                    return classDefenition;

                classDefenition = new ClassDefenition(objectType);
                classDefenitions.Add(objectType, classDefenition);
                return classDefenition;
            }
        }

        /// <summary>
        /// Get class definition
        /// </summary>
        /// <typeparam name="TObject">Type of the class</typeparam>
        /// <returns>The class definition.</returns>
        public ClassDefenition GetClassDefenition<TObject>()
        {
            Type objectType = typeof(TObject);
            ClassDefenition classDefenition = GetClassDefenition(objectType);
            return classDefenition;
        }

        /// <summary>
        /// Get class definition
        /// </summary>
        /// <param name="objectType">Type of the class</param>
        /// <returns>The class definition.</returns>
        public ClassDefenition GetClassDefenition(Type objectType)
        {
            ClassDefenition classDefenition;
            if (classDefenitions.TryGetValue(objectType, out classDefenition))
                return classDefenition;

            classDefenition = LoadOrCreateClassDefenition(objectType);
            return classDefenition;
        }
        #endregion

        #region Retreiving properties...

        public PropertyDefinition SingleOrDefault<TObject>(Type attributeType)
        {
            PropertyDefinition result = SingleOrDefault(typeof(TObject), attributeType);
            return result;
        }

        public PropertyDefinition SingleOrDefault(Type objectType, Type attributeType)
        {
            if (objectType == null)
                throw new ArgumentNullException(nameof(objectType));
            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));

            ClassDefenition classDefinition = GetClassDefenition(objectType);
            List<PropertyDefinition> properties = classDefinition.GetProperties(attributeType);

            if (properties.Count == 0)
                return null;

            return properties[0];
        }

        public PropertyDefinition Single<TObject>(Type attributeType) 
        {
            PropertyDefinition result = Single(typeof(TObject), attributeType);
            return result;
        }

        public PropertyDefinition Single(Type objectType, Type attributeType)
        {
            if (objectType == null)
                throw new ArgumentNullException(nameof(objectType));
            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));

            ClassDefenition classDefinition = GetClassDefenition(objectType);
            List<PropertyDefinition> properties = classDefinition.GetProperties(attributeType);

            if (properties.Count > 1)
            {
                throw new ReflectionManagerException("Sequence contains more than one element", new Dictionary<string, string>
                {
                    {"objectType", objectType.ToString()},
                    {"attributeType", attributeType.ToString()},
                });
            }

            if (properties.Count == 0)
            {
                throw new ReflectionManagerException("There is no property with such attribute", new Dictionary<string, string>
                {
                    { "objectType",objectType.ToString()},
                    { "attributeType",attributeType.ToString()},
                });
            }

            return properties[0];
        }

        public ICollection<PropertyDefinition> SelectAll<TObject>(Type attributeType)
        {
            var result = SelectAll(typeof(TObject), attributeType);
            return result;
        }

        public ICollection<PropertyDefinition> SelectAll(Type objectType, Type attributeType)
        {
            if (objectType == null)
                throw new ArgumentNullException(nameof(objectType));
            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));

            ClassDefenition classDefinition = GetClassDefenition(objectType);
            List<PropertyDefinition> properties = classDefinition.GetProperties(attributeType);

            return properties;
        }
        #endregion
    }
}
