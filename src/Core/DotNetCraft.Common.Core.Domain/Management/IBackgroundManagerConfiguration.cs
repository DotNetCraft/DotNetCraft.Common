using System;

namespace DotNetCraft.Common.Core.Domain.Management
{
    public interface IBackgroundManagerConfiguration: IManagerConfiguration
    {
        /// <summary>
        /// Flag shows will manager show statistic or not.
        /// </summary>
        bool StartImmediately { get; set; }

        /// <summary>
        /// Sleep time that will be used in the StopwatchManager's background worker.
        /// </summary>
        TimeSpan SleepTime { get; set; }
    }
}
