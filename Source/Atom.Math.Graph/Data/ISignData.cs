// <copyright file="ISignData.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Data.ISignData interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Graph.Data
{
    /// <summary>
    /// Enables the storage of <see cref="Sign"/> information
    /// in a Vertex or Edge.
    /// </summary>
    public interface ISignData
    {
        /// <summary>
        /// Gets or sets the associated <see cref="Sign"/> value.
        /// </summary>
        /// <value>The associated sign.</value>
        Sign Sign
        {
            get;
            set;
        }
    }
}
