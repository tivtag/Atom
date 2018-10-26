// <copyright file="LogErrorReporter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Reporters.LogErrorReporter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Reporters
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Diagnostics;

    /// <summary>
    /// Implements an <see cref="IErrorReporter"/> that writes the full exception details to
    /// an <see cref="ILog"/>.
    /// </summary>
    /// <remarks>
    /// The ToString method of an <see cref="IError"/> is invoked to receive the full error description.
    /// </remarks>
    public class LogErrorReporter : IErrorReporter
    {
        /// <summary>
        /// Initializes a new instance of the LogErrorReporter class.
        /// </summary>
        /// <param name="log">
        /// The ILog to which <see cref="IError"/>s should be written.
        /// </param>
        public LogErrorReporter( ILog log )
        {
            Contract.Requires<ArgumentNullException>( log != null );

            this.log = log;
        }

        /// <summary>
        /// Notifies this IErrorReporter that the specified IError has occurred.
        /// </summary>
        /// <param name="error">
        /// The error to report.
        /// </param>
        public void Report( IError error )
        {
            this.log.WriteLine();
            this.log.WriteLine( error.ToString() );
            this.log.WriteLine();
        }

        /// <summary>
        /// The ILog to which <see cref="IError"/>s should be written.
        /// </summary>
        private readonly ILog log;
    }
}
