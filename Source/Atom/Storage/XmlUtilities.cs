// <copyright file="XmlUtilities.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.XmlUtilities class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Provides XML serialization related utility methods.
    /// </summary>
    public static class XmlUtilities
    {
        /// <summary>
        /// Serializes the specified object of type <typeparamref name="T"/> to the file
        /// at the specified <paramref name="path"/>
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to serialize.
        /// </typeparam>
        /// <param name="path">
        /// The path of the target file.
        /// </param>
        /// <param name="obj">
        /// The object to serialize.
        /// </param>
        public static void Serialize<T>( string path, T obj )
        {
            using( var stream = File.Open( path, FileMode.Create, FileAccess.Write ) )
            {
                var serializer = new XmlSerializer( typeof( T ) );
                serializer.Serialize( stream, obj );
            }
        }

        /// <summary>
        /// Deserializes an object of the given type <typeparamref name="T"/> from
        /// the file at the given <paramref name="path"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to deserialize.
        /// </typeparam>
        /// <param name="path">
        /// The path of the file to open.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        public static T Deserialize<T>( string path )
        {
            using( var stream = File.OpenRead( path ) )
            {
                var serializer = new XmlSerializer( typeof( T ) );
                return (T)serializer.Deserialize( stream );
            }
        }

        /// <summary>
        /// Deserializes an object of the given type <typeparamref name="T"/> from
        /// the file at the given <paramref name="path"/>.
        /// If an error has occurred the specified <paramref name="defaultFactory"/> will
        /// be used to create an instance of type T.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to deserialize.
        /// </typeparam>
        /// <param name="path">
        /// The path of the file to open.
        /// </param>
        /// <param name="defaultFactory">
        /// The factory method used when an exception occurs during the process.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        public static T TryDeserialize<T>( string path, Func<Exception, T> defaultFactory )
        {
            try
            {
                return Deserialize<T>( path );
            }
            catch( Exception exc )
            {
                return defaultFactory( exc );
            }
        }
    }
}
