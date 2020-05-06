using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Quan.Word.Core
{
    /// <summary>
    /// Logs the messages to the Debug log
    /// </summary>
    public class DebugLogger : ILogger
    {
        /// <summary>
        /// Logs the given message to the system Console
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public void Log(string message, LogLevel level)
        {
            // The default category
            var category = default(string);

            // Color console based on level
            switch (level)
            {
                // Debug
                case LogLevel.Debug:
                    category = "information";
                    break;

                // Verbose
                case LogLevel.Verbose:
                    category = "verbose";
                    break;

                // Warning
                case LogLevel.Warning:
                    category = "warning";
                    break;

                // Error
                case LogLevel.Error:
                    category = "error";
                    break;

                // Success
                case LogLevel.Success:
                    category = "-----";
                    break;
            }

            // Write message to console
            Debug.WriteLine(message, category);
        }
    }
}
