// <copyright file="IUpdateContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.IUpdateContext interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom
{
    /// <summary>
    /// Provides information about the current context an object is updated in.
    /// </summary>
    /// <remarks>
    /// Usually an IUpdateContext is passed to the Update method of various objects.
    /// </remarks>
    /// <seealso cref="IUpdateable"/>
    /// <seealso cref="IPreUpdateable"/>
    public interface IUpdateContext
    {
        /// <summary>
        /// Gets the time the last frame took to execute (in seconds).
        /// </summary>
        /// <value>
        /// The time the last frame took to execute (in seconds).
        /// </value>
        float FrameTime { get; }
    }
}
