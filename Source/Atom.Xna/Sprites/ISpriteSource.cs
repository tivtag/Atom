// <copyright file="ISpriteSource.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ISpriteSource interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    /// <summary>
    /// Provides a mechanism that allows receiving a set of <see cref="Sprite"/>s
    /// and <see cref="AnimatedSprite"/>s.
    /// </summary>
    public interface ISpriteSource : INormalSpriteSource, IAnimatedSpriteSource
    {
    }
}
