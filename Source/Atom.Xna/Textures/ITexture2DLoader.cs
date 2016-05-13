// <copyright file="ITexture2DLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ITexture2DLoader interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Provides a mechanism that allows loading of Texture2D assets.
    /// </summary>
    public interface ITexture2DLoader : IAssetLoader
    {
        /// <summary>
        /// Tries to receive the <see cref="Texture2D"/> with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name of the texture asset to load.
        /// </param>
        /// <returns>
        /// The loaded Texture2D.
        /// </returns>
        Texture2D Load( string assetName );
    }
}
