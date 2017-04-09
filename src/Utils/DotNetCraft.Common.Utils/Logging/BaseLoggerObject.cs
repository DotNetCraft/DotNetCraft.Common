using System;
using DotNetCraft.Common.Core.Utils.Logging;

namespace DotNetCraft.Common.Utils.Logging
{
    /// <summary>
    /// Base class for objects that should have logger.
    /// </summary>
    public abstract class BaseLoggerObject
    {
        /// <summary>
        /// The <see cref="ICommonLogger"/> instance.
        /// </summary>
        protected readonly ICommonLogger logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        protected BaseLoggerObject(ICommonLoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException("loggerFactory");

            logger = loggerFactory.Create(GetType());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger">The <see cref="ICommonLogger"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is <see langword="null" />.</exception>
        protected BaseLoggerObject(ICommonLogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            this.logger = logger;
        }
    }
}
