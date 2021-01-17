// <copyright file="SpriteSheetDrawComponent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteSheetDrawComponent class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;
    using Atom.Xna.Batches;

    /// <summary>
    /// Implements rendering and input handling routines for an <see cref="ISpriteSheet"/>.
    /// </summary>
    /// <remarks>
    /// This component is mostly required for visualizers or editors.
    /// </remarks>
    public class SpriteSheetDrawComponent
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the <see cref="ISpriteSheet"/> that is visualized by this SpriteSheetDrawComponent.
        /// </summary>
        public ISpriteSheet Sheet
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position the <see cref="SpriteSheet"/> is draw at.
        /// </summary>
        public Vector2 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum number of sprites per row.
        /// </summary>
        /// <value>The default value is 10.</value>
        public int MaximumSpritesPerRow
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the set <see cref="SpriteSheet"/> is updated 
        /// automatically by this SpriteSheetDrawComponent.
        /// </summary>
        /// <value>
        /// The default value is 'true'.
        /// </value>
        public bool IsUpdatingSheet
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size on the x-axis of a single entry in the grid.
        /// </summary>
        /// <value>The default value is 16.</value>
        /// <remarks>
        /// It's assumed that all sprites in the set <see cref="SpriteSheet"/> have the same SpriteWidth.
        /// </remarks>
        public int SpriteWidth
        {
            get
            {
                return this.spriteWidth;
            }

            set
            {
                Contract.Requires<ArgumentException>( value > 0 );

                this.spriteWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the size on the y-axis of a single entry in the grid.
        /// </summary>
        /// <remarks>
        /// It's assumed that all sprites in the set <see cref="SpriteSheet"/> have the same SpriteHeight.
        /// </remarks>
        /// <value>The default value is 16.</value>
        public int SpriteHeight
        {
            get
            {
                return this.spriteHeight;
            }

            set
            {
                Contract.Requires<ArgumentException>( value > 0 );

                this.spriteHeight = value;
            }
        }

        /// <summary>
        /// Gets the area the <see cref="SpriteSheetDrawComponent"/> draws in.
        /// </summary>
        /// <value>
        /// The client area used by the SpriteSheetDrawComponent.
        /// </value>
        public RectangleF BoundingRectangle
        {
            get
            {
                if( this.Sheet == null || this.MaximumSpritesPerRow == 0 )
                    return new RectangleF( this.Position, 0.0f, 0.0f );

                return new RectangleF(
                    this.Position,
                    this.MaximumSpritesPerRow * this.SpriteWidth,
                    ((this.Sheet.Count / this.MaximumSpritesPerRow) + 1) * this.SpriteHeight
                );
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the SpriteSheetDrawComponent class.
        /// </summary>
        public SpriteSheetDrawComponent()
        {
            this.IsUpdatingSheet = true;
            this.MaximumSpritesPerRow = 10;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the <see cref="ISprite"/> at the given position.
        /// </summary>
        /// <param name="position">
        /// The position of the sprite to get. 
        /// </param>
        /// <param name="index">
        /// Will contain the index of the sprite.
        /// </param>
        /// <remarks>
        /// The upper left sprite ranges from 0.0/0.0 to SpriteWidth/SpriteHeight.
        /// The sprite at index x/y ranges from x*SpriteWidth/y*SpriteHeight to x*SpriteWidth/y*SpriteHeight.
        /// </remarks>
        /// <returns>The sprite to get; null if the position is invalid.</returns>
        public ISprite GetSpriteAt( Point2 position, out int index )
        {
            Contract.Requires<InvalidOperationException>( this.Sheet != null );
            
            int x = position.X / this.SpriteWidth;
            int y = position.Y / this.SpriteHeight;

            if( x < 0 || x > this.MaximumSpritesPerRow )
            {
                index = -1;
                return null;
            }

            index = (y * this.MaximumSpritesPerRow) + x;
            if( index >= 0 && index < this.Sheet.Count )
                return this.Sheet[index];
            return null;
        }

        /// <summary>
        /// Gets the area the Sprite at the given <paramref name="index"/>
        /// takes up.
        /// </summary>
        /// <param name="index">
        /// The zero-based index into the SpriteSheet.
        /// </param>
        /// <returns>
        /// The area the sprite takes up
        /// when drawn with this SpriteSheetDrawComponent.
        /// </returns>
        public Rectangle GetSpriteArea( int index )
        {
            if( index < 0 || this.Sheet == null || index >= this.Sheet.Count )
                return Rectangle.Empty;

            int row = index / this.MaximumSpritesPerRow;
            int column = index % this.MaximumSpritesPerRow;

            return new Rectangle(
                (int)this.Position.X + (column * this.spriteWidth),
                (int)this.Position.Y + (row * this.spriteHeight),
                this.spriteWidth,
                this.spriteHeight
            );
        }

        /// <summary>
        /// Updates this SpriteSheetDrawComponent.
        /// </summary>
        /// <param name="updateContext">
        /// The current <see cref="IUpdateContext"/>.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            if( this.IsUpdatingSheet && this.Sheet != null )
            {
                this.Sheet.Update( updateContext );
            }
        }

        /// <summary>
        /// Draw the SpriteSheet using this SpriteSheetDrawComponent.
        /// </summary>
        /// <param name="drawContext">
        /// The current <see cref="ISpriteDrawContext"/>.
        /// </param>
        public void Draw( ISpriteDrawContext drawContext )
        {
            if( this.Sheet == null )
                return;

            int spritesDrawnInRow = 0;
            Vector2 position = this.Position;
            ISpriteBatch batch = drawContext.Batch;

            for( int i = 0; i < this.Sheet.Count; ++i )
            {
                ISprite sprite = this.Sheet[i];
                if( sprite != null )
                    sprite.Draw( position, batch );

                ++spritesDrawnInRow;
                if( spritesDrawnInRow >= this.MaximumSpritesPerRow )
                {
                    // Jump to next row:
                    position.X  = this.Position.X;
                    position.Y += this.spriteHeight;

                    spritesDrawnInRow = 0;
                }
                else
                {
                    position.X += this.spriteWidth;
                }
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The size of a single entry in the grid.
        /// </summary>
        private int spriteWidth = 16, spriteHeight = 16;

        #endregion
    }
}
