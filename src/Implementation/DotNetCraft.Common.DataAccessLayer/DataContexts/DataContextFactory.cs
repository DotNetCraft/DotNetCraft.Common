﻿using System;
using System.Collections.Generic;
using System.Threading;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.DataAccessLayer.Exceptions;

namespace DotNetCraft.Common.DataAccessLayer.DataContexts
{
    public abstract class DataContextFactory<TDataContext, TContextSettings> : IDataContextFactory
        where TDataContext: IDataContext
        where TContextSettings : IContextSettings
    {

        private readonly Dictionary<int, IDataContextPoolItem> dataContextPool;

        #region Implementation of IDataContextFactory

        /// <summary>
        /// Constructor.
        /// </summary>
        protected DataContextFactory()
        {
            dataContextPool = new Dictionary<int, IDataContextPoolItem>();
        }

        /// <summary>
        /// Create a new data context.
        /// </summary>
        /// <param name="contextSettings"></param>
        /// <returns>The IDataContext instance.</returns>
        public IDataContext CreateDataContext(IContextSettings contextSettings)
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
                        dataContext = (TDataContext) dataContextPoolItem.DataContext;
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
                    {"Type of DataContext", typeof(TDataContext).ToString()},
                    {"Type of Settings", typeof(TContextSettings).ToString()}
                };
                throw new DataAccessLayerException("There was a problem during creating a data context", ex, errorParameters);
            }
        }

        /// <summary>
        /// Release an existing data context.
        /// </summary>
        /// <param name="dataContext">The IDataContext instance</param>
        /// <returns>
        /// True if data context has been released. 
        /// False when data context hasn't been released but it has been returned into the factory.
        /// </returns>
        public bool ReleaseDataContext(IDataContext dataContext)
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
                    {"dataContext", dataContext.ToString()},
                    {"Type of DataContext", typeof(TDataContext).ToString()},
                    {"Type of Settings", typeof(TContextSettings).ToString()}
                };
                throw new DataAccessLayerException("There was a problem during releasering the data context", ex, errorParameters);
            }
        }

        protected abstract TDataContext OnCreateDataContext(TContextSettings dataBaseSettings);

        #endregion
    }
}
