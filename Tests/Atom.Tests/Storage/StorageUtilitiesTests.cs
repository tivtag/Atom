// <copyright file="StorageUtilitiesTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.Tests.StorageUtilitiesTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage.Tests
{
    using System;
    using System.IO;
    using System.IO.Moles;
    using Atom.Storage.Moles;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="StorageUtilities"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class StorageUtilitiesTests
    {
        [TestMethod]
        [HostType( "Moles" )]
        public void SaveToFileLoadFromFile_ReturnsInputObject()
        {
            // Expected
            const string FileName = "Hi!";
            const int ExpectedObject = 12345;
            FileStream stream = null;
            
            // Mole
            MFile.OpenStringFileModeFileAccess =
                (fileName, fileMode, fileAccess) => {
                    Assert.AreEqual( FileName, fileName );
                    return stream = StubFactory.CreateFileStream( fileName, fileMode, fileAccess, new MemoryStream() );
                };

            // Arrange
            var writer = new SIObjectWriter01<int>() {
                SerializeTObjectISerializationContext = (obj, context) => {
                    Assert.AreEqual( ExpectedObject, obj );
                    Assert.IsNotNull( context );

                    context.Write( true );
                    context.Write( ExpectedObject );
                }
            };

            // Act
            StorageUtilities.SaveToFile( FileName, ExpectedObject, writer );
            
            // Mole
            MFile.OpenReadString = fileName => {
                Assert.AreEqual( FileName, fileName );
                
                stream.Position = 0;
                return stream;
            };

            // Arrange
            int deserializedObject = 0;
            var reader = new SIObjectReader01<int>() {
                DeserializeTObjectIDeserializationContext = (obj, context) => {
                    Assert.AreEqual( 0, obj );
                    Assert.IsNotNull( context );

                    bool dummy = context.ReadBoolean();
                    Assert.IsTrue( dummy );

                    deserializedObject = context.ReadInt32();                  
                }
            };

            // Act
            StorageUtilities.LoadFromFile( FileName, reader );

            // Assert
            Assert.AreEqual( ExpectedObject, deserializedObject );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void SafeSaveToFile_DoesntOpenFile_WhenSerializeThrowsException()
        {
            // Mole
            MFile.OpenStringFileModeFileAccess =
                (fileName, fileMode, fileAccess) => {
                    Assert.Fail( "Should not have opened the file." );
                    return null;
                };

            // Arrange
            var writer = new SIObjectWriter01<int>() {
                 SerializeTObjectISerializationContext = (obj, context) =>{
                    throw new InvalidOperationException();
                 }
            };

            // Act & Assert
            CustomAssert.Throws<InvalidOperationException>( () => {
                StorageUtilities.SafeSaveToFile( "Hello", 12345, writer );
            } );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void SafeSaveToFile_InitiallyWritesIntoMemory_AndThenToFile_WhenNoExceptionIsThrown()
        {
            bool wasWrittenToFile = false;

            // Arrange
            var writer = new SIObjectWriter01<int>() {
                SerializeTObjectISerializationContext = (obj, context) => {
                    var binaryContext = context as IBinarySerializationContext;
                    Assert.IsNotNull( binaryContext );
                    Assert.IsInstanceOfType( binaryContext.Writer.BaseStream, typeof( MemoryStream ) );

                    context.Write( obj );
                }
            };

            // Mole
            MFile.OpenStringFileModeFileAccess =
                (fileName, fileMode, fileAccess) => {
                    wasWrittenToFile = true;
                    return StubFactory.CreateFileStream( fileName, fileMode, fileAccess, new MemoryStream() );
                };

            // Act
            StorageUtilities.SafeSaveToFile( "test.t", 1245, writer );

            // Assert
            Assert.IsTrue( wasWrittenToFile );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void SaveToFile_OpensFile_WhenSerializeThrowsException()
        {
            bool wasFileOpened = false;

            // Mole
            MFile.OpenStringFileModeFileAccess =
                (fileName, fileMode, fileAccess) => {
                    wasFileOpened = true;
                    return new SFileStream( fileName, fileMode, fileAccess )  {
                        Close01 = () => { },
                        CanWriteGet = () => true,
                        DisposeBoolean = disposeManaged => { }
                    };
                };

            // Arrange
            var writer = new SIObjectWriter01<int>() {
                SerializeTObjectISerializationContext = (obj, context) => {
                    throw new InvalidOperationException();
                }
            };

            // Act & Assert
            CustomAssert.Throws<InvalidOperationException>( () =>  {
                StorageUtilities.SaveToFile( "Hello", 12345, writer );
            } );

            Assert.IsTrue( wasFileOpened );
        }

        [TestMethod]
        public void LoadFromFile_WhenPassedNullPath_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => {
                StorageUtilities.LoadFromFile( null, new SIObjectReader01<Guid>() );
            } );
        }

        [TestMethod]
        public void LoadFromFile_WhenPassedNullReader_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => {
                StorageUtilities.LoadFromFile<Guid>( "Hello", null );
            } );
        }

        [TestMethod]
        public void SaveToFile_WhenPassedNullPath_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => {
                StorageUtilities.SaveToFile( null, new Guid(), new SIObjectWriter01<Guid>() );
            } );
        }

        [TestMethod]
        public void SaveToFile_WhenPassedNullObject_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => {
                StorageUtilities.SaveToFile( "Hello.txt", null, new SIObjectWriter01<string>() );
            } );
        }

        [TestMethod]
        public void SaveToFile_WhenPassedNullWriter_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => {
                StorageUtilities.SaveToFile( "Hello.txt", new Guid(), null );
            } );
        }

        [TestMethod]
        public void SafeSaveToFile_WhenPassedNullPath_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => {
                StorageUtilities.SafeSaveToFile( null, new Guid(), new SIObjectWriter01<Guid>() );
            } );
        }

        [TestMethod]
        public void SafeSaveToFile_WhenPassedNullObject_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => {
                StorageUtilities.SafeSaveToFile( "Hello.txt", null, new SIObjectWriter01<string>() );
            } );
        }

        [TestMethod]
        public void SafeSaveToFile_WhenPassedNullWriter_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => {
                StorageUtilities.SafeSaveToFile( "Hello.txt", new Guid(), null );
            } );
        }
    }
}