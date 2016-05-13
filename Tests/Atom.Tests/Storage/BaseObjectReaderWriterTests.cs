// <copyright file="BaseObjectReaderWriterTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.Tests.BaseObjectReaderWriterTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage.Tests
{
    using Atom.Storage.Moles;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="BaseObjectReaderWriter{T}"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class BaseObjectReaderWriterTests
    {
        [TestMethod]
        public void TypeGet_ReturnsGenericTypeArgument()
        {
            // Arrange
            var readerWriter = new SBaseObjectReaderWriter<int>();

            // Assert
            Assert.AreEqual( typeof( int ), readerWriter.Type );
        }

        [TestMethod]
        public void Serialize_CorrectlyCastsObjectToType()
        {
            int objectToSerialize = 100;
            int? serializedObject = 0;

            // Arrange
            IObjectReaderWriter readerWriter = new SBaseObjectReaderWriter<int>() {
                 SerializeTObjectISerializationContext = (obj, context) => {
                     serializedObject = objectToSerialize;
                 }
            };

            // Act
            readerWriter.Serialize( objectToSerialize, new SISerializationContext() );

            // Assert
            Assert.IsTrue( serializedObject.HasValue );
            Assert.AreEqual( objectToSerialize, serializedObject.Value );
        }
        
        [TestMethod]
        public void Deserialize_CorrectlyCastsObjectToType()
        {
            int objectToDeserialize = 100;
            int? deserializedObject = 0;

            // Arrange
            IObjectReaderWriter readerWriter = new SBaseObjectReaderWriter<int>() {
                 DeserializeTObjectIDeserializationContext = (obj, context) => {
                     deserializedObject = objectToDeserialize;
                 }
            };

            // Act
            readerWriter.Deserialize( objectToDeserialize, new SIDeserializationContext() );

            // Assert
            Assert.IsTrue( deserializedObject.HasValue );
            Assert.AreEqual( objectToDeserialize, deserializedObject.Value );
        }

        [TestMethod]
        public void Serialize_WhenPassedObjectOfWrongType_Throws()
        {   
            // Arrange
            IObjectReaderWriter readerWriter = new SBaseObjectReaderWriter<int>();

            // Act & Assert
            CustomAssert.Throws<System.InvalidCastException>( () => {
                readerWriter.Serialize( "Obj of wrong type", null );
            } );            
        }

        [TestMethod]
        public void Deserialize_WhenPassedObjectOfWrongType_Throws()
        {
            // Arrange
            IObjectReaderWriter readerWriter = new SBaseObjectReaderWriter<int>();

            // Act & Assert
            CustomAssert.Throws<System.InvalidCastException>( () => {
                readerWriter.Deserialize( "Obj of wrong type", null );
            } );
        }
    }
}