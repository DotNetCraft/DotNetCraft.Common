using DotNetCraft.Common.Core.Utils.Disposal;

namespace DotNetCraft.Common.Core.Domain.Management
{
    /// <summary>
    /// Interface shows that object is a manager.
    /// </summary>
    public interface IManager: IDisposableObject
    {
        /// <summary>
        /// Manager's name
        /// </summary>
        string Name { get; }
    }
}
