// <copyright file="ComposedSpriteBatch.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Batches.ComposedSpriteBatch interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.Batches
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Implements mechanisms for drawing various sprites and arabitary forms.
    /// </summary>
    public class ComposedSpriteBatch : IComposedSpriteBatch
    {
        /// <summary>
        /// Initializes a new instance of the ComposedSpriteBatch class.
        /// </summary>
        /// <param name="spriteBatch">
        /// The batch that is used to draw sprites in various ways.
        /// </param>
        /// <param name="textureBatch">
        /// The batch that is used to draw various rectangular shapes.
        /// </param>
        public ComposedSpriteBatch( ISpriteBatch spriteBatch, ITextureDrawBatch textureBatch )
        {
            Contract.Requires<ArgumentNullException>( spriteBatch != null );
            Contract.Requires<ArgumentNullException>( textureBatch != null );

            this.spriteBatch = spriteBatch;
            this.textureBatch = textureBatch;
        }

        /// <summary>
        /// Initializes a new instance of the ComposedSpriteBatch class;
        /// which initializes the CompoedSpriteBatch to internally use a <see cref="SpriteDrawBatch"/>
        /// and a <see cref="SpriteTextureDrawBatch"/> that has a white texture.
        /// </summary>
        /// <param name="device">
        /// The XNA graphics device.
        /// </param>
        public ComposedSpriteBatch( GraphicsDevice device )
        {
            Contract.Requires<ArgumentNullException>( device != null );

            this.spriteBatch = new SpriteDrawBatch( new SpriteBatch( device ) );
            this.textureBatch = new SpriteTextureDrawBatch( this.spriteBatch, TextureUtilities.CreateWhite( device ) );
        }

        /// <summary>
        /// Initializes this ISpriteBatch to use the specified settings.
        /// </summary>
        /// <param name="blendState">Blending options to use when rendering.</param>
        /// <param name="samplerState">Sampler options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="transform">
        /// A matrix to apply to position, rotation, scale, and depth data passed to Spritebatch.Draw.
        /// </param>>
        public void Initialize( BlendState blendState, SamplerState samplerState, SpriteSortMode sortMode, Matrix4 transform )
        {
            this.spriteBatch.Initialize( blendState, samplerState, sortMode, transform );
        }

        /// <summary>
        /// Initializes this ISpriteBatch to use the specified settings.
        /// </summary>
        /// <param name="blendState">Blending options to use when rendering.</param>
        /// <param name="samplerState">Sampler options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="transform">
        /// A matrix to apply to position, rotation, scale, and depth data passed to Spritebatch.Draw.
        /// </param>>
        public void Initialize( BlendState blendState, SamplerState samplerState, SpriteSortMode sortMode, Xna.Matrix transform )
        {
            this.spriteBatch.Initialize( blendState, samplerState, sortMode, transform );
        }

        /// <summary>
        /// Begins drawing to this IDrawBatch.
        /// </summary>
        /// <param name="drawContext">
        /// The current <see cref="IXnaDrawContext"/>.
        /// </param>
        public void Begin( IXnaDrawContext drawContext )
        {
            this.textureBatch.Begin( drawContext );
        }

        /// <summary>
        /// Ends drawing to this IDrawBatch, outputing the result.
        /// </summary>
        public void End()
        {
            this.textureBatch.End();
        }

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
        public void Draw( Texture2D texture, Vector2 position, Xna.Color color, float layerDepth = 0.0f )
        {
            this.spriteBatch.Draw( texture, position, color, layerDepth );
        }
        
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
        public void Draw( Texture2D texture, Rectangle rectangle, Xna.Color color, float layerDepth = 0.0f )
        {
            this.spriteBatch.Draw( texture, rectangle, color, layerDepth );
        }
        
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
        public void Draw( Texture2D texture, Xna.Rectangle rectangle, Xna.Color color, float layerDepth = 0.0f )
        {
            this.spriteBatch.Draw( texture, rectangle, color, layerDepth );
        }

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
        public void Draw(
            Texture2D texture,
            Xna.Rectangle destinationRectangle,
            Xna.Rectangle? sourceRectangle,
            Xna.Color color )
        {
            this.spriteBatch.Draw( texture, destinationRectangle, sourceRectangle, color );
        }

        /// <summary>
        /// Draws a sprite, using the specifyed the texture,
        /// destination, and source rectangles, color tint, rotation, origin, effects,
        /// and sort depth. Reference page contains links to related code samples.
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
        public void Draw(
            Texture2D texture,
            Rectangle destinationRectangle,
            Rectangle? sourceRectangle,
            Xna.Color color,
            float rotation, 
            Vector2 origin,
            SpriteEffects effects, 
            float layerDepth )
        {
            this.spriteBatch.Draw( 
                texture, 
                destinationRectangle,
                sourceRectangle,
                color,
                rotation,
                origin,
                effects,
                layerDepth
            );
        }

        /// <summary>
        /// Draws a sprite, using the specifyed the texture,
        /// destination, and source rectangles, color tint, rotation, origin, effects,
        /// and sort depth. Reference page contains links to related code samples.
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
        public void Draw( 
            Texture2D texture,
            Rectangle destinationRectangle,
            Xna.Rectangle? sourceRectangle,
            Xna.Color color, 
            float rotation, 
            Xna.Vector2 origin, 
            SpriteEffects effects,
            float layerDepth )
        {
            this.spriteBatch.Draw(
                texture,
                destinationRectangle,
                sourceRectangle,
                color,
                rotation,
                origin,
                effects,
                layerDepth
            );
        }

        /// <summary>
        /// Draws a sprite, using the specifyed the texture,
        /// screen position, source rectangle, color tint, rotation, origin, scale, effects,
        /// and sort depth. Reference page contains links to related code samples.
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
        public void Draw(
            Texture2D texture,
            Vector2 position,
            Rectangle? sourceRectangle,
            Xna.Color color,
            float rotation, 
            Vector2 origin,
            Vector2 scale, 
            SpriteEffects effects,
            float layerDepth )
        {
            this.spriteBatch.Draw(
                texture,
                position,
                sourceRectangle,
                color,
                rotation,
                origin,
                scale,
                effects,
                layerDepth
            );
        }

        /// <summary>
        /// Draws a sprite, using the specifyed the texture,
        /// screen position, source rectangle, color tint, rotation, origin, scale, effects,
        /// and sort depth. Reference page contains links to related code samples.
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
        public void Draw( 
            Texture2D texture,
            Vector2 position,
            Xna.Rectangle? sourceRectangle,
            Xna.Color color, 
            float rotation,
            Xna.Vector2 origin,
            Xna.Vector2 scale,
            SpriteEffects effects,
            float layerDepth )
        {
            this.spriteBatch.Draw(
                texture,
                position,
                sourceRectangle,
                color,
                rotation,
                origin,
                scale,
                effects,
                layerDepth
            );
        }

        /// <summary>
        /// Draws a sprite, using the specifyed the texture,
        /// screen position, source rectangle, color tint, rotation, origin, scale, effects,
        /// and sort depth. Reference page contains links to related code samples.
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
        public void Draw(
            Texture2D texture,
            Xna.Vector2 position,
            Xna.Rectangle? sourceRectangle,
            Xna.Color color,
            float rotation,
            Xna.Vector2 origin,
            Xna.Vector2 scale,
            SpriteEffects effects,
            float layerDepth )
        {
            this.spriteBatch.Draw(
                texture,
                position,
                sourceRectangle,
                color,
                rotation,
                origin,
                scale,
                effects,
                layerDepth
            );
        }

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
        public void DrawString( SpriteFont spriteFont, string text, Vector2 position, Xna.Color color )
        {
            this.spriteBatch.DrawString( spriteFont, text, position, color );
        }

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
        public void DrawString(
            SpriteFont spriteFont,
            string text,
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth )
        {
            this.spriteBatch.DrawString( 
                spriteFont,
                text,
                position,
                color,
                rotation,
                origin,
                scale,
                effects,
                layerDepth
            );
        }

        /// <summary>
        /// Draws a colored rectangle with the specified arguments.
        /// </summary>
        /// <param name="rectangle">
        /// A rectangle specifying, in screen coordinates, where the sprite will be drawn.
        /// If this rectangle is not the same size as sourcerectangle, the sprite is scaled to fit.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use. Use Xna.Color.White for full color with no tinting.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).You must
        /// specify either SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for
        /// this parameter to affect sprite drawing.
        /// </param>
        public void DrawRect( Xna.Rectangle rectangle, Xna.Color color, float layerDepth = 0.0f )
        {
            this.textureBatch.DrawRect( rectangle, color, layerDepth );
        }

        /// <summary>
        /// Draws a colored rectangle with the specified arguments.
        /// </summary>
        /// <param name="rectangle">
        /// A rectangle specifying, in screen coordinates, where the sprite will be drawn.
        /// If this rectangle is not the same size as sourcerectangle, the sprite is scaled to fit.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use. Use Xna.Color.White for full color with no tinting.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).You must
        /// specify either SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for
        /// this parameter to affect sprite drawing.
        /// </param>
        public void DrawRect( Rectangle rectangle, Xna.Color color, float layerDepth = 0.0f )
        {
            this.textureBatch.DrawRect( rectangle, color, layerDepth );
        }

        /// <summary>
        /// Draws a colored rectangle with the specified arguments.
        /// </summary>
        /// <param name="rectangle">
        /// A rectangle specifying, in screen coordinates, where the sprite will be drawn.
        /// If this rectangle is not the same size as sourcerectangle, the sprite is scaled to fit.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use. Use Xna.Color.White for full color with no tinting.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).You must
        /// specify either SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for
        /// this parameter to affect sprite drawing.
        /// </param>
        public void DrawRect( RectangleF rectangle, Xna.Color color, float layerDepth = 0.0f )
        {
            this.textureBatch.DrawRect( rectangle, color, layerDepth );
        }

        /// <summary>
        /// Queues drawing a line by this ITextureDrawBatch.
        /// </summary>
        /// <param name="start">
        /// The start position of the line.
        /// </param>
        /// <param name="end">
        /// The end position of the line.
        /// </param>
        /// <param name="color">
        /// The color of the line.
        /// </param>
        /// <param name="thickness">
        /// The thickness of the line in pixels.
        /// </param>
        /// <param name="layerDepth">
        /// The layer depth the line is drawn at. (Z-index)
        /// </param>
        public void DrawLine( Vector2 start, Vector2 end, Xna.Color color, int thickness = 2, float layerDepth = 1.0f )
        {
            this.textureBatch.DrawLine( start, end, color, thickness, layerDepth );
        }

        /// <summary>
        /// Queues drawing a line by this ITextureDrawBatch.
        /// </summary>
        /// <param name="start">
        /// The start position of the line.
        /// </param>
        /// <param name="end">
        /// The end position of the line.
        /// </param>
        /// <param name="color">
        /// The color of the line.
        /// </param>
        /// <param name="origin">
        /// The origin of rotation.
        /// </param>
        /// <param name="thickness">
        /// The thickness of the line in pixels.
        /// </param>
        /// <param name="layerDepth">
        /// The layer depth the line is drawn at. (Z-index)
        /// </param>
        public void DrawLine( Vector2 start, Vector2 end, Xna.Color color, Vector2 origin, int thickness = 2, float layerDepth = 1.0f )
        {
            this.textureBatch.DrawLine( start, end, color, origin, thickness, layerDepth );
        }

        /// <summary>
        /// Queues drawing a outer shape of a <see cref="Polygon2"/> using lines by this ITextureDrawBatch.
        /// </summary>
        /// <param name="polygon">
        /// The polygon to draw.
        /// </param>
        /// <param name="color">
        /// The color of the lines.
        /// </param>
        /// <param name="thickness">
        /// The thickness of the lines in pixels.
        /// </param>
        /// <param name="layerDepth">
        /// The layer depth the lines are drawn at. (Z-index)
        /// </param>
        public void DrawLinePoly( Polygon2 polygon, Xna.Color color, int thickness = 2, float layerDepth = 1.0f )
        {
            this.textureBatch.DrawLinePoly( polygon, color, thickness, layerDepth );
        }

        /// <summary>
        /// Queues drawing a outer shape of a <see cref="Rectangle"/> using lines by this ITextureDrawBatch.
        /// </summary>
        /// <param name="rectangle">
        /// The rectangle to draw.
        /// </param>
        /// <param name="color">
        /// The color of the lines.
        /// </param>
        /// <param name="thickness">
        /// The thickness of the lines in pixels.
        /// </param>
        /// <param name="layerDepth">
        /// The layer depth the lines are drawn at. (Z-index)
        /// </param>
        public void DrawLineRect( Rectangle rectangle, Xna.Color color, int thickness = 2, float layerDepth = 1.0f )
        {
            this.textureBatch.DrawLineRect( rectangle, color, thickness, layerDepth );
        }

        /// <summary>
        /// Queues drawing a outer shape of a <see cref="OrientedRectangleF"/> using lines by this ITextureDrawBatch.
        /// </summary>
        /// <param name="rectangle">
        /// The rectangle to draw.
        /// </param>
        /// <param name="color">
        /// The color of the lines.
        /// </param>
        /// <param name="thickness">
        /// The thickness of the lines in pixels.
        /// </param>
        /// <param name="layerDepth">
        /// The layer depth the lines are drawn at. (Z-index)
        /// </param>
        public void DrawLineRect( OrientedRectangleF rectangle, Xna.Color color, int thickness = 2, float layerDepth = 1.0f )
        {
            this.textureBatch.DrawLineRect( rectangle, color, thickness, layerDepth );
        }        

        /// <summary>
        /// Provides a mechanism for drawing sprites in various ways.
        /// </summary>
        private readonly ISpriteBatch spriteBatch;

        /// <summary>
        /// Provides a mechanism for drawing various rectangular shapes.
        /// </summary>
        private readonly ITextureDrawBatch textureBatch;
    }
}