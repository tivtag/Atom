// <copyright file="DynamicSpriteButton.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Controls.DynamicSpriteButton class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI.Controls
{
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a button whose Sprite resources are received every frame on the fly.
    /// </summary>
    /// <remarks>
    /// The SpiteReceiver delegate is sued to receive the Sprite Data.
    /// </remarks>
    public class DynamicSpriteButton : Button
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the delegate that is used to receive sprite data on the fly.
        /// </summary>
        public DynamicSpriteButtonSpriteReceiver SpriteReceiver
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Xna.Color"/> the Button is tinted in its default state.
        /// </summary>
        /// <value>The default value is <see cref="Xna.Color.White"/>.</value>
        public Xna.Color ColorDefault
        {
            get 
            {
                return this.colorDefault; 
            }

            set
            {
                this.colorDefault = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Xna.Color"/> the Button is tinted in its selected state.
        /// </summary>
        /// <value>The default value is <see cref="Xna.Color.White"/>.</value>
        public Xna.Color ColorSelected
        {
            get 
            { 
                return this.colorSelected; 
            }

            set
            { 
                this.colorSelected = value;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicSpriteButton"/> class.
        /// </summary>
        /// <param name="name"> The name of the <see cref="Button"/>. </param>
        public DynamicSpriteButton( string name )
            : base( name )
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Updates this <see cref="DynamicSpriteButton"/>.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        protected override void OnUpdate( IUpdateContext updateContext )
        {
        }

        /// <summary>
        /// Draws this <see cref="DynamicSpriteButton"/>.
        /// </summary>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        protected override void OnDraw( ISpriteDrawContext drawContext )
        {
            ISprite spriteDefault = null, spriteSelected = null;
            if( this.SpriteReceiver != null )
                this.SpriteReceiver( this, out spriteDefault, out spriteSelected );

            if( this.IsMouseOver || this.IsSelected )
            {
                if( spriteSelected != null )
                {
                    spriteSelected.Draw(
                        this.Position,
                        colorSelected,
                        0.0f,
                        Vector2.Zero,
                        Vector2.One,
                        SpriteEffects.None,
                        this.RelativeDrawOrder,
                        drawContext.Batch
                    );
                }
            }
            else
            {
                if( spriteDefault != null )
                {
                    spriteDefault.Draw(
                        this.Position,
                        colorDefault,
                        0.0f,
                        Vector2.Zero,
                        Vector2.One,
                        SpriteEffects.None,
                        this.RelativeDrawOrder,
                        drawContext.Batch
                    );
                }
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The color the button is tinted.
        /// </summary>
        private Xna.Color colorDefault = Xna.Color.White, colorSelected = Xna.Color.White;

        #endregion
    }

    /// <summary>
    /// Defines the delegate that is used to get sprite data for the <see cref="DynamicSpriteButton"/>.
    /// </summary>
    /// <param name="sender">
    /// The button which wants to receive sprite data.
    /// </param>
    /// <param name="spriteDefault">
    /// The sprite to display when the button is in default state.
    /// </param>
    /// <param name="spriteSelected">
    /// The sprite to display when the button is in selected state
    /// .</param>
    public delegate void DynamicSpriteButtonSpriteReceiver( DynamicSpriteButton sender, out ISprite spriteDefault, out ISprite spriteSelected );
}

