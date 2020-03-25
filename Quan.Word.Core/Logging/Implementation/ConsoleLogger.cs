using System;
using System.Collections.Generic;
using System.Text;

namespace Quan.Word.Core
{
    /// <summary>
    /// Logs the message to the Console
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Logs the given message to system Console
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The level of the message</param>
        public void Log(string message, LogFactoryLevel level)
        {
            // Write message to console
            Console.WriteLine($"[{level}]".PadRight(13, ' ') + message);
        }
    }
}
