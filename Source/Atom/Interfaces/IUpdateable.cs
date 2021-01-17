// <copyright file="IUpdateable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.IUpdateable interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom
{
    /// <summary>
    /// Provides the mechanism of updating an object.
    /// </summary>
    /// <seealso cref="IPreUpdateable"/>
    public interface IUpdateable
    {
        /// <summary>
        /// Updates this IUpdateable.
        /// </summary>
        /// <param name="updateContext">
        /// The current <see cref="IUpdateContext"/>.
        /// </param>
        void Update( IUpdateContext updateContext );
    }
}
