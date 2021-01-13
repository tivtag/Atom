// <copyright file="StringExtensionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.StringExtensionsTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="StringExtensions"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class StringExtensionsTests
    {
        [TestMethod]
        public void Contains_WithKnownString_AndOrdinalIgnoreCase_ReturnsTrue()
        {
            // Arrange
            const string TestString = "Hello, my lovely WoRlD!";

            // Act & Assert
            Assert.IsTrue( TestString.Contains( "world", StringComparison.OrdinalIgnoreCase ) );
            Assert.IsTrue( TestString.Contains( "WORLD", StringComparison.OrdinalIgnoreCase ) );
        }
    }
}
