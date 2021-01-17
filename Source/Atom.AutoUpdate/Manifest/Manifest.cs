// <copyright file="Manifest.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.Manifest.Manifest class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.AutoUpdate.Manifest
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Encapsultes the state of the files of the application that are
    /// supposed to be updateable.
    /// </summary>
    /// <remarks>
    /// Manifests are usually stored on a web-server and downloaded to the client.
    /// </remarks>
    public sealed class Manifest : IManifest
    {
        /// <summary>
        /// Gets the IManifestFileEntries that are part of this Manifest.
        /// </summary>
        public IEnumerable<IManifestFile> Files
        {
            get
            {
                return this.files;
            }
        }

        /// <summary>
        /// Adds the specified <see cref="IManifestFile"/> to this Manifest.
        /// </summary>
        /// <param name="entry">
        /// The entry to add.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If the specified <paramref name="entry"/> is null.
        /// </exception>
        public void AddFile( IManifestFile entry )
        {
            if( entry == null )
                throw new ArgumentNullException( "entry" );

            this.files.Add( entry );
        }

        /// <summary>
        /// The files that are part of this Manifest.
        /// </summary>
        private readonly List<IManifestFile> files = new List<IManifestFile>();
    }
}
