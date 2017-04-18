using DotNetCraft.Common.Core.Utils;

namespace DotNetCraft.Common.Core.Domain.Management
{
    public interface IBackgroundManager: IManager, IStartStoppable
    {
        /// <summary>
        /// Background thread will be wake up immediately.
        /// </summary>
        /// <param name="reason">The reason</param>
        void ForceRun(string reason);
    }
}
