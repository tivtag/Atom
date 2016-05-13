// <copyright file="ColorExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ColorExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines extension methods for the <see cref="Color"/> structure.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Returns this Color, but with a different alpha component.
        /// </summary>
        /// <example>
        /// <code>
        /// Color c = Color.Red.WithAlpha( 125 );
        /// </code>
        /// </example>
        /// <param name="color">
        /// The input color.
        /// </param>
        /// <param name="alpha">
        /// The new alpha component.
        /// </param>
        /// <returns>
        /// The resulting color.
        /// </returns>
        public static Color WithAlpha( this Color color, byte alpha )
        {
            return new Color( color.R, color.G, color.B, alpha );
        }

        /// <summary>
        /// Returns the result of multiplying the elements of this Color with the given multipliers.
        /// </summary>
        /// <param name="color">
        /// The input color.
        /// </param>
        /// <param name="redMultiplier">
        /// The multiplier value for the red color component.
        /// </param>
        /// <param name="greenMultiplier">
        /// The multiplier value for the green color component.
        /// </param>
        /// <param name="blueMultiplier">
        /// The multiplier value for the blue color component.
        /// </param>
        /// <returns>
        /// The resulting color.
        /// </returns>
        public static Color MultiplyBy( this Color color, float redMultiplier, float greenMultiplier, float blueMultiplier )
        {
            return new Color(
                (byte)(color.R * redMultiplier),
                (byte)(color.G * greenMultiplier),
                (byte)(color.B * blueMultiplier),
                color.A
            );
        }

        /// <summary>
        /// Returns the result of multiplying the elements of this Color with the given multipliers.
        /// </summary>
        /// <param name="color">
        /// The input color.
        /// </param>
        /// <param name="redMultiplier">
        /// The multiplier value for the red color component.
        /// </param>
        /// <param name="greenMultiplier">
        /// The multiplier value for the green color component.
        /// </param>
        /// <param name="blueMultiplier">
        /// The multiplier value for the blue color component.
        /// </param>
        /// <param name="alphaMultiplier">
        /// The multiplier value for the alpha color component.
        /// </param>
        /// <returns>
        /// The resulting color.
        /// </returns>
        public static Color MultiplyBy( this Color color, float redMultiplier, float greenMultiplier, float blueMultiplier, float alphaMultiplier )
        {
            return new Color(
                (byte)(color.R * redMultiplier),
                (byte)(color.G * greenMultiplier),
                (byte)(color.B * blueMultiplier),
                (byte)(color.A * alphaMultiplier)
            );
        }
    }
}
