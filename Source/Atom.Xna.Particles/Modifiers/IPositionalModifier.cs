// <copyright file="IPositionalModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Modifiers.IPositionalModifier interface.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Xna.Particles.Modifiers
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Provides a mechanism to get or set a position that is associated with a Modifier.
    /// </summary>
    public interface IPositionalModifier
    {
        /// <summary>
        /// Gets or sets the position associated with this IPositionalModifier.
        /// </summary>
        Vector2 Position { get; set; }
    }
}
