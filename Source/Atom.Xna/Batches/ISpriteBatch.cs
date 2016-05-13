// <copyright file="ISpriteBatch.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Batches.ISpriteBatch interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.Batches
{
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Provides a mechanism that allows queueing of sprites to draw
    /// and then later to output them onto the current RenderTarget.
    /// </summary>
    public interface ISpriteBatch : IDrawBatch
    {
        /// <summary>
        /// Initializes this ISpriteBatch to use the specified settings.
        /// </summary>
        /// <param name="blendState">Blending options to use when rendering.</param>
        /// <param name="samplerState">Sampler options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="transform">
        /// A matrix to apply to position, rotation, scale, and depth data passed to Spritebatch.Draw.
        /// </param>>
        void Initialize( BlendState blendState, SamplerState samplerState, SpriteSortMode sortMode, Matrix4 transform );

        /// <summary>
        /// Initializes this ISpriteBatch to use the specified settings.
        /// </summary>
        /// <param name="blendState">Blending options to use when rendering.</param>
        /// <param name="samplerState">Sampler options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="transform">
        /// A matrix to apply to position, rotation, scale, and depth data passed to Spritebatch.Draw.
        /// </param>>
        void Initialize( BlendState blendState, SamplerState samplerState, SpriteSortMode sortMode, Xna.Matrix transform );
                  
        /// <summary>
        /// Adds a sprite to the batch of sprites to be rendered, specifying the texture,
        /// screen position, and color tint.
        /// </summary>
        /// <param name="texture">
        /// The sprite texture.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the sprite will be drawn.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use. Use Xna.Color.White for full color with no tinting.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).You must
        /// specify either SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for
        /// this parameter to affect sprite drawing.
        /// </param>
        void Draw( Texture2D texture, Vector2 position, Xna.Color color, float layerDepth = 0.0f );

        /// <summary>
        /// Draws a rectangular sprite using the specified arguments.
        /// </summary>
        /// <param name="texture">
        /// The sprite texture.
        /// </param>
        /// <param name="rectangle">
        /// The area the sprite should take up.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use. Use Xna.Color.White for full color with no tinting.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).You must
        /// specify either SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for
        /// this parameter to affect sprite drawing.
        /// </param>
        void Draw( Texture2D texture, Rectangle rectangle, Xna.Color color, float layerDepth = 0.0f );

        /// <summary>
        /// Draws a rectangular sprite using the specified arguments.
        /// </summary>
        /// <param name="texture">
        /// The sprite texture.
        /// </param>
        /// <param name="rectangle">
        /// The area the sprite should take up.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use. Use Xna.Color.White for full color with no tinting.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).You must
        /// specify either SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for
        /// this parameter to affect sprite drawing.
        /// </param>
        void Draw( Texture2D texture, Xna.Rectangle rectangle, Xna.Color color, float layerDepth = 0.0f );

        /// <summary>
        /// Draws a sprite, using the specifyed the texture,
        /// destination, and source rectangles and color tint.
        /// </summary>
        /// <param name="texture">
        /// The sprite texture.
        /// </param>
        /// <param name="destinationRectangle">
        /// A rectangle specifying, in screen coordinates, where the sprite will be drawn.
        /// If this rectangle is not the same size as sourceRectangle, the sprite is scaled to fit.
        /// </param>
        /// <param name="sourceRectangle">
        /// A rectangle specifying, in texels, which section of the rectangle to draw. 
        /// Use null to draw the entire texture.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use. Use Xna.Color.White for full color with no tinting.
        /// </param>
        void Draw(
            Texture2D texture,
            Xna.Rectangle destinationRectangle,
            Xna.Rectangle? sourceRectangle,
            Xna.Color color
        );
        
        /// <summary>
        /// Draws a sprite, using the specifyed the texture,
        /// destination, and source rectangles, color tint, rotation, origin, effects,
        /// and sort depth.
        /// </summary>
        /// <param name="texture">
        /// The sprite texture.
        /// </param>
        /// <param name="destinationRectangle">
        /// A rectangle specifying, in screen coordinates, where the sprite will be drawn.
        /// If this rectangle is not the same size as sourceRectangle, the sprite is scaled to fit.
        /// </param>
        /// <param name="sourceRectangle">
        /// A rectangle specifying, in texels, which section of the rectangle to draw. 
        /// Use null to draw the entire texture.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use. Use Xna.Color.White for full color with no tinting.
        /// </param>
        /// <param name="rotation">
        /// The angle, in radians, to rotate the sprite around the origin.
        /// </param>
        /// <param name="origin">
        /// The origin of the sprite. Specify (0,0) for the upper-left corner.
        /// </param>
        /// <param name="effects">
        /// Rotations to apply prior to rendering.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).You must
        /// specify either SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for
        /// this parameter to affect sprite drawing.
        /// </param>
        void Draw( 
            Texture2D texture,
            Rectangle destinationRectangle, 
            Rectangle? sourceRectangle,
            Xna.Color color, 
            float rotation,
            Vector2 origin, 
            SpriteEffects effects, 
            float layerDepth
        );

        /// <summary>
        /// Draws a sprite, using the specifyed the texture,
        /// destination, and source rectangles, color tint, rotation, origin, effects,
        /// and sort depth.
        /// </summary>
        /// <param name="texture">
        /// The sprite texture.
        /// </param>
        /// <param name="destinationRectangle">
        /// A rectangle specifying, in screen coordinates, where the sprite will be drawn.
        /// If this rectangle is not the same size as sourceRectangle, the sprite is scaled to fit.
        /// </param>
        /// <param name="sourceRectangle">
        /// A rectangle specifying, in texels, which section of the rectangle to draw. 
        /// Use null to draw the entire texture.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use. Use Xna.Color.White for full color with no tinting.
        /// </param>
        /// <param name="rotation">
        /// The angle, in radians, to rotate the sprite around the origin.
        /// </param>
        /// <param name="origin">
        /// The origin of the sprite. Specify (0,0) for the upper-left corner.
        /// </param>
        /// <param name="effects">
        /// Rotations to apply prior to rendering.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).You must
        /// specify either SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for
        /// this parameter to affect sprite drawing.
        /// </param>
        void Draw( 
            Texture2D texture,
            Rectangle destinationRectangle, 
            Xna.Rectangle? sourceRectangle,
            Xna.Color color,
            float rotation, 
            Xna.Vector2 origin,
            SpriteEffects effects,
            float layerDepth 
        );

        /// <summary>
        /// Draws a sprite, using the specifyed the texture,
        /// screen position, source rectangle, color tint, rotation, origin, scale, effects,
        /// and sort depth.
        /// </summary>
        /// <param name="texture">
        /// The sprite texture.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the sprite will be drawn.
        /// </param>
        /// <param name="sourceRectangle">
        /// A rectangle specifying, in texels, which section of the rectangle to draw. 
        /// Use null to draw the entire texture.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use. Use Xna.Color.White for full color with no tinting.
        /// </param>
        /// <param name="rotation">
        /// The angle, in radians, to rotate the sprite around the origin.
        /// </param>
        /// <param name="origin">
        /// The origin of the sprite. Specify (0,0) for the upper-left corner.
        /// </param>
        /// <param name="scale">
        /// Vector containing separate scalar multiples for the x- and y-axes of the sprite.
        /// </param>
        /// <param name="effects">
        /// Rotations to apply prior to rendering.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).You must
        /// specify either SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for
        /// this parameter to affect sprite drawing.
        /// </param>
        void Draw( 
            Texture2D texture, 
            Vector2 position, 
            Rectangle? sourceRectangle,
            Xna.Color color, 
            float rotation,
            Vector2 origin, 
            Vector2 scale,
            SpriteEffects effects,
            float layerDepth 
        );

        /// <summary>
        /// Draws a sprite, using the specifyed the texture,
        /// screen position, source rectangle, color tint, rotation, origin, scale, effects,
        /// and sort depth.
        /// </summary>
        /// <param name="texture">
        /// The sprite texture.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the sprite will be drawn.
        /// </param>
        /// <param name="sourceRectangle">
        /// A rectangle specifying, in texels, which section of the rectangle to draw. 
        /// Use null to draw the entire texture.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use. Use Xna.Color.White for full color with no tinting.
        /// </param>
        /// <param name="rotation">
        /// The angle, in radians, to rotate the sprite around the origin.
        /// </param>
        /// <param name="origin">
        /// The origin of the sprite. Specify (0,0) for the upper-left corner.
        /// </param>
        /// <param name="scale">
        /// Vector containing separate scalar multiples for the x- and y-axes of the sprite.
        /// </param>
        /// <param name="effects">
        /// Rotations to apply prior to rendering.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).You must
        /// specify either SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for
        /// this parameter to affect sprite drawing.
        /// </param>
        void Draw(
            Texture2D texture,
            Vector2 position, 
            Xna.Rectangle? sourceRectangle,
            Xna.Color color, 
            float rotation, 
            Xna.Vector2 origin,
            Xna.Vector2 scale, 
            SpriteEffects effects,
            float layerDepth );
        
        /// <summary>
        /// Draws a sprite, using the specifyed the texture,
        /// screen position, source rectangle, color tint, rotation, origin, scale, effects,
        /// and sort depth.
        /// </summary>
        /// <param name="texture">
        /// The sprite texture.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the sprite will be drawn.
        /// </param>
        /// <param name="sourceRectangle">
        /// A rectangle specifying, in texels, which section of the rectangle to draw. 
        /// Use null to draw the entire texture.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use. Use Xna.Color.White for full color with no tinting.
        /// </param>
        /// <param name="rotation">
        /// The angle, in radians, to rotate the sprite around the origin.
        /// </param>
        /// <param name="origin">
        /// The origin of the sprite. Specify (0,0) for the upper-left corner.
        /// </param>
        /// <param name="scale">
        /// Vector containing separate scalar multiples for the x- and y-axes of the sprite.
        /// </param>
        /// <param name="effects">
        /// Rotations to apply prior to rendering.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).You must
        /// specify either SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for
        /// this parameter to affect sprite drawing.
        /// </param>
        void Draw(
            Texture2D texture,
            Xna.Vector2 position,
            Xna.Rectangle? sourceRectangle,
            Xna.Color color,
            float rotation,
            Xna.Vector2 origin,
            Xna.Vector2 scale,
            SpriteEffects effects,
            float layerDepth );

        /// <summary>
        /// Draws a sprite string using the specifyed arguments.
        /// </summary>
        /// <param name="spriteFont">
        /// The sprite font.
        /// </param>
        /// <param name="text">
        /// The string to draw.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the text will be drawn.
        /// </param>
        /// <param name="color">
        /// The desired color of the text.
        /// </param>
        void DrawString( SpriteFont spriteFont, string text, Vector2 position, Xna.Color color );

        /// <summary>
        /// Draws a sprite string using the specifyed font, output text, screen position, 
        /// color tint, rotation, origin, scale,and effects.
        /// </summary>
        /// <param name="spriteFont">
        /// The sprite font.
        /// </param>
        /// <param name="text">
        /// The string to draw.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the text will be drawn.
        /// </param>
        /// <param name="color">
        /// The desired color of the text.
        /// </param>
        /// <param name="rotation">
        /// The angle, in radians, to rotate the text around the origin.
        /// </param>
        /// <param name="origin">
        /// The origin of the string. Specify (0,0) for the upper-left corner.
        /// </param>
        /// <param name="scale">
        /// Uniform multiple by which to scale the sprite width and height.
        /// </param>
        /// <param name="effects">
        /// Rotations to apply prior to rendering.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).
        /// </param>
        void DrawString(
            SpriteFont spriteFont,
            string text,
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth );
    }
}
