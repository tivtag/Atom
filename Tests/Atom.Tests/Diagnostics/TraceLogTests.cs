// <copyright file="TraceLogTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.Tests.TraceLogTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics.Tests
{
    using System.Diagnostics.Moles;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="TraceLog"/> class.
    /// </summary>
    [TestClass]
    public sealed class TraceLogTests
    {
        [TestMethod]
        public void DefaultSeverity_ShouldBe_Info()
        {
            var log = new TraceLog();
            Assert.AreEqual( LogSeverities.Info, log.DefaultSeverity );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void Write_WritesLineToTrace()
        {
            // Expected
            const string Message = "Hhey";

            // Actual
            string writtenMessage = null;

            // Mole
            MTrace.BehaveAsNotImplemented();
            MTrace.WriteString = str => {
                writtenMessage = str;
            };

            // Arrange
            var log = new TraceLog();

            // Act
            log.Write( Message );

            // Assert
            Assert.AreEqual( Message, writtenMessage );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void WriteLine_WritesLineToTrace()
        {
            // Expected
            const string Message = "Hhey";

            // Actual
            string writtenMessage = null;

            // Mole
            MTrace.BehaveAsNotImplemented();
            MTrace.WriteLineString = str => {
                writtenMessage = str;
            };

            // Arrange
            var log = new TraceLog();

            // Act
            log.WriteLine( Message );

            // Assert
            Assert.AreEqual( Message, writtenMessage );
        }
    }
}
