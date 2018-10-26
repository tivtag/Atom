// <copyright file="SpriteSheetLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteSheetLoader class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Storage;

    /// <summary>
    /// Implements a mechanism that allows loading of <see cref="SpriteSheet"/> assets.
    /// This class can't be inherited.
    /// </summary>
    public sealed class SpriteSheetLoader : CachingCustomAssetLoader<ISpriteSheet>, ISpriteSheetLoader
    {
        /// <summary>
        /// Initializes a new instance of the SpriteSheetLoader class.
        /// </summary>
        /// <param name="spriteLoader">
        /// Provides a mechanism for loading the sprite resources used by the ISpriteSheets.
        /// </param>
        public SpriteSheetLoader( ISpriteLoader spriteLoader )
        {
            Contract.Requires<ArgumentNullException>( spriteLoader != null );

            this.RootDirectory = "Content/Sheets/";
            this.reader = new SpriteSheet.ReaderWriter( spriteLoader );
        }

        /// <summary>
        /// Loads the <see cref="ISpriteSheet"/> resource with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the ISpriteSheet resource to load.
        /// </param>
        /// <returns>
        /// The ISpriteSheet resource thas has been loaden.
        /// </returns>
        public ISpriteSheet LoadSpriteSheet( string assetName )
        {
            return base.Load( assetName );
        }
        
        /// <summary>
        /// Implements the actual asset loading logic used by this CachingCustomAssetLoader{TAsset}.
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
        protected override ISpriteSheet ActuallyLoad( string assetName, string filePath )
        {
            if( !filePath.EndsWith( SpriteSheet.ReaderWriter.Extension, StringComparison.Ordinal ) )
            {
                filePath += SpriteSheet.ReaderWriter.Extension;
            }

            return StorageUtilities.LoadFromFile<SpriteSheet>( filePath, this.reader );
        }

        /// <summary>
        /// Provides a mechanism for loading the sprite resources used by the ISpriteSheets.
        /// </summary>
        private readonly SpriteSheet.ReaderWriter reader;
    }
}
