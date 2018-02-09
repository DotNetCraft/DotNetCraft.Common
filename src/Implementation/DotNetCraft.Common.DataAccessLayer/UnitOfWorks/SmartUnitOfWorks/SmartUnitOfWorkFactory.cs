using System;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Smart;
using DotNetCraft.Common.Core.Utils.Mapping;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SmartUnitOfWorks
{
    public class SmartUnitOfWorkFactory: UnitOfWorkFactory, ISmartUnitOfWorkFactory
    {
        private readonly IMapperManager mapperManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmartUnitOfWorkFactory(IDataContextFactory dataContextFactory, IMapperManager mapperManager) : base(dataContextFactory)
        {
            if (mapperManager == null)
                throw new ArgumentNullException(nameof(mapperManager));

            this.mapperManager = mapperManager;
        }

        #region Implementation of IUnitOfWorkFactory

        public ISmartUnitOfWork CreateSmartUnitOfWork()
        {
            IDataContextWrapper contextWrapper = dataContextFactory.CreateDataContext();
            ISmartUnitOfWork unitOfWork = new SmartUnitOfWork(contextWrapper, mapperManager);
            return unitOfWork;
        }

        #endregion
    }
}
