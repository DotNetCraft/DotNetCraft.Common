using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

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

        #region Overrides of Exception

        /// <summary>Creates and returns a string representation of the current exception.</summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> errorParameter in ErrorParameters)
            {
                stringBuilder.AppendLine(string.Format("{0}:{1}", errorParameter.Key, errorParameter.Value));
            }
            stringBuilder.AppendLine(base.ToString());
            return stringBuilder.ToString();
        }

        #endregion
    }
}
