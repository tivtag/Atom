// <copyright file="Heuristic.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Math.Graph.Heuristic{TVertexData, TEdgeData} delegate.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Math.Graph
{
    /// <summary>
    /// A heuristic is a function that associates a value with a node to gauge it considering the node to reach.
    /// </summary>
    /// <typeparam name="TVertexData">
    /// The type of data stored within the vertices of the Graph.
    /// </typeparam>
    /// <typeparam name="TEdgeData">
    /// The type of data stored within the edges of the Graph.
    /// </typeparam>
    /// <param name="source">The source vertex.</param>
    /// <param name="target">The target vertex.</param>
    /// <returns>
    /// A value that tells how hard it is to reach the <paramref name="target"/> vertex;
    /// starting at the <paramref name="source"/> vertex.
    /// </returns>
    public delegate float Heuristic<TVertexData, TEdgeData>(
        Vertex<TVertexData, TEdgeData> source,
        Vertex<TVertexData, TEdgeData> target
    ) where TVertexData : System.IEquatable<TVertexData>;
}
