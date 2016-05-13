// <copyright file="XmlUtilitiesTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.Tests.XmlUtilitiesTests class.
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
    /// Tests the usage of the <see cref="XmlUtilities"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class XmlUtilitiesTests
    {
        [TestMethod]
        [HostType( "Moles" )]
        public void SerializeDeserialize_ReturnsInputObject()
        {
            // Expected
            const string FileName = "File.hah";
            const int InputValue = 123456;
            FileStream stream = null;
                        
            // Mole
            MFile.OpenStringFileModeFileAccess =
                (fileName, fileMode, fileAccess) => {
                    Assert.AreEqual( FileName, fileName );
                    return stream = StubFactory.CreateFileStream( fileName, fileMode, fileAccess, new MemoryStream() );           
                };

            // Act
            XmlUtilities.Serialize( FileName, InputValue );

            // Mole
            MFile.OpenReadString = fileName => {
                Assert.AreEqual( FileName, fileName );

                stream.Position = 0;
                return stream;
            };

            // Act
            int readValue = XmlUtilities.Deserialize<int>( FileName );
            
            // Assert
            Assert.AreEqual( InputValue, readValue );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void SerializeTryDeserialize_ReturnsInputObject()
        {
            // Expected
            const string FileName = "File.hah";
            const int InputValue = 123456;
            FileStream stream = null;

            // Mole
            MFile.OpenStringFileModeFileAccess =
                ( fileName, fileMode, fileAccess ) =>
                {
                    Assert.AreEqual( FileName, fileName );
                    return stream = StubFactory.CreateFileStream( fileName, fileMode, fileAccess, new MemoryStream() );
                };

            // Act
            XmlUtilities.Serialize( FileName, InputValue );

            // Mole
            MFile.OpenReadString = fileName => {
                Assert.AreEqual( FileName, fileName );

                stream.Position = 0;
                return stream;
            };

            // Act
            int readValue = XmlUtilities.TryDeserialize<int>( 
                FileName, exception => {
                    Assert.Fail( "No exception should have occurred." );
                    return -1;
                });

            // Assert
            Assert.AreEqual( InputValue, readValue );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void TrySerialize_ReturnsObjectReturnedByFactoryMethod_WhenSerializeThrows()
        {
            // Expected
            Exception exception = new InvalidOperationException();
            const string FileName = "hax";
            const int ExpectedValue = 12345;

            // Mole
            MXmlUtilities.DeserializeString<int>(
                fileName => {
                    Assert.AreEqual( FileName, fileName );
                    throw exception; 
                }
            );

            // Act
            int value = XmlUtilities.TryDeserialize(
                FileName,
                exc => {
                    Assert.AreEqual( exception, exc );
                    return ExpectedValue;
                }
            );

            // Assert
            Assert.AreEqual( ExpectedValue, value );
        }
    }
}