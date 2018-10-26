// <copyright file="TilePathSegment.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Waypoints.TilePathSegment class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Waypoints
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Scene.Tiles;
    using Atom.Components.Transform;

    /// <summary>
    /// Represents a <see cref="PathSegment"/> that calculates the TilePath that connects the
    /// <see cref="PathSegment.From"/> <see cref="Waypoint"/> with the <see cref="PathSegment.To"/> <see cref="Waypoint"/>
    /// on the tile-level.
    /// </summary>
    /// <remarks>
    /// The limitations of this implementation is that the path is calculated arbitrarly,
    /// not taking into account the properties of the 'object' traveling the TilePath.
    /// </remarks>
    public class TilePathSegment : PathSegment
    {
        /// <summary>
        /// Gets the number that uniquely identifies the floor this TilePathSegment uses
        /// to generate the underlying TilePath.
        /// </summary>
        public int FloorNumber
        {
            get
            {
                if( this.PreferredWaypoint == PathSegmentWaypoint.From )
                    return this.From.FloorNumber;
                else
                    return this.To.FloorNumber;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating what Waypoint is choosen to get the <see cref="FloorNumber"/> of this TilePathSegment.
        /// </summary>
        public PathSegmentWaypoint PreferredWaypoint
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the TilePath of this TilePathSegment is currently cached.
        /// </summary>
        public bool IsTilePathCached
        {
            get
            {
                return this.tilePath != null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the TilePathSegment class.
        /// </summary>
        /// <param name="pathBuildService">
        /// Provides a mechanism for building the underlying TilePath of this TilePathSegment.
        /// </param>
        public TilePathSegment( ITilePathSegmentPathBuildService pathBuildService )
        {
            Contract.Requires<ArgumentNullException>( pathBuildService != null );

            this.pathBuildService = pathBuildService;
        }

        /// <summary>
        /// Gets the <see cref="TilePath"/> that connects the <see cref="PathSegment.From"/> <see cref="Waypoint"/> with
        /// the <see cref="PathSegment.To"/> <see cref="Waypoint"/> on the tile-level.
        /// </summary>
        /// <returns>
        /// The TilePath that has been cached.
        /// </returns>
        public TilePath GetTilePath()
        {
            this.CacheTilePath();
            return this.tilePath;
        }

        /// <summary>
        /// Creates and caches the underlying TilePath, if not already cached.
        /// </summary>
        public void CacheTilePath()
        {
            if( !this.IsTilePathCached )
            {
                this.BuildTilePath();
            }
        }

        /// <summary>
        /// Invalidates the currently cached TIlePath.
        /// </summary>
        public void InvalidateCachedTilePath()
        {
            this.tilePath = null;
        }

        /// <summary>
        /// Builds the TilePath.
        /// </summary>
        private void BuildTilePath()
        {
            this.tilePath = this.pathBuildService.BuildPath( this );
            this.OnTilePathBuild( tilePath );
        }

        /// <summary>
        /// Called when the TilePath underlying this TilePathSegment has been build.
        /// </summary>
        /// <param name="path">
        /// The TilePath that has been build.
        /// </param>
        protected virtual void OnTilePathBuild( TilePath path )
        {
        }

        /// <summary>
        /// Called when the transformation of the starting or ending Waypoint has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        protected override void OnWaypointTransformChanged( ITransform2 sender )
        {
            if( this.IsTilePathCached )
            {
                this.BuildTilePath();
            }
        }

        /// <summary>
        /// The TilePath that connects the From Waypoint with the To Waypoint on the tile level.
        /// </summary>
        private TilePath tilePath;

        /// <summary>
        /// Provides a mechanism for building the underlying TilePath of this TilePathSegment.
        /// </summary>
        private ITilePathSegmentPathBuildService pathBuildService;
    }
}
