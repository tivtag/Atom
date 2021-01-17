// <copyright file="IWaypointGraphDataFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Waypoints.IWaypointGraphDataFactory interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Waypoints
{
    using Atom.Math.Graph;

    /// <summary>
    /// Represents the factory that is used to build the Waypoint and PathSegment data stored in
    /// the vertices and edges of the new WaypointGraph.
    /// </summary>
    public interface IWaypointGraphDataFactory : IGraphDataFactory<Waypoint, PathSegment>
    {
    }
}
