namespace DotNetCraft.Common.Core.Domain.Management
{
    /// <summary>
    /// Interface shows that object is a manager's configuration.
    /// </summary>
    public interface IManagerConfiguration
    {
        /// <summary>
        /// Manager's name
        /// </summary>
        string Name { get; set; }
    }
}
