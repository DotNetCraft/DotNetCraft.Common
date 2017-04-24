using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using DotNetCraft.Common.Core.BaseEntities;

namespace DotNetCraft.Common.DataAccessLayer.BaseEntities
{
    /// <summary>
    /// Base class for all entities.
    /// </summary>
    public abstract class BaseEntity<TEntityId> : IEntity
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
