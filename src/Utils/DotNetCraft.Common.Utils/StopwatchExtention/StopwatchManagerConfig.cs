using System;

namespace DotNetCraft.Common.Utils.StopwatchExtention
{
    /// <summary>
    /// Configuration for the Stopwatch manager.
    /// </summary>
    /// <remarks>
    /// Example of configuration: <StopwatchConfig UseBackgroundWorker="true" SleepTime="15"/>
    /// </remarks>
    public class StopwatchManagerConfig
    {
        #region Fields...

        /// <summary>
        /// Flag shows will manager show statistic or not.
        /// </summary>
        public bool UseBackgroundWorker { get; set; }

        /// <summary>
        /// Sleep time that will be used in the StopwatchManager's background worker.
        /// </summary>
        public TimeSpan SleepTime { get; set; }

        #endregion       
    }
}
