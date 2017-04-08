using System;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks
{
    public class UnitOfWorkFactory : BaseLoggerObject, IUnitOfWorkFactory
    {
        protected readonly IDataContextFactory dataContextFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public UnitOfWorkFactory(IDataContextFactory dataContextFactory, ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (dataContextFactory == null)
                throw new ArgumentNullException(nameof(dataContextFactory));

            this.dataContextFactory = dataContextFactory;
        }

        #region Implementation of IUnitOfWorkFactory

        public IUnitOfWork CreateUnitOfWork(IContextSettings contextSettings)
        {
            try
            {
                IDataContext context = dataContextFactory.CreateDataContext(contextSettings);
                IUnitOfWork unitOfWork = new UnitOfWork(context, logger);
                return unitOfWork;
            }
            catch (Exception ex)
            {
                logger.WriteError(ex, "There was an exception during creating a data context.");
                throw new UnitOfWorkException("There was an exception during creating a data context.", ex);
            }
        }

        #endregion
    }
}
