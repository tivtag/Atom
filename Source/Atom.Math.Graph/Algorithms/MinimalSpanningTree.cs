// <copyright file="MinimalSpanningTree.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Algorithms.MinimalSpanningTree class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Graph.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Atom.Collections;
    using Atom.Collections.Comparers;
    using Atom.Math.Graph.Data;

    /// <summary>
    /// Provides mechanisms to calculate the minimal spanning tree of a given
    /// input <see cref="Graph{TVertexData, TEdgeData}"/>.
    /// </summary>
    public static class MinimalSpanningTree
    {
        #region - Prim -

        /// <summary>
        /// Finds the minimal spanning tree of the supplied graph using Prim's algorithm.
        /// </summary>
        /// <param name="inputGraph">The weighted input graph.</param>
        /// <param name="fromVertex">The vertex to start from.</param>
        /// <returns>A graph representing the minimal spanning tree of the graph supplied.</returns>
        /// <typeparam name="TVertexData">
        /// The type of data stored by the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="inputGraph"/> and/or <paramref name="fromVertex"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified <paramref name="fromVertex"/> could not be found in the specified <paramref name="inputGraph"/>.
        /// </exception>
        public static Graph<TVertexData, TEdgeData> Prim<TVertexData, TEdgeData>( 
            Graph<TVertexData, TEdgeData> inputGraph, 
            Vertex<TVertexData, TEdgeData> fromVertex )
                where TVertexData : IEquatable<TVertexData>
                where TEdgeData : IWeightData
        {
            #region Verify Arguments

            if( inputGraph == null )
                throw new ArgumentNullException( "inputGraph" );

            if( fromVertex == null )
                throw new ArgumentNullException( "fromVertex" );

            if( !inputGraph.ContainsVertex( fromVertex ) )
                throw new ArgumentException( GraphErrorStrings.VertexCouldNotBeFound, "fromVertex" );

            #endregion

            var heap = new Heap<Association<float, Vertex<TVertexData, TEdgeData>>>(
                HeapType.Minimum, new AssociationKeyComparer<float, Vertex<TVertexData, TEdgeData>>() );

            var vertexStatus = new Dictionary<Vertex<TVertexData, TEdgeData>, VertexInfo<TVertexData, TEdgeData>>();

            // Initialise the vertex distances to 
            foreach( var vertex in inputGraph.Vertices )
            {
                vertexStatus.Add( vertex, new VertexInfo<TVertexData, TEdgeData>( float.MaxValue, null, false ) );
            }

            vertexStatus[fromVertex].Distance = 0.0f;

            // Add the source vertex to the heap - we'll be branching out from it.
            heap.Add( new Association<float, Vertex<TVertexData, TEdgeData>>( 0.0f, fromVertex ) );

            while( heap.Count > 0 )
            {
                var item  = heap.Pop();
                var edges = item.Value.IncidentEdges;

                vertexStatus[item.Value].IsFinalised = true;

                // Enumerate through all the edges emanating from this node.
                foreach( var edge in edges )
                {
                    var partnerVertex = edge.GetPartnerVertex( item.Value );
                    var newVertexInfo = vertexStatus[partnerVertex];

                    if( !newVertexInfo.IsFinalised )
                    {
                        float edgeWeight = edge.Data != null ? edge.Data.Weight : 0.0f;

                        if( edgeWeight < newVertexInfo.Distance )
                        {
                            newVertexInfo.EdgeFollowed = edge;
                            newVertexInfo.Distance = edgeWeight;

                            heap.Add( new Association<float, Vertex<TVertexData, TEdgeData>>( edgeWeight, partnerVertex ) );
                        }
                    }
                }
            }

            return BuildPrimGraph( inputGraph, vertexStatus );
        }

        /// <summary>
        /// Helper method that builds a new graph.
        /// </summary>
        /// <param name="inputGraph">The weighted input graph.</param>
        /// <param name="vertexStatus">The vertex status.</param>
        /// <returns>A new graph from the edges followed.</returns>
        /// <typeparam name="TVertexData">
        /// The type of data stored by the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        private static Graph<TVertexData, TEdgeData> BuildPrimGraph<TVertexData, TEdgeData>(
            Graph<TVertexData, TEdgeData> inputGraph,
            Dictionary<Vertex<TVertexData, TEdgeData>, VertexInfo<TVertexData, TEdgeData>> vertexStatus )
                where TVertexData : IEquatable<TVertexData>
                where TEdgeData : IWeightData
        {
            // Now build the new graph:
            var newGraph = new Graph<TVertexData, TEdgeData>( inputGraph.DataFactory, inputGraph.IsDirected );

            // This dictionary is used for mapping between 
            // the old vertices and the new vertices put into the graph.
            var vertexMap = new Dictionary<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>( vertexStatus.Count );

            foreach( var current in vertexStatus )
            {
                var vertex    = current.Key;
                var newVertex = vertex.Clone();

                vertexMap.Add( vertex, newVertex );
                newGraph.AddVertex( newVertex );
            }

            foreach( var current in vertexStatus )
            {
                var vertex = current.Key;
                var info   = current.Value;

                // Check if an edge has been included to this vertex
                if( info.EdgeFollowed != null )
                {
                    var edge = newGraph.AddEdge(
                        vertexMap[info.EdgeFollowed.GetPartnerVertex( vertex )],
                        vertexMap[vertex]
                    );

                    edge.Data.Weight = info.Distance;
                }
            }

            return newGraph;
        }

        #endregion

        #region - Kruskal -

        /// <summary>
        /// Finds the minimal spanning tree of the given Graph using
        /// Kruskal's algorithm.
        /// </summary>
        /// <remarks>
        /// Taken (and then modified) from the great library 'NGenerics' http://www.codeplex.com/NGenerics/.
        /// Thanks for the great work!
        /// </remarks>
        /// <param name="inputGraph">The weighted input graph.</param>
        /// <returns>
        /// A graph representing the minimal spanning tree of the graph supplied.
        /// </returns>
        /// <typeparam name="TVertexData">
        /// The type of data stored by the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        public static Graph<TVertexData, TEdgeData> Kruskal<TVertexData, TEdgeData>( Graph<TVertexData, TEdgeData> inputGraph )
            where TVertexData : IEquatable<TVertexData>
            where TEdgeData : IWeightData
        {
            Contract.Requires<ArgumentNullException>( inputGraph != null );

            // There is going be Vertices - 1 edges when we finish
            int edgeCount = inputGraph.VertexCount - 1;

            var vertexToParent = new Dictionary<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>();
            var oldToNew       = new Dictionary<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>();
            var edgeQueue      = new Heap<Association<float, Edge<TVertexData, TEdgeData>>>( HeapType.Minimum, new AssociationKeyComparer<float, Edge<TVertexData, TEdgeData>>() );

            // Now build the return graph, always return non directed.
            var returnGraph = new Graph<TVertexData, TEdgeData>( false );

            // As we mew the new vertices for the new graph from the old
            // one, we also map the old ones to the new ones, and set up
            // our dictionary used to track forests of vertices.
            foreach( Vertex<TVertexData, TEdgeData> vertex in inputGraph.Vertices )
            {
                var newVertex = vertex.Clone();

                oldToNew.Add( vertex, newVertex );
                returnGraph.AddVertex( newVertex );

                vertexToParent.Add( vertex, null );
            }

            // We need to move the edges into a priority queue
            // and use the weight in the association to sort them.
            foreach( var edge in inputGraph.Edges )
            {
                edgeQueue.Add( new Association<float, Edge<TVertexData, TEdgeData>>( edge.Data.Weight, edge ) );
            }

            // We know when we are done, when we hit the number of edges
            // or when there is no more edges.
            while( (edgeQueue.Count > 0) && (edgeCount > 0) )
            {
                // Pull off the least weight edge that hasn't been added or discarded yet.
                Association<float, Edge<TVertexData, TEdgeData>> t = edgeQueue.Pop();

                // Save the value of the association to the proper type, an edge
                Edge<TVertexData, TEdgeData> edge = t.Value;

                // Here is the start of search to make find the heads
                // of the forest, because if they are the same heads, 
                // there is a cycle.
                Vertex<TVertexData, TEdgeData> fromVertexHead = edge.From;
                Vertex<TVertexData, TEdgeData> toVertexHead   = edge.To;

                // Find the head vertex of the forest the fromVertex is in
                while( vertexToParent[fromVertexHead] != null )
                {
                    fromVertexHead = vertexToParent[fromVertexHead];
                }

                // Find the head vertex of the forest the toVertex is in
                while( vertexToParent[toVertexHead] != null )
                {
                    toVertexHead = vertexToParent[toVertexHead];
                }

                // Check to see if the heads are the same
                // if are equal, it is a cycle, and we cannot 
                // include the edge in the new graph.
                if( fromVertexHead != toVertexHead )
                {
                    // Join the FromVertex forest to the ToVertex
                    vertexToParent[fromVertexHead] = edge.To;

                    // We have one less edge we need to find
                    --edgeCount;

                    // Add the edge to the new new graph, map the old vertexs to the new ones
                    var newEdge = returnGraph.AddEdge( oldToNew[edge.From], oldToNew[edge.To] );
                    newEdge.Data.Weight =  edge.Data.Weight;
                }
            }

            // All done :)
            return returnGraph;
        }

        #endregion
    }
}
