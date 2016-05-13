// <copyright file="SpriteButton.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Controls.SpriteButton class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI.Controls
{
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a <see cref="Button"/> that is visualized using Sprites.
    /// </summary>
    public class SpriteButton : Button
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the <see cref="ISprite"/> that is usually displayed.
        /// </summary>
        /// <remarks>
        /// Setting this value also refreshes the ClientArea.
        /// </remarks>
        public ISprite SpriteDefault
        {
            get
            {
                return this.spriteDefault;
            }

            set
            {
                this.spriteDefault = value;
                this.RefreshButtonClientArea();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ISprite"/> that is displayed when 
        /// the <see cref="Button"/> is either selected or if the mouse is over it.
        /// </summary>
        public ISprite SpriteSelected
        {
            get
            {
                return this.spriteSelected;
            }

            set
            {
                this.spriteSelected = value;
            }
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
        /// Initializes a new instance of the <see cref="SpriteButton"/> class.
        /// </summary>
        /// <param name="name"> The name of the <see cref="Button"/>. </param> 
        /// <param name="spriteDefault"> The sprite of the button in its default state. </param>
        /// <param name="spriteSelected"> The sprite of the button when the mouse is over/selected the button. </param>
        public SpriteButton( string name, ISprite spriteDefault, ISprite spriteSelected )
            : base( name )
        {
            this.spriteDefault  = spriteDefault;
            this.spriteSelected = spriteSelected;

            if( spriteDefault == null )
                this.Size = Vector2.Zero;
            else
                this.Size = spriteDefault.Size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteButton"/> class.
        /// </summary>
        /// <param name="name"> The name of the <see cref="Button"/>. </param> 
        public SpriteButton( string name )
            : base( name )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteButton"/> class.
        /// </summary>
        public SpriteButton()
            : base()
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Refreshes the <see cref="UIElement.ClientArea"/> of this <see cref="SpriteButton"/>
        /// based on the currently set <see cref="SpriteDefault"/>.
        /// </summary>
        protected virtual void RefreshButtonClientArea()
        {
            if( this.spriteDefault == null )
            {
                this.Size = Vector2.Zero;
            }
            else
            {
                this.Size = this.spriteDefault.Size;
            }
        }

        /// <summary>
        /// Updates this <see cref="SpriteButton"/>.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        protected override void OnUpdate( IUpdateContext updateContext )
        {
            ISprite sprite;
            if( this.IsMouseOver || this.IsSelected )
                sprite = this.spriteSelected;
            else
                sprite = this.spriteDefault;

            var updateable = sprite as IUpdateable;

            if( updateable != null )
            {
                updateable.Update( updateContext );
            }
        }

        /// <summary>
        /// Draws this <see cref="SpriteButton"/>.
        /// </summary>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        protected override void OnDraw( ISpriteDrawContext drawContext )
        {
            if( this.IsMouseOver || this.IsSelected )
            {
                if( this.spriteSelected != null )
                {
                    this.spriteSelected.Draw(
                        this.Position,
                        this.colorSelected,
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
                if( this.spriteDefault != null )
                {
                    this.spriteDefault.Draw(
                        this.Position,
                        this.colorDefault,
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
        /// Stores the sprites of this <see cref="SpriteButton"/>.
        /// </summary>
        private ISprite spriteDefault, spriteSelected;

        /// <summary>
        /// The color the button is tinted.
        /// </summary>
        private Xna.Color colorDefault = Xna.Color.White, colorSelected = Xna.Color.White;

        #endregion
    }
}

