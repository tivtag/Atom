// <copyright file="ITextBlockSplitter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.ITextBlockSplitter interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI
{
    /// <summary>
    /// Provides a mechanism to split a text string into a block of strings.
    /// </summary>
    public interface ITextBlockSplitter
    {
        /// <summary>
        /// Splits the given <paramref name="text"/> string.
        /// </summary>
        /// <param name="text">
        /// The text to split. Can be null.
        /// </param>
        /// <returns>
        /// The split text block; or null.
        /// </returns>
        string[] Split( string text );
    }
}
