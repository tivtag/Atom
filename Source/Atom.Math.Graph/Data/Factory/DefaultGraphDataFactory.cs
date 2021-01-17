// <copyright file="DefaultGraphDataFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Data.DefaultGraphDataFactory{TVertexData, TEdgeData} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Graph
{
    using System;

    /// <summary>
    /// Implements an <see cref="IGraphDataFactory{TVertexData, TEdgeData}"/> that returns default values.
    /// </summary>
    /// <typeparam name="TVertexData">
    /// The type of data stored within the vertices of the Graph.
    /// </typeparam>
    /// <typeparam name="TEdgeData">
    /// The type of data stored within the edges of the Graph.
    /// </typeparam>
    public sealed class DefaultGraphDataFactory<TVertexData, TEdgeData> : IGraphDataFactory<TVertexData, TEdgeData>
        where TVertexData : IEquatable<TVertexData>
    {
        /// <summary>
        /// Represents an instance of the DefaultGraphDataFactory{TVertexData, TEdgeData} class.
        /// </summary>
        public static readonly DefaultGraphDataFactory<TVertexData, TEdgeData> Instance = new DefaultGraphDataFactory<TVertexData, TEdgeData>();

        /// <summary>
        /// Builds the TVertexData stored in a <see cref="Vertex{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <returns>
        /// The TVertexData that will be assigned to the Vertex{TVertexData, TEdgeData}.
        /// </returns>
        public TVertexData BuildVertexData()
        {
            return default( TVertexData );
        }

        /// <summary>
        /// Builds the TEdgeData stored in a <see cref="Edge{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <returns>
        /// The TEdgeData that will be assigned to the Edge{TVertexData, TEdgeData}.
        /// </returns>
        public TEdgeData BuildEdgeData()
        {
            return default( TEdgeData );
        }
    }
}
