// <copyright file="IAnimatedSpriteSource.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.IAnimatedSpriteSource interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a mechanism that allows receiving a set of <see cref="AnimatedSprite"/>s.
    /// </summary>
    public interface IAnimatedSpriteSource
    {
        /// <summary>
        /// Gets the <see cref="AnimatedSprite"/>s this IAnimatedSpriteSource contains.
        /// </summary>
        IEnumerable<AnimatedSprite> AnimatedSprites
        {
            get;
        }
    }
}
