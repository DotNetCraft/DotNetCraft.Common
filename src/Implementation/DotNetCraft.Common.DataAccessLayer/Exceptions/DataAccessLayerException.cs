using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetCraft.Common.DataAccessLayer.Exceptions
{
    [Serializable]
    public class DataAccessLayerException : Exception
    {
        public Dictionary<string, string> ErrorParameters { get; private set; }

        public DataAccessLayerException(string message, Dictionary<string, string> errorParameters = null) : base(message)
        {
            ErrorParameters = errorParameters ?? new Dictionary<string, string>();
        }

        public DataAccessLayerException(string message, Exception innerException, Dictionary<string, string> errorParameters = null) : base(message, innerException)
        {
            ErrorParameters = errorParameters ?? new Dictionary<string, string>();
        }

        protected DataAccessLayerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
