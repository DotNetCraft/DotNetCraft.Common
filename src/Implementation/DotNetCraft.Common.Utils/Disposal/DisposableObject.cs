using System;
using DotNetCraft.Common.Core.Utils.Disposal;

namespace DotNetCraft.Common.Utils.Disposal
{
    /// <summary>
    /// An object that notifies when it is disposed.
    /// </summary>
    public abstract class DisposableObject : INotifyWhenDisposed
    {
        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        ~DisposableObject()
        {
            Dispose(false);
        }

        /// <summary>
        /// Occurs when the object is disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases resources held by the object.
        /// </summary>
        /// <param name="disposing"><c>True</c> if called manually, otherwise by GC.</param>
        public virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !IsDisposed)
                {
                    IsDisposed = true;
                    Disposed?.Invoke(this, EventArgs.Empty);
                    Disposed = null;
                    GC.SuppressFinalize(this);
                }
            }
        }
    }
}