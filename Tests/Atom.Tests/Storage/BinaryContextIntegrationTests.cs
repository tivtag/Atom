// <copyright file="BinaryContextIntegrationTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.Tests.BinaryContextIntegrationTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage.Tests
{
    using System.IO;
    using Microsoft.Pex.Framework;
    using Microsoft.Pex.Framework.Domains;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests how the <see cref="BinarySerializationContext"/> and <see cref="BinaryDeserializationContext"/> classes
    /// work together.
    /// </summary>
    [TestClass]
    public sealed partial class BinaryContextIntegrationTests
    {
        [PexMethod]
        public void WrittenInt32_CanBeReadAgain( int value )
        {
            // Write
            var writer = CreateWriteContext();
            writer.Write( value );

            // Read
            var reader = CreateReadContext( writer );
            int readValue = reader.ReadInt32();

            // Assert
            Assert.AreEqual( value, readValue );
        }

        [PexMethod]
        public void WrittenInt16_CanBeReadAgain( short value )
        {
            // Write
            var writer = CreateWriteContext();
            writer.Write( value );

            // Read
            var reader = CreateReadContext( writer );
            short readValue = reader.ReadInt16();

            // Assert
            Assert.AreEqual( value, readValue );
        }

        [PexMethod]
        public void WrittenInt64_CanBeReadAgain( long value )
        {
            // Write
            var writer = CreateWriteContext();
            writer.Write( value );

            // Read
            var reader = CreateReadContext( writer );
            long readValue = reader.ReadInt64();

            // Assert
            Assert.AreEqual( value, readValue );
        }

        [PexMethod]
        [PexBooleanAsZeroOrOne]
        public void WrittenBoolean_CanBeReadAgain( bool value )
        {
            // Write
            var writer = CreateWriteContext();
            writer.Write( value );

            // Read
            var reader = CreateReadContext( writer );
            bool readValue = reader.ReadBoolean();

            // Assert
            Assert.AreEqual( value, readValue );
        }

        [PexMethod]
        public void WrittenChar_CanBeReadAgain( char value )
        {
            // Write
            var writer = CreateWriteContext();
            writer.Write( value );

            // Read
            var reader = CreateReadContext( writer );
            char readValue = reader.ReadChar();

            // Assert
            Assert.AreEqual( value, readValue );
        }

        [PexMethod]
        public void WrittenSingle_CanBeReadAgain( float value )
        {
            // Write
            var writer = CreateWriteContext();
            writer.Write( value );

            // Read
            var reader = CreateReadContext( writer );
            float readValue = reader.ReadSingle();

            // Assert
            Assert.AreEqual( value, readValue );
        }

        [PexMethod]
        public void WrittenDouble_CanBeReadAgain( double value )
        {
            // Write
            var writer = CreateWriteContext();
            writer.Write( value );

            // Read
            var reader = CreateReadContext( writer );
            double readValue = reader.ReadDouble();

            // Assert
            Assert.AreEqual( value, readValue );
        }

        [PexMethod]
        public void WrittenString_CanBeReadAgain( [PexAssumeNotNull]string value )
        {
            // Write
            var writer = CreateWriteContext();
            writer.Write( value );

            // Read
            var reader = CreateReadContext( writer );
            string readValue = reader.ReadString();

            // Assert
            Assert.AreEqual( value, readValue );
        }

        [PexMethod]
        public void WrittenString_CanBeReadAgain( [PexAssumeNotNull]byte value )
        {
            // Write
            var writer = CreateWriteContext();
            writer.Write( value );

            // Read
            var reader = CreateReadContext( writer );
            byte readValue = reader.ReadByte();

            // Assert
            Assert.AreEqual( value, readValue );
        }

        [PexMethod]
        public void WrittenUInt32_CanBeReadAgain( [PexAssumeNotNull]uint value )
        {
            // Write
            var writer = CreateWriteContext();
            writer.WriteUnsigned( value );

            // Read
            var reader = CreateReadContext( writer );
            uint readValue = reader.ReadUInt32();

            // Assert
            Assert.AreEqual( value, readValue );
        }

        [TestMethod]
        public void GetReader_OfNewBinarySerializationContext_ReturnsNonNull()
        {
            var context = new BinaryDeserializationContext( new MemoryStream() );
            Assert.IsNotNull( context.Reader );
        }

        private static BinarySerializationContext CreateWriteContext()
        {
            return new BinarySerializationContext(  new MemoryStream() );
        }

        private static BinaryDeserializationContext CreateReadContext( BinarySerializationContext writeContext )
        {
            var stream = writeContext.Writer.BaseStream;
            stream.Position = 0;

            return new BinaryDeserializationContext( stream );
        }
    }
}
