using System;
using System.Text;
using DotNetCraft.Common.Core.Utils.Logging;

namespace DotNetCraft.Common.Utils.Logging
{
    public class DebugLogger: ICommonLogger
    {
        private readonly string typeName;

        public DebugLogger(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (string.IsNullOrWhiteSpace(type.FullName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(typeName));

            typeName = type.FullName;
        }

        public DebugLogger(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(typeName));

            this.typeName = typeName;
        }

        #region Implementation of ICommonLogger

        /// <summary>
        /// Write trace information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        public void Trace(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        /// <summary>
        /// Write trace information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        public void Trace(string msg, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(msg, args);
        }

        /// <summary>
        /// Write debug information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        public void Debug(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        /// <summary>
        /// Write debug information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        public void Debug(string msg, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(msg, args);
        }

        /// <summary>
        /// Write information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        public void Info(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        /// <summary>
        /// Write information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        public void Info(string msg, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(msg, args);
        }

        /// <summary>
        /// Write warning message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        public void Warning(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        /// <summary>
        /// Write warning message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        public void Warning(string msg, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(msg, args);
        }

        /// <summary>
        /// Write warning message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="exception"><see cref="Exception"/> that will be added into the message.</param>
        public void Warning(Exception exception, string msg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(msg);
            stringBuilder.AppendLine(exception.Message);
            stringBuilder.AppendLine(exception.StackTrace);
            System.Diagnostics.Debug.WriteLine(stringBuilder.ToString());
        }

        /// <summary>
        /// Write warning message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        /// <param name="exception"><see cref="Exception"/> that will be added into the message.</param>
        public void Warning(Exception exception, string msg, params object[] args)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format(msg, args));
            stringBuilder.AppendLine(exception.Message);
            stringBuilder.AppendLine(exception.StackTrace);
            System.Diagnostics.Debug.WriteLine(stringBuilder.ToString());
        }

        /// <summary>
        /// Write error message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        public void Error(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        /// <summary>
        /// Write error message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        public void Error(string msg, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(msg, args);
        }

        /// <summary>
        /// Write error message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="exception"><see cref="Exception"/> that will be added into the message.</param>
        public void Error(Exception exception, string msg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(msg);
            stringBuilder.AppendLine(exception.Message);
            stringBuilder.AppendLine(exception.StackTrace);
            System.Diagnostics.Debug.WriteLine(stringBuilder.ToString());
        }

        /// <summary>
        /// Write error message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        /// <param name="exception"><see cref="Exception"/> that will be added into the message.</param>
        public void Error(Exception exception, string msg, params object[] args)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format(msg, args));
            stringBuilder.AppendLine(exception.Message);
            stringBuilder.AppendLine(exception.StackTrace);
            System.Diagnostics.Debug.WriteLine(stringBuilder.ToString());
        }

        #endregion
    }
}
