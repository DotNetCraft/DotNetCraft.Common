using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DotNetCraft.Common.Utils.Exceptions;

namespace DotNetCraft.Common.Utils.DependencyInjection.Exceptions
{
    public class ResolveException : DotNetCraftExtendedException
    {
        public ResolveException(string message, Dictionary<string, string> errorParameters = null) : base(message, errorParameters)
        {
        }

        public ResolveException(string message, Exception innerException, Dictionary<string, string> errorParameters = null) : base(message, innerException, errorParameters)
        {
        }

        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        public ResolveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
