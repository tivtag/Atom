// <copyright file="FontLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Fonts.FontLoader class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.Fonts
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements an <see cref="IFontLoader"/> that loads <see cref="Font"/>s
    /// that internally use xna <see cref="SpriteFont"/>s which are loaded
    /// using the xna Content Pipeline.
    /// </summary>
    public class FontLoader : CachingCustomAssetLoader<IFont>, IFontLoader
    {
        /// <summary>
        /// Gets or sets the root directory that is automatically
        /// prefixed to assets loaded by this IAssetLoader.
        /// </summary>
        public override string RootDirectory
        {
            get
            {
                return this.content.RootDirectory;
            }

            set
            {
                this.content.RootDirectory = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the FontLoader class, manually creating a new ContentManager
        /// with a default RootDirectory of "Content/Fonts/".
        /// </summary>
        /// <param name="serviceProvider">
        /// The IServiceProvider the ContentManager should use to locate services.
        /// </param>
        public FontLoader( IServiceProvider serviceProvider )
            : this( new ContentManager( serviceProvider ) { RootDirectory = "Content/Fonts/" } )
        {
        }

        /// <summary>
        /// Initializes a new instance of the FontLoader class.
        /// </summary>
        /// <param name="content">
        /// The xna ContentManager that should be used to load the internally used SpriteFonts.
        /// </param>
        public FontLoader( ContentManager content )
        {
            Contract.Requires<ArgumentNullException>( content != null );

            this.content = content;
        }

        /// <summary>
        /// Implements the actual asset loading logic used by this FontLoader.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the asset to load.
        /// </param>
        /// <param name="filePath">
        /// The path of the file thatis assumed to contain the asset to load.
        /// </param>
        /// <returns>
        /// The loaded asset.
        /// </returns>
        protected override IFont ActuallyLoad( string assetName, string filePath )
        {
            SpriteFont spriteFont = this.content.Load<SpriteFont>( assetName );
            return new Font( assetName, spriteFont );
        }

        /// <summary>
        /// Disposes the unmanaged resources used by this FontLoader.
        /// </summary>
        protected override void DisposeUnmanagedResources()
        {
            this.content.Dispose();
        }

        /// <summary>
        /// Unloads all assets that have been loaden with this IAssetLoader.
        /// </summary>
        public override void UnloadAll()
        {
            this.content.Unload();
            base.UnloadAll();
        }

        /// <summary>
        /// Reloads the underlying data of all cached <see cref="IFont"/> assets.
        /// </summary>
        public void Reload()
        {
            this.content.Unload();

            foreach( Font font in this.Assets )
            {
                font.SpriteFont = this.content.Load<SpriteFont>( font.Name );                
            }
        }

        /// <summary>
        /// The xna ContentManager that is used to load xna SpriteFonts.
        /// </summary>
        private readonly ContentManager content;
    }
}
