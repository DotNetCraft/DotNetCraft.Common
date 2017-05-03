namespace DotNetCraft.Common.Core.DataAccessLayer.DataContexts
{
    /// <summary>
    /// Interface shows that object is a data context factory.
    /// </summary>
    public interface IDataContextFactory
    {
        /// <summary>
        /// Create a new data context.
        /// </summary>
        /// <param name="contextSettings"></param>
        /// <returns>The IDataContext instance.</returns>
        IDataContext CreateDataContext(IContextSettings contextSettings);

        /// <summary>
        /// Release an existing data context.
        /// </summary>
        /// <param name="dataContext">The IDataContext instance</param>
        /// <returns>
        /// True if data context has been released. 
        /// False when data context hasn't been released but it has been returned into the factory.
        /// </returns>
        bool ReleaseDataContext(IDataContext dataContext);
    }
}
