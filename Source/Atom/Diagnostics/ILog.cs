// <copyright file="ILog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.ILog interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics
{
    /// <summary>
    /// Provides a mechanism to log string messages.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Gets the default <see cref="LogSeverities"/> messages are logged as.
        /// </summary>
        /// <value>
        /// The default value is <see cref="LogSeverities.Info"/>.
        /// </value>
        LogSeverities DefaultSeverity 
        {
            get;
        }

        /// <summary>
        /// Writes the given message to this ILog using the <see cref="DefaultSeverity"/>.
        /// </summary>
        /// <param name="message">
        /// The message to log.
        /// </param>
        void Write( string message );

        /// <summary>
        /// Writes the given message to this ILog using
        /// the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        void Write( LogSeverities severity, string message );

        /// <summary>
        /// Writes a new empty line to this ILog with the <see cref="DefaultSeverity"/>.
        /// </summary>
        void WriteLine();

        /// <summary>
        /// Writes the given message, followed by a new line, to 
        /// this ILog using the <see cref="DefaultSeverity"/>.
        /// </summary>
        /// <param name="message">
        /// The message to log.
        /// </param>
        void WriteLine( string message );

        /// <summary>
        /// Writes the given message, followed by a new line, to 
        /// this ILog using the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        void WriteLine( LogSeverities severity, string message );
    }
}
