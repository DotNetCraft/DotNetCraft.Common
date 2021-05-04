using System;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks
{
    public class UnitOfWorkFactory: IUnitOfWorkFactory
    {
        private readonly ILogger<UnitOfWorkFactory> _logger;

        private readonly IServiceProvider _serviceProvider;
        protected readonly IDataContextFactory dataContextFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitOfWorkFactory(IServiceProvider serviceProvider, IDataContextFactory dataContextFactory, ILogger<UnitOfWorkFactory> logger)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            if (dataContextFactory == null)
                throw new ArgumentNullException(nameof(dataContextFactory));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _serviceProvider = serviceProvider;
            this.dataContextFactory = dataContextFactory;
            _logger = logger;
        }

        #region Implementation of IUnitOfWorkFactory

        public IUnitOfWork CreateUnitOfWork(IUniqueKey uniqueKey = null)
        {
            try
            {
                IDataContextWrapper contextWrapper = dataContextFactory.CreateDataContext();
                IUnitOfWork unitOfWork = ActivatorUtilities.CreateInstance<UnitOfWork>(_serviceProvider, contextWrapper);
                return unitOfWork;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an exception during creating a data contextWrapper.");
                throw new UnitOfWorkException("There was an exception during creating a data contextWrapper.", ex);
            }
        }

        #endregion
    }
}
