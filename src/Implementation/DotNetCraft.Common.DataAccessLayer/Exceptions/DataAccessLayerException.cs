using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DotNetCraft.Common.Utils.Exceptions;

namespace DotNetCraft.Common.DataAccessLayer.Exceptions
{
    [Serializable]
    public class DataAccessLayerException : DotNetCraftExtendedException
    {
        public DataAccessLayerException(string message, Dictionary<string, string> errorParameters = null) : base(message, errorParameters)
        {
        }

        public DataAccessLayerException(string message, Exception innerException, Dictionary<string, string> errorParameters = null) : base(message, innerException, errorParameters)
        {
        }

        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        public DataAccessLayerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
