using System;
using System.Runtime.CompilerServices;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.Utils.Logging;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ICommonLogger logger = LogManager.GetCurrentClassLogger();

        protected readonly IDataContextFactory dataContextFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitOfWorkFactory(IDataContextFactory dataContextFactory)
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
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                return unitOfWork;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was an exception during creating a data context.");
                throw new UnitOfWorkException("There was an exception during creating a data context.", ex);
            }
        }

        #endregion
    }
}
