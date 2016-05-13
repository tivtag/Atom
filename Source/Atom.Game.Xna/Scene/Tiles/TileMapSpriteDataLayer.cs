// <copyright file="TileMapSpriteDataLayer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Tiles.TileMapSpriteDataLayer class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene.Tiles
{
    using System;
    using System.Diagnostics;
    using Atom.Xna;

    /// <summary>
    /// Represents a <see cref="TileMapDataLayer"/> that is visualized using a <see cref="SpriteSheet"/>.
    /// </summary>
    public class TileMapSpriteDataLayer : TileMapDataLayer
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the <see cref="SpriteSheet"/> that is used 
        /// to convert the interal data stored in this <see cref="TileMapSpriteDataLayer"/>
        /// into drawable <see cref="ISprite"/>s.
        /// </summary>
        /// <value>The default value is null.</value>
        public SpriteSheet Sheet
        {
            get
            {
                return this.sheet;
            }

            set
            {
                this.sheet = value;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMapSpriteDataLayer"/> class.
        /// </summary>
        public TileMapSpriteDataLayer()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMapSpriteDataLayer"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the new TileMapSpriteDataLayer.
        /// </param>
        /// <param name="floor">
        /// The floor that owns the new TileMapSpriteDataLayer.
        /// </param>
        /// <param name="width">
        /// The width of the new TileMapSpriteDataLayer (in tile-space).
        /// </param>
        /// <param name="height">
        /// The height of the new TileMapSpriteDataLayer (in tile-space).
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="floor"/> is null.
        /// </exception>
        public TileMapSpriteDataLayer( string name, TileMapFloor floor, int width, int height )
            : base( name, floor, width, height )
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Draws this TileMapSpriteDataLayer.
        /// </summary>
        /// <param name="scrollX">The scroll value on the x-axis.</param>
        /// <param name="scrollY">The scroll value on the y-axis.</param>
        /// <param name="screenW">The width of the viewable area.</param>
        /// <param name="screenH">The height of the viewable area.</param>
        /// <param name="drawContext">
        /// The current draw context.
        /// </param>
        public void Draw( int scrollX, int scrollY, int screenW, int screenH, ISpriteDrawContext drawContext )
        {
            this.DrawOffset( scrollX, scrollY, screenW, screenH, 0, 0, drawContext );
        }

        /// <summary>
        /// Draws thisTileMapSpriteDataLayer, with the specified offset.
        /// </summary>
        /// <param name="scrollX">The scroll value on the x-axis.</param>
        /// <param name="scrollY">The scroll value on the y-axis.</param>
        /// <param name="screenW">The width of the viewable area.</param>
        /// <param name="screenH">The height of the viewable area.</param>
        /// <param name="offsetX">The drawing offset applied on the x-axis.</param>
        /// <param name="offsetY">The drawing offset applied on the y-axis.</param>
        /// <param name="drawContext">The current draw context.</param>
        public void DrawOffset(
            int scrollX,
            int scrollY,
            int screenW,
            int screenH,
            int offsetX,
            int offsetY,
            ISpriteDrawContext drawContext )
        {
            if( !this.IsVisible || this.sheet == null )
                return;

            var batch = drawContext.Batch;
            int tileSize = this.TileSize;

            int tileScrollY = scrollY % tileSize;
            int startX = 0 - (scrollX % tileSize);
            int dataStartX = (startX + scrollX) / tileSize;

            startX += offsetX;
            screenW += offsetX;
            screenH += offsetY;

            int width = this.Width;
            var data = this.Data;

            for( int y = offsetY - tileScrollY, dataY = (scrollY - tileScrollY) / tileSize; y < screenH; y += tileSize )
            {
                int row = dataY * width;

                for( int x = startX, dataX = dataStartX; x < screenW; x += tileSize )
                {
                    ISprite sprite = this.sheet[data[dataX + row]];

                    if( sprite != null )
                    {
                        sprite.Draw( x, y, batch );
                    }

                    ++dataX;
                }

                ++dataY;
            }
        }
        
        /// <summary>
        /// Serializes this TileMapSpriteDataLayer.
        /// </summary>
        /// <param name="context">
        /// The context that provides everything required for the serialization process.
        /// </param>
        protected override void Serialize( Atom.Storage.ISerializationContext context )
        {
            base.Serialize( context );

            const int CurrentVersion = 0;
            context.Write( CurrentVersion );
            context.Write( this.sheet != null ? this.sheet.Name : string.Empty );
        }

        /// <summary>
        /// Deserializes this TileMapSpriteDataLayer.
        /// </summary>
        /// <param name="context">
        /// The context that provides everything required for the deserialization process.
        /// </param>
        protected override void Deserialize( Atom.Storage.IDeserializationContext context )
        {
            base.Deserialize( context );

            const int CurrentVersion = 0;
            int version = context.ReadInt32();
            Debug.Assert( version == CurrentVersion, "An invalid version has been readen." );

            string sheetName = context.ReadString();

            if( sheetName.Length != 0 )
            {
                this.LoadSpriteSheet( sheetName, context );
            }
            else
            {
                this.sheet = null;
            }
        }

        /// <summary>
        /// Loads the SpriteSheet of this TileMapSpriteDataLayer.
        /// </summary>
        /// <param name="sheetName">
        /// The name of the SpriteSheet to load.
        /// </param>
        /// <param name="context">
        /// The context that provides everything required for the deserialization process.
        /// </param>
        private void LoadSpriteSheet( string sheetName, Atom.Storage.IDeserializationContext context )
        {
            var serviceProvider = context as IServiceProvider;

            if( serviceProvider == null )
            {
                throw new ArgumentException(
                    "The specified IDeserializationContext is required to be an IServiceProvider.",
                    "context"
                );
            }

            var sheetLoader = serviceProvider.GetService<ISpriteSheetLoader>();

            if( sheetLoader == null )
            {
                throw new ServiceNotFoundException( typeof( ISpriteSheetLoader ) );
            }

            this.sheet = (SpriteSheet)sheetLoader.LoadSpriteSheet( sheetName );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The <see cref="SpriteSheet"/> that is used to convert 
        /// the interal data stored in this <see cref="TileMapDataLayer"/>
        /// into drawable <see cref="ISprite"/>s.
        /// </summary>
        private SpriteSheet sheet;

        #endregion
    }
}