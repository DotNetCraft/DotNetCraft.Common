namespace DotNetCraft.Common.Core.Domain.Management.StopwatchManager
{
    /// <summary>
    /// Interface shows that <c>object</c> is a stopwatch manager.
    /// </summary>
    public interface IStopwatchManager : IManager
    {
        #region Start timer / Stop timer...

        /// <summary>
        /// Start timer using timer name.
        /// </summary>
        /// <param name="timerName">Timer name.</param>
        void StartTimer(string timerName);

        /// <summary>
        /// Stop timer using timer name.
        /// </summary>
        /// <param name="timerName">Timer name.</param>
        void StopTimer(string timerName);

        #endregion

        #region Display statistics...

        /// <summary>
        /// Display all statistics that manager has.
        /// </summary>
        void DisplayAllStatistic();

        /// <summary>
        /// Display statistic that manager has for current timer.
        /// </summary>
        /// <param name="timerName">Timer name.</param>
        void DisplayStatistic(string timerName);

        #endregion
    }
}
