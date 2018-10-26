// <copyright file="Text.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Text class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;
    using Atom.Xna.Effects;
    using Atom.Xna.Fonts;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Encapsulates the settings needed to be known
    /// to draw a text string.
    /// </summary>
    public class Text : IUpdateable
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the text string.
        /// </summary>
        public string TextString
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
                this.textBlock = this.SplitIntoBlock( text );
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ITextBlockSplitter"/> to use.
        /// </summary>
        public ITextBlockSplitter BlockSplitter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font which is used to display the <see cref="TextString"/>.
        /// </summary>
        public IFont Font
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of this Text.
        /// </summary>
        public Xna.Color Color
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets how the text is aligned relative to its position.
        /// </summary>
        public TextAlign Align
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the layer depth the Text is drawn at.
        /// </summary>
        public float LayerDepth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the size of the text block that is descriped by this <see cref="Text"/>.
        /// </summary>
        public virtual Vector2 TextBlockSize
        {
            get
            {
                if( this.Font == null )
                {
                    throw new InvalidOperationException( UIErrorStrings.CantGetTextBlockSizeFontIsNull );
                }

                if( this.text == null || this.textBlock == null )
                    return Vector2.Zero;

                Vector2 size = new Vector2( 0.0f, textBlock.Length * this.Font.LineSpacing );

                for( int i = 0; i < textBlock.Length; ++i )
                {
                    float width = this.Font.MeasureStringWidth( textBlock[i] );

                    if( width > size.X )
                        size.X = width;
                }

                return size;
            }
        }
        
        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        public Text()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="font">
        /// The font to use for the new Text.
        /// </param>
        /// <param name="align">
        /// The text alignment mode.
        /// </param>
        /// <param name="color">
        /// The color of the new Text.
        /// </param>
        public Text( IFont font, TextAlign align, Xna.Color color )
        {
            this.Align = align;
            this.Color = color;
            this.Font = font;
            this.BlockSplitter = DefaultBlockSplitter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="font">
        /// The font to use for the new Text.
        /// </param>
        /// <param name="text">
        /// The string the new Text may draw.
        /// </param>
        /// <param name="align">
        /// The text alignment mode.
        /// </param>
        /// <param name="color">
        /// The color of the new Text.
        /// </param>
        public Text( IFont font, string text, TextAlign align, Xna.Color color )
        {
            this.Align = align;
            this.Color = color;
            this.Font = font;
            this.BlockSplitter = DefaultBlockSplitter;

            this.TextString = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="font">
        /// The font to use for the new Text.
        /// </param>
        /// <param name="align">
        /// The text alignment mode.
        /// </param>
        /// <param name="color">
        /// The color of the new Text.
        /// </param>
        /// <param name="blockSplitter">
        /// The <see cref="ITextBlockSplitter"/> to use when splitting the text into blocks.
        /// </param>
        public Text( IFont font, TextAlign align, Xna.Color color, ITextBlockSplitter blockSplitter )
        {
            this.Align = align;
            this.Color = color;
            this.Font = font;
            this.BlockSplitter = blockSplitter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="font">
        /// The font to use for the new Text.
        /// </param>
        /// <param name="text">
        /// The string the new Text may draw.
        /// </param>
        /// <param name="align">
        /// The text alignment mode.
        /// </param>
        /// <param name="color">
        /// The color of the new Text.
        /// </param>
        /// <param name="blockSplitter">
        /// The <see cref="ITextBlockSplitter"/> to use when splitting the text into blocks.
        /// </param>
        public Text( IFont font, string text, TextAlign align, Xna.Color color, ITextBlockSplitter blockSplitter )
        {
            this.Align = align;
            this.Color = color;
            this.Font = font;
            this.BlockSplitter = blockSplitter;

            this.TextString = text;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Updates this <see cref="Text"/>.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public virtual void Update( IUpdateContext updateContext )
        {
            foreach( var effect in this.colorEffects )
            {
                effect.Update( updateContext );
                this.Color = effect.Apply( this.Color );
            }
        }

        /// <summary>
        /// Splits the specified text line into a block of lines to render to the screen.
        /// </summary>
        /// <param name="text">
        /// The text string to split into a block of independent lines. Can be null.
        /// </param>
        /// <returns>The split block of lines. Can be null. </returns>
        private string[] SplitIntoBlock( string text )
        {
            if( this.BlockSplitter != null )
                return this.BlockSplitter.Split( text );
            else
                return DefaultBlockSplitter.Split( text );
        }

        /// <summary>
        /// Draws this Text at the specified position.
        /// </summary>
        /// <param name="position">
        /// The position to draw this Text at.
        /// </param>
        /// <param name="drawContext">
        /// The current <see cref="ISpriteDrawContext"/>.
        /// </param>
        public virtual void Draw( Vector2 position, ISpriteDrawContext drawContext )
        {
            if( this.text == null || this.Font == null )
                return;

            if( this.textBlock != null )
            {
                this.Font.DrawBlock(
                    this.textBlock,
                    position,
                    this.Align,
                    this.Color,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    this.LayerDepth,
                    drawContext
                );
            }
            else
            {
                this.Font.Draw(
                    this.text,
                    position,
                    this.Align,
                    this.Color,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    this.LayerDepth,
                    drawContext
                 );
            }
        }

        #region > Xna.Color Effects <

        /// <summary>
        /// Adds the specified <see cref="IColorEffect"/> to this <see cref="Text"/>
        /// to manibulate its <see cref="Xna.Color"/> over-time.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="effect"/> is null. </exception>
        /// <param name="effect"> The effect to add. </param>
        public void AddColorEffect( ITimedColorEffect effect )
        {
            Contract.Requires<ArgumentNullException>( effect != null );

            this.colorEffects.Add( effect );
        }

        /// <summary>
        /// Tries to remove the specified <see cref="IColorEffect"/> from this <see cref="Text"/>.
        /// </summary>
        /// <param name="effect"> The effect to remove. </param>
        /// <returns>Whether it has been removed.</returns>
        public bool RemoveColorEffect( ITimedColorEffect effect )
        {
            return this.colorEffects.Remove( effect );
        }

        /// <summary>
        /// Returns whether this Text contains the specified <see cref="IColorEffect"/>.
        /// </summary>
        /// <param name="effect">
        /// The effect to .locate.
        /// </param>
        /// <returns>
        /// true if it contains the effect;
        /// otherwise false.
        /// </returns>
        public bool ContainsColorEffect( ITimedColorEffect effect )
        {
            return this.colorEffects.Contains( effect );
        }

        /// <summary>
        /// Removes all <see cref="IColorEffect"/>s from this Text.
        /// </summary>
        public void ClearColorEffects()
        {
            this.colorEffects.Clear();
        }

        /// <summary>
        /// Resets all <see cref="IColorEffect"/>s of this Text.
        /// </summary>
        public void ResetColorEffects()
        {
            for( int i = 0; i < this.colorEffects.Count; ++i )
            {
                this.colorEffects[i].Reset();
            }
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The text to display.
        /// </summary>
        private string text;

        /// <summary>
        /// Contains the text split into lines if there is more than one line.
        /// </summary>
        protected string[] textBlock;

        /// <summary>
        /// The list of effects that are applied to this <see cref="Text"/>s color.
        /// </summary>
        private readonly List<ITimedColorEffect> colorEffects = new List<ITimedColorEffect>( 0 );

        /// <summary>
        /// The splitter which is used by default.
        /// </summary>
        private static readonly DefaultTextBlockSplitter DefaultBlockSplitter = new DefaultTextBlockSplitter();

        #endregion
    }
}
