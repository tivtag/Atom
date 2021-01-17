// <copyright file="ICustomAssetLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.ICustomAssetLoader interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna
{
    using System;

    /// <summary>
    /// Represents an <see cref="IAssetLoader"/> that loads custom <see cref="IAsset"/>s.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The exact type of the IAsset the ICustomAssetLoader can load.
    /// </typeparam>
    public interface ICustomAssetLoader<TAsset> : IAssetLoader
        where TAsset : IAsset
    {
        /// <summary>
        /// Tries to receive the asset with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the asset to load.
        /// </param>
        /// <returns>
        /// The loaded asset.
        /// </returns>
        TAsset Load( string assetName );
    }
}
