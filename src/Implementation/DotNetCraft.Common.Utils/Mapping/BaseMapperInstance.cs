using System;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.Core.Utils.Mapping;
using DotNetCraft.Common.Utils.Logging;

namespace DotNetCraft.Common.Utils.Mapping
{
    public abstract class BaseMapperInstance<TSource, TDestination> : IMapperInstance<TSource, TDestination>
    {
        private readonly ICommonLogger logger = LogManager.GetCurrentClassLogger();

        #region Implementation of IMapperInstance

        public TDestination Map(TSource source)
        {
            logger.Trace("Converting {0} into the {1}...", source, typeof(TDestination));
            try
            {
                TDestination result = OnMap(source);
                logger.Trace("Object has been converted");
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was an exception during converting {0} to {1}", typeof(TSource), typeof(TDestination));
                throw;
            }            
        }

        protected abstract TDestination OnMap(TSource source);

        #endregion
    }
}
