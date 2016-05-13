// <copyright file="INormalSpriteSource.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.INormalSpriteSource interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a mechanism that allows receiving a set of <see cref="Sprite"/>s.
    /// </summary>
    public interface INormalSpriteSource
    {
        /// <summary>
        /// Gets the <see cref="Sprite"/>s this INormalSpriteSource contains.
        /// </summary>
        IEnumerable<Sprite> Sprites
        {
            get;
        }
    }
}
