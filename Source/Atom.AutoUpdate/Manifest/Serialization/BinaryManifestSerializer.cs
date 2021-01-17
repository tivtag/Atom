// <copyright file="BinaryManifestSerializer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.Manifest.BinaryManifestSerializer class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.AutoUpdate.Manifest.Serialization
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Implements a mechanism that allows to de-/serializes an <see cref="IManifest"/>
    /// from or into a binary format.
    /// </summary>
    public sealed class BinaryManifestSerializer : IManifestSerializer
    {
        /// <summary>
        /// Gets or sets a value indicating whether this BinaryManifestSerializer
        /// should include the hash-information when hashing.
        /// </summary>
        public bool SerializeHash
        {
            get;
            set;
        }

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
        public void Serialize( IManifest manifest, Stream stream )
        {
            var writer = new BinaryWriter( stream );

            // Header
            const int CurrentVersion = 3;
            writer.Write( CurrentVersion );
            writer.Write( manifest.GetType().GetTypeName() );
            writer.Write( this.SerializeHash );
            writer.Write( GetHashType() );

            // Files
            writer.Write( manifest.Files.Count() );

            if( this.SerializeHash )
            {
                foreach( var file in manifest.Files )
                {
                    writer.Write( file.FileName );
                    writer.Write( file.FileHash );
                    writer.Write( file.FileModificationTime.ToBinary() );
                }
            }
            else
            {
                foreach( var file in manifest.Files )
                {
                    writer.Write( file.FileName );
                    writer.Write( file.FileModificationTime.ToBinary() );
                }
            }
        }

        /// <summary>
        /// Deserializes an <see cref="IManifest"/> instance from the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        /// The input stream to read from.
        /// </param>
        /// <returns>
        /// The IManifest that has been deserialized.
        /// </returns>
        public IManifest Deserialize( Stream stream )
        {
            var reader = new BinaryReader( stream );
            
            // Header
            const int CurrentVersion = 3;
            int version = reader.ReadInt32();
            
            if( version != CurrentVersion )
            {
                throw new InvalidDataException(
                    string.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        "Error while deserializing an IManifest. Version not supported. Expected {0} or older, while it was {1}",
                        CurrentVersion,
                        version
                    )
                );
            }

            // Read Manifest Type.
            string manifestTypeName = reader.ReadString();
            Type manifestType = Type.GetType( manifestTypeName );
            var manifest = (IManifest)Activator.CreateInstance( manifestType );

            // Read Hash Properties.
            bool includesHash = reader.ReadBoolean();
            string hashType = reader.ReadString();
            
            if( !hashType.Equals( GetHashType(), StringComparison.Ordinal ) )
            {
                throw new InvalidDataException(
                    string.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        "Error while deserializing an IManifest. Unexpected HashType. Expected {0} or older, while it was {1}",
                        GetHashType(),
                        hashType
                    )
                );
            }

            // Files
            int fileCount = reader.ReadInt32();

            if( includesHash )
            {
                for( int i = 0; i < fileCount; ++i )
                {
                    string fileName = reader.ReadString();
                    string fileHash = reader.ReadString();
                    DateTime fileModificationDateTime = DateTime.FromBinary( reader.ReadInt64() );

                    manifest.AddFile( new ManifestFile( fileName, fileHash, fileModificationDateTime ) );
                }
            }
            else
            {
                for( int i = 0; i < fileCount; ++i )
                {
                    string fileName = reader.ReadString();
                    DateTime fileModificationDateTime = DateTime.FromBinary( reader.ReadInt64() );
                  
                    manifest.AddFile( new ManifestFile( fileName, string.Empty, fileModificationDateTime ) );
                }
            }

            return manifest;
        }

        /// <summary>
        /// Gets a string that uniquely identifies the hashing-algorithm used by the BinaryManifestSerializer.
        /// </summary>
        /// <returns>
        /// The string written to the Manifest.
        /// </returns>
        private static string GetHashType()
        {
            return "MD5";
        }
    }
}
