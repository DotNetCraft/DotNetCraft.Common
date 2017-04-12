using System;
using DotNetCraft.Common.Core.Utils.Logging;

namespace DotNetCraft.Common.Utils.Logging
{
    /// <summary>
    /// Creates a Debug Logger, that logs all messages to: System.Diagnostics.Debug
    /// </summary>
    public class DebugLoggerFactory : ICommonLoggerFactory
    {
        #region Implementation of ICommonLoggerFactory

        /// <summary>
        /// Creates a logger for current <c>type</c>.
        /// </summary>
        /// <param name="type">The <c>type</c>.</param>
        /// <returns>The <see cref="ICommonLogger"/> instance.</returns>
        /// <example>
        /// ICommonLogger logger = factory.Create(GetType());
        /// </example>
        public ICommonLogger Create(Type type)
        {
            return new DebugLogger(type);
        }

        /// <summary>
        /// Creates a logger for current <c>type</c>.
        /// </summary>
        /// <typeparam name="TType"><c>Type</c> of <c>object</c> for which logger will be created.</typeparam>
        /// <example>
        /// <c>ICommonLogger</c> logger = factory.Create<SomeClass>();
        /// </example>
        /// <returns>The <see cref="ICommonLogger"/> instance.</returns>
        public ICommonLogger Create<TType>()
        {
            return new DebugLogger(typeof(TType));
        }

        /// <summary>
        /// Creates a logger with name.
        /// </summary>
        /// <example>
        /// <c>ICommonLogger</c> logger = factory.Create(GetType());
        /// </example>
        /// <param name="typeName">The logger's name.</param>
        /// <returns>The <see cref="ICommonLogger"/> instance.</returns>
        public ICommonLogger Create(string typeName)
        {
            return new DebugLogger(typeName);
        }

        #endregion
    }
}