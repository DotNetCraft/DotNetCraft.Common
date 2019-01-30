using System;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks
{
    public class UnitOfWorkFactory: IUnitOfWorkFactory
    {
        private readonly ICommonLogger logger;

        protected readonly IDataContextFactory dataContextFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitOfWorkFactory(IDataContextFactory dataContextFactory, ICommonLoggerFactory loggerFactory)
        {
            if (dataContextFactory == null)
                throw new ArgumentNullException(nameof(dataContextFactory));
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            this.dataContextFactory = dataContextFactory;
            logger = loggerFactory.Create<UnitOfWorkFactory>();
        }

        #region Implementation of IUnitOfWorkFactory

        public IUnitOfWork CreateUnitOfWork(IUniqueKey uniqueKey = null)
        {
            try
            {
                IDataContextWrapper contextWrapper = dataContextFactory.CreateDataContext();
                IUnitOfWork unitOfWork = new UnitOfWork(contextWrapper, logger);
                return unitOfWork;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was an exception during creating a data contextWrapper.");
                throw new UnitOfWorkException("There was an exception during creating a data contextWrapper.", ex);
            }
        }

        #endregion
    }
}
