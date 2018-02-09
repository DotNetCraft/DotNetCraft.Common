using System;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer.Exceptions;
using DotNetCraft.Common.Utils.Logging;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks
{
    public class UnitOfWorkFactory: IUnitOfWorkFactory
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

        public IUnitOfWork CreateUnitOfWork()
        {
            try
            {
                IDataContextWrapper contextWrapper = dataContextFactory.CreateDataContext();
                IUnitOfWork unitOfWork = new UnitOfWork(contextWrapper);
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
