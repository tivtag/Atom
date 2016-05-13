// <copyright file="TextureUtilities.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.TextureUtilities class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines Texture related utility methods.
    /// </summary>
    public static class TextureUtilities
    {
        /// <summary>
        /// Creates a new fully white <see cref="Texture2D"/> of size 1 x 1.
        /// </summary>
        /// <remarks>
        /// This texture can be used to draw arabitary rectangles with a <see cref="SpriteBatch"/>.
        /// </remarks>
        /// <param name="graphicsDevice">
        /// The <see cref="GraphicsDevice"/> used to display the texture.
        /// </param>
        /// <returns>
        /// The newly created Texture2D.
        /// </returns>
        public static Texture2D CreateWhite( GraphicsDevice graphicsDevice )
        {
            var texture = new Texture2D( graphicsDevice, 1, 1 );
            var colorData = new Color[1] { Color.White };

            texture.SetData<Color>( colorData );

            return texture;
        }
    }
}
