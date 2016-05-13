// <copyright file="ILineBatch.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Batches.ILineBatch interface.
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
    /// Provides a mechanism that allows queueing and then drawing of 2D lines, objects consisting of 2D lines
    /// and rectangles.
    /// </summary>
    public interface ITextureDrawBatch : IDrawBatch
    {
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
        void DrawRect( RectangleF rectangle, Xna.Color color, float layerDepth = 0.0f );

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
        void DrawRect( Rectangle rectangle, Xna.Color color, float layerDepth = 0.0f );

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
        void DrawRect( Xna.Rectangle rectangle, Xna.Color color, float layerDepth = 0.0f );

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
        void DrawLine( Vector2 start, Vector2 end, Xna.Color color, int thickness = 2, float layerDepth = 1.0f );

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
        void DrawLine( Vector2 start, Vector2 end, Xna.Color color, Vector2 origin, int thickness = 2, float layerDepth = 1.0f );
        
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
        void DrawLinePoly( Polygon2 polygon, Xna.Color color, int thickness = 2, float layerDepth = 1.0f );

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
        void DrawLineRect( Rectangle rectangle, Xna.Color color, int thickness = 2, float layerDepth = 1.0f );

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
        void DrawLineRect( OrientedRectangleF rectangle, Xna.Color color, int thickness = 2, float layerDepth = 1.0f );        
    }
}
