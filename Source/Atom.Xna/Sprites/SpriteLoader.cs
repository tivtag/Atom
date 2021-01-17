// <copyright file="SpriteLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteLoader class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Atom.Storage;
    
    /// <summary>
    /// Implements a mechanism that allows loading of <see cref="Sprite"/> and
    /// <see cref="AnimatedSprite"/> assets.
    /// </summary>
    public sealed class SpriteLoader : ISpriteLoader, ISpriteSource
    {
        /// <summary>
        /// Gets or sets the root directory that is automatically
        /// prefixed to assets loaded by this SpriteLoader.
        /// </summary>
        public string RootDirectory
        {
            get
            {
                return this.rootDirectory;
            }

            set
            {
                this.rootDirectory = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="Sprite"/> that have been loaded into this SpriteLoader.
        /// </summary>
        public IEnumerable<Sprite> Sprites
        {
            get
            {
                return this.sprites.Values;
            }
        }

        /// <summary>
        /// Gets the <see cref="AnimatedSprite"/> that have been loaded into this SpriteLoader.
        /// </summary>
        public IEnumerable<AnimatedSprite> AnimatedSprites
        {
            get
            {
                return this.animatedSprites.Values;
            }
        }

        /// <summary>
        /// Gets the number of normal <see cref="Sprite"/>s that this SpriteLoader contains.
        /// </summary>
        public int SpriteCount
        {
            get
            {
                return this.sprites.Count;
            }
        }

        /// <summary>
        /// Loads the Sprite with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the Sprite to load.
        /// </param>
        /// <returns>
        /// The Sprite that has been loaden.
        /// </returns>
        public Sprite LoadSprite( string assetName )
        {
            assetName = this.TransformAssetName( assetName );

            Sprite sprite;

            if( this.sprites.TryGetValue( assetName, out sprite ) )
            {
                return sprite;
            }
            else
            {
                throw CreateNotFound( "Sprite", assetName );
            }
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
            assetName = TransformAssetName( assetName );
            return this.sprites.ContainsKey( assetName );
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
            assetName = this.TransformAssetName( assetName );

            AnimatedSprite animatedSprite;

            if( this.animatedSprites.TryGetValue( assetName, out animatedSprite ) )
            {
                return animatedSprite;
            }
            else
            {
                throw CreateNotFound( "AnimatedSprite", assetName );
            }
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
            var asset = this.LoadAsset( assetName );
            return asset.CreateInstance();
        }

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
        public ISpriteAsset LoadAsset( string assetName )
        {
            assetName = this.TransformAssetName( assetName );

            Sprite sprite;

            if( this.sprites.TryGetValue( assetName, out sprite ) )
            {
                return sprite;
            }
            else
            {
                AnimatedSprite animatedSprite;

                if( this.animatedSprites.TryGetValue( assetName, out animatedSprite ) )
                {
                    return animatedSprite;
                }
                else
                {
                    throw CreateNotFound( "Sprite or AnimatedSprite", assetName );
                }
            }
        }

        /// <summary>
        /// Transforms the given assetName.
        /// </summary>
        /// <param name="assetName">
        /// The input assetName.
        /// </param>
        /// <returns>
        /// The output assetName.
        /// </returns>
        private string TransformAssetName( string assetName )
        {
            Contract.Requires<ArgumentNullException>( assetName != null );

            if( assetName.StartsWith( this.rootDirectory, StringComparison.Ordinal ) )
            {
                assetName = assetName.Substring( 0, this.rootDirectory.Length );
            }

            return assetName;
        }

        /// <summary>
        /// Loads the content of the given SpriteDatabase into this SpriteLoader.
        /// </summary>
        /// <param name="database">
        /// The database to load.
        /// </param>
        public void Insert( SpriteDatabase database )
        {
            Contract.Requires<ArgumentNullException>( database != null );

            // Sprites
            var spriteList = database.Atlas.Sprites;

            for( int index = 0; index < spriteList.Count; ++index )
            {
                var sprite = spriteList[index];
                var assetName = this.TransformAssetName( sprite.Name );

                this.sprites[assetName] = sprite;
            }

            // Animated Sprites
            var animartedSpriteList = database.AnimatedSprites;

            for( int index = 0; index < animartedSpriteList.Count; ++index )
            {
                var animatedSprite = animartedSpriteList[index];
                var assetName = this.TransformAssetName( animatedSprite.Name );

                this.animatedSprites[assetName] = animatedSprite;
            }
        }

        /// <summary>
        /// Inserts the content of the <see cref="SpriteDatabase"/>s stored in the specified files into this SpriteLoader.
        /// </summary>
        /// <param name="spriteDatabaseFiles">
        /// The file names of the SpriteDatabases to load.
        /// </param>
        /// <param name="textureLoader">
        /// Provides a mechanism for loading texture information.
        /// </param>
        public void Insert( string[] spriteDatabaseFiles, ITexture2DLoader textureLoader )
        {
            var reader = new SpriteDatabase.ReaderWriter( textureLoader );
            SpriteDatabase[] databases = new SpriteDatabase[spriteDatabaseFiles.Length];

            Parallel.For(
                0,
                spriteDatabaseFiles.Length,
                index =>
                {
                    databases[index] = StorageUtilities.LoadFromFile<SpriteDatabase>(
                        spriteDatabaseFiles[index],
                        reader
                    );
                }
            );

            foreach( var database in databases )
            {
                this.Insert( database );
            }
        }

        /// <summary>
        /// Inserts the content of the <see cref="SpriteDatabase"/>s stored in the specified file into this SpriteLoader.
        /// </summary>
        /// <param name="spriteDatabaseFile">
        /// The file name of the SpriteDatabase to load.
        /// </param>
        /// <param name="textureLoader">
        /// Provides a mechanism for loading texture information.
        /// </param>
        public void Insert( string spriteDatabaseFile, ITexture2DLoader textureLoader )
        {
            var database = StorageUtilities.LoadFromFile<SpriteDatabase>(
                spriteDatabaseFile,
                new SpriteDatabase.ReaderWriter( textureLoader )
            );

            this.Insert( database );
        }

        /// <summary>
        /// Disposes the resources this SpriteLoader has aquired.
        /// </summary>
        public void Dispose()
        {
            this.UnloadAll();
        }

        /// <summary>
        /// Unloads all assets that have been loaden with this IAssetLoader.
        /// </summary>
        public void UnloadAll()
        {
            this.sprites.Clear();
            this.animatedSprites.Clear();
        }

        /// <summary>
        /// Relodas all textures of all sprites that have been cached.
        /// </summary>
        /// <param name="textureLoader">
        /// The loader to use.
        /// </param>
        public void ReloadTextures( ITexture2DLoader textureLoader )
        {
            foreach( var sprite in this.sprites.Values )
            {
                sprite.Texture = textureLoader.Load( sprite.Texture.Name );
            }
        }

        /// <summary>
        /// Creates a new <see cref="NotFoundException"/>.
        /// </summary>
        /// <param name="typeName">
        /// A string that identifies the type of the sprites that was attempted to be loaded.
        /// </param>
        /// <param name="assetName">
        /// The name of the asset that was attempted to be loaded.
        /// </param>
        /// <returns>
        /// A newly created NotFoundException.
        /// </returns>
        private static NotFoundException CreateNotFound( string typeName, string assetName )
        {
            return new NotFoundException(
                string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    "Could not load the {0} '{1}'. Not found!",
                    typeName,
                    assetName ?? string.Empty
                )
            );
        }
        
        /// <summary>
        /// Represents the storage field of the <see cref="RootDirectory"/> property.
        /// </summary>
        private string rootDirectory = "Content/Sprites/";

        /// <summary>
        /// The dictionary that maps assetNames onto Sprites.
        /// </summary>
        private readonly Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>( StringComparer.OrdinalIgnoreCase );
        
        /// <summary>
        /// The dictionary that maps assetNames onto AnimatedSprite.
        /// </summary>
        private readonly Dictionary<string, AnimatedSprite> animatedSprites = new Dictionary<string, AnimatedSprite>( StringComparer.OrdinalIgnoreCase );
    }
}
