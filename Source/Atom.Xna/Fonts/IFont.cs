// <copyright file="IFont.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Fonts.IFont interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Fonts
{
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Provides a mechanism for drawing text.
    /// </summary>
    public interface IFont : IAsset
    {
        /// <summary>
        /// Gets the height of one line of this IFont (in pixels).
        /// </summary>
        int LineSpacing
        {
            get;
        }

        /// <summary>
        /// Gets the size the specified <paramref name="text"/> takes up (in pixels).
        /// </summary>
        /// <param name="text">
        /// The string to measure.
        /// </param>
        /// <returns>
        /// The measured width in pixels.
        /// </returns>
        Vector2 MeasureString( string text );

        /// <summary>
        /// Gets the width the specified <paramref name="text"/> takes up (in pixels).
        /// </summary>
        /// <param name="text">
        /// The string to measure.
        /// </param>
        /// <returns>
        /// The measured width in pixels.
        /// </returns>
        float MeasureStringWidth( string text );

        /// <summary>
        /// Draws the specified <paramref name="text"/> string using the specified parameters.
        /// </summary>
        /// <param name="text">
        /// The string to draw.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the text should be drawn.
        /// </param>
        /// <param name="color">
        /// The desired color of the text.
        /// </param>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        void Draw( string text, Vector2 position, Xna.Color color, ISpriteDrawContext drawContext );

        /// <summary>
        /// Draws the specified <paramref name="text"/> string using the specified parameters.
        /// </summary>
        /// <param name="text">
        /// The string to draw.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the text should be drawn.
        /// </param>
        /// <param name="color">
        /// The desired color of the text.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).
        /// </param>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        void Draw( string text, Vector2 position, Xna.Color color, float layerDepth, ISpriteDrawContext drawContext );

        /// <summary>
        /// Draws the specified <paramref name="text"/> string using the specified parameters.
        /// </summary>
        /// <param name="text">
        /// The text to draw.
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
        /// The effects to apply prior to rendering.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).
        /// </param>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        void Draw(
            string text,
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth,
            ISpriteDrawContext drawContext
        );

        /// <summary>
        /// Draws the specified <paramref name="text"/> string using the specified parameters.
        /// </summary>
        /// <param name="text">
        /// The text to draw.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the text will be drawn.
        /// </param>
        /// <param name="align">
        /// The text alignment mode to use.
        /// </param>
        /// <param name="color">
        /// The desired color of the text.
        /// </param>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        void Draw( string text, Vector2 position, TextAlign align, Xna.Color color, ISpriteDrawContext drawContext );
        
        /// <summary>
        /// Draws the specified <paramref name="text"/> string using the specified parameters.
        /// </summary>
        /// <param name="text">
        /// The text to draw.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the text will be drawn.
        /// </param>
        /// <param name="align">
        /// The text alignment mode to use.
        /// </param>
        /// <param name="color">
        /// The desired color of the text.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).
        /// </param>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        void Draw( string text, Vector2 position, TextAlign align, Xna.Color color, float layerDepth, ISpriteDrawContext drawContext );
        
        /// <summary>
        /// Draws the specified <paramref name="text"/> string using the specified parameters.
        /// </summary>
        /// <param name="text">
        /// The text to draw.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the text will be drawn.
        /// </param>
        /// <param name="align">
        /// The text alignment mode to use.
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
        /// The effects to apply prior to rendering.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).
        /// </param>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        void Draw( 
            string text, 
            Vector2 position,
            TextAlign align, 
            Xna.Color color, 
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects, 
            float layerDepth,
            ISpriteDrawContext drawContext 
        );

        /// <summary>
        /// Draws the given block of text using the specified parameters.
        /// </summary>
        /// <param name="textBlock">
        /// The block of text to draw.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the text will be drawn.
        /// </param>
        /// <param name="align">
        /// The text alignment mode to use.
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
        /// The effects to apply prior to rendering.
        /// </param>
        /// <param name="layerDepth">
        /// The sorting depth of the sprite, between 0 (front) and 1 (back).
        /// </param>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        void DrawBlock(
            string[] textBlock,
            Vector2 position,
            TextAlign align,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth,
            ISpriteDrawContext drawContext
        );
    }
}
