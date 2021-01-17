// <copyright file="ManifestBuilder.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.Manifest.ManifestBuilder class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.AutoUpdate.Manifest
{
    using System;
    
    /// <summary>
    /// Implements a mechanism that makes it easier to
    /// create <see cref="IManifest"/> instances.
    /// </summary>
    public sealed class ManifestBuilder : IManifestBuilder
    {
        /// <summary>
        /// Initializes a new instance of the ManifestBuilder class.
        /// </summary>
        /// <param name="fileSystem">
        /// Provides access to the file-system.
        /// </param>
        public ManifestBuilder( IFileSystem fileSystem )
        {
            if( fileSystem == null )
                throw new ArgumentNullException( "fileSystem" );

            this.fileSystem = fileSystem;
        }

        /// <summary>
        /// Builds a new IManifest that includes all files
        /// the directory at the specified path contains.
        /// </summary>
        /// <param name="path">
        /// The path of the directory.
        /// </param>
        /// <returns>
        /// A new IManifest instance.
        /// </returns>
        public IManifest BuildFromDirectory( string path )
        {
            var files = fileSystem.GetFilesInDirectory( path );
            return Build( files );
        }
        
        /// <summary>
        /// Builds a new IManifest from the specified IManifestFiles.
        /// </summary>
        /// <param name="files">
        /// The input IManifestFiles.
        /// </param>
        /// <returns>
        /// A new IManifest instance.
        /// </returns>
        private static IManifest Build( IManifestFile[] files )
        {
            Manifest manifest = new Manifest();

            foreach( var file in files )
            {
                manifest.AddFile( file );
            }

            return manifest;
        }

        /// <summary>
        /// Provides access to the file-system.
        /// </summary>
        private readonly IFileSystem fileSystem;
    }
}
