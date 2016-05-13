// <copyright file="ITilePathSegmentPathBuildService.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Waypoints.ITilePathSegmentPathBuildService interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Waypoints
{
    using Atom.Scene.Tiles;

    /// <summary>
    /// Provides mechanism that builds the <see cref="TilePath"/> of a <see cref="TilePathSegment"/>.
    /// </summary>
    public interface ITilePathSegmentPathBuildService
    {
        /// <summary>
        /// Attempts to find a path from the From <see cref="Waypoint"/> to the To <see cref="Waypoint"/>
        /// of the specified <see cref="TilePathSegment"/>.
        /// </summary>
        /// <param name="pathSegment">
        /// The path segment to analayze.
        /// </param>
        /// <returns>
        /// The path for the specified TilePathSegment on the tile level.
        /// </returns>
        TilePath BuildPath( TilePathSegment pathSegment );
    }
}
