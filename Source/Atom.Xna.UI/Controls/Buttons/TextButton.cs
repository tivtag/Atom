// <copyright file="TextButton.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Controls.TextButton class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI.Controls
{
    using System;
    using Atom.Math;
    using Atom.Xna.Fonts;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a simple <see cref="Button"/> that is visualized by a String.
    /// </summary>
    public class TextButton : Button
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the <see cref="String"/> that is displayed.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
                this.RefreshButtonClientArea();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="TextAlign"/>.
        /// </summary>
        public TextAlign TextAlign
        {
            get
            {
                return this.aligmentMode;
            }

            set
            {
                if( value == this.aligmentMode )
                    return;

                this.aligmentMode = value;
                this.RefreshButtonClientArea();
            }
        }

        /// <summary>
        /// The offset from the Position of this TextButton to the point at which the text is rendered.
        /// </summary>
        /// <value>
        /// The default value is Vector2.Zero.
        /// </value>
        public Vector2 TextOffset
        { 
            get;
            set; 
        }

        /// <summary>
        /// Gets or sets the <see cref="IFont"/> object that is used to draw the string.
        /// </summary>
        public IFont Font
        {
            get
            {
                return this.font;
            }

            set
            {
                this.font = value;
                this.RefreshButtonClientArea();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Xna.Color"/> the text of the Button is tinted in its default state.
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
        /// Gets or sets the <see cref="Xna.Color"/> the text of the Button is tinted in its selected state.
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

        /// <summary>
        /// Gets or sets the <see cref="Xna.Color"/> that the background of the Button is tinted in its default state.
        /// </summary>
        /// <value>The default value is null.</value>
        public Xna.Color? BackgroundColorDefault
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Xna.Color"/> that the background of the Button is tinted in its selected state.
        /// </summary>
        /// <value>The default value is null.</value>
        public Xna.Color? BackgroundColorSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this TextButton should
        /// use the selected foreground and background colors.
        /// </summary>
        private bool ShouldShowAsSelected
        {
            get
            {
                return this.IsSelected || this.IsMouseOver;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="TextButton"/> class.
        /// </summary>
        public TextButton()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextButton"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the new <see cref="TextButton"/>.
        /// </param> 
        public TextButton( string name )
            : base( name )
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Updates this <see cref="TextButton"/>.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        protected override void OnUpdate( IUpdateContext updateContext )
        {
        }

        /// <summary>
        /// Draws this <see cref="TextButton"/>.
        /// </summary>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        protected override void OnDraw( ISpriteDrawContext drawContext )
        {
            if( this.text == null || this.font == null )
                return;

            bool showAsSelected = this.ShouldShowAsSelected;
            Xna.Color? colorBackground = showAsSelected ? this.BackgroundColorSelected : this.BackgroundColorDefault;
            
            if( colorBackground.HasValue )
            {
                this.DrawBackground( colorBackground.Value, drawContext );
            }

            this.font.Draw(
                text,
                this.Position + TextOffset,
                aligmentMode,
                showAsSelected ? this.colorSelected : this.colorDefault,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                this.RelativeDrawOrder + 0.0001f,
                drawContext
            );
        }

        /// <summary>
        /// Draws the background of this TextButton.
        /// </summary>
        /// <param name="color">
        /// The color of the background.
        /// </param>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        private void DrawBackground( Xna.Color color, ISpriteDrawContext drawContext )
        {
            drawContext.Batch.DrawRect( this.ClientArea, color, this.RelativeDrawOrder );
        }

        /// <summary>
        /// Refreshes the collision rectangle based on the set properties.
        /// </summary>
        private void RefreshButtonClientArea()
        {
            if( this.text == null || this.font == null )
            {
                this.Offset = Vector2.Zero;
                this.Size = Vector2.Zero;
                return;
            }

            var textSize = this.font.MeasureString( this.text );

            switch( aligmentMode )
            {
                case TextAlign.Left:
                    this.Offset = Vector2.Zero;
                    this.Size = textSize;
                    break;

                case TextAlign.Right:
                    this.Offset = new Vector2( -textSize.X, 0.0f );
                    this.Size = textSize;
                    break;

                case TextAlign.Center:
                    this.Offset = new Vector2( (int)(-textSize.X / 2.0f), 0.0f );
                    this.Size = textSize;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Stores the text that is displayed by the TextButton.
        /// </summary>
        private string text;

        /// <summary>
        /// Specifies the used text alogment mode.
        /// </summary>
        private TextAlign aligmentMode = TextAlign.Left;

        /// <summary>
        /// The font object that is used to draw the string.
        /// </summary>
        private IFont font;

        /// <summary>
        /// The color the text is tinted in.
        /// </summary>
        private Xna.Color colorDefault = Xna.Color.White, colorSelected = Xna.Color.White;

        #endregion
    }
}
