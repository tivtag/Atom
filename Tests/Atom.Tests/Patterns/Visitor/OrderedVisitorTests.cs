// <copyright file="OrderedVisitorTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.Tests.OrderedVisitorTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Patterns.Visitor.Tests
{
    using System;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="OrderedVisitor{T}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( OrderedVisitor<> ) )]
    public sealed partial class OrderedVisitorTests
    {
        [TestMethod]
        public void Construction_WithNullWrappedVisitor_ThrowsArgumentNull()
        {
            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    var visitor = new OrderedVisitor<int>( null );
                }
            );
        }

        [TestMethod]
        public void Visitor_IsSameAsSpecifiedOnConstruction()
        {
            var wrappedVisitor = new EmptyVisitor<int>();
            var visitor = new OrderedVisitor<int>( wrappedVisitor );

            // Assert
            Assert.AreEqual( wrappedVisitor, visitor.Visitor );
        }

        //[TestMethod]
        //public void HasCompleted_ReturnsSameAsWrappedVisitor_True()
        //{
        //    // Arrange
        //    var wrappedVisitorStub = new Stubs.SIVisitor<int>();
        //    wrappedVisitorStub.HasCompletedGet = () => { return true; };

        //    IVisitor<int> wrappedVisitor = wrappedVisitorStub;
        //    var visitor = new OrderedVisitor<int>( wrappedVisitor );
            
        //    // Assert
        //    Assert.AreEqual( wrappedVisitor.HasCompleted, visitor.HasCompleted );
        //}
        
        //[TestMethod]
        //public void HasCompleted_ReturnsSameAsWrappedVisitor_False()
        //{
        //    // Arrange
        //    var wrappedVisitorStub = new Stubs.SIVisitor<int>();
        //    wrappedVisitorStub.HasCompletedGet = () => { return false; };

        //    IVisitor<int> wrappedVisitor = wrappedVisitorStub;
        //    var visitor = new OrderedVisitor<int>( wrappedVisitor );

        //    // Assert
        //    Assert.AreEqual( wrappedVisitor.HasCompleted, visitor.HasCompleted );
        //}

        [PexMethod]
        public void VisitPreOrder_WithCountingVisitor_IncreasesCountByOne<T>( T item )
        {
            // Arrange
            var countingVisitor = new CountingVisitor<T>();
            var visitor = new OrderedVisitor<T>( countingVisitor );

            // Act
            visitor.VisitPreOrder( item );

            // Assert
            Assert.AreEqual( 1, countingVisitor.Count );
        }

        [PexMethod]
        public void VisitPostOrder_WithCountingVisitor_IncreasesCountByOne<T>( T item )
        {
            // Arrange
            var countingVisitor = new CountingVisitor<T>();
            var visitor = new OrderedVisitor<T>( countingVisitor );

            // Act
            visitor.VisitPostOrder( item );

            // Assert
            Assert.AreEqual( 1, countingVisitor.Count );
        }

        [PexMethod]
        public void VisitInOrder_WithCountingVisitor_IncreasesCountByOne<T>( T item )
        {
            // Arrange
            var countingVisitor = new CountingVisitor<T>();
            var visitor = new OrderedVisitor<T>( countingVisitor );

            // Act
            visitor.VisitInOrder( item );

            // Assert
            Assert.AreEqual( 1, countingVisitor.Count );
        }

        [PexMethod]
        public void VisitPreOrder_PassesItem_ToWrappedVisitor<T>( T item )
        {
            // Arrange
            var trackingVisitor = new TrackingVisitor<T>();
            var visitor = new OrderedVisitor<T>( trackingVisitor );

            // Act
            visitor.VisitPreOrder( item );

            // Assert
            Assert.IsTrue( trackingVisitor.TrackingList.Contains( item ) );
        }

        [PexMethod]
        public void VisitPostOrder_PassesItem_ToWrappedVisitor<T>( T item )
        {
            // Arrange
            var trackingVisitor = new TrackingVisitor<T>();
            var visitor = new OrderedVisitor<T>( trackingVisitor );

            // Act
            visitor.VisitPostOrder( item );

            // Assert
            Assert.IsTrue( trackingVisitor.TrackingList.Contains( item ) );
        }

        [PexMethod]
        public void VisitInOrder_PassesItem_ToWrappedVisitor<T>( T item )
        {
            // Arrange
            var trackingVisitor = new TrackingVisitor<T>();
            var visitor = new OrderedVisitor<T>( trackingVisitor );

            // Act
            visitor.VisitInOrder( item );

            // Assert
            Assert.IsTrue( trackingVisitor.TrackingList.Contains( item ) );
        }

        //protected void VisitItems<T>( T[] items, IOrderedVisitor<T> visitor )
        //{
        //    foreach( var item in items )
        //    {
        //        visitor.VisitPreOrder( item );
        //        visitor.VisitInOrder( item );
        //        visitor.VisitPostOrder( item );
        //    }
        //}
    }
}
