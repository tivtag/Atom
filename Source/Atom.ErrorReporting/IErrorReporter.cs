// <copyright file="IErrorReporter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.IErrorReporter interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.ErrorReporting
{
    /// <summary>
    /// Provides a mechanism for reporting an <see cref="IError"/>.
    /// </summary>
    public interface IErrorReporter
    {
        /// <summary>
        /// Notifies this IErrorReporter that the specified IError has occurred.
        /// </summary>
        /// <param name="error">
        /// The error to report.
        /// </param>
        void Report( IError error );
    }
}
