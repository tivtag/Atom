﻿// <copyright file="PreOrderVisitorTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.Tests.PreOrderVisitorTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Patterns.Visitor.Tests
{
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="OrderedVisitor{T}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( PreOrderVisitor<> ) )]
    public sealed partial class PreOrderVisitorTests
    {
        [PexMethod]
        public void VisitPreOrder_WithCountingVisitor_IncreasesCountByOne<T>( T item )
        {
            // Arrange
            var countingVisitor = new CountingVisitor<T>();
            var visitor = new PreOrderVisitor<T>( countingVisitor );

            // Act
            visitor.VisitPreOrder( item );

            // Assert
            Assert.AreEqual( 1, countingVisitor.Count );
        }

        [PexMethod]
        public void VisitPostOrder_WithCountingVisitor_DoesntIncreasesCountByOne<T>( T item )
        {
            // Arrange
            var countingVisitor = new CountingVisitor<T>();
            var visitor = new PreOrderVisitor<T>( countingVisitor );

            // Act
            visitor.VisitPostOrder( item );

            // Assert
            Assert.AreEqual( 0, countingVisitor.Count );
        }

        [PexMethod]
        public void VisitInOrder_WithCountingVisitor_DoesntIncreasesCountByOne<T>( T item )
        {
            // Arrange
            var countingVisitor = new CountingVisitor<T>();
            var visitor = new PreOrderVisitor<T>( countingVisitor );

            // Act
            visitor.VisitInOrder( item );

            // Assert
            Assert.AreEqual( 0, countingVisitor.Count );
        }

        [PexMethod]
        public void VisitPreOrder_PassesItem_ToWrappedVisitor<T>( T item )
        {
            // Arrange
            var trackingVisitor = new TrackingVisitor<T>();
            var visitor = new PreOrderVisitor<T>( trackingVisitor );

            // Act
            visitor.VisitPreOrder( item );

            // Assert
            Assert.IsTrue( trackingVisitor.TrackingList.Contains( item ) );
        }

        [PexMethod]
        public void VisitPostOrder_DoesntPassesItem_ToWrappedVisitor<T>( T item )
        {
            // Arrange
            var trackingVisitor = new TrackingVisitor<T>();
            var visitor = new PreOrderVisitor<T>( trackingVisitor );

            // Act
            visitor.VisitPostOrder( item );

            // Assert
            Assert.IsFalse( trackingVisitor.TrackingList.Contains( item ) );
        }

        [PexMethod]
        public void VisitInOrder_DoesntPassesItem_ToWrappedVisitor<T>( T item )
        {
            // Arrange
            var trackingVisitor = new TrackingVisitor<T>();
            var visitor = new PreOrderVisitor<T>( trackingVisitor );

            // Act
            visitor.VisitInOrder( item );

            // Assert
            Assert.IsFalse( trackingVisitor.TrackingList.Contains( item ) );
        }
    }
}
