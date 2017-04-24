namespace DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks
{
    /// <summary>
    /// Interface shows that object is a database parameter
    /// </summary>
    public interface IDataBaseParameter
    {
        /// <summary>
        /// Parameter's name.
        /// </summary>
        string ParameterName { get; }

        /// <summary>
        /// Parameter's value.
        /// </summary>
        object ParameterValue { get; }
    }
}
