using System;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;

namespace DotNetCraft.Common.DataAccessLayer.DataContexts
{
    /// <summary>
    /// Simple implementation of the pool item that contains information about data contextWrapper.
    /// </summary>
    public class DataContextPoolItem : IDataContextPoolItem
    {
        #region Properties...

        /// <summary>
        /// The IDataContextWrapper instance.
        /// </summary>
        public IDataContextWrapper DataContextWrapper { get; }

        /// <summary>
        /// Count of references to this contextWrapper.
        /// </summary>
        public int ReferenceCount { get; private set; }

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataContextWrapper">The IDataContextWrapper instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataContextWrapper"/> is <see langword="null"/></exception>
        public DataContextPoolItem(IDataContextWrapper dataContextWrapper)
        {
            if (dataContextWrapper == null)
                throw new ArgumentNullException(nameof(dataContextWrapper));

            DataContextWrapper = dataContextWrapper;
            ReferenceCount = 1;
        }
        #endregion

        #region Methods...
        /// <summary>
        /// Increase reference count by 1.
        /// </summary>
        /// <returns>Current amount of references</returns>
        public int IncreaseRef()
        {
            ReferenceCount++;
            return ReferenceCount;
        }

        /// <summary>
        /// Decrease reference count by 1.
        /// </summary>
        /// <returns>Current amount of references</returns>
        /// <exception cref="IndexOutOfRangeException"><see cref="ReferenceCount"/> should be greater than 0.</exception>
        public int DecreaseRef()
        {
            if (ReferenceCount <= 0)
                throw new IndexOutOfRangeException("ReferenceCount should be greater than 0.");

            ReferenceCount--;
            return ReferenceCount;
        }
        #endregion

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0} has {1} active reference(s)", DataContextWrapper, ReferenceCount);
        }

        #endregion
    }
}
