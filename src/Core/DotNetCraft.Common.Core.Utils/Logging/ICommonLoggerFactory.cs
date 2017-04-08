using System;

namespace DotNetCraft.Common.Core.Utils.Logging
{
    /// <summary>
    /// Interface shows that <c>object</c> is a loggers' factory.
    /// </summary>
    public interface ICommonLoggerFactory
    {
        /// <summary>
        /// Creates a logger for current <c>type</c>.
        /// </summary>
        /// <param name="type">The <c>type</c>.</param>
        /// <returns>The <see cref="ICommonLogger"/> instance.</returns>
        /// <example>
        /// ICommonLogger logger = factory.Create(GetType());
        /// </example>
        ICommonLogger Create(Type type);

        /// <summary>
        /// Creates a logger for current <c>type</c>.
        /// </summary>
        /// <typeparam name="TType"><c>Type</c> of <c>object</c> for which logger will be created.</typeparam>
        /// <example>
        /// <c>ICommonLogger</c> logger = factory.Create<SomeClass>();
        /// </example>
        /// <returns>The <see cref="ICommonLogger"/> instance.</returns>
        ICommonLogger Create<TType>();

        /// <summary>
        /// Creates a logger with name.
        /// </summary>
        /// <example>
        /// <c>ICommonLogger</c> logger = factory.Create(GetType());
        /// </example>
        /// <param name="loggerName">The logger's name.</param>
        /// <returns>The <see cref="ICommonLogger"/> instance.</returns>
        ICommonLogger Create(string loggerName);
    }
}
