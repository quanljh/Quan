﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Quan.Word.Core
{
    /// <summary>
    /// A logger that will handle log message from a <see cref="ILogFactory"/>
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Handles the logged message being passed in 
        /// </summary>
        /// <param name="message">The message being log</param>
        /// <param name="level">The level of the log message</param>
        void Log(string message, LogLevel level);
    }
}
