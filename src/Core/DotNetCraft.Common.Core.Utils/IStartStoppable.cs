namespace DotNetCraft.Common.Core.Utils
{
    /// <summary>
    /// Interface shows that object should be started before using and should be stopped after it.
    /// </summary>
    public interface IStartStoppable
    {
        /// <summary>
        /// Flag shows that object is running
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Start manager.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop.
        /// </summary>
        void Stop();
    }
}
