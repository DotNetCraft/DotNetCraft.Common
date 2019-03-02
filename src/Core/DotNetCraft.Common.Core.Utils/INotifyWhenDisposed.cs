using System;
using DotNetCraft.Common.Core.Utils.Disposal;

namespace DotNetCraft.Common.Core.Utils
{
    /// <summary>
    /// An object that fires an event when it is disposed.
    /// </summary>
    public interface INotifyWhenDisposed : IDisposableObject
    {
        /// <summary>
        /// Occurs when the object is disposed.
        /// </summary>
        event EventHandler Disposed;
    }
}
