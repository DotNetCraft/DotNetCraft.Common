using System.Collections.Generic;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;
using DotNetCraft.Common.Core.Utils.Disposal;

namespace DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks.Simple
{
    /// <summary>
    /// Pattern Unit of Work.
    /// </summary>
    public interface IUnitOfWork : IDisposableObject
    {
        /// <summary>
        /// Unique key.
        /// </summary>
        IUniqueKey UniqueKey { get; }

        /// <summary>
        /// Flag shows that transaction opened.
        /// </summary>
        bool IsTransactionOpened { get; }        

        /// <summary>
        /// Commit transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        void Rollback();
    }
}
