// <copyright file="ISpriteAsset.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ISpriteAsset interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using Atom.Math;

    /// <summary>
    /// Represents a definition of a two dimensional image that can easily be
    /// drawn on the screen by creating an <see cref="ISprite"/> instance from it.
    /// </summary>
    public interface ISpriteAsset : IAsset, ISizeable2
    {
        /// <summary>
        /// Creates an instance of this ISpriteAsset.
        /// </summary>
        /// <returns>
        /// The instance that has been created.
        /// </returns>
        ISprite CreateInstance();
    }
}