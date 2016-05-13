// <copyright file="NullTexture2DLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.NullTexture2DLoader interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    /// <summary>
    /// Implements an <see cref="ITexture2DLoader"/> that does
    /// absolutely nothing.
    /// </summary>
    public sealed class NullTexture2DLoader : ITexture2DLoader
    {
        /// <summary>
        /// Tries to receive the <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> with
        /// the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name of the texture asset to load.
        /// </param>
        /// <returns>
        /// The loaded Texture2D.
        /// </returns>
        public Microsoft.Xna.Framework.Graphics.Texture2D Load( string assetName )
        {
            return null;
        }

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
        /// Disposes this NullTexture2DLoader.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Unloads all Texture2D resources that have been loaden.
        /// </summary>
        public void UnloadAll()
        {
        }
    }
}
