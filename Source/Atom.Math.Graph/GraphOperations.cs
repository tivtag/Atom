// <copyright file="GraphOperations.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.GraphOperations class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Graph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SysContract = System.Diagnostics.Contracts.Contract;

    /// <summary>
    /// Defines operations that act on <see cref="Graph{TVertexData, TEdgeData}"/>s.
    /// </summary>
    public static class GraphOperations
    {
        /// <summary>
        /// Removes the specified Edge{TVertexData, TEdgeData} from the specified Graph{TVertexData, TEdgeData};
        /// contracting the graph along the removed edge in the process.
        /// </summary>
        /// <remarks>
        /// A "contraction" of a graph G along the edge e is the result of deleting the edge e from G and then
        /// identifying its endpoints. We imagine that the edge e is topologically shrunk ("contracted") to a point,
        /// and we denote the result by G/e. The word "contraction" refers to the operation aswell as to the resulting graph.
        /// </remarks>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="edge">
        /// The edge to remove.
        /// </param>
        /// <param name="graph">
        /// The graph to operate on.
        /// </param>
        /// <param name="discardExtraAdjacentEdges">
        /// States whether double edges are discarded.
        /// </param>
        public static void Contract<TVertexData, TEdgeData>( 
            Edge<TVertexData, TEdgeData> edge, 
            Graph<TVertexData, TEdgeData> graph, 
            bool discardExtraAdjacentEdges = true )
            where TVertexData : IEquatable<TVertexData>
        {
            SysContract.Requires<ArgumentNullException>( edge != null );
            SysContract.Requires<ArgumentNullException>( graph != null );
            SysContract.Requires<ArgumentException>( !graph.IsDirected );
            SysContract.Requires<ArgumentException>( graph.ContainsEdge( edge ) );

            var vertexA = edge.From;
            var vertexB = edge.To;

            // 1. Find the vertices that will be re-connected with vertexB.
            var verticesConnectedToA = new List<Vertex<TVertexData, TEdgeData>>();
            
            foreach( var e in vertexA.EmanatingEdges )
            {
                if( e == edge )
                    continue;

                if( e.To == vertexA )
                {
                    verticesConnectedToA.Add( e.From );
                }
                else
                {
                    verticesConnectedToA.Add( e.To );
                }
            }

            // 2. Purge the edge to contract.
            while( graph.RemoveEdge( vertexA, vertexB ) ) { }
            
            // 3. Purge the first vertex of the edge.
            graph.RemoveVertex( vertexA );

            // 4. Reconnect the graph.
            foreach( var vertex in verticesConnectedToA )
            {
                if( discardExtraAdjacentEdges )
                {
                    if( graph.ContainsEdge( vertexB, vertex ) )
                        continue;
                }

                graph.AddEdge( vertexB, vertex );
            }
        }

        /// <summary>
        /// Removes the specified Vertex{TVertexData, TEdgeData} and the Edge{TVertexData, TEdgeData}s that are connected
        /// to it. After that the vertices previously connected over the removed vertex are connected using new edges.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="vertex">
        /// The vertex to cut out.
        /// </param>
        /// <param name="graph">
        /// The graph to operate on.
        /// </param>
        public static void Cut<TVertexData, TEdgeData>( Vertex<TVertexData, TEdgeData> vertex, Graph<TVertexData, TEdgeData> graph )
            where TVertexData : IEquatable<TVertexData>
        {
            SysContract.Requires<ArgumentNullException>( vertex != null );
            SysContract.Requires<ArgumentNullException>( graph != null );
            SysContract.Requires<ArgumentException>( graph.ContainsVertex( vertex ) );

            // For each connected vertex add an edge to each other connected vertex,
            // unless there already exists an edge that connects those vertices.
            var connectedVertices = vertex.EmanatingEdges.Select( edge => edge.To != vertex ? edge.To : edge.From ).ToList();

            // Remove vertex and its edges.
            graph.RemoveVertex( vertex );

            // Add new edges that fill the cut.
            foreach( var vertexA in connectedVertices )
            {
                // The complexity of this function might possibly be improved
                // here.
                foreach( var vertexB in connectedVertices )
                {
                    if( vertexA == vertexB )
                        continue;

                    if( !graph.ContainsEdge( vertexA, vertexB ) )
                    {
                        graph.AddEdge( vertexA, vertexB );
                    }
                }
            }
        }

        /// <summary>
        /// Gets the edge complement Graph of the specified <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <returns>
        /// The edge-complement graph of the graph.
        /// </returns>
        public static Graph<TVertexData, TEdgeData> EdgeComplement<TVertexData, TEdgeData>( Graph<TVertexData, TEdgeData> graph )
            where TVertexData : IEquatable<TVertexData>
        {
            Graph<TVertexData, TEdgeData> outputGraph = graph.CloneWithVertices();
            
            foreach( var vertexA in graph.Vertices )
            {
                foreach( var vertexB in graph.Vertices )
                {
                    if( vertexA == vertexB )
                        continue;

                    if( !(vertexA.HasEmanatingEdgeTo( vertexB ) || vertexA.HasIncidentEdgeWith( vertexB )) )
                    {
                        if( !outputGraph.ContainsEdge( vertexA.Data, vertexB.Data ) )
                        {
                            outputGraph.AddEdge( vertexA.Data, vertexB.Data );
                        }
                    }
                }
            }

            return outputGraph;
        }

        /// <summary>
        /// Calculates the cartesian product of the given Graphs.
        /// </summary>
        /// <remarks>
        /// The cartesian product of the graphs G and G' is denoted G x G' and
        /// defined to be the graph with the vertex set Vg x Vg' and the edge set (Eg x Vg')u(Vg x Eg').
        /// </remarks>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="left">The <see cref="Graph{TVertexData, TEdgeData}"/> on the left side.</param>
        /// <param name="right">The <see cref="Graph{TVertexData, TEdgeData}"/> on the right side.</param>
        /// <param name="vertexFunction">
        /// The function that is responsible for mapping two vertices together.
        /// </param>
        /// <returns>
        /// The cartesian product of the given Graphs.
        /// </returns>
        public static Graph<TVertexData, TEdgeData> Cross<TVertexData, TEdgeData>(
            Graph<TVertexData, TEdgeData> left,
            Graph<TVertexData, TEdgeData> right,
            Func<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>, TVertexData> vertexFunction )
                where TVertexData : IEquatable<TVertexData>
        {
            SysContract.Requires<ArgumentNullException>( left != null );
            SysContract.Requires<ArgumentNullException>( right != null );
            SysContract.Requires<ArgumentNullException>( vertexFunction != null );
            SysContract.Ensures( SysContract.Result<Graph<TVertexData, TEdgeData>>() != null );

            // Vertex set: Vg x Vg'
            var vertexSet = new List<Tuple<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>>( left.VertexCount * right.VertexCount );

            foreach( var vertexA in left.Vertices )
            {
                foreach( var vertexB in right.Vertices )
                {
                    vertexSet.Add( new Tuple<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>( vertexA, vertexB ) );
                }
            }

            // Edge set: (Eg x Vg')u(Vg x Eg')
            var edgeSet = new List<Tuple<Tuple<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>, Tuple<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>>>( left.VertexCount * right.VertexCount );

            foreach( var vertexU in vertexSet )
            {
                foreach( var vertexV in vertexSet )
                {
                    if( vertexU == vertexV )
                        continue;

                    if( vertexU.Item1.Equals( vertexV.Item1 ) )
                    {
                        if( vertexU.Item2.HasEmanatingEdgeTo( vertexV.Item2 ) ||
                            vertexU.Item2.HasIncidentEdgeWith( vertexV.Item2 ) )
                        {
                            var edgeMapping = new Tuple<Tuple<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>,
                                                        Tuple<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>
                                                       >( vertexU, vertexV );
                            edgeSet.Add( edgeMapping );
                        }
                    }
                    else if( vertexU.Item2.Equals( vertexV.Item2 ) )
                    {
                        if( vertexU.Item1.HasEmanatingEdgeTo( vertexV.Item1 ) ||
                            vertexU.Item1.HasIncidentEdgeWith( vertexV.Item1 ) )
                        {
                            var edgeMapping = new Tuple<Tuple<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>,
                                                        Tuple<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>
                                                       >( vertexU, vertexV );
                            edgeSet.Add( edgeMapping );
                        }
                    }
                }
            }

            // Build graph:
            var graph = new Graph<TVertexData, TEdgeData>( left.IsDirected );
            var vertexMap = new Dictionary<Tuple<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>>, TVertexData>( vertexSet.Count );

            foreach( var vertex in vertexSet )
            {
                var vertexData = vertexFunction( vertex.Item1, vertex.Item2 );

                vertexMap.Add( vertex, vertexData );
                graph.AddVertex( vertexData );
            }

            foreach( var edge in edgeSet )
            {
                var vertexA = vertexMap[edge.Item1];
                var vertexB = vertexMap[edge.Item2];

                if( !graph.ContainsEdge( vertexA, vertexB ) )
                    graph.AddEdge( vertexA, vertexB );
            }

            return graph;
        }

        /// <summary>
        /// Joins (also sometimes called suspense) the given Graphs into a single Graph
        /// by connecting all vertices of the <paramref name="first"/> Graph with all vertices of the <paramref name="second"/> Graph.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="first">
        /// The first <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </param>
        /// <param name="second">
        /// The second <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </param>
        /// <returns>
        /// The suspension of the given graphs.
        /// </returns>
        public static Graph<TVertexData, TEdgeData> Join<TVertexData, TEdgeData>(
            Graph<TVertexData, TEdgeData> first,
            Graph<TVertexData, TEdgeData> second ) 
                where TVertexData : IEquatable<TVertexData>
        {
            SysContract.Requires<ArgumentNullException>( first != null );
            SysContract.Requires<ArgumentNullException>( second != null );
            SysContract.Requires( first.IsDirected == second.IsDirected, GraphErrorStrings.GraphDirectionMismatch );
            SysContract.Ensures( SysContract.Result<Graph<TVertexData, TEdgeData>>() != null );

            var graph = new Graph<TVertexData, TEdgeData>( first.IsDirected );
            var verticesA = new List<Vertex<TVertexData, TEdgeData>>( first.VertexCount );
            var verticesB = new List<Vertex<TVertexData, TEdgeData>>( second.VertexCount );

            // Insert vertices of first graph:
            foreach( var vertex in first.Vertices )
            {
                var clone = vertex.Clone();

                verticesA.Add( clone );
                graph.AddVertex( clone );
            }

            // Insert edges of first graph:
            foreach( var edge in first.Edges )
            {
                graph.AddEdge(
                    verticesA[first.IndexOfVertex( edge.From )],
                    verticesA[first.IndexOfVertex( edge.To )]
                );
            }

            // Insert vertices of second graph:
            foreach( var vertex in second.Vertices )
            {
                var clone = vertex.Clone();

                verticesB.Add( clone );
                graph.AddVertex( clone );
            }

            // Insert edges of second graph:
            foreach( var edge in second.Edges )
            {
                graph.AddEdge(
                    verticesB[second.IndexOfVertex( edge.From )],
                    verticesB[second.IndexOfVertex( edge.To )]
                );
            }

            // Connect the first graph with second graph:
            foreach( var vertexA in verticesA )
            {
                foreach( var vertexB in verticesB )
                {
                    graph.AddEdge( vertexA, vertexB );
                }
            }

            return graph;
        }
    }
}