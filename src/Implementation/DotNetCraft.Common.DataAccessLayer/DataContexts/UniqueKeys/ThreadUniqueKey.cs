using System.Threading;
using DotNetCraft.Common.Core.DataAccessLayer.DataContexts;

namespace DotNetCraft.Common.DataAccessLayer.DataContexts.UniqueKeys
{
    public class ThreadUniqueKey: IUniqueKey
    {
        private const string UniqueKeyName = "ThreadUniqueKey";

        #region Implementation of IUniqueKey

        public string Key { get; }

        #endregion

        public ThreadUniqueKey()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Key = threadId.ToString();
        }

        #region Implementation of IEquatable<IUniqueKey>

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IUniqueKey other)
        {
            if (other == null)
                return false;

            return Key == other.Key;
        }

        #endregion

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0}({1})", UniqueKeyName, Key);
        }

        #endregion
    }
}
