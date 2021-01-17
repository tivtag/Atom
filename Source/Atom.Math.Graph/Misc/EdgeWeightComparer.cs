// <copyright file="EdgeWeightComparer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.EdgeWeightComparer{TVertexData, TEdgeData} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Graph
{
    using System;
    using System.Collections.Generic;
    using Atom.Math.Graph.Data;

    /// <summary>
    /// Imeplement the comparing of weights of <see cref="Edge{TVertexData, TEdgeData}"/> instances.
    /// This is a sealed class.
    /// </summary>
    /// <typeparam name="TVertexData">
    /// The type of the data contained within the vertices of the edges to compare.
    /// </typeparam>
    /// <typeparam name="TEdgeData">
    /// The type of data stored within the edges of the Graph.
    /// </typeparam>
    [Serializable]
    public sealed class EdgeWeightComparer<TVertexData, TEdgeData> : IComparer<Edge<TVertexData, TEdgeData>>
        where TVertexData : IEquatable<TVertexData>
        where TEdgeData : IReadOnlyWeightData
    {
        /// <summary>
        /// Gets the singleton-instance of the <see cref="EdgeWeightComparer{TVertexData, TEdgeData}"/> class.
        /// </summary>
        public static EdgeWeightComparer<TVertexData, TEdgeData> Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// The instance of the <see cref="EdgeWeightComparer{TVertexData, TEdgeData}"/> class. This is a read-only field.
        /// </summary>
        private static readonly EdgeWeightComparer<TVertexData, TEdgeData> instance = new EdgeWeightComparer<TVertexData, TEdgeData>();
        
        /// <summary>
        /// Prevents a default instance of the <see cref="EdgeWeightComparer{TVertexData, TEdgeData}"/> class from being created.
        /// </summary>
        private EdgeWeightComparer()
        {
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
        /// </returns>
        public int Compare( Edge<TVertexData, TEdgeData> x, Edge<TVertexData, TEdgeData> y )
        {
            float weightX = x.Data != null ? x.Data.Weight : 0.0f;
            float weightY = y.Data != null ? y.Data.Weight : 0.0f;

            return weightX.CompareTo( weightY );
        }
    }
}
