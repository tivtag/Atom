// <copyright file="ManifestFileEntry.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.Manifest.ManifestFile class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.AutoUpdate.Manifest
{
    using System;

    /// <summary>
    /// Specifies the data that is stored in an <see cref="IManifest"/>
    /// descriping the properties required for updating a specific file of the application.
    /// </summary>
    public sealed class ManifestFile : IManifestFile
    {
        /// <summary>
        /// Gets the hash of the file.
        /// </summary>
        public string FileHash
        {
            get
            {
                return this.fileHash;
            }
        }

        /// <summary>
        /// Gets the date and time the file was last modified.
        /// </summary>
        public DateTime FileModificationTime
        {
            get
            {
                return this.fileModificationDateTime;
            }
        }

        /// <summary>
        /// Gets the full name of the file.
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ManifestFileEntry class.
        /// </summary>
        /// <param name="fileName">
        /// The full name of the file.
        /// </param>
        /// <param name="fileHash">
        /// The hash-code of the file.
        /// </param>
        /// <param name="fileModificationDateTime">
        /// The DateTime the file was last modified.
        /// </param>
        public ManifestFile( string fileName, string fileHash, DateTime fileModificationDateTime )
        {
            this.fileName = fileName;
            this.fileModificationDateTime = fileModificationDateTime;
            this.fileHash = fileHash;
        }

        /// <summary>
        /// The hash-code of the file.
        /// </summary>
        private readonly string fileHash;

        /// <summary>
        ///  The DateTime the file was last modified.
        /// </summary>
        private readonly DateTime fileModificationDateTime;

        /// <summary>
        /// The full name of the file.
        /// </summary>
        private readonly string fileName;
    }
}
