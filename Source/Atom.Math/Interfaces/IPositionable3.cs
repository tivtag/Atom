// <copyright file="IPositionable3.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.IPositionable3 interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    /// <summary>
    /// Provides a mechanism to get or sets the position
    /// of an object in three-dimensional space.
    /// </summary>
    public interface IPositionable3
    {
        /// <summary>
        /// Gets or sets the position of the IPositionable3 object.
        /// </summary>
        /// <value>The position of the object.</value>
        Vector3 Position 
        {
            get;
            set;
        }
    }
}
