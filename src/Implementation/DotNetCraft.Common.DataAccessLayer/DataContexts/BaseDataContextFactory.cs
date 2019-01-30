using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.DataContexts.UniqueKeys;
using DotNetCraft.Common.DataAccessLayer.Exceptions;

namespace DotNetCraft.Common.DataAccessLayer.DataContexts
{
    public abstract class BaseDataContextFactory<TDataContext, TContextSettings> : IDataContextFactory
        where TDataContext: IDataContextWrapper
    {
        #region Fields...
        private readonly TContextSettings contextSettings;

        private readonly ICommonLogger logger;

        private readonly Dictionary<IUniqueKey, IDataContextPoolItem> dataContextPool;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BaseDataContextFactory(TContextSettings contextSettings, ICommonLoggerFactory loggerFactory)
        {
            if (contextSettings == null)
                throw new ArgumentNullException(nameof(contextSettings));
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            this.contextSettings = contextSettings;
            logger = loggerFactory.Create<BaseDataContextFactory<TDataContext, TContextSettings>>();

            dataContextPool = new Dictionary<IUniqueKey, IDataContextPoolItem>();
        }

        #region Implementation of IDataContextFactory

        /// <summary>
        /// Create a new data contextWrapper.
        /// </summary>
        /// <returns>The IDataContextWrapper instance.</returns>
        public IDataContextWrapper CreateDataContext(IUniqueKey uniqueKey = null)
        {
            try
            {
                logger.Debug("Createing DataContext...");
                IUniqueKey key = uniqueKey;
                if (key == null)
                {
                    key = new ThreadUniqueKey();
                    logger.Trace("UniqueKey was not provided so default one will be used: " + key);
                }

                logger.Trace("UniqueKey: {0}", key);

                TDataContext dataContext;
                lock (dataContextPool)
                {
                    IDataContextPoolItem dataContextPoolItem;
                    if (dataContextPool.TryGetValue(key, out dataContextPoolItem))
                    {
                        int count = dataContextPoolItem.IncreaseRef();
                        dataContext = (TDataContext) dataContextPoolItem.DataContextWrapper;
                        logger.Trace("DataContext has been found in the pool (RefCount: {0])", count);
                    }
                    else
                    {
                        logger.Trace("DataContext has not been found in the pool. Creating...");
                        TContextSettings settings = (TContextSettings) contextSettings;
                        dataContext = OnCreateDataContext(settings);
                        dataContextPoolItem = new DataContextPoolItem(dataContext);
                        dataContextPool.Add(key, dataContextPoolItem);
                        logger.Trace("DataContext has been created.");
                    }
                }

                return dataContext;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was a problem during creating a DataContext: {0}", ex.Message);
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"DataBaseSettings", contextSettings.ToString()},
                    {"Type of DataContextWrapper", typeof(TDataContext).ToString()},
                    {"Type of Settings", typeof(TContextSettings).ToString()}
                };
                throw new DataAccessLayerException("There was a problem during creating a DataContext", ex, errorParameters);
            }
        }

        /// <summary>
        /// Release an existing data contextWrapper.
        /// </summary>
        /// <param name="dataContextWrapper">The IDataContextWrapper instance</param>
        /// <param name="uniqueKey">The unique key</param>
        /// <returns>
        /// True if data contextWrapper has been released. 
        /// False when data contextWrapper hasn't been released but it has been returned into the factory.
        /// </returns>
        public bool ReleaseDataContext(IDataContextWrapper dataContextWrapper, IUniqueKey uniqueKey = null)
        {
            try
            {
                logger.Debug("Releasing DataContext...");
                IUniqueKey key = uniqueKey;
                if (key == null)
                {
                    key = new ThreadUniqueKey();
                    logger.Trace("UniqueKey was not provided so default one will be used: " + key);
                }

                logger.Trace("UniqueKey: {0}", key);

                lock (dataContextPool)
                {
                    IDataContextPoolItem dataContextPoolItem;
                    if (dataContextPool.TryGetValue(key, out dataContextPoolItem))
                    {
                        logger.Trace("DataContext has been found: {0}", dataContextPoolItem);
                        int currentRefCount = dataContextPoolItem.DecreaseRef();
                        if (currentRefCount == 0)
                        {                            
                            dataContextPool.Remove(key);
                            logger.Debug("As DataContext doesn't have any references it's been deleted from the pool");
                            return true;
                        }
                    }
                }

                logger.Debug("Pool doesn't have such DataContext => do nothing.");
                return false;
            }
            catch(Exception ex)
            {
                logger.Error(ex, "There was a problem during releasering DataContext: {0}", ex.Message);
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"dataContextWrapper", dataContextWrapper.ToString()},
                    {"Type of DataContextWrapper", typeof(TDataContext).ToString()},
                    {"Type of Settings", typeof(TContextSettings).ToString()}
                };
                throw new DataAccessLayerException("There was a problem during releasering DataContext", ex, errorParameters);
            }
        }

        protected abstract TDataContext OnCreateDataContext(TContextSettings dataBaseSettings);

        #endregion
    }
}
