// <copyright file="IIsDisposable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.IIsDisposable interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    using System;

    /// <summary>
    /// Defines a method to release allocated resources, and receive
    /// a value indicating whether this has been done.
    /// </summary>
    public interface IIsDisposable : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IDisposable"/> object
        /// has been disposed.
        /// </summary>
        bool IsDisposed { get; }
    }
}
