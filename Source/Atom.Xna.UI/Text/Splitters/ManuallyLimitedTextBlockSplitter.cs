// <copyright file="ManuallyLimitedTextBlockSplitter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.ManuallyLimitedTextBlockSplitter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Text;
    using Atom.Xna.Fonts;

    /// <summary>
    /// Defines a <see cref="ITextBlockSplitter"/> that splits text 
    /// so that it stay inside a specific area.
    /// Line breaks are done by complete word or on manual delimiters
    /// This class can't be inherited.
    /// </summary>
    public sealed class ManuallyLimitedTextBlockSplitter : ITextBlockSplitter
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the maximum allowed width one text block row is allowed to have.
        /// </summary>
        public float MaximumAllowedWidth
        {
            get
            {
                return this.maximumAllowedWidth;
            }

            set
            {
                this.maximumAllowedWidth = value;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ManuallyLimitedTextBlockSplitter"/> class.
        /// </summary>
        /// <param name="font">The font which is later used to render the text.</param>
        /// <param name="maximumAllowedWidth">
        /// The maximum allowed width one text block row is allowed to have.
        /// </param>
        public ManuallyLimitedTextBlockSplitter( IFont font, float maximumAllowedWidth )
            : this( font, maximumAllowedWidth, new string[] { " ", "<br/>" } )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManuallyLimitedTextBlockSplitter"/> class.
        /// </summary>
        /// <param name="font">The font which is later used to render the text.</param>
        /// <param name="maximumAllowedWidth">
        /// The maximum allowed width one text block row is allowed to have.
        /// </param>
        /// <param name="delimiters">
        /// The split delimiters that decide when a string is split.
        /// </param>
        public ManuallyLimitedTextBlockSplitter( IFont font, float maximumAllowedWidth, string[] delimiters )
        {
            Contract.Requires<ArgumentNullException>( font != null );
            Contract.Requires<ArgumentNullException>( delimiters != null );

            this.font = font;
            this.delimeters = delimiters;
            this.maximumAllowedWidth = maximumAllowedWidth;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Splits the specified text string.
        /// </summary>
        /// <param name="text">
        /// The text to split. Can be null.
        /// </param>
        /// <returns>
        /// The split text block; or null.
        /// </returns>
        public string[] Split( string text )
        {
            if( text == null )
                return null;

            text = text.Trim();

            float offsetX = 0.0f;
            int wordIndex = 0;
            int lineIndex = 0;

            var lines = new List<StringBuilder>();
            var words = text.Split( this.delimeters, StringSplitOptions.None );

            if( words.Length > 0 )
                lines.Add( new StringBuilder() );

            for( int i = 0; i < words.Length; )
            {
                string word = words[i];

                if( wordIndex != 0 )
                    word = ' ' + word;

                float wordWidth = this.font.MeasureStringWidth( word );
                float newOffsetX = offsetX + wordWidth;

                if( word.Length == 0 )
                {
                    // words with no characters indicate that a new line should start new lines.
                    newOffsetX = this.maximumAllowedWidth;
                }

                if( newOffsetX <= this.maximumAllowedWidth || wordWidth > this.maximumAllowedWidth )
                {
                    lines[lineIndex].Append( word );

                    offsetX = newOffsetX;
                    ++wordIndex;
                    ++i;
                }
                else
                {
                    lines.Add( new StringBuilder() );

                    ++lineIndex;
                    offsetX = 0;
                    wordIndex = 0;
                }
            }

            // Create the final block
            string[] finalBlocks = new string[lines.Count];

            for( int i = 0; i < finalBlocks.Length; ++i )
            {
                finalBlocks[i] = lines[i].ToString();
            }

            return finalBlocks;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The maximum allowed width one text block row is allowed to have.
        /// </summary>
        private float maximumAllowedWidth;

        /// <summary>
        /// The font to use to find out how long a single word is.
        /// </summary>
        private readonly IFont font;

        /// <summary>
        /// Additional delimiters which can be used to split the text further.
        /// </summary>
        private readonly string[] delimeters;

        #endregion
    }
}
