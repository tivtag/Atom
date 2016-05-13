// <copyright file="ISizeable2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ISizeable2 interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    /// <summary>
    /// Represents an object that has a two-dimensional size.
    /// </summary>
    public interface ISizeable2
    {
        /// <summary>
        /// Gets the height of this ISizeable2 object.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets the width of this ISizeable2 object.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the width and height of this ISizeable2 object.
        /// </summary>
        Point2 Size { get; }
    }
}
