// <copyright file="ISpriteLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ISpriteLoader interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    /// <summary>
    /// Provides a mechanism that allows loading of <see cref="Sprite"/> and
    /// <see cref="AnimatedSprite"/> assets.
    /// </summary>
    public interface ISpriteLoader : INormalSpriteLoader, IAnimatedSpriteLoader
    {
        /// <summary>
        /// Attempts to loads the <see cref="Sprite"/> or an instance of
        /// an <see cref="AnimatedSprite"/> that has the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the asset to load.
        /// </param>
        /// <returns>
        /// The ISprite that has been loaden.
        /// </returns>
        ISprite Load( string assetName );

        /// <summary>
        /// Determines whether an <see cref="Sprite"/> with the given <paramref name="assetName"/> exists.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the asset.
        /// </param>
        /// <returns>
        /// true if it exists; -or- otherwise false.
        /// </returns>
        bool Exists( string assetName );

        /// <summary>
        /// Attempts to loads the <see cref="Sprite"/> or an instance of
        /// an <see cref="AnimatedSprite"/> that has the given <paramref name="assetName"/>.
        /// </summary>
        /// <remarks>
        /// No ISprite instance is created.
        /// </remarks>
        /// <param name="assetName">
        /// The name that uniquely identifies the asset to load.
        /// </param>
        /// <returns>
        /// The ISpriteAsset that has been loaden.
        /// </returns>
        ISpriteAsset LoadAsset( string assetName );
    }
}
