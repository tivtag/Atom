// <copyright file="LogProviderTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.Tests.LogProviderTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics.Tests
{
    using System;
    using Atom.Diagnostics.Moles;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="LogProvider"/> class.
    /// </summary>
    [TestClass]
    public sealed class LogProviderTests
    {
        [TestMethod]
        public void Construction_WithNullLog_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => new LogProvider( null ) );
        }

        [TestMethod]
        public void Log_WithValidLogConstructed_ReturnsSameLog()
        {
            // Arrange
            var log = new SILog();
            var logProvider = new LogProvider( log );

            // Act
            var returnedLog = logProvider.Log;

            // Assert
            Assert.AreEqual( log, returnedLog );
        }
    }
}
