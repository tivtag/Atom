// <copyright file="PathSegmentWaypoint.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Waypoints.PathSegmentWaypoint enumeration.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Waypoints
{
    /// <summary>
    /// Enumerates the Waypoints that are part of a <see cref="PathSegment"/>.
    /// </summary>
    public enum PathSegmentWaypoint
    {
        /// <summary>
        /// The first Waypoint in a PathSegment.
        /// </summary>
        From = 0,

        /// <summary>
        /// The second Waypoint in a PathSegment.
        /// </summary>
        To
    }
}
