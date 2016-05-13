// <copyright file="IReadOnlyWeightData.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Data.IReadOnlyWeightData interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Graph.Data
{
    /// <summary>
    /// Enables the receiving of weight information stored in a Vertex or Edge.
    /// </summary>
    public interface IReadOnlyWeightData
    {
        /// <summary>
        /// Gets the weight stored in this IReadOnlyWeightableData.
        /// </summary>
        float Weight
        {
            get;
        }
    }
}
