// <copyright file="TextWriterLogTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.Tests.TextWriterLogTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics.Tests
{
    using System;
    using System.IO;
    using System.IO.Moles;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="TextWriterLog"/> class.
    /// </summary>
    [TestClass]
    public sealed class TextWriterLogTests
    {
        [TestMethod]
        public void Construction_WithNullTextWriter_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => new TextWriterLog( null ) );
        }

        [TestMethod]
        public void WriteLine_WritesToTextWriter()
        {  
            // Arrange
            string writtenString = null;
            const string Message = "Boo";

            TextWriter writer = new STextWriter()
            {
                DisposeBoolean = disposeManaged => { },
                WriteLineString = str => {
                    Assert.IsNull( writtenString );
                    writtenString = str;
                }
            };

            var log = new TextWriterLog( writer );

            // Act
            log.WriteLine( Message );

            // Assert
            Assert.AreEqual( Message, writtenString );
        }

        [TestMethod]
        public void Write_WritesToTextWriter()
        {
            // Arrange
            string writtenString = null;
            const string Message = "Boo";

            TextWriter writer = new STextWriter() {
                DisposeBoolean = disposeManaged => { },
                WriteString = str => {
                    Assert.IsNull( writtenString );
                    writtenString = str;
                }
            };

            var log = new TextWriterLog( writer );

            // Act
            log.Write( Message );

            // Assert
            Assert.AreEqual( Message, writtenString );
        }

        [TestMethod]
        public void WriteString_WithNoArgs_CallsTextWriter_WriteLineWithEmptyString()
        {
            // Arrange
            string writtenString = null;

            TextWriter writer = new STextWriter() {
                DisposeBoolean = disposeManaged => {},
                WriteLineString = str => {
                    Assert.IsNull( writtenString );
                    writtenString = str;
                }
            };
            
            var log = new TextWriterLog( writer );

            // Act
            log.WriteLine();

            // Assert
            Assert.AreEqual( string.Empty, writtenString );
        }

        [TestMethod]
        public void Dispose_DisposesUnderlyingTextWriter()
        {
            // Arrange
            bool wasDisposed = false;

            TextWriter writer = new STextWriter() {
                DisposeBoolean = disposeManaged => {
                    Assert.IsFalse( wasDisposed );
                    wasDisposed = true;
                }
            };

            var log = new TextWriterLog( writer );

            // Act
            log.Dispose();

            // Assert
            Assert.IsTrue( wasDisposed );
        }

        [TestMethod]
        public void Dispose_AfterDispose_DoesNotThrow()
        {        
            // Arrange
            bool wasDisposed = false;

            TextWriter writer = new STextWriter() {
                DisposeBoolean = disposeManaged => {
                    if( wasDisposed )
                        throw new ObjectDisposedException( "TextWriter" );

                    wasDisposed = true;
                }
            };

            var log = new TextWriterLog( writer );

            // Act & Assert
            CustomAssert.DoesNotThrow( () => {
                log.Dispose();
                log.Dispose();
            } );
        }
    }
}
