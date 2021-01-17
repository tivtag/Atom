// <copyright file="IStorable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IStorable interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Storage
{
    /// <summary>
    /// Provides a mechanism to serialize and deserialize the object
    /// that implements the interface.
    /// </summary>
    public interface IStorable
    {
        /// <summary>
        /// Serializes this IStoreable object using the given ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// Provides access to the mechanisms required to serialize this IStoreable object.
        /// </param>
        void Serialize( ISerializationContext context );

        /// <summary>
        /// Deserializes this IStoreable object using the given IDeserializationContext.
        /// </summary>
        /// <param name="context">
        /// Provides access to the mechanisms required to deserialize this IStoreable object.
        /// </param>
        void Deserialize( IDeserializationContext context );
    }
}
