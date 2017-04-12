using System;
using DotNetCraft.Common.Core.Domain.Management;
using DotNetCraft.Common.Utils.Disposal;

namespace DotNetCraft.Common.Domain.Management
{
    public abstract class BaseManager<TManagerConfiguration>: DisposableObject, IManager
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
        protected BaseManager(TManagerConfiguration managerConfiguration)
        {
            if (managerConfiguration == null)
                throw new ArgumentNullException(nameof(managerConfiguration));

            this.managerConfiguration = managerConfiguration;
            Name = managerConfiguration.Name;
        }
    }
}
