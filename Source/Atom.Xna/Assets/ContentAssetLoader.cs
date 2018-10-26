// <copyright file="ContentAssetLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ContentAssetLoader class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// Implements the base implementation of an IAssetLoader that uses the
    /// xna <see cref="ContentManager"/> to load and manage assets.
    /// </summary>
    public abstract class ContentAssetLoader : IAssetLoader
    {
        /// <summary>
        /// Gets or sets the root directory that is automatically
        /// prefixed to assets loaded by this IAssetLoader.
        /// </summary>
        public string RootDirectory
        {
            get
            {
                return this.relativeRootDirectory;
            }

            set
            {
                this.relativeRootDirectory = value;
                this.contentManager.RootDirectory = value;
            }
        }

        /// <summary>
        /// Gets the ContentManager this ContentAssetLoader internally uses
        /// to load assets.
        /// </summary>
        protected ContentManager ContentManager
        {
            get
            {
                return this.contentManager;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ContentAssetLoader class.
        /// </summary>
        /// <param name="contentManager">
        /// The Xna ContentManager responsible for loading the assets.
        /// </param>
        protected ContentAssetLoader( ContentManager contentManager )
        {
            Contract.Requires<ArgumentNullException>( contentManager != null );

            this.contentManager = contentManager;
            this.relativeRootDirectory = contentManager.RootDirectory.Replace( AppDomain.CurrentDomain.BaseDirectory, string.Empty );
        }

        /// <summary>
        /// Releases all resources used by this ContentAssetLoader.
        /// </summary>
        public void Dispose()
        {
            this.contentManager.Dispose();
        }

        /// <summary>
        /// Unloads all assets that have been loaden with this ContentAssetLoader.
        /// </summary>
        public void UnloadAll()
        {
            this.contentManager.Unload();
        }

        /// <summary>
        /// The relative root directory.
        /// </summary>
        private string relativeRootDirectory;

        /// <summary>
        /// The Xna ContentManager responsible for loading the Texture2D assets.
        /// </summary>
        private readonly ContentManager contentManager;
    }
}
