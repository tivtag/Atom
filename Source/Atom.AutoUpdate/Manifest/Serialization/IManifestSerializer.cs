// <copyright file="IBinaryManifestSerializer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.Manifest.IManifestSerializer interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.AutoUpdate.Manifest.Serialization
{
    using System.IO;

    /// <summary>
    /// Implements a mechanism that allow to de-/serializes an <see cref="IManifest"/>.
    /// </summary>
    public interface IManifestSerializer
    {
        /// <summary>
        /// Serializes the specified <see cref="IManifest"/> into
        /// the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="manifest">
        /// The IManifest to serialize.
        /// </param>
        /// <param name="stream">
        /// The output stream to which should be written.
        /// </param>
        void Serialize( IManifest manifest, Stream stream );

        /// <summary>
        /// Deserializes an <see cref="IManifest"/> instance from the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        /// The input stream to read from.
        /// </param>
        /// <returns>
        /// The IManifest that has been deserialized.
        /// </returns>
        IManifest Deserialize( Stream stream );
    }
}
