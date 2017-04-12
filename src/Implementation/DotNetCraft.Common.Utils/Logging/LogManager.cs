using System;
using System.Diagnostics;
using System.Reflection;
using DotNetCraft.Common.Core.Utils.Logging;

namespace DotNetCraft.Common.Utils.Logging
{
    /// <summary>
    /// Logging API for this library. You can inject your own implementation otherwise
    /// will use the DebugLogFactory to write to System.Diagnostics.Debug
    /// </summary>
    public class LogManager
    {
        /// <summary>
        /// Common logger factory.
        /// </summary>
        private static ICommonLoggerFactory loggerFactory;

        /// <summary>
        /// Gets or sets the log factory.
        /// Use this to override the factory that is used to create loggers
        /// </summary>
        public static ICommonLoggerFactory LoggerFactory
        {
            get { return loggerFactory ?? (loggerFactory = new DebugLoggerFactory()); }
            set { loggerFactory = value; }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public static ICommonLogger GetLogger(Type type)
        {
            return LoggerFactory.Create(type);
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public static ICommonLogger GetLogger<TType>()
        {
            return LoggerFactory.Create<TType>();
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public static ICommonLogger GetLogger(string typeName)
        {
            return LoggerFactory.Create(typeName);
        }

        /// <summary>
        /// Gets the logger with the name of the current class.  
        /// </summary>
        /// <returns>The logger.</returns>
        /// <remarks>This is a slow-running method. 
        /// Make sure you're not doing this in a loop.</remarks>
        public static ICommonLogger GetCurrentClassLogger()
        {
            string typeName = GetClassFullName();
            return GetLogger(typeName);
        }

        /// <summary>
        /// Gets the fully qualified name of the class invoking the LogManager, including the 
        /// namespace but not the assembly.    
        /// </summary>
        private static string GetClassFullName()
        {
            string className;
            Type declaringType;
            int framesToSkip = 2;

            do
            {
#if SILVERLIGHT
                StackFrame frame = new StackTrace().GetFrame(framesToSkip);
#else
                StackFrame frame = new StackFrame(framesToSkip, false);
#endif
                MethodBase method = frame.GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    className = method.Name;
                    break;
                }

                framesToSkip++;
                className = declaringType.FullName;
            } while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return className;
        }
    }
}