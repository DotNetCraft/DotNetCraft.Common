using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DotNetCraft.Common.Utils.Exceptions;

namespace DotNetCraft.Common.Domain.ServiceMessenger.Exceptions
{
    public class ServiceMessageException : DotNetCraftExtendedException
    {
        public ServiceMessageException(string message, Dictionary<string, string> errorParameters = null) : base(message, errorParameters)
        {
        }

        public ServiceMessageException(string message, Exception innerException, Dictionary<string, string> errorParameters = null) : base(message, innerException, errorParameters)
        {
        }

        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        public ServiceMessageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
