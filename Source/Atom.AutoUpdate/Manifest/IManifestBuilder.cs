// <copyright file="IManifestBuilder.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.Manifest.IManifestBuilder interface.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.AutoUpdate.Manifest
{
    /// <summary>
    /// Provides a mechanism that makes it easier to
    /// create <see cref="IManifest"/> instances.
    /// </summary>
    public interface IManifestBuilder
    {
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
        IManifest BuildFromDirectory( string path );
    }
}
