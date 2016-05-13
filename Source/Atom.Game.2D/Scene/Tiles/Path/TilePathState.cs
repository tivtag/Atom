// <copyright file="TilePathState.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Tiles.TilePathState enumeration.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene.Tiles
{
    /// <summary>
    /// Specifies the state of a <see cref="TilePath"/>.
    /// </summary>
    public enum TilePathState
    {
        /// <summary>
        /// A path has been found.</summary>
        Found,

        /// <summary>
        /// No path has been found.
        /// </summary>
        NotFound
    }
}
