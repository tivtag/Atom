// <copyright file="IObjectWriter.TObject.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IObjectWriter{TObject} interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage
{
    /// <summary>
    /// Provides a mechanism that allows the serialization of a specific object type.
    /// </summary>
    /// <typeparam name="TObject">
    /// The type of the object that is serialized.
    /// </typeparam>
    public interface IObjectWriter<TObject> : IObjectWriter
    {
        /// <summary>
        /// Serializes the given object using the given ISerializationContext.
        /// </summary>
        /// <param name="object">
        /// The object to serialize.
        /// </param>
        /// <param name="context">
        /// The context that provides everything required for the serialization process.
        /// </param>
        void Serialize( TObject @object, ISerializationContext context );
    }
}
