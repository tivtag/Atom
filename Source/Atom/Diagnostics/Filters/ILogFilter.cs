// <copyright file="ILogFilter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Diagnostics.Filters.ILogFilter interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Diagnostics.Filters
{
    /// <summary>
    /// Provides a mechanism to filter <see cref="ILog"/> messages.
    /// </summary>
    public interface ILogFilter
    {        
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
        bool Filters( LogSeverities severity, string message );
    }
}
