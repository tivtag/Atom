// <copyright file="Traversal.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Algorithms.Traversal class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Graph.Algorithms
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using Atom.Patterns.Visitor;

    /// <summary>
    /// Provides mechanism for traversing over the vertices and edges of a Graph{TVertexData, TEdgeData}.
    /// </summary>
    public static class Traversal
    {
        #region DepthFirst

        /// <summary>
        /// Performs a depth-first traversal, starting at the specified vertex.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="visitor">
        /// The visitor to use. Note that in-order is not applicable in a graph.
        /// </param>
        /// <param name="startVertex">
        /// The vertex to start from.
        /// </param>
        public static void DepthFirst<TVertexData, TEdgeData>(
            OrderedVisitor<Vertex<TVertexData, TEdgeData>> visitor,
            Vertex<TVertexData, TEdgeData> startVertex )
            where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( visitor != null );
            Contract.Requires<ArgumentNullException>( startVertex != null );

            var visitedVertices = new List<Vertex<TVertexData, TEdgeData>>();
            DepthFirstCore( visitor, startVertex, visitedVertices );
        }

        /// <summary>
        /// Performs a depth-first traversal.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="startVertex">The start vertex.</param>
        /// <param name="visitedVertices">The visited vertices.</param>
        private static void DepthFirstCore<TVertexData, TEdgeData>(
            OrderedVisitor<Vertex<TVertexData, TEdgeData>> visitor,
            Vertex<TVertexData, TEdgeData> startVertex,
            List<Vertex<TVertexData, TEdgeData>> visitedVertices )
            where TVertexData : IEquatable<TVertexData>
        {
            if( visitor.HasCompleted )
                return;

            // Add the vertex to the "visited" list
            visitedVertices.Add( startVertex );

            // Visit the vertex in pre-order
            visitor.VisitPreOrder( startVertex );

            foreach( var edge in startVertex.EmanatingEdges )
            {
                // Get the partner vertex of the start vertex
                var vertexToVisit = edge.GetPartnerVertex( startVertex );

                // If the vertex hasn't been visited before, do a depth-first
                // traversal starting at that vertex
                if( !visitedVertices.Contains( vertexToVisit ) )
                {
                    DepthFirstCore( visitor, vertexToVisit, visitedVertices );
                }
            }

            // Visit the vertex in post order
            visitor.VisitPostOrder( startVertex );
        }

        #endregion

        #region BreadthFirst

        /// <summary>
        /// Performs a breadth-first traversal from the specified vertex.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="startVertex">The vertex to start from.</param>
        /// <param name="visitor">The visitor to use.</param>
        public static void BreadthFirst<TVertexData, TEdgeData>( 
            Vertex<TVertexData, TEdgeData> startVertex,
            IVisitor<Vertex<TVertexData, TEdgeData>> visitor )
                where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( visitor != null );
            Contract.Requires<ArgumentNullException>( startVertex != null );

            var queue = new Queue<Vertex<TVertexData, TEdgeData>>();
            var visitedVertices = new List<Vertex<TVertexData, TEdgeData>>();

            queue.Enqueue( startVertex );
            visitedVertices.Add( startVertex );

            while( !(queue.Count == 0 || visitor.HasCompleted) )
            {
                var vertex = queue.Dequeue();
                visitor.Visit( vertex );

                foreach( var edge in vertex.EmanatingEdges )
                {
                    var vertexToVisit = edge.GetPartnerVertex( vertex );

                    if( !visitedVertices.Contains( vertexToVisit ) )
                    {
                        queue.Enqueue( vertexToVisit );
                        visitedVertices.Add( vertexToVisit );
                    }
                }
            }
        }

        #endregion

        #region Topological

        /// <summary>
        /// Allows a visitor to visit each vertex in topological order.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="graph">
        /// The input graph to visit.
        /// </param>
        /// <param name="visitor">
        /// The visitor to use.
        /// </param>
        /// <returns>
        /// The number of items visited. If less than graph.VertexCount then
        /// the specified graph has circles.
        /// </returns>
        public static int Topological<TVertexData, TEdgeData>( 
            Graph<TVertexData, TEdgeData> graph,
            IVisitor<Vertex<TVertexData, TEdgeData>> visitor )
            where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( visitor != null );
            Contract.Requires<InvalidOperationException>( graph.IsDirected, GraphErrorStrings.OperationOnlyValidonDirectedGraph );

            if( graph.VertexCount == 0 )
                return 0;

            var queue = new Queue<Vertex<TVertexData, TEdgeData>>(); // Store the vertices to visit.
            var depth = new Dictionary<Vertex<TVertexData, TEdgeData>, int>( graph.VertexCount );
            
            foreach( var vertex in graph.Vertices )
            {
                int incomingCount = vertex.IncomingEdgeCount;
                depth.Add( vertex, incomingCount );

                // Enqueue those with depth 0
                if( incomingCount == 0 )
                    queue.Enqueue( vertex );
            }

            int visitCount = 0;

            // If no vertices are found with incoming edge count 0, the graph is cyclic,
            // and we don't visit any vertices
            if( queue.Count > 0 )
            {
                while( (queue.Count > 0) && (!visitor.HasCompleted) )
                {
                    Vertex<TVertexData, TEdgeData> vertex = queue.Dequeue();
                    depth.Remove( vertex );

                    // Visit the vertex in the topological sort order
                    visitor.Visit( vertex );

                    // Keep track of the amount of vertices we visit,
                    // so we can know if the graph has cycles in it or not.
                    ++visitCount;

                    // Enumerate through all the edges emanating from this node,
                    // decreasing the depth of the vertex (thereby "removing" it
                    // from the graph, and enqueue all those with depth 0.  The
                    // effect is an ordering by incoming edge counts.
                    foreach( var edge in vertex.EmanatingEdges )
                    {
                        Vertex<TVertexData, TEdgeData> partnerVertex = edge.To;

                        depth[partnerVertex]--;

                        if( depth[partnerVertex] == 0 )
                            queue.Enqueue( partnerVertex );
                    }
                }
            }

            return visitCount;
        }

        #endregion
    }
}
