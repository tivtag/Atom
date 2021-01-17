// <copyright file="LogSeverityTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.Tests.LogSeverityTests class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Diagnostics.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="LogSeverity"/> enumeration.
    /// </summary>
    [TestClass]
    public sealed class LogSeverityTests
    {
        [TestMethod]
        public void LogSeveritiesAll_Contains_AllBut_LogSeverityNone()
        {
            var s = LogSeverities.All;
            Assert.IsTrue( s.Contains( LogSeverities.Info ) );
            Assert.IsTrue( s.Contains( LogSeverities.Debug ) );
            Assert.IsTrue( s.Contains( LogSeverities.Warning ) );
            Assert.IsTrue( s.Contains( LogSeverities.Fatal ) );
            Assert.IsTrue( s.Contains( LogSeverities.All ) );

            Assert.IsTrue( s.Contains( LogSeverities.Info | LogSeverities.Debug ) );
            Assert.IsTrue( s.Contains( LogSeverities.Debug | LogSeverities.Warning ) );
            Assert.IsTrue( s.Contains( LogSeverities.Warning | LogSeverities.Fatal ) );

            Assert.IsFalse( s.Contains( LogSeverities.None ) );
        }

        [TestMethod]
        public void LogSeveritiesNone_Contains_NoSeverity()
        {
            var s = LogSeverities.None;
            Assert.IsFalse( s.Contains( LogSeverities.Info ) );
            Assert.IsFalse( s.Contains( LogSeverities.Debug ) );
            Assert.IsFalse( s.Contains( LogSeverities.Warning ) );
            Assert.IsFalse( s.Contains( LogSeverities.Fatal ) );
            Assert.IsFalse( s.Contains( LogSeverities.All ) );

            Assert.IsFalse( s.Contains( LogSeverities.Info | LogSeverities.Debug ) );
            Assert.IsFalse( s.Contains( LogSeverities.Debug | LogSeverities.Warning ) );
            Assert.IsFalse( s.Contains( LogSeverities.Warning | LogSeverities.Fatal ) );

            Assert.IsFalse( s.Contains( LogSeverities.None ) );
        }

        [TestMethod]
        public void LogSeveritiesInfo_OnlyContains_InfoSeverity()
        {
            var s = LogSeverities.Info;
            Assert.IsTrue( s.Contains( LogSeverities.Info ) );
            Assert.IsTrue( s.Contains( LogSeverities.All ) );

            Assert.IsFalse( s.Contains( LogSeverities.Debug ) );
            Assert.IsFalse( s.Contains( LogSeverities.Warning ) );
            Assert.IsFalse( s.Contains( LogSeverities.Fatal ) );
            Assert.IsFalse( s.Contains( LogSeverities.None ) );
        }

        [TestMethod]
        public void TestContains2()
        {
            var s = LogSeverities.Info | LogSeverities.Debug;
            Assert.IsTrue( s.Contains( LogSeverities.Info ) );
            Assert.IsTrue( s.Contains( LogSeverities.Debug ) );
            Assert.IsTrue( s.Contains( LogSeverities.All ) );

            Assert.IsFalse( s.Contains( LogSeverities.Warning ) );
            Assert.IsFalse( s.Contains( LogSeverities.Fatal ) );
            Assert.IsFalse( s.Contains( LogSeverities.None ) );
        }
    }
}
