// <copyright file="ConsoleLog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.ConsoleLog class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Diagnostics
{
    using System;

    /// <summary>
    /// Provides a mechanism to write string messages to the <see cref="System.Console"/>.
    /// </summary>
    public class ConsoleLog : BaseLog
    {
        /// <summary>
        /// Writes the given message to the console using
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
            Console.Write( message );
        }

        /// <summary>
        /// Writes the given message, followed by a new line, to 
        /// the console using the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        protected override void ActuallyWriteLine( LogSeverities severity, string message )
        {
            Console.WriteLine( message );
        }

        /// <summary>
        /// Writes a new empty line to this ConsoleLog.
        /// </summary>
        public override void WriteLine()
        {
            Console.WriteLine();
        }
    }
}
