// <copyright file="AnonymousTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.AnonymousTests class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="Anonymous"/> class.
    /// </summary>
    [TestClass]
    public sealed class AnonymousTests
    {
        [TestMethod]
        public void CreateList_WithLambda_ReturnsEmptyListOfAnonymousObjects()
        {
            // Act
            var list = Anonymous.CreateList(
                () => new { Name = default( string ), Age = default( int ) }
            );

            // Assert
            Assert.AreEqual( 0, list.Count );
        }

        [TestMethod]
        public void CreateList_WithLambda_DoesNotCallLambda()
        {
            bool wasCalled = false;

            // Act
            var list = Anonymous.CreateList(
                () => {
                    wasCalled = true;
                    return new { Name = default( string ), Age = default( int ) };
                }
            );

            // Assert
            Assert.IsFalse( wasCalled );
        }
    }
}
