// <copyright file="IObjectReader.TObject.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IObjectReader{TObject} interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Storage
{
    /// <summary>
    /// Provides a mechanism that allows the deserialization of a specific object type.
    /// </summary>
    /// <typeparam name="TObject">
    /// The type of the object that is deserialized.
    /// </typeparam>
    public interface IObjectReader<TObject> : IObjectReader
    {
        /// <summary>
        /// Deserializes the given object using the given IDeserializationContext.
        /// </summary>
        /// <param name="object">
        /// The object to deserialize.
        /// </param>
        /// <param name="context">
        /// The context that provides everything required for the deserialization process.
        /// </param>
        void Deserialize( TObject @object, IDeserializationContext context );
    }
}
