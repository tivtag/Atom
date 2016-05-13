// <copyright file="FileSystem.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.FileSystem class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.AutoUpdate
{
    using System;
    using System.IO;
    using Atom.AutoUpdate.Manifest;

    /// <summary>
    /// Provides access to the file system.
    /// </summary>
    public sealed class FileSystem : IFileSystem
    {
        /// <summary>
        /// Initializes a new instance of the FileSystem class.
        /// </summary>
        /// <param name="applicationDirectory">
        /// The base directory of the application.
        /// </param>
        public FileSystem( string applicationDirectory )
            : this( applicationDirectory, new MD5FileHasher() )
        {
        }

        /// <summary>
        /// Initializes a new instance of the FileSystem class.
        /// </summary>
        /// <param name="applicationDirectory">
        /// The base directory of the application.
        /// </param>
        /// <param name="fileHasher">
        /// Provides the mechanism that is used to hash
        /// the generate the hash of the files.
        /// </param>
        public FileSystem( string applicationDirectory, IFileHasher fileHasher )
        {
            this.applicationDirectory = applicationDirectory;
            this.fileHasher = fileHasher;
        }

        /// <summary>
        /// Gets information about the files in the specified directory path.
        /// </summary>
        /// <param name="path">
        /// The path of the directory.
        /// </param>
        /// <returns>
        /// The files in the specified directory.
        /// </returns>
        public IManifestFile[] GetFilesInDirectory( string path )
        {
            if( !path.StartsWith( this.applicationDirectory, StringComparison.Ordinal ) )
            {
                throw new ArgumentException(
                    string.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        "The specified directory path '{0}' doesn't start with the expected application directory '{1}'.",
                        path,
                        this.applicationDirectory
                    ),
                    "path"
                );
            }

            string[] files = Directory.GetFiles( path, "*", SearchOption.AllDirectories );
            IManifestFile[] manifestFiles = new IManifestFile[files.Length];

            for( int i = 0; i < files.Length; ++i )
            {
                manifestFiles[i] = GetFile( files[i] );
            }

            return manifestFiles;
        }

        /// <summary>
        /// Gets information about the file at the specified full path.
        /// </summary>
        /// <param name="file">
        /// The full path of the file to get.
        /// </param>
        /// <returns>
        /// Information about the reqiested file encapsulated in an IManifestFile instance.
        /// </returns>
        private IManifestFile GetFile( string file )
        {
            using( var fileStream = File.OpenRead( file ) )
            {
                string fileName = this.GetFileName( file );
                string fileHash = this.GetFileHash( fileStream );
                DateTime modificationTime = File.GetLastWriteTime( file );

                return new ManifestFile( fileName, fileHash, modificationTime );
            }
        }

        /// <summary>
        /// Gets the file name associated with the given file.
        /// </summary>
        /// <param name="file">
        /// The full path of the file.
        /// </param>
        /// <returns>
        /// The filename including all application directories.
        /// </returns>
        private string GetFileName( string file )
        {
            string fileName = file.Replace( this.applicationDirectory, "" );

            if( fileName.StartsWith( @"\", StringComparison.Ordinal ) )
            {
                fileName = fileName.Remove( 0, 1 );
            }

            return fileName;
        }

        /// <summary>
        /// Gets the hash associated with the given file.
        /// </summary>
        /// <param name="fileStream">
        /// The file that has been loaden.
        /// </param>
        /// <returns>
        /// The hash.
        /// </returns>
        private string GetFileHash( FileStream fileStream )
        {
            return this.fileHasher.GetHash( fileStream );
        }

        /// <summary>
        /// Provides the mechanism that is used to hash
        /// the generate the hash of the files.
        /// </summary>
        private readonly IFileHasher fileHasher;

        /// <summary>
        /// The base directory of the application.
        /// </summary>
        private readonly string applicationDirectory; 
    }
}
