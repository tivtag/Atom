
namespace Atom.Xna.UI.Controls
{
    using System;
    using Atom.Math;
    using Atom.Xna.Fonts;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Represents a combination of a <see cref="SpriteButton"/> and a <see cref="TextButton"/>.
    /// </summary>
    public class SpriteTextButton : SpriteButton
    {
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
        public Xna.Color ColorTextDefault
        {
            get
            {
                return this.colorTextDefault;
            }

            set
            {
                this.colorTextDefault = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Xna.Color"/> the text of the Button is tinted in its selected state.
        /// </summary>
        /// <value>The default value is <see cref="Xna.Color.White"/>.</value>
        public Xna.Color ColorTextSelected
        {
            get
            {
                return this.colorTextSelected;
            }

            set
            {
                this.colorTextSelected = value;
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
        /// Initializes a new instance of the <see cref="SpriteTextButton"/> class.
        /// </summary>
        /// <param name="name"> The name of the <see cref="Button"/>. </param> 
        /// <param name="spriteDefault"> The sprite of the button in its default state. </param>
        /// <param name="spriteSelected"> The sprite of the button when the mouse is over/selected the button. </param>
        public SpriteTextButton( string name, ISprite spriteDefault, ISprite spriteSelected )
            : base( name, spriteDefault, spriteSelected )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteTextButton"/> class.
        /// </summary>
        /// <param name="name"> The name of the <see cref="Button"/>. </param> 
        public SpriteTextButton( string name )
            : base( name )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteTextButton"/> class.
        /// </summary>
        public SpriteTextButton()
            : base()
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
            base.OnDraw( drawContext );
            
            this.font.Draw(
                text,
                this.Position + TextOffset,
                aligmentMode,
                (this.IsSelected || this.IsMouseOver) ? this.colorTextSelected : this.colorTextDefault,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                this.RelativeDrawOrder + 0.0001f,
                drawContext
            );
        }

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
        private Xna.Color colorTextDefault = Xna.Color.White, colorTextSelected = Xna.Color.White;
    }
}
