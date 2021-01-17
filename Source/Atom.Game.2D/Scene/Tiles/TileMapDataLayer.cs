// <copyright file="TileMapDataLayer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Tiles.TileMapDataLayer class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Scene.Tiles
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;

    /// <summary>
    /// Defines a basic data layer of a <see cref="TileMapFloor"/> of a <see cref="TileMap"/>.
    /// </summary>
    public partial class TileMapDataLayer : ITileMapProvider
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the name of this <see cref="TileMapDataLayer"/>.
        /// </summary>
        /// <value>
        /// The name that (should) uniquely identify this <see cref="TileMapDataLayer"/>.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object where the user can store any optional data. 
        /// Is not serialized.
        /// </summary>
        /// <value>The default value is null.</value>
        public object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that indicates what kind of data 
        /// is stored in this <see cref="TileMapDataLayer"/>.
        /// </summary>
        /// <value>The default value is 0.</value>
        public int TypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TileMapDataLayer"/> is visible.
        /// </summary>
        /// <value>
        /// The default value is <see langword="true"/>.
        /// </value>
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }

            set 
            { 
                this.isVisible = value; 
            }
        }

        /// <summary>
        /// Gets the <see cref="TileMapFloor"/> this <see cref="TileMapDataLayer"/> is part of.
        /// </summary>
        /// <value>The <see cref="TileMapFloor"/> this TileMapDataLayer is part of.</value>
        public TileMapFloor Floor
        {
            get
            {
                return this.floor;
            }

            internal set
            {
                Contract.Requires<ArgumentNullException>( value != null );

                this.floor = value;
                this.tileSize = floor.Map.TileSize;
            }
        }

        /// <summary>
        /// Gets the <see cref="TileMap"/> this <see cref="TileMapDataLayer"/> is part of.
        /// </summary>
        /// <value>The <see cref="TileMap"/> this TileMapDataLayer is part of.</value>
        public TileMap Map
        {
            get
            {
                return this.Floor.Map;
            }
        }

        /// <summary>
        /// Gets the size of the tiles in this <see cref="TileMapDataLayer"/>.
        /// </summary>
        /// <value>The default value is 16.</value>
        public int TileSize
        {
            get 
            { 
                return this.tileSize;
            }
        }

        /// <summary>
        /// Gets the width of this <see cref="TileMapDataLayer"/> (in tile-space).
        /// </summary>
        /// <value>The number of tiles from the left to the right.</value>
        public int Width
        {
            get 
            {
                return this.width;
            }
        }

        /// <summary>
        /// Gets the height of this <see cref="TileMapDataLayer"/> (in tile-space).
        /// </summary>
        /// <value>The number of tiles from the top to the bottom.</value>
        public int Height
        {
            get 
            {
                return this.height;
            }
        }

        /// <summary>
        /// Gets the data array of this <see cref="TileMapDataLayer"/>.
        /// </summary>
        /// <value>
        /// The array is returned directly; without copying the data.
        /// </value>
        protected int[] Data
        {
            get
            { 
                return this.data;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMapDataLayer"/> class.
        /// </summary>
        public TileMapDataLayer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMapDataLayer"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the new TileMapDataLayer.
        /// </param>
        /// <param name="floor">
        /// The floor that owns the new TileMapDataLayer.
        /// </param>
        /// <param name="width">
        /// The width of the new TileMapDataLayer (in tile-space).
        /// </param>
        /// <param name="height">
        /// The height of the new TileMapDataLayer (in tile-space).
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="floor"/> is null.
        /// </exception>
        public TileMapDataLayer( string name, TileMapFloor floor, int width, int height )
        {
            Contract.Requires<ArgumentNullException>( floor != null );

            this.Name  = name;
            this.Floor = floor;
            this.data  = new int[width * height];

            this.width    = width;
            this.height   = height;
            this.tileSize = floor.Map.TileSize;
        }

        #endregion

        #region [ Methods ]

        #region SetTile

        /// <summary> 
        /// Sets the tile at the given position.
        /// </summary>
        /// <param name="x"> The X coordinate of the tile in tile space. </param>
        /// <param name="y"> The Y coordinate of the tile in tile space. </param>
        /// <param name="newIndex"> The index to set to. </param>
        public void SetTile( int x, int y, int newIndex )
        {
            this.data[x + (y * width)] = newIndex;
        }

        /// <summary> 
        /// Sets the tile at the given position.
        /// </summary>
        /// <param name="x"> X coordinate of the tile in tile space. </param>
        /// <param name="y"> Y coordinate of the tile in tile space. </param>
        /// <param name="newIndex"> The index to set to. </param>
        /// <returns>
        /// Whether the tile could be set; false if out of valid range.
        /// </returns>
        public bool TrySetTile( int x, int y, int newIndex )
        {
            int i = x + (y * width);
            if( i < 0 || i >= data.Length )
                return false;

            this.data[i] = newIndex;
            return true;
        }

        #endregion

        #region GetTileAt

        /// <summary>
        /// Receives the tile at the specified position.
        /// </summary>
        /// <param name="x">The X-coordinate of the tile to receive (in tile space).</param>
        /// <param name="y">The Y-coordinate of the tile to receive (in tile space).</param>
        /// <returns> 
        /// The tile at the given indices.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// If the given input indices are out of valid range.
        /// </exception>
        public int GetTileAt( int x, int y )
        {
            int i = x + (y * width);
            return this.data[i];
        }

        /// <summary>
        /// Receives the tile at the specified position.
        /// </summary>
        /// <param name="x">The X-coordinate of the tile to receive (in tile space).</param>
        /// <param name="y">The Y-coordinate of the tile to receive (in tile space).</param>
        /// <returns> 
        /// The tile or <see cref="TileMap.InvalidTile"/> if the given indices are out of valid range.
        /// </returns>
        public int GetTileAtSafe( int x, int y )
        {
            int i = x + (y * width);
            if( i < 0 || i >= data.Length )
                return TileMap.InvalidTile;

            return this.data[i];
        }

        /// <summary> 
        /// Receives the tile at the specified position.
        /// </summary>
        /// <param name="x">The X-coordinate of the tile to receive (in tile space).</param>
        /// <param name="y">The Y-coordinate of the tile to receive (in tile space).</param>
        /// <param name="tileId">
        /// Will contain the tile at the specified position.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the tile could be sucessfully received;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool TryGetTileAt( int x, int y, out int tileId )
        {
            int i = x + (y * width);
            if( i < 0 || i >= data.Length )
            {
                tileId = TileMap.InvalidTile;
                return false;
            }

            tileId = this.data[i];
            return true;
        }

        #endregion

        #region Fill

        /// <summary>
        /// Fills all entries in this TileMapDataLayer with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public void Fill( int value )
        {
            for( int i = 0; i < this.data.Length; ++i )
            {
                this.data[i] = value;
            }
        }

        #endregion

        #region RefreshTileSize

        /// <summary>
        /// Refreshes the tile size stored within this <see cref="TileMapDataLayer"/>.
        /// </summary>
        public void RefreshTileSize()
        {
            this.tileSize = this.Floor.Map.TileSize;
        }

        #endregion

        /// <summary>
        /// Casts a ray against this TileMapDatalayer.
        /// </summary>
        /// <typeparam name="TCallerType">
        /// The type of the object that is walking on the line.
        /// </typeparam>
        /// <param name="ray">
        /// The ray to walk on.
        /// </param>
        /// <param name="totalLength">
        /// The maximum length to walk on the ray.
        /// </param>
        /// <param name="stepSize">
        /// The length of a single step on the ray.
        /// </param>
        /// <param name="idHandler">
        /// The handler that is used to decide.
        /// </param>
        /// <param name="caller">
        /// The object that is walking on the ray.
        /// </param>
        /// <returns>
        /// The length that could be walked on the ray.
        /// </returns>
        public float RayWalk<TCallerType>( Ray2 ray, float totalLength, float stepSize, ITileHandler<TCallerType> idHandler, TCallerType caller )
        {
            float length = 0.0f;

            while( length <= totalLength )
            {
                Vector2 point = ray.GetPointOnRay( length );

                int id = this.GetTileAtSafe(
                    (int)(point.X / this.tileSize),
                    (int)(point.Y / this.tileSize)
                );

                if( !idHandler.IsWalkable( id, caller ) )
                {
                    return length;
                }

                length += stepSize;
            }

            return totalLength;
        }

        #region > Collision Tests <

        #region - TestCollisionVertical -

        /// <summary>
        /// Tests for vertical collision against this <see cref="TileMapDataLayer"/>.
        /// </summary>
        /// <param name="x">
        /// The position of the object on the x-axis.
        /// </param>
        /// <param name="y">
        /// The position of the object on the y-axis.
        /// </param>
        /// <param name="height">
        /// The height of the object.
        /// </param>
        /// <param name="velocityX"> 
        /// The celocity of the object on the x-axis.
        /// </param>
        /// <param name="tileCoordinateX">
        /// This value will contain the end position after moving on the x-axis in tile-space.</param>
        /// <param name="tileCoordinateY">
        /// This value will contain the end position after moving on the y-axis in tile-space.
        /// </param>
        /// <param name="idToTest">
        /// The id to test against.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the it will intersect with the specified <paramref name="idToTest"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool TestCollisionVertical(
            int x,
            int y,
            int height,
            float velocityX,
            out int tileCoordinateX,
            out int tileCoordinateY,
            int idToTest )
        {
            int tileYpixels = y - (y % tileSize);
            int testEnd     = y + height;

            tileCoordinateX = ((int)(x + velocityX)) / tileSize;
            tileCoordinateY = tileYpixels / tileSize;

            while( tileYpixels <= testEnd )
            {
                int id = this.GetTileAtSafe( tileCoordinateX, tileCoordinateY );
                if( id == idToTest )
                    return true;

                ++tileCoordinateY;
                tileYpixels += tileSize;
            }

            return false;
        }

        /// <summary>
        /// Tests for vertical collision against this <see cref="TileMapDataLayer"/>.
        /// </summary>
        /// <typeparam name="TCallerType">The type of object that is tested against the map.</typeparam>
        /// <param name="x">
        /// The position of the object on the x-axis.
        /// </param>
        /// <param name="y">
        /// The position of the object on the y-axis.
        /// </param>
        /// <param name="height">
        /// The height of the object.
        /// </param>
        /// <param name="velocityX">
        /// The celocity of the object on the x-axis.
        /// </param>
        /// <param name="tileCoordinateX">
        /// This value will contain the end position after moving on the x-axis in tile-space.
        /// </param>
        /// <param name="tileCoordinateY">
        /// This value will contain the end position after moving on the y-axis in tile-space.
        /// </param>
        /// <param name="idHandler">
        /// Contains the method which handles the ids. </param>
        /// <param name="caller">
        /// The object whos collesion borders are tested in this method.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the <paramref name="idHandler"/> told the algorithm to stop;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool TestCollisionVertical<TCallerType>(
            int x,
            int y,
            int height,
            float velocityX,
            out int tileCoordinateX,
            out int tileCoordinateY,
            ITileHandler<TCallerType> idHandler,
            TCallerType caller )
        {
            int tileYpixels = y - (y % tileSize);
            int testEnd = y + height;

            tileCoordinateX = ((int)(x + velocityX)) / tileSize;
            tileCoordinateY = tileYpixels / tileSize;

            while( tileYpixels <= testEnd )
            {
                int id = this.GetTileAtSafe( tileCoordinateX, tileCoordinateY );
                if( idHandler.Handle( tileCoordinateX, tileCoordinateY, id, caller ) )
                    return true;

                ++tileCoordinateY;
                tileYpixels += tileSize;
            }

            return false;
        }

        #endregion

        #region - TestCollisionHorizontal -

        /// <summary>
        /// Tests for horizontal collision against this <see cref="TileMapDataLayer"/>.
        /// </summary>
        /// <param name="x">
        /// The position of the object on the x-axis.
        /// </param>
        /// <param name="y">
        /// The position of the object on the y-axis.
        /// </param>
        /// <param name="width">
        /// The width of the object.
        /// </param>
        /// <param name="tileCoordinateX">
        /// This value will contain the end position after moving on the x-axis in tile-space. </param>
        /// <param name="tileCoordinateY">
        /// This value will contain the end position after moving on the y-axis in tile-space. </param>
        /// <param name="idToTest">
        /// The id to test against.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the it will intersect with the specified <paramref name="idToTest"/>;
        /// otherwise false.
        /// </returns>
        public bool TestCollisionHorizontal(
            int x,
            int y,
            int width,
            out int tileCoordinateX,
            out int tileCoordinateY,
            int idToTest )
        {
            int tileXpixels = x - (x % tileSize);
            int testEnd     = x + width;

            tileCoordinateY = y / tileSize;
            tileCoordinateX = tileXpixels / tileSize;

            while( tileXpixels <= testEnd )
            {
                int id = this.GetTileAtSafe( tileCoordinateX, tileCoordinateY );

                if( id == idToTest )
                    return true;

                ++tileCoordinateX;
                tileXpixels += tileSize;
            }

            return false;
        }

        /// <summary>
        /// Tests for horizontal collision against this <see cref="TileMapDataLayer"/>.
        /// </summary>
        /// <typeparam name="TCallerType">
        /// The type of object that is tested against the layer.
        /// </typeparam>
        /// <param name="x">
        /// The position of the object on the x-axis.
        /// </param>
        /// <param name="y">
        /// The position of the object on the y-axis.
        /// </param>
        /// <param name="width">
        /// The width of the object.
        /// </param>
        /// <param name="tileCoordinateX">
        /// This value will contain the end position after moving on the x-axis in tile-space. </param>
        /// <param name="tileCoordinateY">
        /// This value will contain the end position after moving on the y-axis in tile-space. </param>
        /// <param name="idHandler">
        /// Contains the method which handles the ids.
        /// </param>
        /// <param name="caller">
        /// The object whos collesion borders are tested in this method.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the <paramref name="idHandler"/> told the algorithm to stop;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool TestCollisionHorizontal<TCallerType>(
            int x,
            int y,
            int width,
            out int tileCoordinateX,
            out int tileCoordinateY,
            ITileHandler<TCallerType> idHandler,
            TCallerType caller )
        {
            int tileXpixels = x - (x % tileSize);
            int testEnd     = x + width;

            tileCoordinateY = y / tileSize;
            tileCoordinateX = tileXpixels / tileSize;

            while( tileXpixels <= testEnd )
            {
                int id = this.GetTileAtSafe( tileCoordinateX, tileCoordinateY );
                if( idHandler.Handle( tileCoordinateX, tileCoordinateY, id, caller ) )
                    return true;

                ++tileCoordinateX;
                tileXpixels += tileSize;
            }

            return false;
        }

        #endregion

        #endregion

        #region > Storage <

        /// <summary>
        /// Serializes this TileMapDataLayer.
        /// </summary>
        /// <param name="context">
        /// The context that provides everything required for the serialization process.
        /// </param>
        internal void SerializeCore( Atom.Storage.ISerializationContext context )
        {
            this.Serialize( context );
        }

        /// <summary>
        /// Serializes this TileMapDataLayer.
        /// </summary>
        /// <param name="context">
        /// The context that provides everything required for the serialization process.
        /// </param>
        protected virtual void Serialize( Atom.Storage.ISerializationContext context )
        {
            context.Write( this.GetType().GetTypeName() );

            const int CurrentVersion = 0;
            context.Write( CurrentVersion );

            context.Write( this.Name ?? string.Empty );
            context.Write( TypeId );
            context.Write( isVisible );

            context.Write( this.width );
            context.Write( this.height );

            for( int i = 0; i < data.Length; ++i )
            {
                context.Write( data[i] );
            }
        }

        /// <summary>
        /// Deserializes this TileMapDataLayer.
        /// </summary>
        /// <param name="context">
        /// The context that provides everything required for the deserialization process.
        /// </param>
        internal void DeserializeCore( Atom.Storage.IDeserializationContext context )
        {
            this.Deserialize( context );
        }

        /// <summary>
        /// Deserializes this TileMapDataLayer.
        /// </summary>
        /// <param name="context">
        /// The context that provides everything required for the deserialization process.
        /// </param>
        protected virtual void Deserialize( Atom.Storage.IDeserializationContext context )
        {
            // The typename has been readen at this point.
            const int CurrentVersion = 0;
            int version = context.ReadInt32();
            System.Diagnostics.Debug.Assert( version == CurrentVersion, "An invalid version has been readen." );

            this.Name = context.ReadString();
            this.TypeId = context.ReadInt32();
            this.isVisible = context.ReadBoolean();

            this.width = context.ReadInt32();
            this.height = context.ReadInt32();

            this.data = new int[width * height];
            
            for( int i = 0; i < data.Length; ++i )
            {
                this.data[i] = context.ReadInt32();
            }
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The data of this TileMapDataLayer.
        /// </summary>
        private int[] data;

        /// <summary>
        /// The width of the TileMapDataLayer in tile-space.
        /// </summary>
        private int width;

        /// <summary>
        /// The height of the TileMapDataLayer in tile-space.
        /// </summary>
        private int height;

        /// <summary>
        /// The size of the tiles used in the TileMapDataLayer.
        /// </summary>
        private int tileSize;

        /// <summary>
        /// States whether this TileMapDataLayer is visible.
        /// </summary>
        private bool isVisible = true;

        /// <summary>
        /// Internal storage of the <see cref="Floor"/> property.
        /// </summary>
        private TileMapFloor floor;

        #endregion
    }
}
