
namespace Atom.Xna
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines extension methods for the <see cref="Texture2D"/> class.
    /// </summary>
    public static class TextureExtensions
    {
        /// <summary>
        /// Gets the color data of this <see cref="Texture2D"/>.
        /// </summary>
        /// <param name="texture">
        /// The texture to query.
        /// </param>
        /// <returns>
        /// The color data of the texture.
        /// </returns>
        public static Color[] GetColorData( this Texture2D texture )
        {
            var data = new Color[texture.Width * texture.Height];
            texture.GetData( data );
            return data;
        }
    }
}
