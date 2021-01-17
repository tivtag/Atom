// <copyright file="IBinaryDeserializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IBinaryDeserializationContext interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Storage
{
    using System.IO;

    /// <summary>
    /// Provides a mechanism that allows deserialization of build-in types
    /// from a binary stream.
    /// </summary>
    public interface IBinaryDeserializationContext : IDeserializationContext
    {
        /// <summary>
        /// Gets the <see cref="BinaryReader"/> this IBinarySerializationContext internally 
        /// uses to read data.
        /// </summary>
        BinaryReader Reader
        {
            get;
        }
    }
}
