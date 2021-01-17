// <copyright file="Font.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Fonts.Font class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Fonts
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Implements a mechanism for drawing text using xna <see cref="SpriteFont"/>s.
    /// </summary>
    public class Font : IFont
    {
        /// <summary>
        /// Gets the name of this Font.
        /// </summary>
        public string Name
        {
            get
            {
                return this.fontName;
            }
        }

        /// <summary>
        /// Gets the height of one line of this Font (in pixels).
        /// </summary>
        public int LineSpacing
        {
            get
            {
                return this.spriteFont.LineSpacing;
            }
        }

        /// <summary>
        /// Gets or sets the underlying XNA SpriteFont object.
        /// </summary>
        public SpriteFont SpriteFont
        {
            get
            {
                return this.spriteFont;
            }

            set
            {
                if( value == null )
                {
                    throw new ArgumentNullException( "value" );
                }

                this.spriteFont = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Font class.
        /// </summary>
        /// <param name="fontName">
        /// The name of the new Font.
        /// </param>
        /// <param name="spriteFont">
        /// The xna SpriteFont the new Font should wrap around.
        /// </param>
        public Font( string fontName, SpriteFont spriteFont )
        {
            Contract.Requires<ArgumentNullException>( fontName != null );
            Contract.Requires<ArgumentNullException>( spriteFont != null );

            this.fontName = fontName;
            this.spriteFont = spriteFont;
        }

        /// <summary>
        /// Gets the size the specified <paramref name="text"/> takes up (in pixels).
        /// </summary>
        /// <param name="text">
        /// The string to measure.
        /// </param>
        /// <returns>
        /// The measured size in pixels.
        /// </returns>
        public Vector2 MeasureString( string text )
        {
            return this.spriteFont.MeasureString( text ).ToAtom();
        }

        /// <summary>
        /// Gets the width the specified <paramref name="text"/> takes up (in pixels).
        /// </summary>
        /// <param name="text">
        /// The string to measure.
        /// </param>
        /// <returns>
        /// The measures width in pixels.
        /// </returns>
        public float MeasureStringWidth( string text )
        {
            return this.spriteFont.MeasureString( text ).X;
        }

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
        public void Draw( string text, Vector2 position, Xna.Color color, ISpriteDrawContext drawContext )
        {
            drawContext.Batch.DrawString( this.spriteFont, text, position, color );
        }

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
        public void Draw( string text, Vector2 position, Xna.Color color, float layerDepth, ISpriteDrawContext drawContext )
        {
            drawContext.Batch.DrawString(
                this.spriteFont, 
                text,
                position,
                color,
                0.0f, 
                Vector2.Zero,
                1.0f, 
                SpriteEffects.None,
                layerDepth
            );
        }

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
        public void Draw(
            string text,
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth,
            ISpriteDrawContext drawContext )
        {
            drawContext.Batch.DrawString( this.spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth ); 
        }

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
        public void Draw( string text, Vector2 position, TextAlign align, Xna.Color color, ISpriteDrawContext drawContext )
        {
            this.Draw(
                text,
                position,
                align,
                color,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f,
                drawContext
            );
        }   

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
        public void Draw(
            string text,
            Vector2 position,
            TextAlign align,
            Xna.Color color,
            float layerDepth,
            ISpriteDrawContext drawContext )
        {
            this.Draw( 
                text,
                position, 
                align,
                color, 
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                layerDepth,
                drawContext 
            );
        }        

        /// <summary>
        /// Draws the given <paramref name="text"/> string using the specified parameters.
        /// </summary>
        /// <param name="text">
        /// The text to draw.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the text will be drawn.
        /// </param>
        /// <param name="align"
        /// >The text alignment mode to use.
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
        public void Draw(
            string text,
            Vector2 position,
            TextAlign align,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth,
            ISpriteDrawContext drawContext )
        {
            var batch = drawContext.Batch;

            switch( align )
            {
                case TextAlign.Center:
                    {
                        var size = spriteFont.MeasureString( text );

                        batch.DrawString(
                            spriteFont,
                            text,
                            new Vector2(
                                (int)(position.X - (size.X / 2.0f)),
                                (int)position.Y
                            ),
                            color,
                            rotation,
                            origin,
                            scale,
                            effects,
                            layerDepth
                        );
                    }
                    break;

                case TextAlign.Right:
                    {
                        var size = spriteFont.MeasureString( text );

                        batch.DrawString(
                            spriteFont,
                            text,
                            new Vector2( (int)(position.X - size.X), (int)position.Y ),
                            color,
                            rotation,
                            origin,
                            scale,
                            effects,
                            layerDepth
                        );
                    }
                    break;

                default:
                case TextAlign.Left:
                    batch.DrawString(
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
                    break;
            }
        }

        /// <summary>
        /// Draws the given block of text using the specified parameters.
        /// </summary>
        /// <param name="textBlock">
        /// The block of text to draw.
        /// </param>
        /// <param name="position">
        /// The location, in screen coordinates, where the text will be drawn.
        /// </param>
        /// <param name="align"
        /// >The text alignment mode to use.
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
        public void DrawBlock(
            string[] textBlock,
            Vector2 position,
            TextAlign align,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth,
            ISpriteDrawContext drawContext )
        {
            if( textBlock == null )
                return;

            Vector2 drawPosition = position;

            for( int index = 0; index < textBlock.Length; ++index )
            {
                this.Draw(
                    textBlock[index],
                    drawPosition,
                    align,
                    color,
                    rotation,
                    origin,
                    scale,
                    effects,
                    layerDepth,
                    drawContext
                );

                drawPosition.Y += spriteFont.LineSpacing;
            }
        }

        /// <summary>
        /// The name of this Font.
        /// </summary>
        private readonly string fontName;

        /// <summary>
        /// The xna SpriteFont this Font wraps around.
        /// </summary>
        private SpriteFont spriteFont;
    }
}
