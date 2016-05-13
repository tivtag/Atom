// <copyright file="AnimatedTypeWriterText.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.AnimatedTypeWriterText class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI
{
    using Atom.Math;
    using Atom.Xna.Fonts;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Represents a <see cref="PartiallyShownText"/> that
    /// is animated to look like someone would write it on a typewriter.
    /// </summary>
    public class AnimatedTypeWriterText : PartiallyShownText
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the time it should take to write one character TypeWriter style.
        /// </summary>
        public float TimeSpendPerCharacter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the end of the current visible block has been reached.
        /// </summary>
        public bool HasReachedEndOfBlock
        {
            get
            {
                return this.hasReachedEndOfBlock;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this AnimatedTypeWriterText has reached the last line of the Text.
        /// </summary>
        public override bool HasReachedEnd
        {
            get
            {
                if( this.textBlock == null )
                    return true;

                return this.LineIndex + this.fullLineCount >= this.textBlock.Length;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedTypeWriterText"/> class.
        /// </summary>
        public AnimatedTypeWriterText()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedTypeWriterText"/> class.
        /// </summary>
        /// <param name="font">
        /// The font to use for the new AnimatedTypeWriterText.
        /// </param>
        /// <param name="align">
        /// The AnimatedTypeWriterText alignment mode.
        /// </param>
        /// <param name="color">
        /// The color of the new AnimatedTypeWriterText.
        /// </param>
        public AnimatedTypeWriterText( IFont font, TextAlign align, Xna.Color color )
            : base( font, align, color )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedTypeWriterText"/> class.
        /// </summary>
        /// <param name="font">
        /// The font to use for the new AnimatedTypeWriterText.
        /// </param>
        /// <param name="text">
        /// The string the new AnimatedTypeWriterText may draw.
        /// </param>
        /// <param name="align">
        /// The AnimatedTypeWriterText alignment mode.
        /// </param>
        /// <param name="color">
        /// The color of the new AnimatedTypeWriterText.
        /// </param>
        public AnimatedTypeWriterText( IFont font, string text, TextAlign align, Xna.Color color )
            : base( font, text, align, color )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedTypeWriterText"/> class.
        /// </summary>
        /// <param name="font">
        /// The font to use for the new AnimatedTypeWriterText.
        /// </param>
        /// <param name="align">
        /// The AnimatedTypeWriterText alignment mode.
        /// </param>
        /// <param name="color">
        /// The color of the new AnimatedTypeWriterText.
        /// </param>
        /// <param name="blockSplitter">
        /// The <see cref="ITextBlockSplitter"/> to use when splitting the AnimatedTypeWriterText into blocks.
        /// </param>
        public AnimatedTypeWriterText( IFont font, TextAlign align, Xna.Color color, ITextBlockSplitter blockSplitter )
            : base( font, align, color, blockSplitter )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedTypeWriterText"/> class.
        /// </summary>
        /// <param name="font">
        /// The font to use for the new AnimatedTypeWriterText.
        /// </param>
        /// <param name="text">
        /// The string the new AnimatedTypeWriterText may draw.
        /// </param>
        /// <param name="align">
        /// The AnimatedTypeWriterText alignment mode.
        /// </param>
        /// <param name="color">
        /// The color of the new AnimatedTypeWriterText.
        /// </param>
        /// <param name="blockSplitter">
        /// The <see cref="ITextBlockSplitter"/> to use when splitting the AnimatedTypeWriterText into blocks.
        /// </param>
        public AnimatedTypeWriterText( IFont font, string text, TextAlign align, Xna.Color color, ITextBlockSplitter blockSplitter )
            : base( font, text, align, color, blockSplitter )
        {
        }

        #endregion

        #region [ Methods ]

        #region > Updating <

        /// <summary>
        /// Updates this <see cref="AnimatedTypeWriterText"/>.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public override void Update( IUpdateContext updateContext )
        {
            base.Update( updateContext );
            if( this.textBlock == null )
                return;

            int currentLineIndex = this.LineIndex + this.fullLineCount;
            if( currentLineIndex >= this.textBlock.Length )
                return;

            string line = this.textBlock[currentLineIndex];

            if( this.charIndex >= line.Length )
            {
                if( (fullLineCount + 1) <= this.LinesShown )
                {
                    this.charIndex = 0;
                    ++this.fullLineCount;

                    if( this.fullLineCount == this.LinesShown )
                    {
                        this.ReachedEndOfBlock();
                        return;
                    }
                }
                else
                {
                    this.ReachedEndOfBlock();
                    return;
                }
            }

            // Update Tick.
            this.timeTick += updateContext.FrameTime;

            if( this.timeTick >= this.TimeSpendPerCharacter )
            {
                ++this.charIndex;
                this.timeTick = 0.0f;
            }
        }

        #endregion

        #region > Draw <

        /// <summary>
        /// Draws this AnimatedTypeWriterText at the specified position.
        /// </summary>
        /// <param name="position">
        /// The position to draw this Text at.
        /// </param>
        /// <param name="drawContext">
        /// The current <see cref="ISpriteDrawContext"/>.
        /// </param>
        public override void Draw( Vector2 position, ISpriteDrawContext drawContext )
        {
            if( this.textBlock == null || this.Font == null || this.LineIndex < 0 )
                return;

            int i = 0;

            // First render the full lines
            for( ; i < this.fullLineCount; ++i )
            {
                int index = this.LineIndex + i;
                if( index >= this.textBlock.Length )
                    break;

                string line = this.textBlock[index];

                this.Font.Draw( line, position, this.Align, this.Color, this.LayerDepth, drawContext );
                position.Y += this.Font.LineSpacing;
            }

            if( i == this.LinesShown )
                return;

            // now render the missing characters
            int animatedLineIndex = this.LineIndex + this.fullLineCount;
            if( animatedLineIndex >= this.textBlock.Length )
                return;

            string animatedLine = this.textBlock[animatedLineIndex].Substring( 0, this.charIndex );
            this.Font.Draw( animatedLine, position, this.Align, this.Color, this.LayerDepth, drawContext );
        }

        #endregion

        /// <summary>
        /// Resets the character currently displayed.
        /// </summary>
        public override void Reset()
        {
            this.charIndex = 0;
            this.fullLineCount = 0;
            this.hasReachedEndOfBlock = false;

            base.Reset();
        }

        /// <summary>
        /// Jumps one full block.
        /// </summary>
        public override void JumpBlock()
        {
            this.charIndex = 0;
            this.fullLineCount = 0;
            this.hasReachedEndOfBlock = false;

            base.JumpBlock();
        }

        /// <summary>
        /// Jumps to the end of the current text block.
        /// </summary>
        public void JumpToEndOfBlock()
        {
            this.charIndex = 0;
            this.fullLineCount = this.LinesShown;

            if( this.LineIndex + this.fullLineCount > this.textBlock.Length )
            {
                this.fullLineCount = this.textBlock.Length - this.LineIndex;
            }

            this.hasReachedEndOfBlock = true;
        }

        /// <summary>
        /// Called when the this AnimatedTypeWriterText has reached 
        /// the end of the current text block.
        /// </summary>
        private void ReachedEndOfBlock()
        {
            this.hasReachedEndOfBlock = true;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The index of the character currently animated.
        /// </summary>
        private int charIndex;

        /// <summary>
        /// The number of full lines which have been drawn.
        /// </summary>
        private int fullLineCount;

        /// <summary>
        /// The current time tick.
        /// </summary>
        private float timeTick;

        /// <summary>
        /// States whether this AnimatedTypeWriterText has reached the end of the current text block.
        /// </summary>
        private bool hasReachedEndOfBlock;

        #endregion
    }
}
