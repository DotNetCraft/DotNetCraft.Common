﻿using System;

namespace DotNetCraft.Common.Core.DataAccessLayer.Repositories.Simple.Predefined
{
    public interface IGuidRepository<TEntity> : IRepository<TEntity>
    {
        /// <summary>
        /// Get entity by identifier.
        /// </summary>
        /// <param name="entityId">The entity's identifier.</param>
        /// <returns>The entity, if it exists.</returns>
        TEntity Get(Guid entityId);
    }
}
