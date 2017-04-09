using System;
using DotNetCraft.Common.Core.Domain.Management;

namespace DotNetCraft.Common.Domain.Management.StopwatchManagement
{
    /// <summary>
    /// Configuration for the Stopwatch manager.
    /// </summary>
    /// <remarks>
    /// Example of configuration: <StopwatchConfig Name="SimpleManager" StartImmedeatly="true" SleepTime="15"/>
    /// </remarks>
    public class StopwatchManagerConfig: IBackgroundManagerConfiguration
    {
        #region Properties...

        /// <summary>
        /// Manager's name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Flag shows will manager show statistic or not.
        /// </summary>
        public bool StartImmediately { get; set; }

        /// <summary>
        /// Sleep time that will be used in the StopwatchManager's background worker.
        /// </summary>
        public TimeSpan SleepTime { get; set; }
        
        #endregion       
    }
}
