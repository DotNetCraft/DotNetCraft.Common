using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DotNetCraft.Common.Utils.Exceptions;

namespace DotNetCraft.Common.Utils.ReflectionExtensions
{
    public class ReflectionManagerException : DotNetCraftExtendedException
    {
        public ReflectionManagerException(string message, Dictionary<string, string> errorParameters = null) : base(message, errorParameters)
        {
        }

        public ReflectionManagerException(string message, Exception innerException, Dictionary<string, string> errorParameters = null) : base(message, innerException, errorParameters)
        {
        }

        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        public ReflectionManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
