using System;

namespace DotNetCraft.Common.DataAccessLayer.BaseEntities
{
    /// <summary>
    /// Base entity where identifier's type is Guid.
    /// </summary>
    public abstract class BaseGuidEntity : BaseEntity<Guid>
    {
    }
}
