// <copyright file="ISerializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.ISerializationContext interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Storage
{
    /// <summary>
    /// Provides a mechanism that allows serialization of build-in types.
    /// </summary>
    public interface ISerializationContext
    {      
        /// <summary>
        /// Writes the given Boolean value.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        void Write( bool value );

        /// <summary>
        /// Writes the given 8-bit unsigned integer.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        void Write( byte value );

        /// <summary>
        /// Writes the given 32-bit signed integer.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        void Write( int value );

        /// <summary>
        /// Writes the given 64-bit signed value.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        void Write( long value );

        /// <summary>
        /// Writes the given single-precision floating-point number.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        void Write( float value );

        /// <summary>
        /// Writes the given double-precision floating-point value.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        void Write( double value );
               
        /// <summary>
        /// Writes the given Unicode character.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        void Write( char value );

        /// <summary>
        /// Writes the given series of Unicode characters.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        void Write( string value );
    }
}
