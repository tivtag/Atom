// <copyright file="WaypointGraphDataFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Waypoints.WaypointGraphDataFactory class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Waypoints
{
    using Atom.Components.Collision;
    using Atom.Components.Transform;
    using Atom.Math.Graph;
    using Atom.Scene;

    /// <summary>
    /// Implements an <see cref="IGraphDataFactory{Waypoint, PathSegment}"/> for the WaypointGraph.
    /// </summary>
    public sealed class WaypointGraphDataFactory : IWaypointGraphDataFactory
    {
        /// <summary>
        /// Represents an instance of the WaypointGraphDataFactory class.
        /// </summary>
        public static readonly WaypointGraphDataFactory Instance = new WaypointGraphDataFactory();

        /// <summary>
        /// Builds the TVertexData stored in a <see cref="Vertex{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <returns>
        /// The TVertexData that will be assigned to the Vertex{TVertexData, TEdgeData}.
        /// </returns>
        public Waypoint BuildVertexData()
        {
            return new Waypoint( new Transform2(), new StaticCollision2(), new QuadTreeItem2() );
        }

        /// <summary>
        /// Builds the TEdgeData stored in a <see cref="Edge{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <returns>
        /// The TEdgeData that will be assigned to the Edge{TVertexData, TEdgeData}.
        /// </returns>
        public PathSegment BuildEdgeData()
        {
            return new PathSegment();
        }
    }
}
