// <copyright file="SpriteFontExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteFontExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Contains extension method for the <see cref="Microsoft.Xna.Framework.Graphics.SpriteFont"/> class.
    /// </summary>
    public static class SpriteFontExtensions
    {
        /// <summary>
        /// Returns the width of the given <see cref="String"/>.
        /// </summary>
        /// <param name="spriteFont">
        /// The related sprite font.
        /// </param>
        /// <param name="text">
        /// The string to measure.
        /// </param>
        /// <returns>
        /// The width of the measured string.
        /// </returns>
        public static float MeasureStringWidth( this SpriteFont spriteFont, string text )
        {
            return spriteFont.MeasureString( text ).X;
        }
    }
}
