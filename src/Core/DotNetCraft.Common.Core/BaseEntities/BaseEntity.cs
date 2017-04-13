using DotNetCraft.Common.Core.Attributes;

namespace DotNetCraft.Common.Core.BaseEntities
{
    /// <summary>
    /// Base class for all entities.
    /// </summary>
    public abstract class BaseEntity<TEntityId> : IEntity
    { 
        /// <summary>
        /// The identifier.
        /// </summary>
        [Identifier]
        public TEntityId Id { get; set; }       

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Id: {0}", Id);
        }

        #endregion
    }
}
