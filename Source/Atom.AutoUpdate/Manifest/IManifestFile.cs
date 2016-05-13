// <copyright file="IManifestFileEntry.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.Manifest.IManifestFile interface.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.AutoUpdate.Manifest
{
    using System;

    /// <summary>
    /// Specifies the data that is stored in an <see cref="IManifest"/>
    /// descriping the properties required for updating a specific file of the application.
    /// </summary>
    public interface IManifestFile
    {
        /// <summary>
        /// Gets the hash of the file.
        /// </summary>
        string FileHash 
        {
            get;
        }

        /// <summary>
        /// Gets the date and time the file was last modified.
        /// </summary>
        DateTime FileModificationTime 
        {
            get;
        }

        /// <summary>
        /// Gets the full name of the file.
        /// </summary>
        string FileName
        {
            get;
        }
    }
}
