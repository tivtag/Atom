// <copyright file="GraphFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Math.Graph.GraphFactory{TVertexData, TEdgeData} class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Math.Graph
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Provides static methods to create various different <see cref="Graph{TVertexData, TEdgeData}"/>s.
    /// </summary>
    public static class GraphFactory
    {
        #region - CreateStar -

        /// <summary>
        /// Creates a new indirected <see cref="Graph{TVertexData, TEdgeData}"/> that contains the given <paramref name="vertex"/>
        /// and all edges and vertices the vertex connects to.
        /// </summary>
        /// <param name="vertex">The input vertex.</param>
        /// <returns>A new Graph, with cloned vertices.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="vertex"/> is null.
        /// </exception>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        public static Graph<TVertexData, TEdgeData> CreateStar<TVertexData, TEdgeData>( Vertex<TVertexData, TEdgeData> vertex ) 
            where TVertexData : IEquatable<TVertexData>
        {
            if( vertex == null )
                throw new ArgumentNullException( "vertex" );

            var graph = new Graph<TVertexData, TEdgeData>( false );

            var startVertex = vertex.Clone();
            graph.AddVertex( startVertex );

            foreach( var edge in vertex.EmanatingEdges )
            {
                var endVertex = edge.GetPartnerVertex( vertex ).Clone();

                graph.AddVertex( endVertex );
                graph.AddEdge( startVertex, endVertex );
            }

            return graph;
        }

        /// <summary>
        /// Creates a new indirected <see cref="Graph{TVertexData, TEdgeData}"/> that contains N vertices and N-1 edges,
        /// where the first vertex connects with all the other vertices.
        /// </summary>
        /// <param name="order">
        /// The order of the star to create.
        /// </param>
        /// <param name="vertexFunction">
        /// The function that creates a new Vertex given an index.
        /// </param>
        /// <returns>A new Graph.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="order"/> is less than 1.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="vertexFunction"/> is null.
        /// </exception>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        public static Graph<TVertexData, TEdgeData> CreateStar<TVertexData, TEdgeData>( 
            int order, 
            VertexCreationFunction<TVertexData> vertexFunction )
            where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires( order >= 1 );
            Contract.Requires<ArgumentNullException>( vertexFunction != null );
            
            var graph = new Graph<TVertexData, TEdgeData>( false );
            graph.EdgeCapacity   = order;
            graph.VertexCapacity = order + 1;

            var startVertex = new Vertex<TVertexData, TEdgeData>( vertexFunction( 0 ) );
            graph.AddVertex( startVertex );

            for( int i = 1; i < order; ++i )
            {
                var endVertex = new Vertex<TVertexData, TEdgeData>( vertexFunction( i ) );

                graph.AddVertex( endVertex );
                graph.AddEdge( startVertex, endVertex );
            }

            return graph;
        }

        #endregion

        #region - CreatePath -

        /// <summary>
        /// Creates a new indirected <see cref="Graph{TVertexData, TEdgeData}"/> that contains a connected path of N vertices and N-1 edges.
        /// </summary>
        /// <param name="vertexCount">
        /// The number of outer vertices.
        /// </param>
        /// <param name="vertexFunction">
        /// The function that creates a new Vertex given an index.
        /// </param>
        /// <returns>A new Graph.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="vertexCount"/> is less than 2.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="vertexFunction"/> is null.
        /// </exception>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        public static Graph<TVertexData, TEdgeData> CreatePath<TVertexData, TEdgeData>( 
            int vertexCount, 
            VertexCreationFunction<TVertexData> vertexFunction )
            where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires( vertexCount >= 2 );
            Contract.Requires<ArgumentNullException>( vertexFunction != null );
            
            var graph = new Graph<TVertexData, TEdgeData>( false );
            graph.EdgeCapacity   = vertexCount;
            graph.VertexCapacity = vertexCount + 1;

            var startVertex = new Vertex<TVertexData, TEdgeData>( vertexFunction( 0 ) );
            graph.AddVertex( startVertex );

            for( int i = 1; i < vertexCount; ++i )
            {
                var endVertex = new Vertex<TVertexData, TEdgeData>( vertexFunction( i ) );

                graph.AddVertex( endVertex );
                graph.AddEdge( startVertex, endVertex );

                startVertex = endVertex;
            }

            return graph;
        }

        #endregion

        #region - CreateComplete -

        /// <summary>
        /// Creates the Nth complete <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <param name="vertexCount">
        /// The number of vertices.
        /// </param>
        /// <param name="vertexFunction">
        /// The function that creates a new Vertex given an index.
        /// </param>
        /// <returns>
        /// A new complete <see cref="Graph{TVertexData, TEdgeData}"/> with <paramref name="vertexCount"/> vertices.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="vertexCount"/> is less than 1.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="vertexFunction"/> is null.
        /// </exception>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        public static Graph<TVertexData, TEdgeData> CreateComplete<TVertexData, TEdgeData>( 
            int vertexCount, 
            VertexCreationFunction<TVertexData> vertexFunction )
            where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires( vertexCount >= 1 );
            Contract.Requires<ArgumentNullException>( vertexFunction != null );

            var graph = new Graph<TVertexData, TEdgeData>( false );

            // Create vertex set:
            graph.VertexCapacity = vertexCount;

            for( int i = 0; i < vertexCount; ++i )
            {
                graph.AddVertex( new Vertex<TVertexData, TEdgeData>( vertexFunction( i ) ) );
            }

            // Create edge set:
            foreach( var vertexA in graph.Vertices )
            {
                foreach( var vertexB in graph.Vertices )
                {
                    if( vertexA == vertexB )
                        continue;

                    if( !graph.ContainsEdge( vertexA, vertexB ) )
                        graph.AddEdge( vertexA, vertexB );
                }
            }

            return graph;
        }

        /// <summary>
        /// Creates the complete bipartite <see cref="Graph{TVertexData, TEdgeData}"/> Km,n.
        /// </summary>
        /// <param name="upperVertexCount">
        /// The number of vertices on the upper part.
        /// </param>
        /// <param name="lowerVertexCount">
        /// The number of vertices on the lower part.
        /// </param>
        /// <param name="vertexFunction">
        /// The function that creates a new Vertex.
        /// The intager represents the index of the vertex,
        /// and the bool represents whether the vertex is part of the upper (true) or the lower (false) sub-graph.
        /// </param>
        /// <returns>
        /// A new complete-bipartite <see cref="Graph{TVertexData, TEdgeData}"/> with 
        /// <paramref name="upperVertexCount"/> + <paramref name="lowerVertexCount"/> vertices.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="upperVertexCount"/> or <paramref name="lowerVertexCount"/> is less than 1.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="vertexFunction"/> is null.
        /// </exception>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        public static Graph<TVertexData, TEdgeData> CreateCompleteBipartite<TVertexData, TEdgeData>(
            int upperVertexCount, 
            int lowerVertexCount,
            Func<int, bool, TVertexData> vertexFunction )
            where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires( upperVertexCount >= 1 );
            Contract.Requires( lowerVertexCount >= 1 );
            Contract.Requires<ArgumentNullException>( vertexFunction != null );
            
            var graph = new Graph<TVertexData, TEdgeData>( false );

            // Create vertex set:
            graph.VertexCapacity = upperVertexCount + lowerVertexCount;

            var upperVertices = new List<Vertex<TVertexData, TEdgeData>>();
            var lowerVertices = new List<Vertex<TVertexData, TEdgeData>>();

            for( int i = 0; i < upperVertexCount; ++i )
            {
                var vertex = new Vertex<TVertexData, TEdgeData>( vertexFunction( i, true ) );
                upperVertices.Add( vertex );
                graph.AddVertex( vertex );
            }

            for( int i = 0; i < lowerVertexCount; ++i )
            {
                var vertex = new Vertex<TVertexData, TEdgeData>( vertexFunction( i, false ) );
                lowerVertices.Add( vertex );
                graph.AddVertex( vertex );
            }

            // Create edge set:
            graph.EdgeCapacity = upperVertexCount * lowerVertexCount;

            foreach( var vertexA in upperVertices )
            {
                foreach( var vertexB in lowerVertices )
                {
                    if( !graph.ContainsEdge( vertexA, vertexB ) )
                    {
                        graph.AddEdge( vertexA, vertexB );
                    }
                }
            }

            return graph;
        }

        #endregion

        #region - CreateCircle -

        /// <summary>
        /// Creates the cyclic <see cref="Graph{TVertexData, TEdgeData}"/> with N vertices.
        /// </summary>
        /// <param name="vertexCount">
        /// The number of vertices.
        /// </param>
        /// <param name="vertexFunction">
        /// The function that creates a new Vertex given an index.
        /// </param>
        /// <returns>
        /// A new complete <see cref="Graph{TVertexData, TEdgeData}"/> with <paramref name="vertexCount"/> vertices.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="vertexCount"/> is less than 3.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="vertexFunction"/> is null.
        /// </exception>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        public static Graph<TVertexData, TEdgeData> CreateCircle<TVertexData, TEdgeData>( 
            int vertexCount,
            VertexCreationFunction<TVertexData> vertexFunction )
            where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires( vertexCount >= 3 );
            Contract.Requires<ArgumentNullException>( vertexFunction != null );

            var graph = new Graph<TVertexData, TEdgeData>( false );

            // Create vertex set:
            graph.VertexCapacity = vertexCount;

            for( int i = 0; i < vertexCount; ++i )
            {
                graph.AddVertex( new Vertex<TVertexData, TEdgeData>( vertexFunction( i ) ) );
            }

            // Create edge set:
            graph.EdgeCapacity = vertexCount;

            for( int i = 0; i < graph.VertexCount; ++i )
            {
                int nextIndex = i + 1;
                if( nextIndex >= graph.VertexCount )
                    nextIndex = 0;

                graph.AddEdge( graph.GetVertexAt( i ), graph.GetVertexAt( nextIndex ) );
            }

            return graph;
        }

        #endregion

        #region - CreateBanana -

        /// <summary>
        /// Creates a Banana <see cref="Graph{TVertexData, TEdgeData}"/> with N leaves and k-stars.
        /// </summary>
        /// <param name="leafCount">
        /// The number of leaves the new banana graph.
        /// </param>
        /// <param name="starOrder">
        /// The order of the star sub-graphs.
        /// </param>
        /// <param name="vertexFunction">
        /// The function that creates a new Vertex given an index.
        /// </param>
        /// <returns>
        /// A new <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="leafCount"/> is less than 2;
        /// or if <paramref name="starOrder"/> is less than 0;
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="vertexFunction"/> is null.
        /// </exception>
        /// <typeparam name="TVertexData">
        /// The type of data stored within the vertices of the Graph.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        public static Graph<TVertexData, TEdgeData> CreateBanana<TVertexData, TEdgeData>( 
            int leafCount, 
            int starOrder,
            VertexCreationFunction<TVertexData> vertexFunction )
            where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires( leafCount >= 2 );
            Contract.Requires( starOrder >= 2 );
            Contract.Requires<ArgumentNullException>( vertexFunction != null );
            
            var graph = new Graph<TVertexData, TEdgeData>( false );

            graph.EdgeCapacity   = leafCount * (2 + starOrder);
            graph.VertexCapacity = 1 + graph.EdgeCapacity;

            // Create root
            var root = new Vertex<TVertexData, TEdgeData>( vertexFunction( 0 ) );

            graph.AddVertex( root );

            for( int i = 1; i <= leafCount; ++i )
            {
                // Create leaf
                var leaf = new Vertex<TVertexData, TEdgeData>( vertexFunction( i ) );
                graph.AddVertex( leaf );
                graph.AddEdge( root, leaf );

                // Create Star:
                var starCenter = new Vertex<TVertexData, TEdgeData>( vertexFunction( i + 1 ) );
                graph.AddVertex( starCenter );

                if( starOrder == 1 )
                {
                    graph.AddEdge( leaf, starCenter );
                }
                else
                {
                    for( int j = 1; j <= starOrder; ++j )
                    {
                        var vertex = new Vertex<TVertexData, TEdgeData>( vertexFunction( i + j + 1 ) );
                        graph.AddVertex( vertex );

                        // Connect the vertex at the center of the star with the new outer vertex:
                        graph.AddEdge( starCenter, vertex );

                        // Connect the first outer vertex of the star with the leaf:
                        if( j == 1 )
                            graph.AddEdge( leaf, vertex );
                    }
                }
            }

            return graph;
        }

        #endregion
    }
}