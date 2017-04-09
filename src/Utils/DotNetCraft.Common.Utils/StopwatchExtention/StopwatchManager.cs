using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.Core.Utils.StopwatchManager;
using DotNetCraft.Common.Utils.Logging;

namespace DotNetCraft.Common.Utils.StopwatchExtention
{
    /// <summary>
    /// This is a simple Stopwatch manager. 
    /// It can be used for calculation how many times methods have been executed and how many times do these execution take.
    /// </summary>
    public class StopwatchManager : BaseLoggerObject, IStopwatchManager
    {
        #region Fields...

        /// <summary>
        /// Contains information about timers that have been executed.
        /// </summary>
        private readonly Dictionary<string, StopwatchInfo> stopwatchDictionary;

        /// <summary>
        /// <c>Object</c> for synchronization.
        /// </summary>
        private readonly object syncObject = new object();

        /// <summary>
        /// The <see cref="CancellationTokenSource"/> instance.
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// <see cref="Task"/> where background work will be doing.
        /// </summary>
        private readonly Task worker;

        /// <summary>
        /// The stopwatch manager configuration.
        /// </summary>
        private readonly StopwatchManagerConfig config;

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="config"/> is <see langword="null" />.</exception>
        public StopwatchManager(StopwatchManagerConfig config, ICommonLogger logger)
            : base(logger)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            stopwatchDictionary = new Dictionary<string, StopwatchInfo>();
            cancellationTokenSource = new CancellationTokenSource();

            this.config = config;

            if (config.UseBackgroundWorker)
            {
                CancellationToken cancellationToken = cancellationTokenSource.Token;
                worker = new Task(OnWorking, cancellationToken);
                worker.Start();
            }
        }

        #endregion

        /// <summary>
        /// This is a background thread.
        /// </summary>
        /// <param name="obj">The object. In deed, it is the <see cref="CancellationToken"/> instance.</param>
        private void OnWorking(object obj)
        {
            CancellationToken cancellationToken = (CancellationToken)obj;

            TimeSpan sleepTime = config.SleepTime;
            while (cancellationToken.IsCancellationRequested == false)
            {
                DisplayAllStatistic();
                cancellationToken.WaitHandle.WaitOne(sleepTime);
            }

            DisplayAllStatistic();
        }

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

            logger.WriteDebug(stringBuilder.ToString());
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

            logger.WriteDebug(stringBuilder.ToString());
        }
        #endregion

        #endregion

        #region Implementation of IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            if (worker == null || worker.IsCanceled)
                return;

            cancellationTokenSource.Cancel();
        }

        #endregion
    }
}
