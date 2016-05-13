// <copyright file="WayGraph.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Waypoints.WaypointGraph class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Waypoints
{
    using Atom.Math.Graph;

    /// <summary>
    /// Represents the graph that contains the internal structure of a <see cref="WaypointMap"/>.
    /// </summary>
    public class WaypointGraph : Graph<Waypoint, PathSegment>
    {
        /// <summary>
        /// Initializes a new instance of the WaypointGraph class.
        /// </summary>
        public WaypointGraph()
            : base( WaypointGraphDataFactory.Instance, false )
        {
        }

        /// <summary>
        /// Initializes a new instance of the WaypointGraph class.
        /// </summary>
        /// <param name="dataFactory">
        /// The factory that is used to build the Waypoint and PathSegment data stored in
        /// the vertices and edges of the new WaypointGraph.
        /// </param>
        public WaypointGraph( IWaypointGraphDataFactory dataFactory )
            : base( dataFactory, false )
        {
        }
    }
}
