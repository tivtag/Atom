// <copyright file="ILogProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.ILogProvider interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics
{
    /// <summary>
    /// Provides a mechanism to receive an <see cref="ILog"/> object
    /// that can be used for logging.
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// Gets the <see cref="ILog"/> object.
        /// </summary>
        /// <value>
        /// The <see cref="ILog"/> object this ILogService provides.
        /// </value>
        ILog Log 
        {
            get; 
        }
    }
}
