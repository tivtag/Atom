// <copyright file="VertexCreationFunction.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Math.Graph.VertexCreationFunction{TVertexData} class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Math.Graph
{
    using System;

    /// <summary>
    /// Represents a function that creates the data for a new Vertex given an (unique) input index. 
    /// </summary>
    /// <typeparam name="TVertexData">
    /// The type of data stored in the new Vertex.
    /// </typeparam>
    /// <param name="index">
    /// The (unique) input index.
    /// </param>
    /// <returns>
    /// The data of the new Vertex.
    /// </returns>
    public delegate TVertexData VertexCreationFunction<TVertexData>( int index )
        where TVertexData : IEquatable<TVertexData>;
}
