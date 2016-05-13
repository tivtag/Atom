// <copyright file="ISpriteSheet.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ISpriteSheet interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System.Collections.Generic;
    using Atom.Math;

    /// <summary>
    /// Represents an ordered sheet of <see cref="ISprite"/>s.
    /// </summary>
    public interface ISpriteSheet : IList<ISprite>, IUpdateable, IAsset
    {
    }
}
