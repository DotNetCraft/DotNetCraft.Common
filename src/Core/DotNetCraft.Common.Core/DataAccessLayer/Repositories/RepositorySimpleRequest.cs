namespace DotNetCraft.Common.Core.DataAccessLayer.Repositories
{
    public class RepositorySimpleRequest
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string OrderBy { get; set; }

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Skip: {Skip}; Take: {Take}; OrderBy: {OrderBy}";
        }

        #endregion
    }
}
