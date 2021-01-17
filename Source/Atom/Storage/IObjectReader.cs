// <copyright file="IObjectReader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IObjectReader interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Storage
{
    using System;

    /// <summary>
    /// Provides a mechanism that allows the deserialization of a specific object type.
    /// </summary>
    public interface IObjectReader
    {
        /// <summary>
        /// Gets the <see cref="Type"/> this IObjectReader deserializes.
        /// </summary>
        Type Type
        {
            get;
        }

        /// <summary>
        /// Deserializes the given object using the given IDeserializationContext.
        /// </summary>
        /// <param name="object">
        /// The object to deserialize.
        /// </param>
        /// <param name="context">
        /// The context that provides everything required for the deserialization process.
        /// </param>
        void Deserialize( object @object, IDeserializationContext context );
    }
}
