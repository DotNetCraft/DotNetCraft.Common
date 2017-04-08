using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetCraft.Common.DataAccessLayer.Exceptions
{
    [Serializable]
    public class UnitOfWorkException: DataAccessLayerException
    {
        public UnitOfWorkException(string message, Dictionary<string, string> errorParameters = null) : base(message, errorParameters)
        {
        }

        public UnitOfWorkException(string message, Exception innerException, Dictionary<string, string> errorParameters = null) : base(message, innerException, errorParameters)
        {
        }

        protected UnitOfWorkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
