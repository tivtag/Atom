// <copyright file="VisitorExtensionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.Tests.VisitorExtensionsTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Patterns.Visitor.Tests
{
    using Atom.Collections;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;

    /// <summary>
    /// Tests the usage of the <see cref="VisitorExtensions"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class VisitorExtensionsTests
    {
        [PexMethod]
        public void Visit_VisitsAllPassedElements<T>( [PexAssumeNotNull]T[] items )
        {
            // Arrange
            var visitor = new TrackingVisitor<T>();
            
            // Act
            items.Visit( visitor );

            // Assert
            Assert.IsTrue( items.ElementsEqual( visitor.TrackingList ) );
        }
        
        [PexMethod]
        public void VisitEnumerable_VisitsAllPassedElements( [PexAssumeNotNull]System.Collections.Generic.IEnumerable<object> items )
        {
            // Arrange
            var visitor = new TrackingVisitor<object>();

            // Act
            ((IEnumerable)items).Visit( visitor );

            // Assert
            Assert.IsTrue( items.ElementsEqual( visitor.TrackingList ) );
        }

        //[PexMethod]
        //public void Visit_StopsVisiting_WhenVisitorHasCompleted<T>( [PexAssumeNotNull]T[] items, int maximumItemsToVisit )
        //{
        //    // Assume
        //    PexAssume.IsTrue( maximumItemsToVisit >= 0 );

        //    // Arrange
        //    int itemVisited = 0;

        //    var visitorStub = new Stubs.SIVisitor<T>();
        //    visitorStub.HasCompletedGet = () => { return itemVisited >= maximumItemsToVisit; };
        //    visitorStub.VisitT = obj => { ++itemVisited; };

        //    // Act
        //    items.Visit( visitorStub );

        //    // Assert
        //    Assert.IsTrue( itemVisited <= maximumItemsToVisit );
        //}

        //[PexMethod]
        //public void VisitEnumerable_StopsVisiting_WhenVisitorHasCompleted( [PexAssumeNotNull]object[] items, int maximumItemsToVisit )
        //{
        //    // Assume
        //    PexAssume.IsTrue( maximumItemsToVisit >= 0 );

        //    // Arrange
        //    int itemVisited = 0;

        //    var visitorStub = new Stubs.SIVisitor<object>();
        //    visitorStub.HasCompletedGet = () => { return itemVisited >= maximumItemsToVisit; };
        //    visitorStub.VisitT = obj => { ++itemVisited; };

        //    // Act
        //    ((IEnumerable)items).Visit( visitorStub );

        //    // Assert
        //    Assert.IsTrue( itemVisited <= maximumItemsToVisit );
        //}
    }
}
