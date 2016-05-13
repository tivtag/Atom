// <copyright file="TilePath.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Tiles.TilePath class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene.Tiles
{
    using System;
    using Atom.Math;

    /// <summary> 
    /// Represents a path of (connected) tiles on a TileMap.
    /// </summary>
    public class TilePath
    {
        #region [ Properties ]

        /// <summary> 
        /// Gets the state of this <see cref="TilePath"/>.
        /// </summary>
        /// <value>The <see cref="TilePathState"/> of this <see cref="TilePath"/>.</value>
        public TilePathState State
        {
            get
            {
                return this.state;
            }
        }

        /// <summary>
        /// Gets the location (in tile-space) at the given index
        /// of the <see cref="TilePath"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the tile location to receive.</param>
        /// <returns>The location of the tile.</returns>
        public Point2 this[int index]
        {
            get
            {
                return this.path[index];
            }
        }

        /// <summary>
        /// Gets the total length of this <see cref="TilePath"/>.
        /// </summary>
        /// <value>The length of the <see cref="TilePath"/>.</value>
        public int Length
        {
            get
            {
                return this.path == null ? 0 : path.Length;
            }
        }

        /// <summary>
        /// Gets the <see cref="TileMapDataLayer"/> that contains this <see cref="TilePath"/>.
        /// </summary>
        /// <value>The TileMapDataLayer that is related to this TilePath.</value>
        public TileMapDataLayer Layer
        {
            get
            {
                return this.layer;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="TilePath"/> class.
        /// </summary>
        /// <param name="layer">The layer the new TilePath is part of.</param>
        /// <param name="state">The state of the new TilePath.</param>
        /// <param name="path">The actual path data of the new TilePath.</param>
        protected TilePath( TileMapDataLayer layer, TilePathState state, Point2[] path )
        {
            this.layer = layer;
            this.state = state;
            this.path  = path;
        }

        /// <summary>
        /// Creates a new <see cref="TilePath"/> instance initialized 
        /// with the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="layer">The layer the new path is in.</param>
        /// <param name="path">
        /// The path data.
        /// </param>
        /// <returns>
        /// A new <see cref="TilePath"/> instance.
        /// </returns>
        public static TilePath CreateFound( TileMapDataLayer layer, Point2[] path )
        {
            return new TilePath( layer, TilePathState.Found, path );
        }

        /// <summary>
        /// Creates a new <see cref="TilePath"/> instance initialized 
        /// to indicate that the starting point is the end point.
        /// </summary>
        /// <param name="layer">The layer the new path is in.</param>
        /// <returns> A new <see cref="TilePath"/> instance. </returns>
        public static TilePath CreateFoundStartIsTarget( TileMapDataLayer layer )
        {
            return new TilePath( layer, TilePathState.Found, null );
        }

        /// <summary>
        /// Creates a new <see cref="TilePath"/> instance initialized 
        /// to indicate that no path has been found.
        /// </summary>
        /// <param name="layer">The layer the new path is in.</param>
        /// <returns> A new <see cref="TilePath"/> instance. </returns>
        public static TilePath CreateNotFound( TileMapDataLayer layer )
        {
            return new TilePath( layer, TilePathState.NotFound, null );
        }

        #endregion

        #region [ Methods ]

        #region GetDirToNext

        /// <summary>
        /// Gets the direction the next tile in this TilePath is facing.
        /// </summary>
        /// <param name="pathLocation"> The current position in the path. </param>
        /// <returns> The direction from the current tile to the next. </returns>
        public Direction8 GetDirToNext( int pathLocation )
        {
            if( this.path == null || pathLocation + 1 >= this.path.Length || pathLocation < 0 )
                return Direction8.None;

            Point2 current = this.path[pathLocation];
            Point2 next    = this.path[pathLocation + 1];

            int dx = next.X - current.X;
            int dy = next.Y - current.Y;

            if( dx == 0 )
            {
                if( dy > 0 )
                {
                    return Direction8.Down;
                }
                else if( dy < 0 )
                {
                    return Direction8.Up;
                }
                else if( dy == 0 )
                {
                    return Direction8.None;
                }
            }
            else if( dx > 0 )
            {
                if( dy > 0 )
                {
                    return Direction8.RightDown;
                }
                else if( dy < 0 )
                {
                    return Direction8.RightUp;
                }
                else if( dy == 0 )
                {
                    return Direction8.Right;
                }
            }
            else if( dx < 0 )
            {
                if( dy > 0 )
                {
                    return Direction8.LeftDown;
                }
                else if( dy < 0 )
                {
                    return Direction8.LeftUp;
                }
                else if( dy == 0 )
                {
                    return Direction8.Left;
                }
            }

            return Direction8.None;
        }

        #endregion

        #region GetDirToNext

        /// <summary>
        /// Gets the direction from the given current tile to the next tile in this TilePath.
        /// </summary>
        /// <param name="currentX">
        /// The current x position in tile-space.
        /// </param>
        /// <param name="currentY">
        /// The current y position in tile-space.
        /// </param>
        /// <param name="pathLocation">
        /// The current position in the path.
        /// </param>
        /// <returns>
        /// The direction from the current tile to the next.
        /// </returns>
        public Direction8 GetDirToNext( int currentX, int currentY, int pathLocation )
        {
            if( this.path == null || pathLocation + 1 >= this.path.Length || pathLocation < 0 )
                return Direction8.None;

            Point2 next = this.path[pathLocation + 1];
            int dx = next.X - currentX;
            int dy = next.Y - currentY;

            if( dx == 0 )
            {
                if( dy > 0 )
                {
                    return Direction8.Down;
                }
                else if( dy < 0 )
                {
                    return Direction8.Up;
                }
                else if( dy == 0 )
                {
                    return Direction8.None;
                }
            }
            else if( dx > 0 )
            {
                if( dy > 0 )
                {
                    return Direction8.RightDown;
                }
                else if( dy < 0 )
                {
                    return Direction8.RightUp;
                }
                else if( dy == 0 )
                {
                    return Direction8.Right;
                }
            }
            else if( dx < 0 )
            {
                if( dy > 0 )
                {
                    return Direction8.LeftDown;
                }
                else if( dy < 0 )
                {
                    return Direction8.LeftUp;
                }
                else if( dy == 0 )
                {
                    return Direction8.Left;
                }
            }

            return Direction8.None;
        }

        #endregion

        /// <summary>
        /// Reverses the path.
        /// </summary>
        public void ReversePath()
        {
            if( this.path != null )
            {
                Array.Reverse( this.path );
            }
        }

        /// <summary>
        /// Gets the underlying path array.
        /// </summary>
        /// <returns>
        /// The reference of the underlying path data array.
        /// </returns>
        public Point2[] GetPath()
        {
            return this.path;
        }

        #endregion

        #region [ Fields ]

        /// <summary> 
        /// The underlying path. 
        /// </summary>
        private readonly Point2[] path;

        /// <summary>
        /// The layer that contains the path.
        /// </summary>
        private readonly TileMapDataLayer layer;

        /// <summary>
        /// The state of the path.
        /// </summary>
        private readonly TilePathState state;

        #endregion
    }
}
