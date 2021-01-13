// <copyright file="MaybeTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.MaybeTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="Maybe{T}"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class MaybeTests
    {
        [TestMethod]
        public void HasValue_ConstructionWithValue_ReturnsTrue()
        {
            const int Value = 100;

            // Arrange
            var maybe = new Maybe<int>( Value );

            // Assert
            Assert.IsTrue( maybe.HasValue );
        }
        
        [TestMethod]
        public void HasValue_ConstructionWithNoValue_ReturnsFalse()
        {
            // Arrange
            var maybe = new Maybe<int>();

            // Assert
            Assert.IsFalse( maybe.HasValue );
        }

        [TestMethod]
        public void GetHashCode_ConstructionWithNoValue_ReturnsZero()
        {
            // Arrange
            var maybe = new Maybe<int>();

            // Assert
            Assert.AreEqual( 0, maybe.GetHashCode() );
        }

        [TestMethod]
        public void GetHashCode_ConstructionWithNullValue_ReturnsOne()
        {
            // Arrange
            var maybe = new Maybe<string>( null );

            // Assert
            Assert.AreEqual( 1, maybe.GetHashCode() );
        }
        
        [TestMethod]
        public void Equals_ConstructionWithNullValues_ReturnsTrue()
        {
            // Arrange
            var maybeA = new Maybe<string>( null );
            var maybeB = new Maybe<string>( null );

            // Assert
            Assert.IsTrue( maybeA.Equals( maybeB ) );
        }

        [TestMethod]
        public void Equals_ToNull_ReturnsFalse()
        {
            // Arrange
            var maybe = new Maybe<string>( null );

            // Assert
            Assert.IsFalse( maybe.Equals( null ) );
        }

        [TestMethod]
        public void AreEqual_ConstructionWithNoValues_ReturnsTrue()
        {
            // Arrange
            var maybeA = new Maybe<string>();
            var maybeB = new Maybe<string>();

            // Assert
            Assert.IsTrue( maybeA.Equals( maybeB ) );
        }

        [TestMethod]
        public void AreEqual_WithNullObject_ReturnsFalse()
        {
            // Arrange
            var maybe = new Maybe<int>( 10 );

            // Assert
            Assert.IsFalse( maybe.Equals( (object)null ) );
        }

        [TestMethod]
        public void GetValue_ConstructionWithNoValue_Throws()
        {
            // Arrange
            var maybe = new Maybe<string>();

            // Act & Assert
            CustomAssert.Throws<InvalidOperationException>( () => {
                string value = maybe.Value;
            } );
        }
    }
}