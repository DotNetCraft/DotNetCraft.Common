using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DotNetCraft.Common.Domain.Management.StopwatchManagement
{
    /// <summary>
    /// Contains information about timer that has been executed.
    /// </summary>
    public class StopwatchInfo
    {
        /// <summary>
        /// Timer name.
        /// </summary>
        private readonly string timerName;

        private readonly Dictionary<Int64, Stopwatch> threads;

        private Int64 invocationCount;

        public StopwatchInfo(string timerName)
        {
            if (string.IsNullOrWhiteSpace(timerName))
                throw new ArgumentException("Value cannot be null or whitespace.", "timerName");

            this.timerName = timerName;

            threads = new Dictionary<long, Stopwatch>();
        }

        public void Start()
        {
            Int64 threadId = Thread.CurrentThread.ManagedThreadId;

            if (threads.ContainsKey(threadId))
                throw new IndexOutOfRangeException("Recursion detected. This library doesn't support it.");

            Stopwatch stopwatch = Stopwatch.StartNew();
            threads.Add(threadId, stopwatch);
        }

        public void Stop()
        {
            Int64 threadId = Thread.CurrentThread.ManagedThreadId;

            if (threads.ContainsKey(threadId) == false)
                throw new IndexOutOfRangeException("There is no timer for the thread = " + threadId);

            Stopwatch stopwatch = threads[threadId];
            threads.Remove(threadId);
            stopwatch.Stop();

            invocationCount++;
        }

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Name: {0}; Active: {1}; Executed: {2}", timerName, threads.Count, invocationCount);
        }

        #endregion
    }
}
