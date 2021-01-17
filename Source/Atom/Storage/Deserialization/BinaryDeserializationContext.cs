// <copyright file="BinaryDeserializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.BinaryDeserializationContext interface.
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
    /// Implements a mechanism that allows deserialization of build-in types
    /// from a binary stream.
    /// </summary>
    [CLSCompliant(false)]
    public class BinaryDeserializationContext : IUnsignedBinaryDeserializationContext
    {
        /// <summary>
        /// Gets the <see cref="BinaryReader"/> this BinarySerializationContext internally 
        /// uses to read data.
        /// </summary>
        public BinaryReader Reader
        {
            get
            {
                return this.reader;
            }
        }

        /// <summary>
        /// Initializes a new instance of the BinaryDeserializationContext class.
        /// </summary>
        /// <param name="stream">
        /// The stream of binary data from which will be read.
        /// </param>
        public BinaryDeserializationContext( Stream stream )
            : this( new BinaryReader( stream ) )
        {
        }

        /// <summary>
        /// Initializes a new instance of the BinaryDeserializationContext class.
        /// </summary>
        /// <param name="reader">
        /// The BinaryReader that is going to be read with.
        /// </param>
        public BinaryDeserializationContext( BinaryReader reader )
        {
            Contract.Requires<ArgumentNullException>( reader != null );

            this.reader = reader;
        }

        /// <summary>
        /// Reads a Boolean value.
        /// </summary>
        /// <returns>
        /// The Boolean value that has been read.
        /// </returns>
        public bool ReadBoolean()
        {
            return this.reader.ReadBoolean();
        }

        /// <summary>
        /// Reads a 8-bit unsigned integer.
        /// </summary>
        /// <returns>
        /// The 8-bit unsigned integer that has been read.
        /// </returns>
        public byte ReadByte()
        {
            return this.reader.ReadByte();
        }

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <returns>
        /// The 16-bit signed integer that has been read.
        /// </returns>
        public short ReadInt16()
        {
            return this.reader.ReadInt16();
        }

        /// <summary>
        /// Reads a 32-bit signed integer.
        /// </summary>
        /// <returns>
        /// The 32-bit signed integer that has been read.
        /// </returns>
        public int ReadInt32()
        {
            return this.reader.ReadInt32();
        }

        /// <summary>
        /// Reads a 64-bit signed integer.
        /// </summary>
        /// <returns>
        /// The 64-bit signed integer that has been read.
        /// </returns>
        public long ReadInt64()
        {
            return this.reader.ReadInt64();
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <returns>
        /// The 32-bit unsigned integer that has been read.
        /// </returns>
        public uint ReadUInt32()
        {
            return this.reader.ReadUInt32();
        }
        
        /// <summary>
        /// Reads a 64-bit unsigned integer.
        /// </summary>
        /// <returns>
        /// The 64-bit unsigned integer that has been read.
        /// </returns>
        public ulong ReadUInt64()
        {
            return this.reader.ReadUInt64();
        }

        /// <summary>
        /// Reads a Unicode character.
        /// </summary>
        /// <returns>
        /// The Unicode character that has been read.
        /// </returns>
        public char ReadChar()
        {
            return this.reader.ReadChar();
        }

        /// <summary>
        /// Reads a series of Unicode characters.
        /// </summary>
        /// <returns>
        /// The series of Unicode characters that has been read.
        /// </returns>
        public string ReadString()
        {
            return this.reader.ReadString();
        }

        /// <summary>
        /// Reads a single-precision floating-point number.
        /// </summary>
        /// <returns>
        /// The single-precision floating-point number that has been read.
        /// </returns>
        public float ReadSingle()
        {
            return this.reader.ReadSingle();
        }

        /// <summary>
        /// Reads a double-precision floating-point number.
        /// </summary>
        /// <returns>
        /// The double-precision floating-point number that has been read.
        /// </returns>
        public double ReadDouble()
        {
            return reader.ReadDouble();
        }

        /// <summary>
        /// The <see cref="BinaryReader"/> this BinarySerializationContext internally 
        /// uses to read data.
        /// </summary>
        private readonly BinaryReader reader;
    }
}
