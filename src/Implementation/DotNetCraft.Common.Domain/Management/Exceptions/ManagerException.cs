using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DotNetCraft.Common.Utils.Exceptions;

namespace DotNetCraft.Common.Domain.Management.Exceptions
{
    [Serializable]
    public class ManagerException : DotNetCraftExtendedException
    {
        public ManagerException(string message, Dictionary<string, string> errorParameters = null) : base(message, errorParameters)
        {
        }

        public ManagerException(string message, Exception innerException, Dictionary<string, string> errorParameters = null) : base(message, innerException, errorParameters)
        {
        }

        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        public ManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
