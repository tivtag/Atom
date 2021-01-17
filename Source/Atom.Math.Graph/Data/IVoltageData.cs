// <copyright file="IVoltageData.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Data.IVoltageData interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Graph.Data
{
    /// <summary>
    /// Enables the storage of voltage information
    /// in a Vertex or Edge.
    /// </summary>
    public interface IVoltageData
    {
        /// <summary>
        /// Gets or sets the associated voltage value.
        /// </summary>
        /// <value>The voltage value.</value>
        int Voltage 
        { 
            get;
            set;
        }
    }
}
