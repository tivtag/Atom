// <copyright file="TraceLog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Diagnostics.TraceLog class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Diagnostics
{
    using System.Diagnostics;

    /// <summary>
    /// Provides a mechanism to write string messages to the <see cref="System.Diagnostics.Trace"/>.
    /// </summary>
    public class TraceLog : BaseLog
    {
        /// <summary>
        /// Writes the given message to the trace using
        /// the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        protected override void ActuallyWrite( LogSeverities severity, string message )
        {            
            Trace.Write( message );
        }

        /// <summary>
        /// Writes the given message, followed by a new line, to 
        /// the trace using the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        protected override void ActuallyWriteLine( LogSeverities severity, string message )
        {
            Trace.WriteLine( message );
        }
    }
}
