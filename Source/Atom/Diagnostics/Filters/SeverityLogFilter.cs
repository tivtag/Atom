// <copyright file="SeverityLogFilter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Diagnostics.Filters.SeverityLogFilter class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Diagnostics.Filters
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Implements an <see cref="ILogFilter"/> that filters by the
    /// severity of the message.
    /// </summary>
    /// <seealso cref="LogSeverities"/>
    public sealed class SeverityLogFilter : ILogFilter
    {
        /// <summary>
        /// Gets or sets the <see cref="LogSeverities"/> that aren't filtered by this SeverityLogFilter.
        /// </summary>
        /// <value>
        /// Is <see cref="LogSeverities.All"/> by default; allowing all messages to pass.
        /// </value>
        public LogSeverities Allowed
        {
            get
            {
                return this.allowed;
            }

            set
            {
                this.allowed = value;
            }
        }

        /// <summary>
        /// Gets whether the specified log message is filtered by this SeverityLogFilter.
        /// </summary>
        /// <param name="severity">
        /// The severity of the message.
        /// </param>
        /// <param name="message">
        /// The actual message.
        /// </param>
        /// <returns>
        /// true if the message is filtered;
        /// otherwise false.
        /// </returns>
        public bool Filters( LogSeverities severity, string message )
        {
            if( severity == LogSeverities.None )
                return this.allowed == LogSeverities.None;

            return this.allowed.Contains( severity ) == false;
        }

        /// <summary>
        /// Tells this SeverityLogFilter to not filter messages with the specified LogSeverity.
        /// </summary>
        /// <param name="severity">
        /// The sevarity to allow.
        /// </param>
        public void Allow( LogSeverities severity )
        {
            Contract.Requires<ArgumentException>( severity != LogSeverities.None );

            this.allowed |= severity;
        }

        /// <summary>
        /// Tells this SeverityLogFilter to filter messages with the specified LogSeverity.
        /// </summary>
        /// <param name="severity">
        /// The sevarity to not allow.
        /// </param>
        public void Disallow( LogSeverities severity )
        {                       
            this.allowed &= ~severity;
        }
        
        /// <summary>
        /// Stores the <see cref="LogSeverities"/> that aren't filtered by this SeverityLogFilter.
        /// </summary>
        private LogSeverities allowed = LogSeverities.All;
    }
}
