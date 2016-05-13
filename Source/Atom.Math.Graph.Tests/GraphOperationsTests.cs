// <copyright file="GraphOperationsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Tests.GraphOperationsTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Graph.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="GraphOperations"/> class.
    /// </summary>
    [TestClass]
    public class GraphOperationsTests
    {
        [TestMethod]
        public void CutVertex_GivenNullVertex_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => {
                GraphOperations.Cut( null, new Graph<int, int>() );
            } );
        }

        [TestMethod]
        public void CutVertex_GivenNullGraph_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => {
                GraphOperations.Cut( new Vertex<int, int>(), null );
            } );
        }

        [TestMethod]
        public void CutVertex_GivenUnknownVertex_Throws()
        {
            CustomAssert.Throws<ArgumentException>( () => {
                GraphOperations.Cut( new Vertex<int, int>(), new Graph<int, int>() );
            } );
        }

        [TestMethod]
        public void CutVertex_WithUndirectedGraph_RemovesVertex()
        {
            // Arrange
            var graph = BuildGraph();

            // Act
            var middleVertex = graph.GetVertex( 2 );
            GraphOperations.Cut( middleVertex, graph );

            // Assert
            Assert.IsFalse( graph.ContainsVertex( middleVertex ) );
        }

        [TestMethod]
        public void CutVertex_WithUndirectedGraph_AddsEdgesBetweenPreviouslyConnectedVertices()
        {
            // Arrange
            var graph = BuildGraph();

            // Act
            var middleVertex = graph.GetVertex( 2 );
            GraphOperations.Cut( middleVertex, graph );

            // Assert
            Assert.IsTrue( graph.ContainsEdge( 1, 3 ) );
            Assert.IsTrue( graph.ContainsEdge( 1, 4 ) );
            Assert.IsTrue( graph.ContainsEdge( 3, 4 ) );
        }

        /// <summary>
        /// Builds a graph with 4 vertices and 3 edges.
        /// </summary>
        /// <returns></returns>
        private static Graph<int, int> BuildGraph()
        {
            var graph = new Graph<int, int>( false );
            graph.AddVertex( 1 );
            graph.AddVertex( 2 );
            graph.AddVertex( 3 );
            graph.AddVertex( 4 );

            graph.AddEdge( 1, 2, 10 );
            graph.AddEdge( 2, 3, 20 );
            graph.AddEdge( 2, 4, 30 );
            return graph;
        }
    }
}
