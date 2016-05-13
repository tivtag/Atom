// <copyright file="IUnsignedBinaryDeserializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IUnsignedBinaryDeserializationContext interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage
{
    using System;

    /// <summary>
    /// Provides a mechanism that allows deserialization of build-in types (including unsigned types)
    /// from a binary stream.
    /// </summary>
    [CLSCompliant(false)]
    public interface IUnsignedBinaryDeserializationContext : IBinaryDeserializationContext, IUnsignedDeserializationContext
    {
    }
}
