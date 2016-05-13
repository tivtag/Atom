// <copyright file="Cycles.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Algorithms.Cycles class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Graph.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Provides algorithms that discover information
    /// about the cycles in a Graph.
    /// </summary>
    public static class Cycles
    {
        #region [ BruteForce Depth Search ]

        #region - Find -

        /// <summary>
        /// Finds the cycles in this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="graph">
        /// The graph to investigate.
        /// </param>
        /// <returns>
        /// A new list that contains all cycles in this Graph.
        /// </returns>
        public static IList<IList<Vertex<TVertexData, TEdgeData>>> Find<TVertexData, TEdgeData>( 
            Graph<TVertexData, TEdgeData> graph )
                where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( graph != null );
            Contract.Ensures( Contract.Result<IList<IList<Vertex<TVertexData, TEdgeData>>>>() != null );

            var cycles = new List<IList<Vertex<TVertexData, TEdgeData>>>();
            var currentWalk = new Stack<Vertex<TVertexData, TEdgeData>>();

            foreach( var vertex in graph.Vertices )
            {
                currentWalk.Clear();
                currentWalk.Push( vertex );

                FindWalks( vertex, vertex, null, currentWalk, cycles );
            }

            return cycles;
        }

        /// <summary>
        /// Finds all cyclic walks starting and ending at the given <see cref="Vertex{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="vertex">
        /// The start vertex.
        /// </param>
        /// <returns>
        /// The list of cyclic walks.
        /// </returns>
        public static IList<IList<Vertex<TVertexData, TEdgeData>>> Find<TVertexData, TEdgeData>(
            Vertex<TVertexData, TEdgeData> vertex )
                where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( vertex != null );
            Contract.Ensures( Contract.Result<IList<IList<Vertex<TVertexData, TEdgeData>>>>() != null );

            var cycles      = new List<IList<Vertex<TVertexData, TEdgeData>>>();
            var currentWalk = new Stack<Vertex<TVertexData, TEdgeData>>();

            currentWalk.Push( vertex );
            FindWalks( vertex, vertex, null, currentWalk, cycles );

            return cycles;
        }

        /// <summary>
        /// Helper method that finds the cycles in a graph recursively.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="startVertex">
        /// The start vertex.
        /// </param>
        /// <param name="currentVertex">
        /// The current vertex.
        /// </param>
        /// <param name="previousVertex">
        /// The previously visited vertex.
        /// </param>
        /// <param name="currentWalk">
        /// The current walk in the graph.
        /// </param>
        /// <param name="cycles">
        /// The list of all cycles.
        /// </param>
        private static void FindWalks<TVertexData, TEdgeData>(
            Vertex<TVertexData, TEdgeData> startVertex,
            Vertex<TVertexData, TEdgeData> currentVertex,
            Vertex<TVertexData, TEdgeData> previousVertex,
            Stack<Vertex<TVertexData, TEdgeData>> currentWalk,
            IList<IList<Vertex<TVertexData, TEdgeData>>> cycles )
                where TVertexData : IEquatable<TVertexData>
        {
            int neightbourCount = currentVertex.OutgoingEdgeCount;
            if( neightbourCount == 0 )
                return;

            var neighbours = new Vertex<TVertexData, TEdgeData>[neightbourCount];

            for( int i = 0; i < neightbourCount; ++i )
            {
                var edge      = currentVertex.GetEmanatingEdge( i );
                neighbours[i] = edge.GetPartnerVertex( currentVertex );
            }

            foreach( var neighbour in neighbours )
            {
                if( neighbour == startVertex && previousVertex != startVertex )
                {
                    currentWalk.Push( startVertex );
                    cycles.Add( currentWalk.Reverse().ToList() );
                    currentWalk.Pop();

                    continue;
                }

                if( currentWalk.Contains( neighbour ) )
                    continue;

                currentWalk.Push( neighbour );

                FindWalks( startVertex, neighbour, currentVertex, currentWalk, cycles );

                currentWalk.Pop();
            }
        }

        #endregion

        #region - FindStatus -

        /// <summary>
        /// Finds information about the cycles in this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <remarks>
        /// This function is a-lot faster and memory efficient than 'Find'.
        /// </remarks>
        /// <param name="graph">
        /// The graph to investigate.
        /// </param>
        /// <param name="minimum">Will contain the length of the minimum cycle.</param>
        /// <param name="maximum">Will contain the length of the maximum cycle.</param>
        /// <param name="cycleCount">WIll contain the number of cycles in the graph.</param>
        public static void FindStatus<TVertexData, TEdgeData>(
            Graph<TVertexData, TEdgeData> graph,
            out int minimum, 
            out int maximum,
            out int cycleCount )
                where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( graph != null );

            minimum    = int.MaxValue;
            maximum    = int.MinValue;
            cycleCount = 0;

            var currentWalk = new Stack<Vertex<TVertexData, TEdgeData>>();

            foreach( var vertex in graph.Vertices )
            {
                currentWalk.Clear();
                currentWalk.Push( vertex );

                FindStatus( vertex, vertex, null, currentWalk, ref minimum, ref maximum, ref cycleCount );
            }

            if( minimum == int.MaxValue )
                minimum = -1;
            if( maximum == int.MinValue )
                maximum = -1;
        }

        /// <summary>
        /// Helper method that finds information about the cycles in a graph recursively.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="startVertex">
        /// The start vertex.
        /// </param>
        /// <param name="currentVertex">
        /// The current vertex.
        /// </param>
        /// <param name="previousVertex">
        /// The previously visited vertex.
        /// </param>
        /// <param name="currentWalk">
        /// The current walk in the graph.
        /// </param>
        /// <param name="minimum">Will contain the length of the minimum cycle.</param>
        /// <param name="maximum">Will contain the length of the maximum cycle.</param>
        /// <param name="cycleCount">WIll contain the number of cycles in the graph.</param>
        private static void FindStatus<TVertexData, TEdgeData>(
            Vertex<TVertexData, TEdgeData> startVertex,
            Vertex<TVertexData, TEdgeData> currentVertex,
            Vertex<TVertexData, TEdgeData> previousVertex,
            Stack<Vertex<TVertexData, TEdgeData>> currentWalk,
            ref int minimum,
            ref int maximum,
            ref int cycleCount )
                where TVertexData : IEquatable<TVertexData>
        {
            int neightbourCount = currentVertex.OutgoingEdgeCount;
            if( neightbourCount == 0 )
                return;

            var neighbours = new Vertex<TVertexData, TEdgeData>[neightbourCount];
            for( int i = 0; i < neightbourCount; ++i )
            {
                var edge      = currentVertex.GetEmanatingEdge( i );
                neighbours[i] = edge.GetPartnerVertex( currentVertex );
            }

            foreach( var neighbour in neighbours )
            {
                if( neighbour == startVertex && previousVertex != startVertex )
                {
                    currentWalk.Push( startVertex );

                    ++cycleCount;
                    int cycleLength = currentWalk.Count - 1;

                    if( cycleLength < minimum )
                        minimum = cycleLength;
                    if( cycleLength > maximum )
                        maximum = cycleLength;

                    currentWalk.Pop();
                    continue;
                }

                if( currentWalk.Contains( neighbour ) )
                    continue;

                currentWalk.Push( neighbour );

                // Continue our walk.
                FindStatus(
                    startVertex,
                    neighbour,
                    currentVertex,
                    currentWalk,
                    ref minimum,
                    ref maximum,
                    ref cycleCount
                );

                currentWalk.Pop();
            }
        }

        #endregion

        #region - FindMinimumLength -

        /// <summary>
        /// Helper method that finds the length of the minimum cycle.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <remarks>
        /// We can break the search for a smaller cycle if we found a cycle of length 3
        /// because the <see cref="Graph{TVertexData, TEdgeData}"/> class doesn't allow self-loops or double-edges.
        /// </remarks>
        /// <param name="startVertex">
        /// The start vertex.
        /// </param>
        /// <param name="currentVertex">
        /// The current vertex.
        /// </param>
        /// <param name="previousVertex">
        /// The previously visited vertex.
        /// </param>
        /// <param name="currentWalk">
        /// The current walk in the graph.
        /// </param>
        /// <param name="minimum">
        /// Will contain the length of the minimum cycle.
        /// </param>
        public static void FindMinimumLength<TVertexData, TEdgeData>(
            Vertex<TVertexData, TEdgeData> startVertex,
            Vertex<TVertexData, TEdgeData> currentVertex,
            Vertex<TVertexData, TEdgeData> previousVertex,
            Stack<Vertex<TVertexData, TEdgeData>> currentWalk,
            ref int minimum )
                where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( startVertex != null );
            Contract.Requires<ArgumentNullException>( currentVertex != null );
            Contract.Requires<ArgumentNullException>( previousVertex != null );
            Contract.Requires<ArgumentNullException>( currentWalk != null );

            int neightbourCount = currentVertex.OutgoingEdgeCount;
            if( neightbourCount == 0 )
                return;

            var neighbours = new Vertex<TVertexData, TEdgeData>[neightbourCount];

            for( int i = 0; i < neightbourCount; ++i )
            {
                var edge      = currentVertex.GetEmanatingEdge( i );
                neighbours[i] = edge.GetPartnerVertex( currentVertex );
            }

            foreach( var neighbour in neighbours )
            {
                if( neighbour == startVertex && previousVertex != startVertex )
                {
                    currentWalk.Push( startVertex );

                    int cycleLength = currentWalk.Count - 1;
                    if( cycleLength < minimum )
                        minimum = cycleLength;

                    if( minimum == 3 )
                        return;

                    currentWalk.Pop();
                    continue;
                }

                if( currentWalk.Contains( neighbour ) )
                    continue;

                currentWalk.Push( neighbour );

                // Continue the walk.
                FindMinimumLength( startVertex, neighbour, currentVertex, currentWalk, ref minimum );

                if( minimum == 3 )
                    return;
                currentWalk.Pop();
            }
        }

        #endregion

        #endregion

        #region [ Tarjans Strongly Connected Components Algorithm ]

        /// <summary>
        /// Finds cycles in a graph using Tarjan's strongly connected components algorithm.
        /// See http://en.wikipedia.org/wiki/Tarjan's_strongly_connected_components_algorithm
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="graph">
        /// The input graph.
        /// </param>
        /// <param name="excludeSingleItems">
        /// if set to <c>true</c> nodes with no edges are excluded.
        /// </param>
        /// <returns>
        /// A list of of vertice arrays (paths) that form cycles in the graph.
        /// </returns>
        public static IList<Vertex<TVertexData, TEdgeData>[]> FindUsingTarjan<TVertexData, TEdgeData>( 
            Graph<TVertexData, TEdgeData> graph,
            bool excludeSingleItems )
                where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( graph != null );
            Contract.Ensures( Contract.Result<IList<Vertex<TVertexData, TEdgeData>[]>>() != null );

            var indices = new Dictionary<Vertex<TVertexData, TEdgeData>, int>();
            var lowLinks = new Dictionary<Vertex<TVertexData, TEdgeData>, int>();
            var connected = new List<Vertex<TVertexData, TEdgeData>[]>();
            var stack = new Stack<Vertex<TVertexData, TEdgeData>>();
            
            foreach( var vertex in graph.Vertices )
            {
                if( !indices.ContainsKey( vertex ) )
                {
                    TarjansStronglyConnectedComponentsAlgorithm( 
                        excludeSingleItems,
                        vertex,
                        indices,
                        lowLinks,
                        connected,
                        stack,
                        0 
                    );
                }
            }

            return connected;
        }

        /// <summary>
        /// Executes Tarjan's algorithm on the graph.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="excludeSinlgeItems">if set to <c>true</c> [exclude sinlge items].</param>
        /// <param name="vertex">The vertex to start with.</param>
        /// <param name="indices">The current indices.</param>
        /// <param name="lowLinks">The current lowlinks.</param>
        /// <param name="connected">The connected components.</param>
        /// <param name="stack">The stack.</param>
        /// <param name="index">The current index.</param>
        private static void TarjansStronglyConnectedComponentsAlgorithm<TVertexData, TEdgeData>(
            bool excludeSinlgeItems,
            Vertex<TVertexData, TEdgeData> vertex,
            IDictionary<Vertex<TVertexData, TEdgeData>, int> indices,
            IDictionary<Vertex<TVertexData, TEdgeData>, int> lowLinks,
            ICollection<Vertex<TVertexData, TEdgeData>[]> connected,
            Stack<Vertex<TVertexData, TEdgeData>> stack,
            int index )
                where TVertexData : IEquatable<TVertexData>
        {
            indices[vertex] = index;
            lowLinks[vertex] = index;
            index++;

            stack.Push( vertex );

            foreach( var edge in vertex.EmanatingEdges )
            {
                var next = edge.To;

                if( !indices.ContainsKey( next ) )
                {
                    TarjansStronglyConnectedComponentsAlgorithm(
                        excludeSinlgeItems, 
                        next, 
                        indices, 
                        lowLinks, 
                        connected, 
                        stack, 
                        index
                    );
                    
                    lowLinks[vertex] = Math.Min( lowLinks[vertex], lowLinks[next] );
                }
                else if( stack.Contains( next ) )
                {
                    lowLinks[vertex] = Math.Min( lowLinks[vertex], lowLinks[next] );
                }
            }

            if( lowLinks[vertex] == indices[vertex] )
            {
                Vertex<TVertexData, TEdgeData> next;
                var component = new List<Vertex<TVertexData, TEdgeData>>();

                do
                {
                    next = stack.Pop();
                    component.Add( next );

                } while( next != vertex );

                if( !excludeSinlgeItems || (component.Count > 1) )
                {
                    connected.Add( component.ToArray() );
                }
            }
        }

        #endregion
    }
}