// <copyright file="IDistanceData.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Data.IDistanceData interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Graph.Data
{
    /// <summary>
    /// Enables the storage of distance information in a Vertex or Edge.
    /// </summary>
    public interface IDistanceData : IReadOnlyDistanceData
    {
        /// <summary>
        /// Gets or sets the associated distance value.
        /// </summary>
        /// <value>The associated distance value.</value>
        new float Distance
        { 
            get; 
            set;
        }
    }
}
