// <copyright file="StorageUtilities.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.StorageUtilities class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage
{
    using System;
    using Atom.Diagnostics.Contracts;
    using System.IO;

    /// <summary>
    /// Defines static storage related utility methods.
    /// </summary>
    public static class StorageUtilities
    {
        /// <summary>
        /// Loads an object of type <typeparamref name="T"/> by deserializing the data stored
        /// within the file at the given <paramref name="filePath"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object to deserialize. Is required to have a public parameterless constructor.
        /// </typeparam>
        /// <param name="filePath">
        /// The path of the file to open.
        /// </param>
        /// <param name="reader">
        /// The IObjectReader{T} that contains the actual deserialization logic.
        /// </param>
        /// <returns>
        /// The de-serialized object.
        /// </returns>
        public static T LoadFromFile<T>( string filePath, IObjectReader<T> reader )
            where T : new()
        {
            Contract.Requires<ArgumentNullException>( filePath != null );
            Contract.Requires<ArgumentNullException>( reader != null );
            // Contract.Ensures( Contract.Result<T>() != null );

            using ( var stream = File.OpenRead( filePath ) )
            {
                var context = new BinaryDeserializationContext( stream );

                var @object = new T();
                reader.Deserialize( @object, context );
                return @object;
            }
        }

        /// <summary>
        /// Saves an object of type <typeparamref name="T"/> by serializing the data stored
        /// into a file at the given <paramref name="filePath"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object to serialize.
        /// </typeparam>
        /// <param name="filePath">
        /// The path of the file to open.
        /// </param>
        /// <param name="object">
        /// The object to serialize.
        /// </param>
        /// <param name="writer">
        /// The IObjectWriter{T} that contains the actual serialization logic.
        /// </param>
        public static void SaveToFile<T>( string filePath, T @object, IObjectWriter<T> writer )
        {
            Contract.Requires<ArgumentNullException>( filePath != null );
            Contract.Requires<ArgumentNullException>( @object != null );
            Contract.Requires<ArgumentNullException>( writer != null );

            using( var stream = File.Open( filePath, FileMode.Create, FileAccess.Write ) )
            {
                var context = new BinarySerializationContext( stream );
                writer.Serialize( @object, context );
            }
        }

        /// <summary>
        /// Saves an object of type <typeparamref name="T"/> by serializing the data stored
        /// into a file at the given <paramref name="filePath"/>.
        /// The file won't be corrupted if an exception occurs during the serializing
        /// process.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object to serialize.
        /// </typeparam>
        /// <param name="filePath">
        /// The path of the file to save to.
        /// </param>
        /// <param name="object">
        /// The object to serialize.
        /// </param>
        /// <param name="writer">
        /// The IObjectWriter{T} that contains the actual serialization logic.
        /// </param>
        public static void SafeSaveToFile<T>( string filePath, T @object, IObjectWriter<T> writer )
        {
            Contract.Requires<ArgumentNullException>( filePath != null );
            Contract.Requires<ArgumentNullException>( @object != null );
            Contract.Requires<ArgumentNullException>( writer != null );

            using( var memoryStream = new MemoryStream() )
            {
                var context = new BinarySerializationContext( memoryStream );
                writer.Serialize( @object, context );

                CopyToFile( memoryStream, filePath );
            }
        }

        /// <summary>
        /// Copies the complete content of the specified <see cref="Stream"/> into the file
        /// at the specified <paramref name="filePath"/>.
        /// </summary>
        /// <param name="stream">
        /// The Stream to copy.
        /// </param>
        /// <param name="filePath">
        /// The path of the file to save to.
        /// </param>
        public static void CopyToFile( Stream stream, string filePath )
        {
            Contract.Requires<ArgumentNullException>( stream != null );
            Contract.Requires<ArgumentNullException>( filePath != null );

            using( var fileStream = File.Open( filePath, FileMode.Create, FileAccess.Write ) )
            {
                IOUtilities.CopyStream( stream, fileStream );
            }
        }
    }
}
