using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.DataAccessLayer.DataContexts.UniqueKeys;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using Microsoft.Extensions.Logging;

namespace DotNetCraft.Common.DataAccessLayer.DataContexts
{
    public abstract class BaseDataContextFactory<TDataContext, TContextSettings> : IDataContextFactory
        where TDataContext: IDataContextWrapper
    {
        #region Fields...
        private readonly TContextSettings contextSettings;

        private readonly ILogger<BaseDataContextFactory<TDataContext, TContextSettings>> _logger;

        private readonly Dictionary<string, IDataContextPoolItem> dataContextPool;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BaseDataContextFactory(TContextSettings contextSettings, ILogger<BaseDataContextFactory<TDataContext, TContextSettings>> logger)
        {
            if (contextSettings == null)
                throw new ArgumentNullException(nameof(contextSettings));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this.contextSettings = contextSettings;
            _logger = logger;

            dataContextPool = new Dictionary<string, IDataContextPoolItem>();
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
                _logger.LogDebug("Creating DataContext...");

                IUniqueKey currentUniqueKey = uniqueKey;
                if (currentUniqueKey == null)
                {
                    currentUniqueKey = new ThreadUniqueKey();
                    _logger.LogTrace("UniqueKey was not provided so default one will be used: " + currentUniqueKey);
                }

                _logger.LogTrace("UniqueKey: {0}", currentUniqueKey);

                TDataContext dataContext;
                lock (dataContextPool)
                {
                    IDataContextPoolItem dataContextPoolItem;
                    if (dataContextPool.TryGetValue(currentUniqueKey.Key, out dataContextPoolItem))
                    {
                        int count = dataContextPoolItem.IncreaseRef();
                        dataContext = (TDataContext) dataContextPoolItem.DataContextWrapper;
                        _logger.LogTrace("DataContext has been found in the pool (RefCount: {0})", count);
                    }
                    else
                    {
                        _logger.LogTrace("DataContext has not been found in the pool. Creating...");
                        dataContext = OnCreateDataContext(contextSettings, currentUniqueKey);
                        dataContextPoolItem = new DataContextPoolItem(dataContext);
                        dataContextPool.Add(currentUniqueKey.Key, dataContextPoolItem);
                        _logger.LogTrace("DataContext has been created.");
                    }
                }

                return dataContext;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem during creating a DataContext: {0}", ex.Message);
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
                _logger.LogDebug("Releasing DataContext...");

                IUniqueKey currentUniqueKey = uniqueKey;
                if (currentUniqueKey == null)
                {
                    currentUniqueKey = new ThreadUniqueKey();
                    _logger.LogTrace("UniqueKey was not provided so default one will be used: " + currentUniqueKey);
                }

                _logger.LogTrace("UniqueKey: {0}", currentUniqueKey);

                lock (dataContextPool)
                {
                    IDataContextPoolItem dataContextPoolItem;
                    if (dataContextPool.TryGetValue(currentUniqueKey.Key, out dataContextPoolItem))
                    {
                        _logger.LogTrace("DataContext has been found: {0}", dataContextPoolItem);
                        int currentRefCount = dataContextPoolItem.DecreaseRef();
                        if (currentRefCount == 0)
                        {                            
                            dataContextPool.Remove(currentUniqueKey.Key);
                            _logger.LogDebug("As DataContext doesn't have any references it's been deleted from the pool");
                            return true;
                        }
                    }
                }

                _logger.LogDebug("Pool doesn't have such DataContext => do nothing.");
                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "There was a problem during releasering DataContext: {0}", ex.Message);
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"dataContextWrapper", dataContextWrapper.ToString()},
                    {"Type of DataContextWrapper", typeof(TDataContext).ToString()},
                    {"Type of Settings", typeof(TContextSettings).ToString()}
                };
                throw new DataAccessLayerException("There was a problem during releasering DataContext", ex, errorParameters);
            }
        }

        protected abstract TDataContext OnCreateDataContext(TContextSettings dataBaseSettings, IUniqueKey key);

        #endregion
    }
}
