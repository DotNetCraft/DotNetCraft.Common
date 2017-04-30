﻿using System;
using DotNetCraft.Common.Core.BaseEntities;

namespace DotNetCraft.Common.Core.DataAccessLayer.Repositories
{
    public interface IGuidRepository<TEntity> : IRepository<TEntity>
       where TEntity : IEntity
    {
        /// <summary>
        /// Get entity by identifier.
        /// </summary>
        /// <param name="entityId">The entity's identifier.</param>
        /// <returns>The entity, if it exists.</returns>
        TEntity Get(Guid entityId);
    }
}