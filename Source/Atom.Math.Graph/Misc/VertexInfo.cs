// <copyright file="VertexInfo.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Math.Graph.VertexInfo{TVertexData, TEdgeData} class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Math.Graph
{
    using System;

    /// <summary>
    /// Utlity class that stores information about a Vertex.
    /// </summary>
    /// <typeparam name="TVertexData">
    /// The type of data stored by the Vertex.
    /// </typeparam>
    /// <typeparam name="TEdgeData">
    /// The type of data stored within the edges of the Graph.
    /// </typeparam>
    internal class VertexInfo<TVertexData, TEdgeData>
        where TVertexData : IEquatable<TVertexData>
    {
        /// <summary>
        /// Gets or sets the distance from the Vertex to the next goal/etc. 
        /// </summary>
        /// <value>
        /// The distance from the Vertex to the next goal/etc. 
        /// Can be interpreted depending how the VertexInfo is used.
        /// </value>
        public float Distance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the edge that follows the Vertex.
        /// </summary>
        /// <value>The edge that follows the Vertex.</value>
        public Edge<TVertexData, TEdgeData> EdgeFollowed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the VertexInfo and/or the related operation has been finalized.
        /// </summary>
        /// <value>States whether the VertexInfo and/or the related operation has been finalized.</value>
        public bool IsFinalised
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexInfo{TVertexData, TEdgeData}"/> class.
        /// </summary>
        /// <param name="distance">The distance from the Vertex to the next goal/etc.</param>
        /// <param name="edgeFollowed">The edge that follows the Vertex.</param>
        /// <param name="isFinalised">States whether the VertexInfo and/or the related operation is finalized.</param>
        internal VertexInfo( float distance, Edge<TVertexData, TEdgeData> edgeFollowed, bool isFinalised )
        {
            this.Distance     = distance;
            this.EdgeFollowed = edgeFollowed;
            this.IsFinalised  = isFinalised;
        }
    }
}
