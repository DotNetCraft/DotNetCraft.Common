using System;
using DotNetCraft.Common.Core;
using DotNetCraft.Common.Core.DataAccessLayer;

namespace DotNetCraft.Common.DataAccessLayer
{
    /// <summary>
    /// Simple implementation of the pool item that contains information about data context.
    /// </summary>
    public class DataContextPoolItem: IDataContextPoolItem
    {
        #region Properties...

        /// <summary>
        /// The IDataContext instance.
        /// </summary>
        public IDataContext DataContext { get; }

        /// <summary>
        /// Count of references to this context.
        /// </summary>
        public int ReferenceCount { get; private set; }

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataContext">The IDataContext instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataContext"/> is <see langword="null"/></exception>
        public DataContextPoolItem(IDataContext dataContext)
        {
            if (dataContext == null)
                throw new ArgumentNullException(nameof(dataContext));

            DataContext = dataContext;
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
    }
}
