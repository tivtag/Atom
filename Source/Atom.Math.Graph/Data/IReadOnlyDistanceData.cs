// <copyright file="IReadOnlyDistanceData.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Data.IReadOnlyDistanceData interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Graph.Data
{
    /// <summary>
    /// Enables the receiving of distance information stored in a Vertex or Edge.
    /// </summary>
    public interface IReadOnlyDistanceData
    {
        /// <summary>
        /// Gets the associated distance value.
        /// </summary>
        /// <value>The associated distance value.</value>
        float Distance
        {
            get;
        }
    }
}
