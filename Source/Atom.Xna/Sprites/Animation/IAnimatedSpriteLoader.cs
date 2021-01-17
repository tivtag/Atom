// <copyright file="IAnimatedSpriteLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.IAnimatedSpriteLoader interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    /// <summary>
    /// Provides a mechanism that allows loading of <see cref="AnimatedSprite"/> assets.
    /// </summary>
    public interface IAnimatedSpriteLoader : IAssetLoader
    {
        /// <summary>
        /// Loads the AnimatedSprite with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the AnimatedSprite to load.
        /// </param>
        /// <returns>
        /// The AnimatedSprite that has been loaden.
        /// </returns>
        AnimatedSprite LoadAnimatedSprite( string assetName );
    }
}
