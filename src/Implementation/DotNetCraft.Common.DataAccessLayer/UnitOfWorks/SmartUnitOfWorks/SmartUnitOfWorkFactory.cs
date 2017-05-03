using System;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Smart;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SmartUnitOfWorks
{
    public class SmartUnitOfWorkFactory: UnitOfWorkFactory, ISmartUnitOfWorkFactory
    {
        private readonly IEntityModelMapper entityModelMapper;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public SmartUnitOfWorkFactory(IDataContextFactory dataContextFactory, IEntityModelMapper entityModelMapper) : base(dataContextFactory)
        {
            if (entityModelMapper == null)
                throw new ArgumentNullException(nameof(entityModelMapper));

            this.entityModelMapper = entityModelMapper;
        }

        #region Implementation of IUnitOfWorkFactory

        public ISmartUnitOfWork CreateSmartUnitOfWork(IContextSettings contextSettings)
        {
            IDataContext context = dataContextFactory.CreateDataContext(contextSettings);
            ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(context, entityModelMapper);
            return unitOfWork;
        }

        #endregion
    }
}
