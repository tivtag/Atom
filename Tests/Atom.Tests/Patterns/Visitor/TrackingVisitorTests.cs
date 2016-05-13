// <copyright file="TrackingVisitorTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.Tests.TrackingVisitorTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Patterns.Visitor.Tests
{
    using Atom.Collections;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="TrackingVisitor{T}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( TrackingVisitor<> ) )]
    public sealed partial class TrackingVisitorTests
    {
        [TestMethod]
        public void TrackingList_NewlyCreatedVisitor_IsEmpty()
        {
            // Arrange
            var visitor = new TrackingVisitor<int>();

            // Assert
            Assert.AreEqual( 0, visitor.TrackingList.Count );
        }

        [TestMethod]
        public void HasCompleted_AlwaysReturnsFalse()
        {
            // Arrange
            var visitor = new TrackingVisitor<int>();

            // Assert
            Assert.IsFalse( visitor.HasCompleted );
        }

        [PexMethod]
        public void TrackingList_ContainsAllVisitedItems<T>( [PexAssumeNotNull]T[] itemsToVisit, int initialCapacity )
        {
            // Assume
            PexAssume.IsTrue( initialCapacity >= 0 );

            // Arrange
            var visitor = new TrackingVisitor<T>( initialCapacity );

            // Act
            foreach( var item in itemsToVisit )
            {
                visitor.Visit( item );
            }

            // Assert
            Assert.IsTrue( visitor.TrackingList.ElementsEqual( itemsToVisit ) );
        }
    }
}
