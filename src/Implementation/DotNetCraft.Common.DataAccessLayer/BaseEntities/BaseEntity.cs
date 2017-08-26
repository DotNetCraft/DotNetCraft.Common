using System.ComponentModel.DataAnnotations;

namespace DotNetCraft.Common.DataAccessLayer.BaseEntities
{
    /// <summary>
    /// Base class for all entities.
    /// </summary>
    public abstract class BaseEntity<TEntityId>
    { 
        /// <summary>
        /// The identifier.
        /// </summary>
        [Key]
        public TEntityId Id { get; set; }       

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("ID: {0}", Id);
        }

        #endregion
    }
}
