using System;
using System.Collections.Generic;
using System.Text;

namespace Quan.Word.Core
{
    /// <summary>
    /// The severity of the log message
    /// </summary>
    public enum LogFactoryLevel
    {
        /// <summary>
        /// Logs everything
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Log all information except debug information 
        /// </summary>
        Verbose = 2,

        /// <summary>
        /// Logs all informative message, ignoring any debug and verbose messages
        /// </summary>
        Infomative = 3,

        /// <summary>
        /// Logs only warnings, errors and standard message
        /// </summary>
        Normal = 4,

        /// <summary>
        /// Log only critical errors and warnings, no general information
        /// </summary>
        Critical = 5,

        /// <summary>
        /// The logger will never output anything
        /// </summary>
        Nothing = 6,
    }
}
