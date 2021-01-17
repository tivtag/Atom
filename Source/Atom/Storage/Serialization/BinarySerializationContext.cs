// <copyright file="BinarySerializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.BinarySerializationContext class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Storage
{
    using System;
    using Atom.Diagnostics.Contracts;
    using System.IO;

    /// <summary>
    /// Implements a mechanism that allows serialization of build-in types
    /// to a binary stream.
    /// </summary>
    [CLSCompliant(false)]
    public class BinarySerializationContext : IUnsignedBinarySerializationContext
    {
        /// <summary>
        /// Gets the <see cref="BinaryWriter"/> this BinarySerializationContext internally 
        /// uses to write data.
        /// </summary>
        public BinaryWriter Writer
        {
            get
            {
                return this.writer;
            }
        }

        /// <summary>
        /// Initializes a new instance of the BinarySerializationContext class.
        /// </summary>
        /// <param name="stream">
        /// The output <see cref="Stream"/> to write into.
        /// </param>
        public BinarySerializationContext( Stream stream )
            : this( new BinaryWriter( stream ) )
        {
        }

        /// <summary>
        /// Initializes a new instance of the BinarySerializationContext class.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="BinaryWriter"/> the new BinarySerializationContext internally 
        /// should use to write data.
        /// </param>
        public BinarySerializationContext( BinaryWriter writer )
        {
            Contract.Requires<ArgumentNullException>( writer != null );
           
            this.writer = writer;
        }

        /// <summary>
        /// Writes the given Boolean value.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        public void Write( bool value )
        {
            this.writer.Write( value );
        }

        /// <summary>
        /// Writes the given 8-bit unsigned integer.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        public void Write( byte value )
        {
            this.writer.Write( value );
        }

        /// <summary>
        /// Writes the given 32-bit signed integer.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        public void Write( int value )
        {
            this.writer.Write( value );
        }

        /// <summary>
        /// Writes the given 64-bit signed value.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        public void Write( long value )
        {
            this.writer.Write( value );
        }

        /// <summary>
        /// Writes the given single-precision floating-point number.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        public void Write( float value )
        {
            this.writer.Write( value );
        }

        /// <summary>
        /// Writes the given double-precision floating-point value.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        public void Write( double value )
        {
            this.writer.Write( value );
        }

        /// <summary>
        /// Writes the given Unicode character.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        public void Write( char value )
        {
            this.writer.Write( value );
        }

        /// <summary>
        /// Writes the given series of Unicode characters.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        public void Write( string value )
        {
            this.writer.Write( value );
        }

        /// <summary>
        /// Writes the given 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        public void WriteUnsigned( uint value )
        {
            this.writer.Write( value );
        }

        /// <summary>
        /// The <see cref="BinaryWriter"/> this BinarySerializationContext internally 
        /// uses to write data.
        /// </summary>
        private readonly BinaryWriter writer;
    }
}
