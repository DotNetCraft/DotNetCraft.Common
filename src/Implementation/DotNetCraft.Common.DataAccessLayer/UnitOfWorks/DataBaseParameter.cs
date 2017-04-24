using System;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;

namespace DotNetCraft.Common.DataAccessLayer.UnitOfWorks
{
    /// <summary>
    /// Database parameter.
    /// </summary>
    /// <remarks>This class is using to provide database parameter into the special methods in the UnitOfWork or in Repositories.</remarks>
    public class DataBaseParameter: IDataBaseParameter
    {
        /// <summary>
        /// Parameter's name.
        /// </summary>
        public string ParameterName { get; private set; }

        /// <summary>
        /// Parameter's value.
        /// </summary>
        public object ParameterValue { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parameterName">Parameter's name.</param>
        /// <param name="parameterValue">Parameter's value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parameterName"/> is <see langword="null"/></exception>
        public DataBaseParameter(string parameterName, object parameterValue)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException(nameof(parameterName));

            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0} = {1}", ParameterName, ParameterValue);
        }

        #endregion
    }
}
