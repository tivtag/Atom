// <copyright file="IDeserializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IDeserializationContext interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Storage
{
    using System;

    /// <summary>
    /// Provides a mechanism that allows deserialization of build-in types
    /// written by a matching <see cref="ISerializationContext"/>.
    /// </summary>
    public interface IDeserializationContext
    {
        /// <summary>
        /// Reads a Boolean value.
        /// </summary>
        /// <returns>
        /// The Boolean value that has been read.
        /// </returns>
        bool ReadBoolean();

        /// <summary>
        /// Reads a 8-bit unsigned integer.
        /// </summary>
        /// <returns>
        /// The 8-bit unsigned integer that has been read.
        /// </returns>
        byte ReadByte();

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <returns>
        /// The 16-bit signed integer that has been read.
        /// </returns>
        short ReadInt16();

        /// <summary>
        /// Reads a 32-bit signed integer.
        /// </summary>
        /// <returns>
        /// The 32-bit signed integer that has been read.
        /// </returns>
        int ReadInt32();

        /// <summary>
        /// Reads a 64-bit signed integer.
        /// </summary>
        /// <returns>
        /// The 64-bit signed integer that has been read.
        /// </returns>
        long ReadInt64();
        
        /// <summary>
        /// Reads a Unicode character.
        /// </summary>
        /// <returns>
        /// The Unicode character that has been read.
        /// </returns>
        char ReadChar();

        /// <summary>
        /// Reads a series of Unicode characters.
        /// </summary>
        /// <returns>
        /// The series of Unicode characters that has been read.
        /// </returns>
        string ReadString();

        /// <summary>
        /// Reads a single-precision floating-point number.
        /// </summary>
        /// <returns>
        /// The single-precision floating-point number that has been read.
        /// </returns>
        float ReadSingle();

        /// <summary>
        /// Reads a double-precision floating-point number.
        /// </summary>
        /// <returns>
        /// The double-precision floating-point number that has been read.
        /// </returns>
        double ReadDouble();
    }
}
