namespace DotNetCraft.Common.Core.DataAccessLayer.DataContexts
{
    /// <summary>
    /// Interface shows that object is a data contextWrapper factory.
    /// </summary>
    public interface IDataContextFactory
    {
        /// <summary>
        /// Create a new data contextWrapper.
        /// </summary>
        /// <param name="uniqueKey">The unique key</param>
        /// <returns>The IDataContextWrapper instance.</returns>
        IDataContextWrapper CreateDataContext(IUniqueKey uniqueKey = null);

        /// <summary>
        /// Release an existing data contextWrapper.
        /// </summary>
        /// <param name="dataContextWrapper">The IDataContextWrapper instance</param>
        /// <param name="uniqueKey">The unique key</param>
        /// <returns>
        /// True if data contextWrapper has been released. 
        /// False when data contextWrapper hasn't been released but it has been returned into the factory.
        /// </returns>
        bool ReleaseDataContext(IDataContextWrapper dataContextWrapper, IUniqueKey uniqueKey = null);
    }
}
