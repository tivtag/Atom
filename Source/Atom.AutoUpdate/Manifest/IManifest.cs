// <copyright file="IManifest.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.Manifest.IManifest interface.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.AutoUpdate.Manifest
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Encapsultes the files of the application that are supposed to be updateable.
    /// </summary>
    /// <remarks>
    /// Manifests are usually stored on a web-server and downloaded to the client.
    /// </remarks>
    public interface IManifest
    {
        /// <summary>
        /// Gets the IManifestFileEntries that are part of this IManifest.
        /// </summary>
        IEnumerable<IManifestFile> Files
        {
            get;
        }
        
        /// <summary>
        /// Adds the specified <see cref="IManifestFile"/> to this IManifest.
        /// </summary>
        /// <param name="entry">
        /// The entry to add.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// If the specified <paramref name="entry"/> is null.
        /// </exception>
        void AddFile( IManifestFile entry );
    }
}
