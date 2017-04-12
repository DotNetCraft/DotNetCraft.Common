using System;
using DotNetCraft.Common.Core.Utils.Logging;

namespace DotNetCraft.Common.NLogger
{
    /// <summary>
    /// Factory that can create loggers.
    /// </summary>
    public sealed class CommonNLogLoggerFactory : ICommonLoggerFactory
    {
        /// <summary>
        /// Creates a logger of the given <see cref="Type"/>
        /// </summary>
        /// <param name="loggerType">The type.</param>
        /// <returns>The <see cref="ICommonLogger"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="loggerType"/> is <see langword="null" />.</exception>
        /// <example>
        /// factory.Create(GetType());
        /// </example>
        public ICommonLogger Create(Type loggerType)
        {
            if (loggerType == null) 
                throw new ArgumentNullException("loggerType");

            string name = loggerType.FullName;
            CommonNLogLogger result = new CommonNLogLogger(name);
            return result;
        }

        /// <summary>
        /// Creates a logger of the given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="TType">Type of object that is going to use logger.</typeparam>
        /// <returns>The <see cref="ICommonLogger"/> instance.</returns>
        /// <example>
        /// factory.Create<SomeClass>();
        /// </example>
        public ICommonLogger Create<TType>()
        {
            Type loggerType = typeof(TType);
            string name = loggerType.FullName;
            CommonNLogLogger result = new CommonNLogLogger(name);
            return result;
        }

        /// <summary>
        /// Creates a logger with name.
        /// </summary>
        /// <example>
        /// <c>ICommonLogger</c> logger = factory.Create(GetType());
        /// </example>
        /// <param name="typeName">The logger's name.</param>
        /// <returns>The <see cref="ICommonLogger"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="typeName"/> is <see langword="null" />.</exception>
        public ICommonLogger Create(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName)) 
                throw new ArgumentNullException("typeName");

            CommonNLogLogger result = new CommonNLogLogger(typeName);
            return result;
        }
    }
}
