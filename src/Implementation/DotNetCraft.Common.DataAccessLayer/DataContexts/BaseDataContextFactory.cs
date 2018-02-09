using System;
using System.Collections.Generic;
using System.Threading;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.DataAccessLayer.Exceptions;

namespace DotNetCraft.Common.DataAccessLayer.DataContexts
{
    public abstract class BaseDataContextFactory<TDataContext, TContextSettings> : IDataContextFactory
        where TDataContext: IDataContextWrapper
        where TContextSettings : IContextSettings
    {
        private readonly IContextSettings contextSettings;

        private readonly Dictionary<int, IDataContextPoolItem> dataContextPool;

        #region Implementation of IDataContextFactory

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BaseDataContextFactory(IContextSettings contextSettings)
        {
            if (contextSettings == null)
                throw new ArgumentNullException(nameof(contextSettings));
            this.contextSettings = contextSettings;

            dataContextPool = new Dictionary<int, IDataContextPoolItem>();
        }

        /// <summary>
        /// Create a new data contextWrapper.
        /// </summary>
        /// <returns>The IDataContextWrapper instance.</returns>
        public IDataContextWrapper CreateDataContext()
        {
            try
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

                TDataContext dataContext;
                lock (dataContextPool)
                {
                    IDataContextPoolItem dataContextPoolItem;
                    if (dataContextPool.TryGetValue(threadId, out dataContextPoolItem))
                    {
                        dataContextPoolItem.IncreaseRef();
                        dataContext = (TDataContext) dataContextPoolItem.DataContextWrapper;
                    }
                    else
                    {
                        TContextSettings settings = (TContextSettings) contextSettings;
                        dataContext = OnCreateDataContext(settings);
                        dataContextPoolItem = new DataContextPoolItem(dataContext);
                        dataContextPool.Add(threadId, dataContextPoolItem);
                    }
                }

                return dataContext;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"DataBaseSettings", contextSettings.ToString()},
                    {"Type of DataContextWrapper", typeof(TDataContext).ToString()},
                    {"Type of Settings", typeof(TContextSettings).ToString()}
                };
                throw new DataAccessLayerException("There was a problem during creating a data contextWrapper", ex, errorParameters);
            }
        }

        /// <summary>
        /// Release an existing data contextWrapper.
        /// </summary>
        /// <param name="dataContextWrapper">The IDataContextWrapper instance</param>
        /// <returns>
        /// True if data contextWrapper has been released. 
        /// False when data contextWrapper hasn't been released but it has been returned into the factory.
        /// </returns>
        public bool ReleaseDataContext(IDataContextWrapper dataContextWrapper)
        {
            try
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

                lock (dataContextPool)
                {
                    IDataContextPoolItem dataContextPoolItem;
                    if (dataContextPool.TryGetValue(threadId, out dataContextPoolItem))
                    {
                        int currentRefCount = dataContextPoolItem.DecreaseRef();
                        if (currentRefCount == 0)
                        {
                            dataContextPool.Remove(threadId);
                            return true;
                        }
                    }
                }

                return false;
            }
            catch(Exception ex)
            {
                Dictionary<string, string> errorParameters = new Dictionary<string, string>
                {
                    {"dataContextWrapper", dataContextWrapper.ToString()},
                    {"Type of DataContextWrapper", typeof(TDataContext).ToString()},
                    {"Type of Settings", typeof(TContextSettings).ToString()}
                };
                throw new DataAccessLayerException("There was a problem during releasering the data contextWrapper", ex, errorParameters);
            }
        }

        protected abstract TDataContext OnCreateDataContext(TContextSettings dataBaseSettings);

        #endregion
    }
}
