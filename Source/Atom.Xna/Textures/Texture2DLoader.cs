// <copyright file="Texture2DLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Texture2DLoader class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System;
    using System.IO;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements a mechanism that allows loading of Texture2D assets
    /// from compiled xna resources by using a <see cref="Microsoft.Xna.Framework.Content.ContentManager"/>.
    /// </summary>
    public class Texture2DLoader : ContentAssetLoader, ITexture2DLoader
    {        
        /// <summary>
        /// Initializes a new instance of the Texture2DLoader class, manually creating a new ContentManager
        /// with a default RootDirectory of "Content/Textures/".
        /// </summary>
        /// <param name="serviceProvider">
        /// The IServiceProvider the ContentManager should use to locate services.
        /// </param>
        public Texture2DLoader( IServiceProvider serviceProvider )
            : this( new ContentManager( serviceProvider ) )
        {
        }

        /// <summary>
        /// Initializes a new instance of the Texture2DLoader class.
        /// </summary>
        /// <param name="contentManager">
        /// The Xna ContentManager responsible for loading the Texture2D assets.
        /// </param>
        public Texture2DLoader( ContentManager contentManager )
            : base( contentManager )
        {
            this.RootDirectory = @"Content\Textures\";
        }

        /// <summary>
        /// Tries to receive the <see cref="Texture2D"/> with the given <paramref name="assetName"/>.
        /// This method is thread safe.
        /// </summary>
        /// <param name="assetName">
        /// The name of the texture asset to load.
        /// </param>
        /// <returns>
        /// The loaded Texture2D.
        /// </returns>
        public Texture2D Load( string assetName )
        {
            string actualAssetName = this.TransformAssetName( assetName );
            
            lock( this.sync )
            {
                Texture2D texture = this.ContentManager.Load<Texture2D>( actualAssetName );
                texture.Name = actualAssetName;
                return texture;
            }
        }

        /// <summary>
        /// Transforms the given assetName into a name that points to a serialized Xna resource.
        /// </summary>
        /// <param name="assetName">
        /// The input asset name.
        /// </param>
        /// <returns>
        /// The transformed asset name.
        /// </returns>
        private string TransformAssetName( string assetName )
        {
            assetName = assetName.Replace( '/', '\\' );

            if( assetName.StartsWith( this.RootDirectory, StringComparison.Ordinal ) )
            {                
                assetName = assetName.Substring( this.RootDirectory.Length, assetName.Length - this.RootDirectory.Length );
            }

            return Path.ChangeExtension( assetName, null );
        }

        /// <summary>
        /// The object that is used to aquire a lock for the texture loading operation.
        /// </summary>
        private readonly object sync = new object();
    }
}
