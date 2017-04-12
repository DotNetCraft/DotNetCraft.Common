using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetCraft.Common.DataAccessLayer.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : DataAccessLayerException
    {
        public EntityNotFoundException(string message, Dictionary<string, string> errorParameters = null) : base(message, errorParameters)
        {
        }

        public EntityNotFoundException(string message, Exception innerException, Dictionary<string, string> errorParameters = null) : base(message, innerException, errorParameters)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
