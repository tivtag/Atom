// <copyright file="NullSpriteLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.NullSpriteLoader class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System;

    /// <summary>
    /// Implements an <see cref="ISpriteLoader"/> that does absolutely nothing.
    /// </summary>
    public sealed class NullSpriteLoader : ISpriteLoader
    {
        /// <summary>
        /// Gets or sets the root directory that is automatically
        /// prefixed to assets loaded by this IAssetLoader.
        /// </summary>
        public string RootDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the NullSpriteLoader class.
        /// </summary>
        public NullSpriteLoader()
        {
            this.RootDirectory = string.Empty;
        }

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
        public ISprite Load( string assetName )
        {
            return null;
        }

        /// <summary>
        /// Determines whether an <see cref="Sprite"/> with the given <paramref name="assetName"/> exists.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the asset.
        /// </param>
        /// <returns>
        /// true if it exists; -or- otherwise false.
        /// </returns>
        public bool Exists( string assetName )
        {
            return false;
        }

        /// <summary>
        /// Attempts to loads the <see cref="Sprite"/> or an instance of
        /// an <see cref="AnimatedSprite"/> that has the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the asset to load.
        /// </param>
        /// <returns>
        /// The ISpriteAsset that has been loaden.
        /// </returns>
        public ISpriteAsset LoadAsset( string assetName )
        {
            return null;
        }

        /// <summary>
        /// Loads the <see cref="Sprite"/> with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the Sprite to load.
        /// </param>
        /// <returns>
        /// The Sprite that has been loaden.
        /// </returns>
        public Sprite LoadSprite( string assetName )
        {
            return null;
        }
        
        /// <summary>
        /// Loads the AnimatedSprite with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the AnimatedSprite to load.
        /// </param>
        /// <returns>
        /// The AnimatedSprite that has been loaden.
        /// </returns>
        public AnimatedSprite LoadAnimatedSprite( string assetName )
        {
            return null;
        }

        /// <summary>
        /// Disposes the resources this IAssetLoader has aquired.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Unloads all assets that have been loaden with this IAssetLoader.
        /// </summary>
        public void UnloadAll()
        {
        }
    }
}
