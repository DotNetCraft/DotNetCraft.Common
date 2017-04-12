using System;
using System.Collections.Generic;
using System.Text;
using DotNetCraft.Common.Core.Domain.Management.StopwatchManager;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.Utils.Logging;

namespace DotNetCraft.Common.Domain.Management.StopwatchManagement
{
    /// <summary>
    /// This is a simple Stopwatch manager. 
    /// It can be used for calculation how many times methods have been executed and how many times do these execution take.
    /// </summary>
    public class StopwatchManager : BaseBackgroundManager<StopwatchManagerConfig>, IStopwatchManager
    {
        private readonly ICommonLogger logger = LogManager.GetCurrentClassLogger();

        #region Fields...

        /// <summary>
        /// Contains information about timers that have been executed.
        /// </summary>
        private readonly Dictionary<string, StopwatchInfo> stopwatchDictionary;

        /// <summary>
        /// <c>Object</c> for synchronization.
        /// </summary>
        private readonly object syncObject = new object();

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        public StopwatchManager(StopwatchManagerConfig config)
            : base(config)
        {
            stopwatchDictionary = new Dictionary<string, StopwatchInfo>();
        }

        #endregion


        #region Implementation of IStopwatchManager

        #region Start timer / Stop timer...

        /// <summary>
        /// Start timer using timer name.
        /// </summary>
        /// <param name="timerName">Timer name.</param>
        public void StartTimer(string timerName)
        {
            lock (syncObject)
            {
                StopwatchInfo stopwatchInfo;
                if (stopwatchDictionary.ContainsKey(timerName))
                {
                    stopwatchInfo = stopwatchDictionary[timerName];
                }
                else
                {
                    stopwatchInfo = new StopwatchInfo(timerName);
                    stopwatchDictionary.Add(timerName, stopwatchInfo);
                }
                stopwatchInfo.Start();
            }
        }

        /// <summary>
        /// Stop timer using timer name.
        /// </summary>
        /// <param name="timerName">Timer name.</param>
        /// <exception cref="IndexOutOfRangeException">There is no timer by name <paramref name="timerName"/>.</exception>
        public void StopTimer(string timerName)
        {
            lock (syncObject)
            {
                if (stopwatchDictionary.ContainsKey(timerName))
                {
                    StopwatchInfo stopwatchInfo = stopwatchDictionary[timerName];
                    stopwatchInfo.Stop();
                }
                else
                {
                    string msg = string.Format("There is no timer by name {0}.", timerName);
                    throw new IndexOutOfRangeException(msg);
                }
            }
        }
        #endregion

        #region Display statistics...
        /// <summary>
        /// Display all statistics that manager has.
        /// </summary>
        public void DisplayAllStatistic()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("-====== Statistic ======-");
            lock (syncObject)
            {
                foreach (StopwatchInfo stopwatchInfo in stopwatchDictionary.Values)
                {
                    stringBuilder.AppendLine(string.Format("Info: {0}", stopwatchInfo));
                }
            }
            stringBuilder.AppendLine("-==== End statistic ====-");

            logger.Debug(stringBuilder.ToString());
        }

        /// <summary>
        /// Display statistic that manager has for current timer.
        /// </summary>
        /// <param name="timerName">Timer name.</param>
        public void DisplayStatistic(string timerName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            lock (syncObject)
            {
                if (stopwatchDictionary.ContainsKey(timerName))
                {
                    StopwatchInfo stopwatchInfo = stopwatchDictionary[timerName];

                    stringBuilder.AppendLine("-====== Statistic for " + timerName + " ======-");
                    stringBuilder.AppendLine(string.Format("Info: {0}", stopwatchInfo));
                    stringBuilder.AppendLine("-==== End statistic for " + timerName + " ====-");
                }
                else
                {
                    stringBuilder.AppendLine("There is no information about " + timerName);
                }
            }

            logger.Debug(stringBuilder.ToString());
        }
        #endregion

        #endregion

        #region Overrides of BaseBackgroundManager

        /// <summary>
        /// Occurs when manager should do background work.
        /// </summary>
        protected override void OnBackroundExecution()
        {
            if (stopwatchDictionary == null)
                return;

            DisplayAllStatistic();
        }

        /// <summary>
        /// Occurs when background work has been completed.
        /// </summary>
        protected override void OnBackgorundWorkCompleted()
        {
            if (stopwatchDictionary == null)
                return;

            DisplayAllStatistic();
            base.OnBackgorundWorkCompleted();
        }

        #endregion        
    }
}
