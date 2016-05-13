// <copyright file="Waypoint.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Waypoints.Waypoint class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Waypoints
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Atom.Components;
    using Atom.Components.Collision;
    using Atom.Components.Transform;
    using Atom.Math;
    using Atom.Math.Graph;
    using Atom.Scene;

    /// <summary>
    /// Represents an important location in a <see cref="WaypointMap"/> that is connected to other locations.
    /// </summary>
    public class Waypoint : Entity, IEquatable<Waypoint>, IOwnedBy<Vertex<Waypoint, PathSegment>>, IPositionable2, IFloorObject
    {
        /// <summary>
        /// Gets or sets the position of this Waypoint.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return this.transform.Position;
            }

            set
            {
                this.transform.Position = value;
            }
        }

        /// <summary>
        /// Gets or sets the number that identifies the floor this Waypoint is on.
        /// </summary>
        /// <remarks>
        /// This represents the Z-axis.
        /// </remarks>
        /// <value>
        /// The default value is 0.
        /// </value>
        public int FloorNumber
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets the <see cref="ITransform2"/> component that gives this Waypoint a position.
        /// </summary>
        public ITransform2 Transform
        {
            get
            {
                return this.transform;
            }
        }

        /// <summary>
        /// Gets the <see cref="IQuadTreeItem2"/> component that allows this Waypoint
        /// to be stored in a Waypoint.
        /// </summary>
        public IQuadTreeItem2 QuadTreeItem
        {
            get
            {
                return this.quadTreeItem;
            }
        }

        /// <summary>
        /// Gets the Vertex{Waypoint, PathSegment} that owns this Waypoint.
        /// </summary>
        public Vertex<Waypoint, PathSegment> Vertex
        {
            get
            {
                return this.vertex;
            }
        }

        /// <summary>
        /// Gets the <see cref="PathSegment"/> that connect this <see cref="Waypoint"/> with other <see cref="Waypoint"/>s.
        /// </summary>
        public IEnumerable<PathSegment> Segments
        {
            get
            {
                return this.vertex.EmanatingEdges.Select( edge => edge.Data );
            }
        }

        /// <summary>
        /// Gets or sets the Vertex{Waypoint, PathSegment} that owns this Waypoint.
        /// </summary>
        Vertex<Waypoint, PathSegment> IOwnedBy<Vertex<Waypoint, PathSegment>>.Owner
        {
            get
            {
                return vertex;
            }

            set
            {
                this.vertex = value;
            }
        }

        /// <summary>
        /// Returns a value indicating whether the specified Waypoints are equal.
        /// </summary>
        /// <param name="other">
        /// The Waypoint to compare to.
        /// </param>
        /// <returns>
        /// true if they are equal;
        /// otherwise false.
        /// </returns>
        public bool Equals( Waypoint other )
        {
            if( object.ReferenceEquals( this, other ) )
                return true;

            return false;
        }

        /// <summary>
        /// Initializes a new instance of the Waypoint class.
        /// </summary>
        public Waypoint()
            : this( new Transform2(), new StaticCollision2() { Size = Vector2.One }, new QuadTreeItem2() )
        {
        }

        /// <summary>
        /// Initializes a new instance of the Waypoint class.
        /// </summary>
        /// <param name="transform">
        /// The component that is used to give the new Waypoint a position.
        /// </param>
        /// <param name="collision">
        /// The component that is used to give the new Waypoint a position.
        /// </param>
        /// <param name="quadTreeItem">
        /// The component that provides a mechanism for inserting the new Waypoint in a QuadTree.
        /// </param>
        public Waypoint( ITransform2 transform, ICollision2 collision, IQuadTreeItem2 quadTreeItem )
        {
            Contract.Requires<ArgumentNullException>( transform != null );
            Contract.Requires<ArgumentNullException>( collision != null );
            Contract.Requires<ArgumentNullException>( quadTreeItem != null );

            this.transform = transform;
            this.collision = collision;
            this.quadTreeItem = quadTreeItem;

            this.Components.BeginSetup();
            {
                this.Components.Add( this.transform );
                this.Components.Add( this.collision );
                this.Components.Add( this.quadTreeItem );
            }
            this.Components.EndSetup();
        }

        /// <summary>
        /// Gets a value indicating whether this Waypoint is directly connected to the specified Waypoint.
        /// </summary>
        /// <param name="waypoint">
        /// The Waypoint to compare to.
        /// </param>
        /// <returns>
        /// true if the Waypoints are connected;
        /// otherwise false.
        /// </returns>
        [Pure]
        public bool HasPathSegmentTo( Waypoint waypoint )
        {
            if( waypoint == null )
                return false;

            return this.vertex.HasEmanatingEdgeTo( waypoint.vertex );
        }

        /// <summary>
        /// Gets the PathSegment that directly connects this Waypoint with the specified Waypoint.
        /// </summary>
        /// <param name="waypoint">
        /// The Waypoint to compare to.
        /// </param>
        /// <returns>
        /// true if the Waypoints are connected;
        /// otherwise false.
        /// </returns>
        [Pure]
        public PathSegment GetPathSegmentTo( Waypoint waypoint )
        {
            if( waypoint == null )
                return null;

            var edge = this.vertex.GetEmanatingEdgeTo( waypoint.vertex );
            return edge != null ? edge.Data : null;
        }

        /// <summary>
        /// Gets a value indicating whether this Waypoint is connected to another Waypoint
        /// using the specified PathSegment.
        /// </summary>
        /// <param name="segment">
        /// The PathSegment to locate.
        /// </param>
        /// <returns>
        /// true if this Waypoint is connected to another Waypoint
        /// using the specified PathSegment -or- otherwise false.
        /// </returns>
        [Pure]
        internal bool HasPathSegment( PathSegment segment )
        {
            return this.vertex.EmanatingEdges.Contains( segment.Edge );
        }

        /// <summary>
        /// Overriden to return the name of this Waypoint.
        /// </summary>
        /// <returns>
        /// The name of this Waypoint.
        /// </returns>
        public override string ToString()
        {
            return this.Name ?? string.Empty;
        }

        /// <summary>
        /// Represents the vertex that owns this Waypoint.
        /// </summary>
        private Vertex<Waypoint, PathSegment> vertex;

        /// <summary>
        /// The component that is used to give this Waypoint a position.
        /// </summary>
        private readonly ITransform2 transform;

        /// <summary>
        /// The component that is used to give this Waypoint a position.
        /// </summary>
        private readonly ICollision2 collision;

        /// <summary>
        /// The component that provides a mechanism for inserting this Waypoint in a QuadTree.
        /// </summary>
        private readonly IQuadTreeItem2 quadTreeItem;
    }
}
