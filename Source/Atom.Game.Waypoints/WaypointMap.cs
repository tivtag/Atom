// <copyright file="WaypointMap.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Waypoints.WaypointMap class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Waypoints
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using System.Linq;
    using Atom.Math;
    using Atom.Math.Graph;
    using Atom.Scene;

    /// <summary>
    /// 
    /// </summary>
    public class WaypointMap
    {
        /// <summary>
        /// Raised when a <see cref="Waypoint"/> has been added to this WaypointMap.
        /// </summary>
        public event RelaxedEventHandler<WaypointMap, Waypoint> WaypointAdded;

        /// <summary>
        /// Raised when a <see cref="PathSegment"/> has been added to this WaypointMap.
        /// </summary>
        public event RelaxedEventHandler<WaypointMap, PathSegment> PathSegmentAdded;

        /// <summary>
        /// Raised when a <see cref="Waypoint"/> has been removed from this WaypointMap.
        /// </summary>
        public event RelaxedEventHandler<WaypointMap, Waypoint> WaypointRemoved;

        /// <summary>
        /// Raised when a <see cref="PathSegment"/> has been removed from this WaypointMap.
        /// </summary>
        public event RelaxedEventHandler<WaypointMap, PathSegment> PathSegmentRemoved;

        /// <summary>
        /// Gets the <see cref="Waypoint"/>s this WaypointMap contains.
        /// </summary>
        public IEnumerable<Waypoint> Waypoints
        {
            get
            {
                return this.graph.Vertices.Select( vertex => vertex.Data );
            }
        }

        /// <summary>
        /// Gets the <see cref="PathSegment"/>s this WaypointMap contains.
        /// </summary>
        public IEnumerable<PathSegment> PathSegments
        {
            get
            {
                return this.graph.Edges.Select( edge => edge.Data );
            }
        }

        /// <summary>
        /// Gets the number of <see cref="Waypoint"/>s this WaypointMap contains.
        /// </summary>
        public int WaypointCount 
        {
            get
            {
                return this.graph.VertexCount;
            }
        }

        /// <summary>
        /// Gets the number of <see cref="PathSegment"/>s this WaypointMap contains.
        /// </summary>
        public int PathSegmentCount
        {
            get
            {
                return this.graph.EdgeCount;
            }
        }

        /// <summary>
        /// Initializes a new instance of the WaypointMap class.
        /// </summary>
        public WaypointMap()
            : this( WaypointGraphDataFactory.Instance )
        {
        }

        /// <summary>
        /// Initializes a new instance of the WaypointMap class.
        /// </summary>
        /// <param name="graphDataFactory">
        /// The factory that is used to build the Waypoint and PathSegment data stored in
        /// the vertices and edges of the WaypointGraph.
        /// </param>
        public WaypointMap( IWaypointGraphDataFactory graphDataFactory )
        {
            this.graph = new WaypointGraph( graphDataFactory );

            this.graph.VertexAdded += (graph, vertex) => { this.WaypointAdded.Raise( this, vertex.Data ); };
            this.graph.VertexRemoved += (graph, vertex) => {
                this.quadTree.Remove( vertex.Data.QuadTreeItem );
                this.WaypointRemoved.Raise( this, vertex.Data );
            };

            this.graph.EdgeAdded += (graph, edge) => { this.PathSegmentAdded.Raise( this, edge.Data ); };
            this.graph.EdgeRemoved += (graph, edge) => { this.PathSegmentRemoved.Raise( this, edge.Data ); };
        }

        /// <summary>
        /// Initializes this WaypointMap.
        /// </summary>
        /// <param name="mapSize">
        /// The size of the map (in pixels).
        /// </param>
        /// <param name="subdivisionCount">
        /// The number of times the WaypointMap is sub-divided.
        /// </param>
        public void Initialize( Vector2 mapSize, int subdivisionCount = 2 )
        {
            this.quadTree.Create( Vector2.Zero, mapSize.X, mapSize.Y, 32.0f, 32.0f, subdivisionCount, 0 );
        }

        /// <summary>
        /// Adds a new Waypoint at the specified position to this WaypointMap.
        /// </summary>
        /// <param name="position">
        /// The initial position of the new Waypoint.
        /// </param>
        /// <returns>
        /// The newly created Waypoint.
        /// </returns>
        public Waypoint AddWaypoint( Vector2 position )
        {
            var vertex = this.graph.AddVertex();
            var waypoint = vertex.Data;

            waypoint.Position = position;
            this.quadTree.Insert( waypoint.QuadTreeItem );

            this.WaypointAdded.Raise( this, waypoint );
            return waypoint;
        }

        /// <summary>
        /// Adds a new <see cref="PathSegment"/> between the specified <see cref="Waypoint"/>s.
        /// </summary>
        /// <param name="from">
        /// The first Waypoint.
        /// </param>
        /// <param name="to">
        /// The second Waypoint.
        /// </param>
        /// <returns>
        /// The newly created PathSegment.
        /// </returns>
        public PathSegment AddPathSegment( Waypoint from, Waypoint to )
        {
            var edge = this.graph.AddEdge( from.Vertex, to.Vertex );
            return edge.Data;
        }

        /// <summary>
        /// Attempts to remove the specified <see cref="Waypoint"/> from this WaypointMap.
        /// </summary>
        /// <param name="waypoint">
        /// The Waypoint to remove.
        /// </param>
        /// <param name="preservePath">
        /// States whether new PathSegments should be added this WaypointMap to fill
        /// the hole that will be created.
        /// </param>
        /// <returns>
        /// true if the specified Waypoint was removed;
        /// otherwise false.
        /// </returns>
        public bool RemoveWaypoint( Waypoint waypoint, bool preservePath = false )
        {
            if( waypoint == null )
                return false;

            if( !preservePath )
            {
                return this.graph.RemoveVertex( waypoint.Vertex );
            }
            else
            {
                if( this.graph.ContainsVertex( waypoint.Vertex ) )
                {
                    GraphOperations.Cut( waypoint.Vertex, this.graph );
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Attempts to remove the specified <see cref="PathSegment"/> from this WaypointMap.
        /// </summary>
        /// <param name="segment">
        /// The PathSegment to remove.
        /// </param>
        /// <returns>
        /// true if the specified PathSegment was removed;
        /// otherwise false.
        /// </returns>
        public bool RemovePathSegment( PathSegment segment )
        {
            if( segment == null )
                return false;

            return this.graph.RemoveEdge( segment.Edge );
        }

        /// <summary>
        /// Gets the <see cref="Waypoint"/>s within the specified <paramref name="area"/>.
        /// </summary>
        /// <param name="area">
        /// The area in which Waypoints should be looked for.
        /// </param>
        /// <returns>
        /// The Waypoints within the specified area.
        /// </returns>
        public IEnumerable<Waypoint> GetWaypointsIn( Rectangle area )
        {
            var list = new List<IQuadTreeItem2>();
            this.quadTree.FindVisible( list, area );

            return list.Select( item => (Waypoint)item.Owner );
        }

        /// <summary>
        /// Attempts to get the <see cref="PathSegment"/> between the specified <see cref="Waypoint"/>s.
        /// </summary>
        /// <param name="from">
        /// The first Waypoint.
        /// </param>
        /// <param name="to">
        /// The second Waypoint.
        /// </param>
        /// <returns>
        /// The PathSegment between the specified Waypoints;
        /// -or- null if there is no direct connection between them.
        /// </returns>
        public PathSegment GetPathSegment( Waypoint from, Waypoint to )
        {
            Contract.Requires<ArgumentNullException>( from != null );
            Contract.Requires<ArgumentNullException>( to != null );

            var edge = this.graph.GetEdge( from, to );
            return edge != null ? edge.Data : null;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="position"></param>
        ///// <returns></returns>
        //public Waypoint GetNearestWaypoint( Vector2 position )
        //{
        //    if( !this.quadTree.Area.Contains( position ) )
        //    {
        //        position = this.quadTree.Area.GetClosestPoint( position );
        //    }
        //
        //    var list = new List<IQuadTreeItem2>();
        //    this.quadTree.FindVisible( list, new Rectangle( (int)position.X, (int)position.Y, 1, 1 ) );
        //
        //    return null;
        //}

        /// <summary>
        /// Gets the <see cref="Waypoint"/> at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the Waypoint to get.
        /// </param>
        /// <returns>
        /// The Waypoint at the specified index.
        /// </returns>
        public Waypoint GetWaypointAt( int index )
        {
            return this.graph.GetVertexAt( index ).Data;
        }

        /// <summary>
        /// Gets the <see cref="PathSegment"/> at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the PathSegment to get.
        /// </param>
        /// <returns>
        /// The PathSegment at the specified index.
        /// </returns>
        public PathSegment GetPathSegmentAt( int index )
        {
            return this.graph.GetEdgeAt( index ).Data;
        }

        /// <summary>
        /// Gets the zero-based index of the specified Waypoint.
        /// </summary>
        /// <param name="waypoint">
        /// The Waypoint to locate.
        /// </param>
        /// <returns>
        /// The index of the specified Waypoint;
        /// -or- "-1" if it could not be found.
        /// </returns>
        public int GetIndexOf( Waypoint waypoint )
        {
            if( waypoint == null )
                return -1;

            for( int index = 0; index < this.graph.VertexCount; ++index )
            {
                if( this.graph.GetVertexAt( index ).Data == waypoint )
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the zero-based index of the specified PathSegment.
        /// </summary>
        /// <param name="segment">
        /// The PathSegment to locate.
        /// </param>
        /// <returns>
        /// The index of the specified PathSegment;
        /// -or- "-1" if it could not be found.
        /// </returns>
        public int GetIndexOf( PathSegment segment )
        {
            if( segment == null )
                return -1;

            for( int index = 0; index < this.graph.EdgeCount; ++index )
            {
                if( this.graph.GetEdgeAt( index ).Data == segment )
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// The QuadTree2 in which Waypoints are inserted, and partially sorted.
        /// </summary>
        private readonly QuadTree2 quadTree = new QuadTree2();

        /// <summary>
        /// The underlying graph that connects the Waypoints using PathSegments.
        /// </summary>
        private readonly WaypointGraph graph;
    }
}
