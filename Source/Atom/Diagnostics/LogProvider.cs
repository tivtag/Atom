// <copyright file="LogProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.LogProvider class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Implements a mechanism to receive an <see cref="ILog"/> object
    /// that can be used for logging.
    /// </summary>
    public sealed class LogProvider : ILogProvider
    {
        /// <summary>
        /// Gets the <see cref="ILog"/> object this LogProvider provides.
        /// </summary>
        /// <value>
        /// The <see cref="ILog"/> object this ILogService provides.
        /// </value>
        public ILog Log
        {
            get
            {
                Contract.Ensures( Contract.Result<ILog>() != null );

                return this.log;
            }
        }

        /// <summary>
        /// Initializes a new instance of the LogProvider class.
        /// </summary>
        /// <param name="log">
        /// The ILog object the new LogProvider provides.
        /// </param>
        public LogProvider( ILog log )
        {
            Contract.Requires<ArgumentNullException>( log != null );

            this.log = log;
        }

        /// <summary>
        /// The <see cref="ILog"/> object this LogProvider provides.
        /// </summary>
        private readonly ILog log;
    }
}
