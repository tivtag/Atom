// <copyright file="LogSeverities.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Diagnostics.LogSeverities enumeration.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Diagnostics
{
    /// <summary>
    /// Enumerates the different types of log severities,
    /// as related to the <see cref="ILog"/> interface.
    /// </summary>
    [System.Flags]
    public enum LogSeverities  
    {
        /// <summary>
        /// Represents no LogSeverity.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents a severity that relates to information messages.
        /// </summary>
        Info = 1 << 0,

        /// <summary>
        /// Represents a severity that relates to debug-related log messages.
        /// </summary>
        Debug = 1 << 1,

        /// <summary>
        /// Represents a severity that relates to warning log messages.
        /// </summary>
        Warning = 1 << 2,

        /// <summary>
        /// Represents a severity that relates to error log messages.
        /// </summary>
        Error = 1 << 3,

        /// <summary>
        /// Represents a severity that relates to fatal log messages.
        /// </summary>
        Fatal = 1 << 4,

        /// <summary>
        /// Represents all LogSeverities at the same time.
        /// </summary>
        All = Info | Debug | Warning | Error | Fatal 
    }
}
