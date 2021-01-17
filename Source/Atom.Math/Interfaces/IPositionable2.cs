// <copyright file="IPositionable2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.IPositionable2 interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    /// <summary>
    /// Provides a mechanism to get or sets the position
    /// of an object in two-dimensional space.
    /// </summary>
    public interface IPositionable2
    {
        /// <summary>
        /// Gets or sets the position of the IPositionable2 object.
        /// </summary>
        /// <value>The position of the object.</value>
        Vector2 Position 
        {
            get;
            set;
        }
    }
}
