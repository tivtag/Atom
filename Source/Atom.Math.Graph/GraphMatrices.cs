// <copyright file="GraphMatrices.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.GraphMatrices class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Graph
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom.Math.Graph.Data;

    /// <summary>
    /// Defines static <see cref="Matrix"/> factory methods that take a <see cref="Graph{TVertexData, TEdgeData}"/> as input.
    /// </summary>
    public static class GraphMatrices
    {
        /// <summary>
        /// Gets the adjacency <see cref="Matrix"/> (VertexCount-X-VertexCount) of the <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <value>The adjacency matrix of the graph.</value>
        public static Matrix Adjacency<TVertexData, TEdgeData>( Graph<TVertexData, TEdgeData> graph )
            where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( graph != null );
            Contract.Ensures( Contract.Result<Matrix>() != null );

            Matrix matrix = new Matrix( graph.VertexCount, graph.VertexCount );

            for( int i = 0; i < graph.VertexCount; ++i )
            {
                var vertexI = graph.GetVertexAt( i );
                if( vertexI.HasEmanatingEdgeTo( vertexI ) )
                    matrix[i, i] += 1.0f;

                for( int j = 0; j < graph.VertexCount; ++j )
                {
                    if( i == j )
                        continue;

                    var vertexJ = graph.GetVertexAt( j );
                    if( vertexI.HasEmanatingEdgeTo( vertexJ ) )
                        matrix[i, j] += 1.0f;
                }
            }

            return matrix;
        }

        /// <summary>
        /// Gets the (weighted) adjacency <see cref="Matrix"/> (VertexCount-X-VertexCount) of the <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <value>The weighted adjacency matrix of the graph.</value>
        public static Matrix WeightedAdjacency<TVertexData, TEdgeData>( Graph<TVertexData, TEdgeData> graph )
            where TVertexData : IEquatable<TVertexData>
            where TEdgeData : IReadOnlyWeightData
        {
            Contract.Requires<ArgumentNullException>( graph != null );
            Contract.Ensures( Contract.Result<Matrix>() != null );

            Matrix matrix = new Matrix( graph.VertexCount, graph.VertexCount );

            for( int i = 0; i < graph.VertexCount; ++i )
            {
                var vertexI  = graph.GetVertexAt( i );

                var selfEdge = vertexI.GetEmanatingEdgeTo( vertexI );
                if( selfEdge != null )
                {
                    if( selfEdge.Data != null )
                    {
                        matrix[i, i] += selfEdge.Data.Weight;
                    }
                }

                for( int j = 0; j < graph.VertexCount; ++j )
                {
                    var vertexJ = graph.GetVertexAt( j );
                    var edge    = vertexI.GetEmanatingEdgeTo( vertexJ );

                    if( edge != null && edge.Data != null )
                    {
                        matrix[i, j] += edge.Data.Weight;
                    }
                }
            }

            return matrix;
        }

        /// <summary>
        /// Gets the seidel adjacency <see cref="Matrix"/> (VertexCount-X-VertexCount) of the <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// This operation is only valid on simplistic Graphs.
        /// </exception>
        /// <value>The seidel adjacency matrix of the graph.</value>
        public static Matrix SeidelAdjacency<TVertexData, TEdgeData>( Graph<TVertexData, TEdgeData> graph )
            where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( graph != null );
            Contract.Requires<InvalidOperationException>( graph.IsSimplistic, GraphErrorStrings.OperationOnlyValidOnSimpleGraphs );
            Contract.Ensures( Contract.Result<Matrix>() != null );

            Matrix matrix = new Matrix( graph.VertexCount, graph.VertexCount );

            for( int i = 0; i < graph.VertexCount; ++i )
            {
                var vertexI = graph.GetVertexAt( i );

                for( int j = 0; j < graph.VertexCount; ++j )
                {
                    if( i == j )
                        continue;

                    var vertexJ = graph.GetVertexAt( j );

                    if( vertexI.HasEmanatingEdgeTo( vertexJ ) ||
                        vertexI.HasIncidentEdgeWith( vertexJ ) )
                    {
                        matrix[i, j] = -1.0f;
                    }
                    else
                    {
                        matrix[i, j] = 1.0f;
                    }
                }
            }

            return matrix;
        }

        /// <summary>
        /// Gets the incidence <see cref="Matrix"/> (VertexCount-X-EdgeCount) of the <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <value>
        /// The incidence matrix of the graph.
        /// </value>
        public static Matrix Incidence<TVertexData, TEdgeData>( Graph<TVertexData, TEdgeData> graph )
            where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( graph != null );
            Contract.Ensures( Contract.Result<Matrix>() != null );

            Matrix matrix = new Matrix( graph.VertexCount, graph.EdgeCount );

            if( graph.IsDirected )
            {
                for( int vertexIndex = 0; vertexIndex < graph.VertexCount; ++vertexIndex )
                {
                    var vertex = graph.GetVertexAt( vertexIndex );

                    for( int edgeIndex = 0; edgeIndex < graph.EdgeCount; ++edgeIndex )
                    {
                        var edge = graph.GetEdgeAt( edgeIndex );

                        if( edge.From != edge.To )
                        {
                            if( edge.From == vertex )
                            {
                                matrix[vertexIndex, edgeIndex] = -1.0f;
                            }
                            else if( edge.To == vertex )
                            {
                                matrix[vertexIndex, edgeIndex] = 1.0f;
                            }
                        }
                        else
                        {
                            matrix[vertexIndex, edgeIndex] = 2;
                        }
                    }
                }
            }
            else
            {
                for( int i = 0; i < graph.VertexCount; ++i )
                {
                    var vertex = graph.GetVertexAt( i );

                    for( int j = 0; j < graph.EdgeCount; ++j )
                    {
                        var edge = graph.GetEdgeAt( j );

                        if( edge.From == edge.To && edge.From == vertex )
                        {
                            matrix[i, j] = 2;
                        }
                        else
                        {
                            if( edge.From == vertex || edge.To == vertex )
                            {
                                matrix[i, j] = 1.0f;
                            }
                        }
                    }
                }
            }

            return matrix;
        }

        /// <summary>
        /// Gets the laplacian <see cref="Matrix"/> (VertexCount-X-VertexCount) of the specified <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <value>
        /// The laplacian matrix of the graph.
        /// </value>
        public static Matrix Laplacian<TVertexData, TEdgeData>( Graph<TVertexData, TEdgeData> graph )
            where TVertexData : IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( graph != null );
            Contract.Ensures( Contract.Result<Matrix>() != null );

            Matrix matrix = new Matrix( graph.VertexCount, graph.VertexCount );

            for( int i = 0; i < graph.VertexCount; ++i )
            {
                var vertexI = graph.GetVertexAt( i );

                for( int j = 0; j < graph.VertexCount; ++j )
                {
                    if( i == j )
                    {
                        matrix[i, j] = vertexI.Degree;
                    }
                    else
                    {
                        var vertexJ = graph.GetVertexAt( j );

                        if( vertexJ.HasEmanatingEdgeTo( vertexI ) || vertexJ.HasIncidentEdgeWith( vertexI ) )
                        {
                            matrix[i, j] = -1.0f;
                        }
                        else
                        {
                            matrix[i, j] = 0.0f;
                        }
                    }
                }
            }

            return matrix;
        }
    }
}
