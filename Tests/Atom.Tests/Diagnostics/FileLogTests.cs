// <copyright file="FileLogTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.Tests.FileLogTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics.Tests
{
    using System;
    using System.IO.Moles;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="FileLog"/> class.
    /// </summary>
    [TestClass]
    public sealed class FileLogTests
    {
        [TestMethod]
        [HostType( "Moles" )]
        public void ConstructionWithOnlyFileName_ChoosesDefaulteEncoding()
        {
            const string ExpectedFileName = "Lock.log";

            // Mole
            MFile.OpenWriteString = fileName => {
                Assert.AreEqual( ExpectedFileName, fileName );
                return StubFactory.CreateFileStream( fileName );
            };

            // Arrange
            var log = new FileLog( ExpectedFileName );

            // Assert
            Assert.AreEqual( Encoding.Default, log.Writer.Encoding );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void Construction_ClosesFileStream_WhenExceptionIsThrown()
        {
            bool wasClosed = false;

            // Mole
            MStreamWriter.ConstructorStreamEncoding = (writer, stream, encoding) => {
                throw new InvalidOperationException();
            };

            MFile.OpenStringFileModeFileAccessFileShare = (fileName, fileMode, fileAccess, fileShare) => {
                var fileStream = StubFactory.CreateFileStream( fileName );
                fileStream.Close01 = () => {
                    wasClosed = true;
                };

                return fileStream;
            };

            // Act
            CustomAssert.Throws<InvalidOperationException>( () => {
                new FileLog( "Heh" );
            } );

            // Assert
            Assert.IsTrue( wasClosed );
        }
    }
}
