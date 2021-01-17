// <copyright file="TileMap.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Tiles.TileMap class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Scene.Tiles
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;

    /// <summary>
    /// Implements a multi-floor multi-layered tile map
    /// which supports one action-layer per floor.
    /// </summary>
    public partial class TileMap
    {
        #region [ Constants ]

        /// <summary>
        /// Identifies an invalid tile.
        /// </summary>
        /// <value>The value is -1.</value>
        public const int InvalidTile = -1;

        #endregion

        #region [ Events ]

        /// <summary>
        /// Fired when a <see cref="TileMapFloor"/> has been added to or removed from this <see cref="TileMap"/>.
        /// </summary>
        public event SimpleEventHandler<TileMap> FloorsChanged;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the name of this <see cref="TileMap"/>.
        /// </summary>
        /// <value>
        /// The name that (should) uniquely identify this <see cref="TileMap"/>.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the width of this <see cref="TileMap"/> (in tile-space).
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
        /// Gets the height of this <see cref="TileMap"/> (in tile-space).
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
        /// Gets the size of this <see cref="TileMap"/> (in tile-space).
        /// </summary>
        public Point2 Size
        {
            get
            {
                return new Point2( this.width, this.height );
            }
        }

        /// <summary>
        /// Gets the size of this <see cref="TileMap"/> in pixels.
        /// </summary>
        public Point2 SizeInPixels
        {
            get
            {
                return this.Size * tileSize;
            }
        }

        /// <summary>
        /// Gets or sets the size of the tiles in this <see cref="TileMap"/>.
        /// </summary>
        /// <value>The default value is 16.</value>
        public int TileSize
        {
            get
            {
                return this.tileSize;
            }

            set
            {
                Contract.Requires<ArgumentException>( value > 0 );

                this.tileSize = value;
            }
        }

        /// <summary>
        /// Gets a direct reference to the list that contains the <see cref="TileMapFloor"/>s of this <see cref="TileMap"/>.
        /// Warning: Don't modify this list directly.
        /// </summary>
        public List<TileMapFloor> Floors
        {
            get 
            {
                return this.floors;
            }
        }

        /// <summary>
        /// Gets a value that indicates how many <see cref="TileMapFloor"/>s this <see cref="TileMap"/> contains.
        /// </summary>
        /// <value>The numbers of floors in this TileMap.</value>
        public int FloorCount
        {
            get 
            { 
                return this.floors.Count; 
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMap"/> class.
        /// </summary>
        public TileMap()
            : this( 0, 0, 16, 0 )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMap"/> class.
        /// </summary>
        /// <param name="tileSize">The size of a single tile in pixels of the new TileMap.</param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="tileSize"/> is less or equals zero.
        /// </exception>
        public TileMap( int tileSize )
            : this( 0, 0, tileSize, 0 )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMap"/> class.
        /// </summary>
        /// <param name="width">The width of the new TileMap (in tile-space).</param>
        /// <param name="height">The height of the new TileMap (in tile-space).</param>
        /// <param name="tileSize">The size of a single tile in pixels of the new TileMap.</param>
        /// <param name="initialFloorCapacity">
        /// The initial number of floors the new TileMap can hold without having to reallocate memory.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="tileSize"/> is less or equals zero.
        /// </exception>
        public TileMap( int width, int height, int tileSize, int initialFloorCapacity )
        {
            Contract.Requires<ArgumentException>( tileSize > 0 );

            this.width = width;
            this.height = height;
            this.tileSize = tileSize;

            this.floors  = new List<TileMapFloor>( initialFloorCapacity );
        }

        #endregion

        #region [ Methods ]

        #region > Organisation <

        /// <summary>
        /// Adds a new <see cref="TileMapFloor"/> to this <see cref="TileMap"/>.
        /// </summary>
        /// <param name="initialLayerCapacity">
        /// The initial number of layers the new <see cref="TileMapFloor"/>
        /// can hold.
        /// </param>
        /// <returns>
        /// The newly added floor.
        /// </returns>
        public TileMapFloor AddFloor( int initialLayerCapacity )
        {
            var floor = this.CreateFloor( initialLayerCapacity );

            floor.FloorNumber = floors.Count;
            floors.Add( floor );

            this.OnFloorsChanged();
            return floor;
        }

        /// <summary>
        /// Removes the <see cref="TileMapFloor"/> at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to remove.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// index is less than 0.-or-index is equal to or greater than the FloorCount.
        /// </exception>
        public void RemoveFloor( int index )
        {
            this.floors.RemoveAt( index );

            // Refresh floor numbers:
            for( int i = index; i < this.floors.Count; ++i )
            {
                this.floors[i].FloorNumber = i;
            }

            this.OnFloorsChanged();
        }

        /// <summary>
        /// Tries to remove the specified <see cref="TileMapFloor"/> from this TileMap.
        /// </summary>
        /// <param name="floor">
        /// The TileMapFloor to remove.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the given TileMapFloor has been removed from this TileMap;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool RemoveFloor( TileMapFloor floor )
        {
            if( !this.floors.Remove( floor ) )
                return false;

            // Refresh floor numbers:
            for( int i = 0; i < this.floors.Count; ++i )
            {
                this.floors[i].FloorNumber = i;
            }

            this.OnFloorsChanged();
            return true;
        }

        /// <summary>
        /// Receives the <see cref="TileMapFloor"/> of this <see cref="TileMap"/>
        /// that has the given <paramref name="floorNumber"/>.
        /// </summary>
        /// <param name="floorNumber">
        /// The floor number of the Floor to receive.
        /// </param>
        /// <returns>
        /// The TileMapFloor at the given floorNumber;
        /// or null if there exists no TileMapFloor at the given floorNumber.
        /// </returns>
        public TileMapFloor GetFloor( int floorNumber )
        {
            if( floorNumber < 0 || floorNumber >= this.floors.Count )
                return null;

            // The floors array is sorted by Floor Number.
            return this.floors[floorNumber];
        }

        /// <summary>
        /// Creates a new <see cref="TileMapFloor"/> for this <see cref="TileMap"/>.
        /// </summary>
        /// <param name="initialLayerCapacity">
        /// The initial number of layers the new <see cref="TileMapFloor"/>
        /// can hold.
        /// </param>
        /// <returns>
        /// The newly added floor.
        /// </returns>
        protected virtual TileMapFloor CreateFloor( int initialLayerCapacity )
        {
            return TileMapFloor.Create( this, initialLayerCapacity );
        }

        #endregion

        /// <summary>
        /// Changes tiles of a specific floor/layer in this TileMap. Two tiles are changed in one pass.
        /// </summary>
        /// <param name="floorNumber">
        /// The number of the floor.
        /// </param>
        /// <param name="layerIndex">
        /// The index of the layer.
        /// </param>
        /// <param name="sourceTile">
        /// The id of the tile to search in the layer.
        /// </param>
        /// <param name="targetTile">
        /// The id to change the tile to.
        /// </param>
        /// <param name="targetActionTile">
        /// The if to change the action tile of the tile to.
        /// </param>
        public void ChangeTiles( int floorNumber, int layerIndex, int sourceTile, int targetTile, int targetActionTile )
        {
            var floor = GetFloor( floorNumber );
            if( floor == null )
                throw new ArgumentOutOfRangeException( "floorNumber", floorNumber, "floorNumber is invalid." );

            var layer = floor.Layers[layerIndex];
            if( layer == null )
                throw new ArgumentOutOfRangeException( "layerIndex", layerIndex, "layerIndex is invalid." );

            var actionLayer = floor.ActionLayer;

            for( int x = 0; x < layer.Width; ++x )
            {
                for( int y = 0; y < layer.Height; ++y )
                {
                    int id = layer.GetTileAt( x, y );

                    if( id == sourceTile )
                    {
                        layer.SetTile( x, y, targetTile );
                        actionLayer.SetTile( x, y, targetActionTile );
                    }
                }
            }
        }

        /// <summary>
        /// Changes tiles of a specific floor/layer in this TileMap. Two tiles are changed in one pass.
        /// </summary>
        /// <param name="floorNumber">
        /// The number of the floor.
        /// </param>
        /// <param name="layerIndex">
        /// The index of the layer.
        /// </param>
        /// <param name="sourceTileA">
        /// The id of the first tile to search in the layer.
        /// </param>
        /// <param name="targetTileA">
        /// The id to change the first tile to.
        /// </param>
        /// <param name="targetActionTileA">
        /// The if to change the action tile of the first tile to.
        /// </param>
        /// <param name="sourceTileB">
        /// The id of the second tile to search in the layer.
        /// </param>
        /// <param name="targetTileB">
        /// The id to change the second tile to.
        /// </param>
        /// <param name="targetActionTileB">
        /// The if to change the action tile of the second tile to.
        /// </param>
        public void ChangeTiles( int floorNumber, int layerIndex, int sourceTileA, int targetTileA, int targetActionTileA, int sourceTileB, int targetTileB, int targetActionTileB )
        {
            var floor = GetFloor( floorNumber );
            if( floor == null )
                throw new ArgumentOutOfRangeException( "floorNumber", floorNumber, "floorNumber is invalid." );

            var layer = floor.Layers[layerIndex];
            if( layer == null )
                throw new ArgumentOutOfRangeException( "layerIndex", layerIndex, "layerIndex is invalid." );

            var actionLayer = floor.ActionLayer;

            for( int x = 0; x < layer.Width; ++x )
            {
                for( int y = 0; y < layer.Height; ++y )
                {
                    int id = layer.GetTileAt( x, y );

                    if( id == sourceTileA )
                    {
                        layer.SetTile( x, y, targetTileA );
                        actionLayer.SetTile( x, y, targetActionTileA );
                    }
                    else if( id == sourceTileB )
                    {
                        layer.SetTile( x, y, targetTileB );
                        actionLayer.SetTile( x, y, targetActionTileB );
                    }
                }
            }
        }

        /// <summary>
        /// Fires the <see cref="FloorsChanged"/> event.
        /// </summary>
        private void OnFloorsChanged()
        {
            if( this.FloorsChanged != null )
            {
                this.FloorsChanged( this );
            }
        }

        /// <summary>
        /// Limits the given scroll value to be inside the map.
        /// </summary>
        /// <param name="scroll">The scroll value to limit.</param>
        /// <param name="mapSize">The size of the map in pixels.</param>
        /// <param name="screenSize">The size of the viewable area.</param>
        public static void LimitScroll( ref Vector2 scroll, Vector2 mapSize, Vector2 screenSize )
        {
            if( scroll.X < 0.0f )
            {
                scroll.X = 0.0f;
            }
            else if( scroll.X + screenSize.X > mapSize.X )
            {
                scroll.X = mapSize.X - screenSize.X;
            }

            if( scroll.Y < 0.0f )
            {
                scroll.Y = 0.0f;
            }
            else if( scroll.Y + screenSize.Y > mapSize.Y )
            {
                scroll.Y = mapSize.Y - screenSize.Y;
            }
        }
        
        /// <summary>
        /// Saves the specified <see cref="TileMap"/> using the default <see cref="TileMap.ReaderWriter"/>.
        /// </summary>
        /// <param name="map">
        /// The TileMap to serialize.
        /// </param>
        /// <param name="context">
        /// The context under which the serialization process should occur.
        /// </param>
        public static void Save( TileMap map, Atom.Storage.ISerializationContext context )
        {
            var writer = new TileMap.ReaderWriter( new TileMapFloor.ReaderWriter( TypeActivator.Instance ) );
            writer.Serialize( map, context );
        }

        /// <summary>
        /// Loads the specified <see cref="TileMap"/> using the default <see cref="TileMap.ReaderWriter"/>.
        /// </summary>
        /// <param name="map">
        /// The TileMap to deserialize.
        /// </param>
        /// <param name="context">
        /// The context under which the deserialization process should occur.
        /// </param>
        /// <param name="typeActivator">
        /// Responsible for creating the <see cref="TileMapDataLayer"/> objects serialized in a TileMapFloor.
        /// </param>
        public static void Load( TileMap map, Atom.Storage.IDeserializationContext context, ITypeActivator typeActivator )
        {
            var writer = new TileMap.ReaderWriter( new TileMapFloor.ReaderWriter( typeActivator ) );
            writer.Deserialize( map, context );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The width of this <see cref="TileMap"/> (in tile-space).
        /// </summary>
        private int width;

        /// <summary>
        /// The height of this <see cref="TileMap"/> (in tile-space).
        /// </summary>
        private int height;

        /// <summary>
        /// The size of the tiles in this <see cref="TileMap"/>.
        /// </summary>
        private int tileSize = 16;

        /// <summary>
        /// The collection of floors of this <see cref="TileMap"/>.
        /// </summary>
        private readonly List<TileMapFloor> floors;

        #endregion
    }
}
