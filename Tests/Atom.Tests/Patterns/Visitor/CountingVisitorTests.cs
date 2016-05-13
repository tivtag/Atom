// <copyright file="CountingVisitorTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Patterns.Visitor.Tests.CountingVisitorTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Patterns.Visitor.Tests
{
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="CountingVisitor{T}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( CountingVisitor<> ) )]
    public sealed partial class CountingVisitorTests
    {
        [TestMethod]
        public void Count_NewlyCreated_EqualZero()
        {
            // Arrange
            var visitor = new CountingVisitor<int>();

            // Assert
            Assert.AreEqual( 0, visitor.Count );
        }

        [TestMethod]
        public void HasCompleted_NewlyCreated_ReturnsFalse()
        {
            // Arrange
            var visitor = new CountingVisitor<int>();

            // Assert
            Assert.IsFalse( visitor.HasCompleted );
        }
        
        [PexMethod]
        public void VisitingOnce_IncreasesCountByOne<T>( T item )
        {
            // Arrange
            var visitor = new CountingVisitor<T>();

            // Act
            visitor.Visit( item );

            // Assert
            Assert.AreEqual( 1, visitor.Count );            
        }

        [PexMethod]
        public void VisitingArrayOfItems_CountEqualToArrayLength<T>( [PexAssumeNotNull]T[] itemsToVisit )
        {
            // Arrange
            var visitor = new CountingVisitor<T>();

            // Act
            itemsToVisit.Visit( visitor );

            // Assert
            Assert.AreEqual( itemsToVisit.Length, visitor.Count );      
        }

        [PexMethod]
        public void Reset_ChangesCountToZero<T>( [PexAssumeNotNull]T[] itemsToVisit )
        {
            // Arrange
            var visitor = new CountingVisitor<T>();
            itemsToVisit.Visit( visitor );

            // Act
            visitor.Reset();

            // Assert
            Assert.AreEqual( 0, visitor.Count );
        }
    }
}
