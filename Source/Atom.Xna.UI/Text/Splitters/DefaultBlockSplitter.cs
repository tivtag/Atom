// <copyright file="DefaultTextBlockSplitter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.DefaultTextBlockSplitter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI
{
    using System;

    /// <summary>
    /// The default <see cref="ITextBlockSplitter"/> simply splits
    /// the text using specific separators.
    /// This class can't be inherited.
    /// </summary>
    public sealed class DefaultTextBlockSplitter : ITextBlockSplitter
    {
        /// <summary>
        /// The default separators which are used to split the text.
        /// </summary>
        private static readonly string[] DefaultSeparators = new string[1] { "<br/>" };

        /// <summary>
        /// The separators which are used to split the text.
        /// </summary>
        private readonly string[] separators;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTextBlockSplitter"/> class,
        /// using the default Seperators.
        /// </summary>
        public DefaultTextBlockSplitter()
        {
            this.separators = DefaultSeparators;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTextBlockSplitter"/> class.
        /// </summary>
        /// <param name="separators">
        /// The separators to use. The default separators are used if null.
        /// </param>
        public DefaultTextBlockSplitter( string[] separators )
        {
            if( separators == null )
                this.separators = DefaultSeparators;
            else
                this.separators = separators;
        }

        /// <summary>
        /// Splits the text string.
        /// </summary>
        /// <param name="text"> The text to split. Can be null. </param>
        /// <returns>
        /// The split text block; or null.
        /// </returns>
        public string[] Split( string text )
        {
            if( text == null )
                return null;

            return text.Split( this.separators, StringSplitOptions.None );
        }
    }
}
