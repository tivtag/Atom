// <copyright file="IObjectWriter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IObjectWriter interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage
{
    using System;

    /// <summary>
    /// Provides a mechanism that allows the deserialization of a specific object type.
    /// </summary>
    public interface IObjectWriter
    {
        /// <summary>
        /// Gets the <see cref="Type"/> this IObjectWriter serializes.
        /// </summary>
        Type Type
        {
            get;
        }

        /// <summary>
        /// Serializes the given object using the given ISerializationContext.
        /// </summary>
        /// <param name="object">
        /// The object to serialize.
        /// </param>
        /// <param name="context">
        /// The context that provides everything required for the serialization process.
        /// </param>
        void Serialize( object @object, ISerializationContext context );
    }
}
