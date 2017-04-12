using System;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Smart;
using DotNetCraft.Common.Core.Utils;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SmartUnitOfWorks
{
    public class SmartUnitOfWorkFactory: UnitOfWorkFactory, ISmartUnitOfWorkFactory
    {
        private readonly IDotNetCraftMapper dotNetCraftMapper;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public SmartUnitOfWorkFactory(IDataContextFactory dataContextFactory, IDotNetCraftMapper dotNetCraftMapper) : base(dataContextFactory)
        {
            if (dotNetCraftMapper == null)
                throw new ArgumentNullException(nameof(dotNetCraftMapper));

            this.dotNetCraftMapper = dotNetCraftMapper;
        }

        #region Implementation of IUnitOfWorkFactory

        public ISmartUnitOfWork CreateSmartUnitOfWork(IContextSettings contextSettings)
        {
            IDataContext context = dataContextFactory.CreateDataContext(contextSettings);
            ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(context, dotNetCraftMapper);
            return unitOfWork;
        }

        #endregion
    }
}
