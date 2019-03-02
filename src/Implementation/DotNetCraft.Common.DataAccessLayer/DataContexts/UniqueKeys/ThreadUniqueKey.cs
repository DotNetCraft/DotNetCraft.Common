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
