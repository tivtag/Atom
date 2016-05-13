// <copyright file="IUnsignedDeserializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IUnsignedDeserializationContext interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage
{
    using System;

    /// <summary>
    /// Provides a mechanism that allows deserialization of build-in types
    /// written by a matching <see cref="IUnsignedDeserializationContext"/>.
    /// </summary>
    [CLSCompliant(false)]
    public interface IUnsignedDeserializationContext : IDeserializationContext
    {
        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <returns>
        /// The 32-bit unsigned integer that has been read.
        /// </returns>
        uint ReadUInt32();

        /// <summary>
        /// Reads a 64-bit unsigned integer.
        /// </summary>
        /// <returns>
        /// The 64-bit unsigned integer that has been read.
        /// </returns>
        ulong ReadUInt64();
    }
}
