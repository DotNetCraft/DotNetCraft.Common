using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.Core.Utils.Mapping;
using DotNetCraft.Common.Utils.Logging;

namespace DotNetCraft.Common.Utils.Mapping
{
    public class MapperManager : IMapperManager
    {
        private readonly Type enumerableType = typeof(IEnumerable);

        private readonly ICommonLogger logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, object> singleMappers;

        public MapperManager()
        {
            singleMappers = new Dictionary<string, object>();
        }

        #region Implementation of IMapperManager

        private static string CreateKey(Type sourceType, Type destinationType)
        {
            string sourceKey = sourceType.FullName;
            string destinationKey = destinationType.FullName;
            string key = string.Format("{0}_to_{1}", sourceKey, destinationKey);
            return key;
        }

        public bool RegisterMapping<TSource, TDestination>(IMapperInstance<TSource, TDestination> mapperInstance)
        {            
            Type sourceType = typeof(TSource);
            Type destinationType = typeof(TDestination);
            logger.Debug("Registering mapper <{0}> to <{1}>...", sourceType, destinationType);
            string key = CreateKey(sourceType, destinationType);
            singleMappers[key] = mapperInstance;
            logger.Debug("The mapper has been registered.");
            return true;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            Type sourceType = typeof(TSource);
            Type destinationType = typeof(TDestination);

            logger.Debug("Mapping {0} to {1}...", sourceType, destinationType);

            TDestination result;
            if (enumerableType.IsAssignableFrom(sourceType))
            {
                logger.Trace("This is a enumerable types.");
                Type sourceItemType = sourceType.GetGenericArguments().Single();
                Type destinationItemType = destinationType.GetGenericArguments().Single();
                string key = CreateKey(sourceItemType, destinationItemType);
                object mapperInstance;
                bool isExist = singleMappers.TryGetValue(key, out mapperInstance);
                if (isExist == false)
                    throw new KeyNotFoundException(string.Format("There is no mapper instance for converting {0} to {1}", source, typeof(TDestination)));

                MethodInfo mappingMethod = mapperInstance.GetType().GetMethod("Map");

                //TODO: Dependency Resolver !!!
                result = Activator.CreateInstance<TDestination>();
                MethodInfo methodInfo = destinationType.GetMethod("Add");

                logger.Trace("{0}.Add = {1}", destinationType, methodInfo);
                foreach (var item in (IEnumerable)source)
                {
                    var destination = mappingMethod.Invoke(mapperInstance, new[] {item });
                    methodInfo.Invoke(result, new[] { destination });
                }                
            }
            else
            {
                logger.Trace("This is a single object mapping");
                string key = CreateKey(sourceType, destinationType);
                object obj;
                bool isExist = singleMappers.TryGetValue(key, out obj);
                if (isExist == false)
                    throw new KeyNotFoundException(string.Format("There is no mapper instance for converting {0} to {1}", source, typeof(TDestination)));
                IMapperInstance<TSource, TDestination> mapperInstance = (IMapperInstance<TSource, TDestination>)obj;
                result = mapperInstance.Map(source);                
            }

            logger.Debug("Done.");
            return result;
        }

        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            Type sourceType = typeof(TSource);
            Type destinationType = typeof(TDestination);
            string key = CreateKey(sourceType, destinationType);

            object obj;
            bool isExist = singleMappers.TryGetValue(key, out obj);

            if (isExist == false)
                throw new KeyNotFoundException(string.Format("There is no mapper instance for converting {0} to {1}", typeof(TSource), typeof(TDestination)));

            IMapperInstance<TSource, TDestination> mapperInstance = (IMapperInstance<TSource, TDestination>) obj;
            List<TDestination> result = new List<TDestination>();

            foreach (TSource item in source)
            {
                TDestination destination = mapperInstance.Map(item);
                result.Add(destination);
            }

            return result;
        }

        #endregion
    }
}
