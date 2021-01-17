// <copyright file="IErrorMessageFormatter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.IErrorMessageFormatter interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.ErrorReporting
{
    /// <summary>
    /// Provides a mechanism for formatting an <see cref="IError"/>
    /// </summary>
    public interface IErrorMessageFormatter
    {
        /// <summary>
        /// Formats and then returns the error message for the given IError.
        /// </summary>
        /// <param name="error">
        /// The error whose error message should be formated.
        /// </param>
        /// <returns>
        /// The formated error message.
        /// </returns>
        string Format( IError error );
    }
}
