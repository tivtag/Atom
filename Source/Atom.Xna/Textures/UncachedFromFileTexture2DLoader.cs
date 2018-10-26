// <copyright file="UncachedFromFileTexture2DLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UncachedFromFileTexture2DLoader class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using System.IO;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements a mechanism that loads Texture2D objects directly from an image file,
    /// without caching them.
    /// </summary>
    public sealed class UncachedFromFileTexture2DLoader : ITexture2DLoader
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
        /// Initializes a new instance of the UncachedFromFileTexture2DLoader class.
        /// </summary>
        /// <param name="graphicsDeviceService">
        /// Provides a mechanism that allows to receive the Xna GraphicsDevice.
        /// </param>
        public UncachedFromFileTexture2DLoader( IGraphicsDeviceService graphicsDeviceService )
        {
            Contract.Requires<ArgumentNullException>( graphicsDeviceService != null );

            this.graphicsDeviceService = graphicsDeviceService;
        }

        /// <summary>
        /// Tries to load the <see cref="Texture2D"/> with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name of the texture asset to load.
        /// </param>
        /// <returns>
        /// The loaded Texture2D.
        /// </returns>
        public Texture2D Load( string assetName )
        {
            string path = Path.Combine( this.RootDirectory ?? string.Empty, assetName );

            using( var stream = File.OpenRead( path ) )
            {
                Texture2D texture = Texture2D.FromStream( this.graphicsDeviceService.GraphicsDevice, stream );
                texture.Name = assetName;
                return texture;
            }
        }

        /// <summary>
        /// This method is not required.
        /// </summary>
        void IDisposable.Dispose()
        {
            // no op.
        }

        /// <summary>
        /// Unloads all Texture2D resources that have been loaden.
        /// </summary>
        void IAssetLoader.UnloadAll()
        {
            // no op.
        }

        /// <summary>
        /// Provides a mechanism that allows to receive the Xna GraphicsDevice.
        /// </summary>
        private readonly IGraphicsDeviceService graphicsDeviceService;
    }
}
