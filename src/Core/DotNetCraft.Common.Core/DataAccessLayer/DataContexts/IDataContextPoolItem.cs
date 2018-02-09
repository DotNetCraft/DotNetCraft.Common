namespace DotNetCraft.Common.Core.DataAccessLayer.DataContexts
{
    public interface IDataContextPoolItem
    {
        /// <summary>
        /// The IDataContextWrapper instance.
        /// </summary>
        IDataContextWrapper DataContextWrapper { get; }

        /// <summary>
        /// Count of references to this contextWrapper.
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
