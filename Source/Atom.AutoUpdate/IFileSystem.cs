// <copyright file="IFileSystem.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.IFileSystem interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.AutoUpdate
{
    using Atom.AutoUpdate.Manifest;

    /// <summary>
    /// Provides access to the file system.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Gets information about the files in the specified directory path.
        /// </summary>
        /// <param name="path">
        /// The path of the directory.
        /// </param>
        /// <returns>
        /// The files in the specified directory.
        /// </returns>
        IManifestFile[] GetFilesInDirectory( string path );
    }
}
