// <copyright file="IGraphDataFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Data.IGraphDataFactory{TVertexData, TEdgeData} interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Graph
{
    using System;

    /// <summary>
    /// Provides a mechanism for building the objects that contain data in a Vertex or Edge
    /// of a Graph.
    /// </summary>
    /// <typeparam name="TVertexData">
    /// The type of data stored within the vertices of the Graph.
    /// </typeparam>
    /// <typeparam name="TEdgeData">
    /// The type of data stored within the edges of the Graph.
    /// </typeparam>
    public interface IGraphDataFactory<TVertexData, TEdgeData>
        where TVertexData : IEquatable<TVertexData>
    {
        /// <summary>
        /// Builds the TVertexData stored in a <see cref="Vertex{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <returns>
        /// The TVertexData that will be assigned to the Vertex{TVertexData, TEdgeData}.
        /// </returns>
        TVertexData BuildVertexData();

        /// <summary>
        /// Builds the TEdgeData stored in a <see cref="Edge{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <returns>
        /// The TEdgeData that will be assigned to the Edge{TVertexData, TEdgeData}.
        /// </returns>
        TEdgeData BuildEdgeData();
    }
}
