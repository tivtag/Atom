// <copyright file="ColorHelper.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ColorHelper class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>


namespace Atom.Xna
{
    using Atom.Math;
    using Xna = Microsoft.Xna.Framework;
    
    /// <summary>
    /// Defines <see cref="Xna.Color"/> related helper methods.
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// Linearly interpolates between the specified start and end color.
        /// </summary>
        /// <param name="start">
        /// The source color.
        /// </param>
        /// <param name="end">
        /// The destination color.
        /// </param>
        /// <param name="amount">
        /// The interpolation factor - a value between 0 and 1.
        /// </param>
        /// <returns>
        /// The interpolated Xna.Color.
        /// </returns>
        public static Xna.Color Lerp( Xna.Color start, Xna.Color end, float amount )
        {
            return new Xna.Color(
                MathUtilities.Lerp( start.R, end.R, amount ),
                MathUtilities.Lerp( start.G, end.G, amount ),
                MathUtilities.Lerp( start.B, end.B, amount ),
                MathUtilities.Lerp( start.A, end.A, amount )
            );
        }

        /// <summary>
        /// Creates a Xna.Color with a single <paramref name="component"/> set to <paramref name="value"/>, 
        /// the other <see cref="ColorComponent"/> set to <paramref name="otherValue"/> and
        /// the <see cref="ColorComponent.Alpha"/> set to <paramref name="alphaValue"/>.
        /// </summary>
        /// <param name="value">
        /// The value of the <paramref name="component"/>.
        /// </param>
        /// <param name="component">
        /// The component to set to <paramref name="value"/>.
        /// </param>
        /// <param name="otherValue">
        /// The value of the other <see cref="ColorComponent"/>s.
        /// </param>
        /// <param name="alphaValue">
        /// The value of <see cref="ColorComponent.Alpha"/>.
        /// </param>
        /// <returns>
        /// The newly created color.
        /// </returns>
        public static Xna.Color Create( byte value, ColorComponent component, byte otherValue = 0, byte alphaValue = 255 )
        {
            switch( component )
            {
                case ColorComponent.Red:
                    return new Xna.Color( value, otherValue, otherValue, alphaValue );

                case ColorComponent.Green:
                    return new Xna.Color( otherValue, value, otherValue, alphaValue );

                case ColorComponent.Blue:
                    return new Xna.Color( otherValue, otherValue, value, alphaValue );

                case ColorComponent.Alpha:
                    return new Xna.Color( otherValue, otherValue, otherValue, value );

                default:
                    return new Xna.Color( otherValue, otherValue, otherValue, alphaValue );
            }
        }

        /// <summary>
        /// Inverts the given color.
        /// </summary>
        /// <param name="color">
        /// The color to invert.
        /// </param>
        /// <returns>
        /// The inverted color.
        /// </returns>
        public static Xna.Color InvertRGB( Xna.Color color )
        {
            return new Xna.Color( 255 - color.R, 255 - color.G, 255 - color.B, color.A );
        }
    }
}
