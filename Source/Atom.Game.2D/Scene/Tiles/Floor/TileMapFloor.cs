// <copyright file="TileMapFloor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Tiles.TileMapFloor class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene.Tiles
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Defines a single floor in a <see cref="TileMap"/>.
    /// </summary>
    /// <remarks> 
    /// <para>
    ///     A floor consists of multiple <see cref="TileMapDataLayer"/>s 
    ///     and one single Action-<see cref="TileMapDataLayer"/>
    /// </para>
    /// <para>
    ///     The entities of a floor are usually rendered 
    ///     above all the layers of the floor.
    /// </para>
    /// </remarks>
    public partial class TileMapFloor : ITileMapProvider
    {
        /// <summary>
        /// Gets a direct reference to the list of layers of this <see cref="TileMapFloor"/>.
        /// WARNING: Don't modify this list directly.
        /// </summary>
        public List<TileMapDataLayer> Layers
        {
            get 
            {
                return this.layers; 
            }
        }

        /// <summary>
        /// Gets or sets the action layer of this <see cref="TileMapFloor"/>.
        /// </summary>
        /// <value>
        /// The action layer contains non-graphical data, such as collision information,
        /// for every tile on this TileMapFloor.
        /// </value>
        public TileMapDataLayer ActionLayer
        {
            get
            {
                return this.actionLayer;
            }

            set
            {
                this.actionLayer = value;

                if( this.actionLayer != null )
                {
                    this.actionLayer.Floor = this;
                }
            }
        }

        /// <summary>
        /// Gets the floor number of this <see cref="TileMapFloor"/>.
        /// </summary>
        /// <value>
        /// The floor number can be used to index into the floors 
        /// of the <see cref="TileMap"/> that owns this TileMapFloor.
        /// </value>
        public int FloorNumber
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the <see cref="TileMap"/> that owns this <see cref="TileMapFloor"/>.
        /// </summary>
        /// <value>The <see cref="TileMap"/> that owns this <see cref="TileMapFloor"/>.</value>
        public TileMap Map
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the (user-setable) tag of this <see cref="TileMapFloor"/>.
        /// </summary>
        /// <value>The dafault value is null.</value>
        public object Tag
        {
            get;
            set;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TileMapFloor"/> class.
        /// </summary>
        /// <param name="map">
        /// The <see cref="TileMap"/> that owns the new TileMapFloor.
        /// </param>
        /// <param name="initialLayerCapacity">
        /// The initial number of layers the TileMapFloor can have without having to reallocate memory.
        /// </param>
        protected TileMapFloor( TileMap map, int initialLayerCapacity )
        {
            Contract.Requires<ArgumentNullException>( map != null );

            this.Map = map;
            this.layers = new List<TileMapDataLayer>( initialLayerCapacity );
        }
        
        /// <summary>
        /// Creates a new instance of the <see cref="TileMapFloor"/> class.
        /// </summary>
        /// <param name="map">
        /// The <see cref="TileMap"/> that owns the new TileMapFloor.
        /// </param>
        /// <param name="initialLayerCapacity">
        /// The initial number of layers the TileMapFloor can have without having to reallocate memory.
        /// </param>
        /// <returns>The newly created TileMapFloor.</returns>
        internal static TileMapFloor Create( TileMap map, int initialLayerCapacity )
        {
            return new TileMapFloor( map, initialLayerCapacity );
        }

        /// <summary>
        /// Adds the given <see cref="TileMapDataLayer"/> to this <see cref="TileMapFloor"/>.
        /// </summary>
        /// <param name="layer">
        /// The layer to add to the TileMapFloor.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="layer"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the size/tilesize of the TileMapDataLayer doesn't match the size 
        /// of the TileMap of this TileMapFloor.
        /// </exception>
        public void AddLayer( TileMapDataLayer layer )
        {
            Contract.Requires<ArgumentNullException>( layer != null );
            Contract.Requires<ArgumentException>( layer.Width == this.Map.Width );
            Contract.Requires<ArgumentException>( layer.Height == this.Map.Height );
            Contract.Requires<ArgumentException>( layer.TileSize == this.Map.TileSize );

            this.layers.Add( layer );
        }

        /// <summary>
        /// Removes the first occurence of the given <see cref="TileMapDataLayer"/> from this <see cref="TileMapFloor"/>.
        /// </summary>
        /// <param name="layer">
        /// The TileMapDataLayer to remove.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the <paramref name="layer"/> has been removed;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool RemoveLayer( TileMapDataLayer layer )
        {
            return this.layers.Remove( layer );
        }

        /// <summary>
        /// Gets the TileMapDataLayer at the given zero-based <paramref name="layerIndex"/>.
        /// </summary>
        /// <param name="layerIndex">
        /// The zero-based index of the TileMapDataLayer to get.
        /// </param>
        /// <returns>
        /// The TileMapDataLayer at the requested index;
        /// otherwise null if the given layerIndex is invalid.
        /// </returns>
        public TileMapDataLayer GetLayer( int layerIndex )
        {
            if( layerIndex < 0 || layerIndex >= this.layers.Count )
                return null;

            return this.layers[layerIndex];
        }
                
        /// <summary>
        /// The action layer of this <see cref="TileMapFloor"/>.
        /// </summary>
        private TileMapDataLayer actionLayer;

        /// <summary>
        /// The collection of (graphical) layers.
        /// </summary>
        private readonly List<TileMapDataLayer> layers;
    }
}
