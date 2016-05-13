// <copyright file="TilePathSegmentPathBuildService.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Waypoints.TilePathSegmentPathBuildService class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Waypoints
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom.AI;
    using Atom.Math;
    using Atom.Scene.Tiles;

    /// <summary>
    /// Provides mechanism that builds the <see cref="TilePath"/> of a <see cref="TilePathSegment"/>.
    /// This class can't be inherited.
    /// </summary>
    public sealed class TilePathSegmentPathBuildService : ITilePathSegmentPathBuildService
    {
        /// <summary>
        /// Initializes a new instance of the TilePathSegmentPathBuildService class.
        /// </summary>
        /// <param name="tileHandler">
        /// The handler that is used traverse the TileMap.
        /// </param>
        /// <param name="pathSearcherProvider">
        /// Provides a mechanism for receicing ITilePathSearchers by floor number.
        /// </param>
        public TilePathSegmentPathBuildService( ITileHandler<object> tileHandler, IMultiFloorPathSearcherProvider pathSearcherProvider )
        {
            Contract.Requires<ArgumentNullException>( tileHandler != null );
            Contract.Requires<ArgumentNullException>( pathSearcherProvider != null );

            this.tileHandler = tileHandler;
            this.pathSearcherProvider = pathSearcherProvider;
        }

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
        public TilePath BuildPath( TilePathSegment pathSegment )
        {
            ITilePathSearcher pathSearcher = this.pathSearcherProvider.GetTilePathSearcher( pathSegment.FloorNumber );
            if( pathSearcher == null )
                return null;
            
            Vector2 from = pathSegment.From.Position;
            Vector2 to = pathSegment.To.Position;

            return pathSearcher.FindPath( (int)from.X, (int)from.Y, (int)to.X, (int)to.Y, null, this.tileHandler );
        }

        /// <summary>
        /// Provides a mechanism for receicing ITilePathSearchers by floor number.
        /// </summary>
        private readonly IMultiFloorPathSearcherProvider pathSearcherProvider;

        /// <summary>
        /// The handler that is used traverse the TileMap.
        /// </summary>
        private readonly ITileHandler<object> tileHandler;
    }
}
