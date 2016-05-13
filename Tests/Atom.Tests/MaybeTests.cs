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
    using Microsoft.Pex.Framework;

    /// <summary>
    /// Tests the usage of the <see cref="Maybe{T}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( Maybe<> ) )]
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

        [PexMethod]
        public void GetValue_ConstructionWithValue_ReturnsValue<T>( T value )
        {
            // Arrange
            var maybe = new Maybe<T>( value );

            // Assert
            Assert.AreEqual( value, maybe.Value );
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

        [PexMethod]
        public void AreEqual_ConstructionWithSameValue_ReturnsTrue<T>( T value )
        {
            // Arrange
            var maybeA = new Maybe<T>( value );
            var maybeB = new Maybe<T>( value );

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


        [PexMethod]
        public void AreEqual_ConstructionWithDifferentValues_ReturnsFalse<T>( T valueA, T valueB )
        {
            // Assume
            PexAssume.AreNotEqual( valueA, valueB );

            // Arrange
            var maybeA = new Maybe<T>( valueA );
            var maybeB = new Maybe<T>( valueB );

            // Assert
            Assert.IsFalse( maybeA.Equals( maybeB ) );
        }

        [PexMethod]
        public void GetHashCode_ConstructionWithValue_ReturnsSameHashCodeAsGetHashCodeOnValue<T>( [PexAssumeNotNull]T value )
        {
            // Arrange
            var maybe = new Maybe<T>( value );

            // Assert
            Assert.AreEqual( value.GetHashCode(), maybe.GetHashCode() );
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