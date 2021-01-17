// <copyright file="CachingCustomAssetLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.CachingCustomAssetLoader{TAsset} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Implements a base implementation of the ICustomAssetLoader{TAsset} class
    /// that caches previously loaded assets by their assetName.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The exact type of the IAsset the ICustomAssetLoader can load.
    /// </typeparam>
    public abstract class CachingCustomAssetLoader<TAsset> : ManagedDisposable, ICustomAssetLoader<TAsset>
        where TAsset : IAsset
    {
        /// <summary>
        /// Gets the assets that have been cached using this CachingCustomAssetLoader{TAsset}.
        /// </summary>
        protected IEnumerable<TAsset> Assets
        {
            get
            {
                return this.assets.Values;
            }
        }

        /// <summary>
        /// Gets or sets the root path that is automatically
        /// prefixed to assets loaded by this CachingCustomAssetLoader{TAsset}.
        /// </summary>
        public virtual string RootDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// Tries to receive the asset with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the asset to load.
        /// </param>
        /// <returns>
        /// The loaded asset.
        /// </returns>
        public TAsset Load( string assetName )
        {
            string filePath;

            assetName = assetName.Replace( '/', '\\' );

            if( assetName.StartsWith( this.RootDirectory, StringComparison.Ordinal ) )
            {
                filePath = assetName;
                assetName = assetName.Substring( this.RootDirectory.Length, assetName.Length - this.RootDirectory.Length );
            }
            else
            {
                filePath = Path.Combine( this.RootDirectory, assetName );
            }

            TAsset asset;

            if( !this.assets.TryGetValue( assetName, out asset ) )
            {
                asset = this.ActuallyLoad( assetName, filePath );
                this.assets.Add( assetName, asset );
            }

            return asset;
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
        protected abstract TAsset ActuallyLoad( string assetName, string filePath );

        /// <summary>
        /// Releases all managed resources.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            foreach( var entry in this.assets )
            {
                var disposable = entry.Value as IDisposable;

                if( disposable != null )
                {
                    disposable.Dispose();
                }
            }

            this.assets.Clear();
        }

        /// <summary>
        /// Unloads all assets that have been loaden with this IAssetLoader.
        /// </summary>
        public virtual void UnloadAll()
        {
            this.assets.Clear();
        }
        
        /// <summary>
        /// The currently cached assets.
        /// </summary>
        private readonly Dictionary<string, TAsset> assets = new Dictionary<string, TAsset>();
    }
}
