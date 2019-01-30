using System.Text;

namespace DotNetCraft.Common.DataAccessLayer.Repositories.Configs
{
    public class RepositoryConfig
    {
        public string IdentifierPropertyName { get; set; }
        public bool UseKeyAttribute { get; set; }

        public RepositoryConfig()
        {
            IdentifierPropertyName = "id";
            UseKeyAttribute = false;
        }

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("IdentifierPropertyName: " + IdentifierPropertyName);
            stringBuilder.AppendLine("UseKeyAttribute: " + UseKeyAttribute);
            return stringBuilder.ToString();
        }

        #endregion
    }
}
