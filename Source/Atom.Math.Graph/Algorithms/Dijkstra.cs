// <copyright file="Dijkstra.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Math.Graph.Algorithms.Dijkstra{TVertexData, TEdgeData} class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Math.Graph.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Atom.Collections;
    using Atom.Collections.Comparers;
    using Atom.Math.Graph.Data;

    /// <summary>
    /// Provides an implementation of Dijkstra's algorithm 
    /// that finds the shortest paths to all other vertices from the specified source vertex.
    /// </summary>
    /// <typeparam name="TVertexData">
    /// The type of data stored within the vertices of the Graph.
    /// Must implement IDistanceData.
    /// </typeparam>
    /// <typeparam name="TEdgeData">
    /// The type of data stored within the edges of the Graph.
    /// </typeparam>
    public static class Dijkstra<TVertexData, TEdgeData>
        where TVertexData : IDistanceData, IEquatable<TVertexData>
        where TEdgeData : IWeightData
    {
        /// <summary>
        /// Finds the shortest paths to all other vertices from the specified source vertex using Dijkstra's Algorithm.
        /// </summary>
        /// <param name="graph">The weighted graph.</param>
        /// <param name="source">The source vertex.</param>
        /// <returns>A graph representing the shortest paths from the source node to all other nodes in the graph.</returns>  
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="graph"/> or <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified <paramref name="source"/> Vertex is not part of the specified <paramref name="graph"/>.
        /// </exception>
        public static Graph<TVertexData, TEdgeData> FindShortestPaths(
            Graph<TVertexData, TEdgeData> graph, 
            Vertex<TVertexData, TEdgeData> source )
        {
            Contract.Requires<ArgumentNullException>( graph != null );
            Contract.Requires<ArgumentNullException>( source != null );
            Contract.Requires<ArgumentException>( graph.ContainsVertex( source ) );

            var heap = new Heap<Tuple<float, Vertex<TVertexData, TEdgeData>>>(
                HeapType.Minimum,
                new TupleItem1Comparer<float, Vertex<TVertexData, TEdgeData>>()
            );

            var vertexStatus = new Dictionary<Vertex<TVertexData, TEdgeData>, VertexInfo<TVertexData, TEdgeData>>();

            // Initialise the vertex distances to the maximum possible.
            foreach( var vertex in graph.Vertices )
            {
                vertexStatus.Add( vertex, new VertexInfo<TVertexData, TEdgeData>( float.MaxValue, null, false ) );
            }

            vertexStatus[source].Distance = 0.0f;

            // Add the source vertex to the heap - we'll be branching out from it.
            heap.Add( new Tuple<float, Vertex<TVertexData, TEdgeData>>( 0, source ) );

            while( heap.Count > 0 )
            {
                var item = heap.Pop();
                var vertex = item.Item2;
                var vertexInfo = vertexStatus[vertex];

                if( !vertexInfo.IsFinalised )
                {
                    vertexInfo.IsFinalised = true;

                    // Enumerate through all the edges emanating from this node
                    foreach( var edge in vertex.EmanatingEdges )
                    {
                        Vertex<TVertexData, TEdgeData> partnerVertex = edge.GetPartnerVertex( vertex );

                        // Calculate the new distance to this distance
                        float distance = vertexInfo.Distance + edge.Data.Weight;

                        var newVertexInfo = vertexStatus[partnerVertex];

                        // Found a better path, update the vertex status and add the 
                        // vertex to the heap for further analysis
                        if( distance < newVertexInfo.Distance )
                        {
                            newVertexInfo.EdgeFollowed = edge;
                            newVertexInfo.Distance     = distance;

                            heap.Add( new Tuple<float, Vertex<TVertexData, TEdgeData>>( distance, partnerVertex ) );
                        }
                    }
                }
            }

            return BuildGraph( graph, source, vertexStatus );
        }

        /// <summary>
        /// Builds the graph for Dijkstra's algorithm with the edges followed.
        /// </summary>
        /// <param name="graph">The weighted graph.</param>
        /// <param name="source">The from vertex.</param>
        /// <param name="vertexStatus">The vertex status.</param>
        /// <returns>The Dijkstra graph.</returns>
        private static Graph<TVertexData, TEdgeData> BuildGraph(
            Graph<TVertexData, TEdgeData> graph,
            Vertex<TVertexData, TEdgeData> source,
            Dictionary<Vertex<TVertexData, TEdgeData>, VertexInfo<TVertexData, TEdgeData>> vertexStatus )
        {
            // Now build the new graph
            var newGraph = new Graph<TVertexData, TEdgeData>( graph.DataFactory, graph.IsDirected );
            var enumerator = vertexStatus.GetEnumerator();

            // This dictionary is used for mapping between the old vertices and the new vertices put into the graph
            var vertexMap = new Dictionary<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>( vertexStatus.Count );

            while( enumerator.MoveNext() )
            {
                var oldVertex = enumerator.Current.Key;
                var newVertex = oldVertex.Clone();

                vertexMap.Add( oldVertex, newVertex );
                newGraph.AddVertex( newVertex );
            }

            enumerator = vertexStatus.GetEnumerator();

            while( enumerator.MoveNext() )
            {
                var info = enumerator.Current.Value;

                // Check if an edge has been included to this vertex
                if( (info.EdgeFollowed != null) && (enumerator.Current.Key != source) )
                {
                    var edge = newGraph.AddEdge(
                        vertexMap[info.EdgeFollowed.GetPartnerVertex( enumerator.Current.Key )],
                        vertexMap[enumerator.Current.Key]
                    );

                    edge.Data.Weight = info.EdgeFollowed.Data.Weight;
                }
            }

            return newGraph;
        }
    }
}
