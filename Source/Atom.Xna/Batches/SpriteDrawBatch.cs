// <copyright file="SpriteDrawBatch.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Batches.SpriteDrawBatch class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.Batches
{
    using System;
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;
    
    /// <summary>
    /// 
    /// </summary>
    public class SpriteDrawBatch : DrawBatchBase, ISpriteBatch
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="SpriteDrawBatch"/> calls SpriteBatch.Begin and SpriteBatch.End
        /// when Begin and End are called.
        /// </summary>
        /// <value>
        /// The default value is true.
        /// </value>
        public bool ShouldBeginEndSpriteBatch
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get
            {
                return this.batch; 
            }
        }

        /// <summary>
        /// Initializes a new instance of the SpriteDrawBatch class.
        /// </summary>
        /// <param name="batch">
        /// The <see cref="SpriteBatch"/> the new SpriteDrawBatch uses behind the scenes.
        /// </param>
        public SpriteDrawBatch( SpriteBatch batch )
        {
            this.batch = batch;
            this.ShouldBeginEndSpriteBatch = true;
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
            this.blendState = blendState;
            this.samplerState = samplerState;
            this.sortMode = sortMode;
            this.transform = transform.ToXna();
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
            this.blendState = blendState;
            this.samplerState = samplerState;
            this.sortMode = sortMode;
            this.transform = transform;
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
            this.batch.Draw(
                texture,
                position.ToXna(),
                null,
                color,
                0.0f,
                Xna.Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                layerDepth
            );
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
            this.batch.Draw(
                texture,
                rectangle.ToXna(),
                null,
                color,
                0.0f,
                Xna.Vector2.Zero,
                SpriteEffects.None,
                layerDepth
            );
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
            this.batch.Draw(
                texture,
                rectangle,
                null,
                color,
                0.0f,
                Xna.Vector2.Zero,
                SpriteEffects.None,
                layerDepth
            );
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
            this.batch.Draw(
                texture,
                destinationRectangle,
                sourceRectangle,
                color
            );
        }

        /// <summary>
        /// Draws a sprite, using the specifying the texture,
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
            this.batch.Draw(
                texture,
                destinationRectangle.ToXna(),
                sourceRectangle.HasValue ? sourceRectangle.Value.ToXna() : new Nullable<Xna.Rectangle>(),
                color,
                rotation,
                origin.ToXna(),
                effects,
                layerDepth
            );
        }

        /// <summary>
        /// Draws a sprite, using the specifying the texture,
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
            this.batch.Draw(
                texture,
                destinationRectangle.ToXna(),
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
            this.batch.Draw(
                texture,
                position.ToXna(),
                sourceRectangle.HasValue ? sourceRectangle.Value.ToXna() : new Nullable<Xna.Rectangle>(),
                color,
                rotation,
                origin.ToXna(),
                scale.ToXna(),
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
            this.batch.Draw(
                texture,
                position.ToXna(),
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
            this.batch.Draw(
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
            this.batch.DrawString( spriteFont, text, position.ToXna(), color );
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
            this.batch.DrawString(
                spriteFont,
                text,
                position.ToXna(),
                color,
                rotation,
                origin.ToXna(),
                scale,
                effects,
                layerDepth
            );
        }

        /// <summary>
        /// Begins drawing to this IDrawBatch.
        /// </summary>
        protected override void BeginCore()
        {
            if( this.ShouldBeginEndSpriteBatch )
            {
                this.batch.Begin( this.sortMode, this.blendState, this.samplerState, null, null, null, this.transform );
            }
        }

        /// <summary>
        /// Ends drawing to this IDrawBatch, outputing the result.
        /// </summary>
        protected override void EndCore()
        {
            if( this.ShouldBeginEndSpriteBatch )
            {
                this.batch.End();
            }
        }

        /// <summary>
        /// The <see cref="SpriteBatch"/> this SpriteDrawBatch uses behind the scenes.
        /// </summary>
        private SpriteBatch batch;

        /// <summary>
        /// Represents the storage field of the BlendMode property.
        /// </summary>
        private BlendState blendState = BlendState.AlphaBlend;

        /// <summary>
        /// Represents the storage field of the SamplerState property.
        /// </summary>
        private SamplerState samplerState = SamplerState.PointClamp;

        /// <summary>        
        /// Represents the storage field of the SortMode property.        
         /// </summary>
        private SpriteSortMode sortMode = SpriteSortMode.FrontToBack;

        /// <summary>
        /// Represents the storage field of the Transform property.
        /// </summary>
        private Xna.Matrix transform = Xna.Matrix.Identity;
    }
}
