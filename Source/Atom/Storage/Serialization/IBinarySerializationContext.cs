// <copyright file="IBinarySerializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IBinarySerializationContext interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage
{
    using System.IO;

    /// <summary>
    /// Provides a mechanism that allows serialization of build-in types
    /// to a binary stream.
    /// </summary>
    public interface IBinarySerializationContext : ISerializationContext
    {
        /// <summary>
        /// Gets the <see cref="BinaryWriter"/> this IBinarySerializationContext internally 
        /// uses to write data.
        /// </summary>
        BinaryWriter Writer
        {
            get;
        }
    }
}
