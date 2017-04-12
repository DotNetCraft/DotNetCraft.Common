using System;

namespace DotNetCraft.Common.Core.Utils.Logging
{
    /// <summary>
    /// Interface shows that <c>object</c> is a logger.
    /// </summary>
    public interface ICommonLogger
    {
        #region Trace...
        /// <summary>
        /// Write trace information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        void Trace(string msg);

        /// <summary>
        /// Write trace information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        void Trace(string msg, params object[] args);
        #endregion

        #region Debug...
        /// <summary>
        /// Write debug information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        void Debug(string msg);

        /// <summary>
        /// Write debug information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        void Debug(string msg, params object[] args);
        #endregion

        #region Info...
        /// <summary>
        /// Write information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        void Info(string msg);

        /// <summary>
        /// Write information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        void Info(string msg, params object[] args);
        #endregion

        #region Warnings...
        /// <summary>
        /// Write warning message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        void Warning(string msg);

        /// <summary>
        /// Write warning message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        void Warning(string msg, params object[] args);

        /// <summary>
        /// Write warning message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="exception"><see cref="Exception"/> that will be added into the message.</param>
        void Warning(Exception exception, string msg);

        /// <summary>
        /// Write warning message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        /// <param name="exception"><see cref="Exception"/> that will be added into the message.</param>
        void Warning(Exception exception, string msg, params object[] args);
        #endregion

        #region Error...
        /// <summary>
        /// Write error message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        void Error(string msg);

        /// <summary>
        /// Write error message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        void Error(string msg, params object[] args);

        /// <summary>
        /// Write error message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="exception"><see cref="Exception"/> that will be added into the message.</param>
        void Error(Exception exception, string msg);

        /// <summary>
        /// Write error message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        /// <param name="exception"><see cref="Exception"/> that will be added into the message.</param>
        void Error(Exception exception, string msg, params object[] args);
        #endregion        
    }
}
