// <copyright file="IError.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.IError interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting
{
    /// <summary>
    /// Represents a reportable error.
    /// </summary>
    public interface IError
    {
        /// <summary>
        /// Gets the name of this IError.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets a description of this IError.
        /// </summary>
        string Description
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this IError is fatal,
        /// and requires the application to shut-down.
        /// </summary>
        bool IsFatal
        {
            get;
        }
    }
}
