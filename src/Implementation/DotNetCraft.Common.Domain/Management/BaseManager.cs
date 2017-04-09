using System;
using DotNetCraft.Common.Core.Domain.Management;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.Utils.Disposal;

namespace DotNetCraft.Common.Domain.Management
{
    public abstract class BaseManager<TManagerConfiguration>: DisposableLoggerObject, IManager
        where TManagerConfiguration: IManagerConfiguration
    {
        /// <summary>
        /// The TManagerConfiguration instance.
        /// </summary>
        protected readonly TManagerConfiguration managerConfiguration;

        #region Implementation of IManager

        /// <summary>
        /// Manager's name
        /// </summary>
        public string Name { get; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="managerConfiguration">The TManagerConfiguration instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        protected BaseManager(TManagerConfiguration managerConfiguration, ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (managerConfiguration == null)
                throw new ArgumentNullException(nameof(managerConfiguration));

            this.managerConfiguration = managerConfiguration;
            Name = managerConfiguration.Name;
        }
    }
}
