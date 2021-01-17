// <copyright file="IUnsignedSerializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IUnsignedSerializationContext interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Storage
{
    /// <summary>
    /// Provides a mechanism that allows serialization of build-in types, including unsigned.
    /// </summary>
    [System.CLSCompliant(false)]
    public interface IUnsignedSerializationContext : ISerializationContext
    {
        /// <summary>
        /// Writes the given 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        void WriteUnsigned( uint value );
    }
}
