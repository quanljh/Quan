using System;
using System.Collections.Generic;
using System.Text;

namespace Quan.Word.Core
{
    /// <summary>
    /// The severity of the log message
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Developer-specific information
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Verbose information
        /// </summary>
        Verbose = 2,

        /// <summary>
        /// General information
        /// </summary>
        Infomative = 3,

        /// <summary>
        /// A warning
        /// </summary>
        Warning = 4,

        /// <summary>
        /// A error
        /// </summary>
        Error = 5,

        /// <summary>
        /// A success
        /// </summary>
        Success = 6,
    }
}
