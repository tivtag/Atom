// <copyright file="IExceptionErrorFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.IExceptionErrorFactory interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting
{
    using System;

    /// <summary>
    /// Provides a mechanism for creating concrete instances of the <see cref="IExceptionError"/>
    /// given an <see cref="Exception"/>.
    /// </summary>
    public interface IExceptionErrorFactory : IErrorFactory
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
        IExceptionError CreateError( Exception exception, bool isFatal );
    }
}
