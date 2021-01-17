// <copyright file="PartiallyShownText.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.PartiallyShownText class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI
{
    using System;
    using Atom.Math;
    using Atom.Xna.Fonts;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Represents some <see cref="Text"/> that shows 
    /// only a part of the text block.
    /// </summary>
    public class PartiallyShownText : Text
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the width (in pixels) of the text block.
        /// </summary>
        public int TextBlockWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum number of text lines to show at a time.
        /// </summary>
        public int LinesShown
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the index of the line the currently shown partial text block starts.
        /// </summary>
        public int LineIndex
        {
            get
            { 
                return this.lineIndex; 
            }
        }

        /// <summary>
        /// Gets the size of the text block that is descriped by this <see cref="PartiallyShownText"/>.
        /// </summary>
        public override Vector2 TextBlockSize
        {
            get
            {
                if( this.Font == null )
                    throw new InvalidOperationException( UIErrorStrings.CantGetTextBlockSizeFontIsNull );

                if( this.textBlock == null || this.TextString == null )
                    return Vector2.Zero;

                return new Vector2( this.TextBlockWidth, this.LinesShown * this.Font.LineSpacing );
            }
        }

        /// <summary>
        /// Gets a value indicating whether the last line 
        /// of the Text in the PartiallyShownText has been reached.
        /// </summary>
        public virtual bool HasReachedEnd
        {
            get
            {
                return this.textBlock == null || this.lineIndex + this.LinesShown >= this.textBlock.Length;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="PartiallyShownText"/> class.
        /// </summary>
        public PartiallyShownText()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartiallyShownText"/> class.
        /// </summary>
        /// <param name="font">
        /// The font to use for the new PartiallyShownText.
        /// </param>
        /// <param name="align">
        /// The PartiallyShownText alignment mode.
        /// </param>
        /// <param name="color">
        /// The color of the new PartiallyShownText.
        /// </param>
        public PartiallyShownText( IFont font, TextAlign align, Xna.Color color )
            : base( font, align, color )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartiallyShownText"/> class.
        /// </summary>
        /// <param name="font">
        /// The font to use for the new PartiallyShownText.
        /// </param>
        /// <param name="text">
        /// The string the new PartiallyShownText may draw.
        /// </param>
        /// <param name="align">
        /// The PartiallyShownText alignment mode.
        /// </param>
        /// <param name="color">
        /// The color of the new PartiallyShownText.
        /// </param>
        public PartiallyShownText( IFont font, string text, TextAlign align, Xna.Color color )
            : base( font, text, align, color )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartiallyShownText"/> class.
        /// </summary>
        /// <param name="font">
        /// The font to use for the new PartiallyShownText.
        /// </param>
        /// <param name="align">
        /// The PartiallyShownText alignment mode.
        /// </param>
        /// <param name="color">
        /// The color of the new PartiallyShownText.
        /// </param>
        /// <param name="blockSplitter">
        /// The <see cref="ITextBlockSplitter"/> to use when splitting the PartiallyShownText into blocks.
        /// </param>
        public PartiallyShownText( IFont font, TextAlign align, Xna.Color color, ITextBlockSplitter blockSplitter )
            : base( font, align, color, blockSplitter )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartiallyShownText"/> class.
        /// </summary>
        /// <param name="font">
        /// The font to use for the new PartiallyShownText.
        /// </param>
        /// <param name="text">
        /// The string the new PartiallyShownText may draw.
        /// </param>
        /// <param name="align">
        /// The PartiallyShownText alignment mode.
        /// </param>
        /// <param name="color">
        /// The color of the new PartiallyShownText.
        /// </param>
        /// <param name="blockSplitter">
        /// The <see cref="ITextBlockSplitter"/> to use when splitting the PartiallyShownText into blocks.
        /// </param>
        public PartiallyShownText( IFont font, string text, TextAlign align, Xna.Color color, ITextBlockSplitter blockSplitter )
            : base( font, text, align, color, blockSplitter )
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Resets the index into the text block.
        /// </summary>
        public virtual void Reset()
        {
            this.lineIndex = 0;
        }

        /// <summary>
        /// Moves the LineIndex down by one line.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if it was possible to move to the next line;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public virtual bool MoveLineIndexDown()
        {
            if( this.textBlock == null || this.lineIndex + this.LinesShown + 1 > this.textBlock.Length )
                return false;

            ++this.lineIndex;
            return true;
        }

        /// <summary>
        /// Jumps one full block of size <see cref="LinesShown"/>.
        /// </summary>
        public virtual void JumpBlock()
        {
            this.lineIndex += this.LinesShown;

            if( this.lineIndex > this.textBlock.Length )
            {
                this.lineIndex = this.textBlock.Length;
            }
        }

        /// <summary>
        /// Moves the LineIndex up by one line.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if it was possible to move to the previous line;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public virtual bool MoveLineIndexUp()
        {
            if( this.textBlock == null || this.lineIndex == 0 )
                return false;

            --this.lineIndex;
            return true;
        }

        /// <summary>
        /// Draws this PartiallyShownText at the specified position.
        /// </summary>
        /// <param name="position">
        /// The position to draw this Text at.
        /// </param>
        /// <param name="drawContext">
        /// The current <see cref="ISpriteDrawContext"/>.
        /// </param>
        public override void Draw( Vector2 position, ISpriteDrawContext drawContext )
        {
            if( this.textBlock == null || this.Font == null || this.lineIndex < 0 )
                return;

            for( int i = 0; i < this.LinesShown; ++i )
            {
                int index = this.lineIndex + i;
                if( index >= this.textBlock.Length )
                    break;

                this.Font.Draw(
                    textBlock[index],
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

                position.Y += this.Font.LineSpacing;
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The current line index.
        /// </summary>
        private int lineIndex;

        #endregion
    }
}
