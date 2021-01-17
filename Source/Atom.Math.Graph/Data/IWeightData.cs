// <copyright file="IWeightData.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Data.IWeightData interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Graph.Data
{
    /// <summary>
    /// Enables the storage of weight information in a Vertex or Edge.
    /// </summary>
    public interface IWeightData : IReadOnlyWeightData
    {
        /// <summary>
        /// Gets or sets the weight stored in this IWeightableData.
        /// </summary>
        new float Weight
        {
            get;
            set;
        }
    }
}
