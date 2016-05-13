// <copyright file="IExceptionError.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.IExceptionError interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting
{
    using System;

    /// <summary>
    /// Represents an <see cref="IError"/> that relates to an <see cref="Exception"/>.
    /// </summary>
    public interface IExceptionError : IError
    {
        /// <summary>
        /// Gets the <see cref="Exception"/> this <see cref="IExceptionError"/> relates to.
        /// </summary>
        Exception Exception
        {
            get;
        }
    }
}
