using System;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.Utils.Logging;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks
{
    public class UnitOfWorkFactory: BaseLoggerObject, IUnitOfWorkFactory
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

        public IUnitOfWork CreateUnitOfWork(IDataBaseSettings dataBaseSettings)
        {
            IDataContext context = dataContextFactory.CreateDataContext(dataBaseSettings);
            IUnitOfWork unitOfWork = new UnitOfWork(context, logger);
            return unitOfWork;
        }

        #endregion
    }
}
