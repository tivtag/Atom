// <copyright file="INormalSpriteLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.INormalSpriteLoader interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    /// <summary>
    /// Provides a mechanism that allows loading of <see cref="Sprite"/> assets.
    /// </summary>
    public interface INormalSpriteLoader : IAssetLoader
    {
        /// <summary>
        /// Loads the <see cref="Sprite"/> with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the Sprite to load.
        /// </param>
        /// <returns>
        /// The Sprite that has been loaden.
        /// </returns>
        Sprite LoadSprite( string assetName );
    }
}
