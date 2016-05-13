// <copyright file="ConsoleLogTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.Tests.ConsoleLogTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics.Tests
{
    using System.Moles;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Atom.Diagnostics.Filters.Moles;

    /// <summary>
    /// Tests the usage of the <see cref="ConsoleLog"/> class.
    /// </summary>
    [TestClass]
    public sealed class ConsoleLogTests
    {
        [TestMethod]
        public void DefaultSeverity_ShouldBe_Info()
        {
            var log = new ConsoleLog();
            Assert.AreEqual( LogSeverities.Info, log.DefaultSeverity );
        }


        [TestMethod]
        [HostType( "Moles" )]
        public void WriteString_WithNoArgs_CallsConsoleWriteLineNoArgs()
        {
            // Arrange
            bool wasCalled = false;

            MConsole.BehaveAsNotImplemented();
            MConsole.SetOutTextWriter = writer => { };
            MConsole.SetErrorTextWriter = writer => { };
            MConsole.WriteLine = () => wasCalled = true;

            var log = new ConsoleLog();

            // Act
            log.WriteLine();

            // Assert
            Assert.IsTrue( wasCalled );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void WriteString_WithActiveMessageFilter_DoesntCallConsoleWriteLine()
        {
            // Arrange
            const string Text = "Meow!";
            string writtenText = null;

            MConsole.BehaveAsNotImplemented();
            MConsole.SetOutTextWriter = writer => { };
            MConsole.SetErrorTextWriter = writer => { };
            MConsole.WriteString = str => {
                writtenText = str;
            };

            var log = new ConsoleLog() {
                MessageFilter = new SILogFilter() { FiltersLogSeveritiesString = ( s, m ) => true }
            };

            // Act
            log.Write( Text );

            // Assert
            Assert.IsNull( writtenText );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void WriteString_CorrectlyCallsConsoleWriteLine()
        {
            // Arrange
            const string Text = "Meow!";
            string writtenText = null;

            MConsole.BehaveAsNotImplemented();
            MConsole.SetOutTextWriter = writer => { };
            MConsole.SetErrorTextWriter = writer => { };
            MConsole.WriteString = str => {
                Assert.IsNull( writtenText );
                writtenText = str;
            };

            var log = new ConsoleLog();

            // Act
            log.Write( Text );
            
            // Assert
            Assert.AreEqual( writtenText, Text );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void WriteLineString_WithActiveMessageFilter_DoesntCallConsoleWriteLine()
        {
            // Arrange
            const string Text = "Meow!";
            string writtenText = null;

            MConsole.BehaveAsNotImplemented();
            MConsole.SetOutTextWriter = writer => { };
            MConsole.SetErrorTextWriter = writer => { };
            MConsole.WriteLineString = str => {
                writtenText = str;
            };

            var log = new ConsoleLog() {
                MessageFilter = new SILogFilter() { FiltersLogSeveritiesString = ( s, m ) => true }
            };

            // Act
            log.WriteLine( Text );

            // Assert
            Assert.IsNull( writtenText );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void WriteLineString_CorrectlyCallsConsoleWriteLine()
        {
            // Arrange
            const string Text = "Meow!";
            string writtenText = null;

            MConsole.BehaveAsNotImplemented();
            MConsole.SetOutTextWriter = writer => { };
            MConsole.SetErrorTextWriter = writer => { };
            MConsole.WriteLineString = str => {
                Assert.IsNull( writtenText );
                writtenText = str;
            };

            var log = new ConsoleLog();

            // Act
            log.WriteLine( Text );

            // Assert
            Assert.AreEqual( writtenText, Text );
        }
    }
}
