// <copyright file="IPreUpdateable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.IPreUpdateable interface.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom
{
    /// <summary>
    /// Provides the mechanism of updating an object
    /// before another update.
    /// </summary>
    /// <remarks>
    /// Usually objects that implement the <see cref="IUpdateable"/> interface 
    /// may also implement the <see cref="IPreUpdateable"/> interface;
    /// but not the other way around.
    /// </remarks>
    /// <seealso cref="IUpdateable"/>
    public interface IPreUpdateable
    {
        /// <summary>
        /// Updates this IUpdateable.
        /// </summary>
        /// <param name="updateContext">
        /// The current <see cref="IUpdateContext"/>.
        /// </param>
        void PreUpdate( IUpdateContext updateContext );
    }
}
