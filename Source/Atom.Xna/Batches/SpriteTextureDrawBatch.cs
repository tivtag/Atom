// <copyright file="SpriteTextureDrawBatch.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Batches.SpriteTextureDrawBatch class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Batches
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Implements a mechanism that allows queueing and then drawing of simple 2D lines and objects consisting of 2D lines.
    /// </summary>
    public class SpriteTextureDrawBatch : ITextureDrawBatch
    {
        /// <summary>
        /// Initializes a new instance of the SpriteLineDrawBatcher class.
        /// </summary>
        /// <param name="batch">
        /// Provides a mechanism for drawing sprites.
        /// </param>
        /// <param name="texture">
        /// The texture that is used to draw the lines.
        /// </param>
        public SpriteTextureDrawBatch( ISpriteBatch batch, Texture2D texture )
        {
            Contract.Requires<ArgumentNullException>( batch != null );
            Contract.Requires<ArgumentNullException>( texture != null );

            this.batch = batch;
            this.texture = texture;
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
            var area = new Xna.Rectangle();
            area.X = (int)rectangle.X;
            area.Y = (int)rectangle.Y;
            area.Width = (int)rectangle.Width;
            area.Height = (int)rectangle.Height;

            this.batch.Draw( this.texture, area, color, layerDepth );
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
            this.batch.Draw( this.texture, rectangle, color, layerDepth );
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
            this.batch.Draw( this.texture, rectangle, color, layerDepth );
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
            Vector2 localEnd = (end - start);
            float angle = (float)System.Math.Atan2( localEnd.Y, localEnd.X );
            float distance = Vector2.Distance( start, end );

            Rectangle area = new Rectangle(
                (int)start.X,
                (int)start.Y,
                (int)distance,
                thickness
            );

            this.batch.Draw( 
                this.texture, 
                area, 
                null, 
                color,
                angle,
                Vector2.Zero, // new Vector2( thickness / 2.0f, thickness / 2.0f ),
                SpriteEffects.None,
                layerDepth
            );
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
            Vector2 localEnd = (end - start);
            float angle = (float)System.Math.Atan2( localEnd.Y, localEnd.X );
            float distance = Vector2.Distance( start, end );

            Rectangle area = new Rectangle(
                (int)start.X,
                (int)start.Y,
                (int)distance,
                thickness
            );

            this.batch.Draw(
                this.texture,
                area,
                null,
                color,
                angle,
                origin,
                SpriteEffects.None,
                layerDepth
            );
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
            if( polygon == null || polygon.VertexCount < 3 )
                return;

            for( int index = 0; index < polygon.VertexCount; ++index )
            {
                int nextIndex = polygon.GetNextIndex( index );

                this.DrawLine(
                    polygon[index],
                    polygon[nextIndex],
                    color,
                    thickness,
                    layerDepth
                );                
            }
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
            // Upper
            this.batch.Draw(
                this.texture,
                new Rectangle( rectangle.X, rectangle.Y, rectangle.Width, thickness ),
                color,
                layerDepth
            );

            // Left
            this.batch.Draw(
                this.texture,
                new Rectangle( rectangle.X, rectangle.Y, thickness, rectangle.Height ),
                color,
                layerDepth
            );

            // Right
            this.batch.Draw(
                this.texture,
                new Rectangle( rectangle.X + rectangle.Width, rectangle.Y, thickness, rectangle.Height ),
                color,
                layerDepth
            );

            // Bottom           
            this.batch.Draw(
                this.texture,
                new Rectangle( rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + thickness, thickness ),
                color,
                layerDepth
            );
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
            foreach( var lineSegment in rectangle.LineSegments )
            {
                this.DrawLine( lineSegment.Start, lineSegment.End, color, thickness, layerDepth );
            }
        }        

        /// <summary>
        /// Begins drawing to this IDrawBatch.
        /// </summary>
        /// <param name="drawContext">
        /// The current <see cref="IXnaDrawContext"/>.
        /// </param>
        public void Begin( IXnaDrawContext drawContext )
        {
            this.batch.Begin( drawContext );
        }

        /// <summary>
        /// Ends drawing to this IDrawBatch, outputing the result.
        /// </summary>
        public void End()
        {
            this.batch.End();
        }

        /// <summary>
        /// The batch that is used to queue drawing of the <see cref="texture"/>
        /// in various ways.
        /// </summary>
        private readonly ISpriteBatch batch;

        /// <summary>
        /// The texture that is used to draw the lines.
        /// </summary>
        private readonly Texture2D texture;
    }
}
