// <copyright file="IUnsignedBinarySerializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IUnsignedBinarySerializationContext interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage
{
    /// <summary>
    /// Provides a mechanism that allows serialization of build-in types, including unsigned
    /// to a binary strea,.
    /// </summary>
    [System.CLSCompliant( false )]
    public interface IUnsignedBinarySerializationContext : IBinarySerializationContext, IUnsignedSerializationContext
    {
    }
}
