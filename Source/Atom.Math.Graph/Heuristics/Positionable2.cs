// <copyright file="Positionable2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Heuristics.Positionable2 class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Graph.Heuristics
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Provides heuristics for vertices that contain TVertexData that implement <see cref="IPositionable2"/>.
    /// </summary>
    public static class Positionable2
    {
        /// <summary>
        /// Returns the euclidian distance between two nodes: Sqrt(Dx²+Dy²+Dz²).
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of vertex data. Must implement <see cref="IPositionable2"/>.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="left">The first node.</param>
        /// <param name="right">The second node.</param>
        /// <returns>The calculated distance.</returns>
        public static float EuclidianDistance<TVertexData, TEdgeData>( Vertex<TVertexData, TEdgeData> left, Vertex<TVertexData, TEdgeData> right )
            where TVertexData : IPositionable2, IEquatable<TVertexData>
        {
            return (float)System.Math.Sqrt( SquareEuclidianDistance( left, right ) );
        }

        /// <summary>
        /// Returns the square euclidian distance between two nodes: Dx²+Dy².
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of vertex data. Must implement <see cref="IPositionable2"/>.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="left">The first node.</param>
        /// <param name="right">The second node.</param>
        /// <returns>The calculated distance.</returns>
        public static float SquareEuclidianDistance<TVertexData, TEdgeData>( Vertex<TVertexData, TEdgeData> left, Vertex<TVertexData, TEdgeData> right )
            where TVertexData : IPositionable2, IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( left != null );
            Contract.Requires<ArgumentNullException>( right != null );
            
            Vector2 leftPosition = left.Data.Position;
            Vector2 rightPosition = right.Data.Position;

            float deltaX = leftPosition.X - rightPosition.X;
            float deltaY = leftPosition.Y - rightPosition.Y;
            return (deltaX * deltaX) + (deltaY * deltaY);
        }

        /// <summary>
        /// Returns the manhattan distance between two nodes: |Dx|+|Dy|.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of vertex data. Must implement <see cref="IPositionable2"/>.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="left">The first node.</param>
        /// <param name="right">The second node.</param>
        /// <returns>The calculated distance.</returns>
        public static float ManhattanDistance<TVertexData, TEdgeData>( Vertex<TVertexData, TEdgeData> left, Vertex<TVertexData, TEdgeData> right )
            where TVertexData : IPositionable2, IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( left != null );
            Contract.Requires<ArgumentNullException>( right != null );

            Vector2 leftPosition = left.Data.Position;
            Vector2 rightPosition = right.Data.Position;

            float deltaX = leftPosition.X - rightPosition.X;
            float deltaY = leftPosition.Y - rightPosition.Y;
            return System.Math.Abs( deltaX ) + System.Math.Abs( deltaY );
        }

        /// <summary>
        /// Returns the maximum distance between two nodes: Max(|Dx|, |Dy|).
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of vertex data. Must implement <see cref="IPositionable2"/>.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored within the edges of the Graph.
        /// </typeparam>
        /// <param name="left">The first node.</param>
        /// <param name="right">The second node.</param>
        /// <returns>The calculated distance.</returns>
        public static float MaxDistanceAlongAxis<TVertexData, TEdgeData>( Vertex<TVertexData, TEdgeData> left, Vertex<TVertexData, TEdgeData> right )
            where TVertexData : IPositionable2, IEquatable<TVertexData>
        {
            Contract.Requires<ArgumentNullException>( left != null );
            Contract.Requires<ArgumentNullException>( right != null );

            Vector2 leftPosition = left.Data.Position;
            Vector2 rightPosition = right.Data.Position;

            float deltaX = System.Math.Abs( leftPosition.X - rightPosition.X );
            float deltaY = System.Math.Abs( leftPosition.Y - rightPosition.Y );
            return System.Math.Max( deltaX, deltaY );
        }
    }
}
