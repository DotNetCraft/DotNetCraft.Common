using System;
using DotNetCraft.Common.Core.Domain.Management;
using DotNetCraft.Common.Utils.Disposal;
using Microsoft.Extensions.Logging;

namespace DotNetCraft.Common.Domain.Management
{
    public abstract class BaseManager<TManagerConfiguration>: DisposableObject, IManager
        where TManagerConfiguration: IManagerConfiguration
    {
        /// <summary>
        /// The TManagerConfiguration instance.
        /// </summary>
        protected readonly TManagerConfiguration managerConfiguration;

        protected readonly ILogger<BaseManager<TManagerConfiguration>> _logger;

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
        protected BaseManager(TManagerConfiguration managerConfiguration, ILogger<BaseManager<TManagerConfiguration>> logger)
        {
            if (managerConfiguration == null)
                throw new ArgumentNullException(nameof(managerConfiguration));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            this.managerConfiguration = managerConfiguration;
            _logger = logger;
            Name = managerConfiguration.Name;
        }
    }
}
