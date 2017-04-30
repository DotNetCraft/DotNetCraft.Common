using DotNetCraft.Common.Core.DataAccessLayer;

namespace DotNetCraft.Common.Core
{
    public interface IDataContextPoolItem
    {
        /// <summary>
        /// The IDataContext instance.
        /// </summary>
        IDataContext DataContext { get; }

        /// <summary>
        /// Count of references to this context.
        /// </summary>
        int ReferenceCount { get; }

        /// <summary>
        /// Increase reference count by 1.
        /// </summary>
        /// <returns>Current amount of references</returns>
        int IncreaseRef();

        /// <summary>
        /// Decrease reference count by 1.
        /// </summary>
        /// <returns>Current amount of references</returns>
        int DecreaseRef();
    }
}
