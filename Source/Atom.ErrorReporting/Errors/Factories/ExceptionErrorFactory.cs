// <copyright file="ExceptionErrorFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Errors.ExceptionErrorFactory class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.ErrorReporting.Errors
{
    using System;

    /// <summary>
    /// Implements an <see cref="IExceptionErrorFactory"/> that creates <see cref="ExceptionError"/> objects
    /// given an <see cref="Exception"/>.
    /// </summary>
    public sealed class ExceptionErrorFactory : IExceptionErrorFactory
    {
        /// <summary>
        /// Creates a concrete instance of the <see cref="IExceptionError"/> class.
        /// </summary>
        /// <param name="exception">
        /// The exception that was thrown.
        /// </param>
        /// <param name="isFatal">
        /// States whether the exception was fatal and requires the application to shutdown.
        /// </param>
        /// <returns>
        /// The newly created concrete instance of the IExceptionError interface.
        /// </returns>
        public IExceptionError CreateError( Exception exception, bool isFatal )
        {
            return new ExceptionError( exception, isFatal );
        }
    }
}
