// <copyright file="MathExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.MathExtensions class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    /// <summary>
    /// Static class that contains utility methods
    /// that are indendet to only be used as extension methods.
    /// </summary>
    /// <remarks>
    /// The class <see cref="MathUtilities"/> may also
    /// contain extension methods, these are not indendet to be only used as extensions tho.
    /// </remarks>
    public static class MathExtensions
    {
        /// <summary>
        /// Returns whether the value is approximately equal to the given <paramref name="value"/>.
        /// </summary>
        /// <remarks>
        /// A tolerence of 0.00001f is used.
        /// </remarks>
        /// <param name="thisValue">The first value to compare.</param>
        /// <param name="value">The second value to compare.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the values are approximately equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsApproximate( this float thisValue, float value )
        {
            const float Tolerance = .00001f;
            return System.Math.Abs( value - thisValue ) <= Tolerance;
        }

        /// <summary>
        /// Returns whether the value is approximately equal to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="thisValue">The first value to compare.</param>
        /// <param name="value">The second value to compare.</param>
        /// <param name="tolerance">
        /// The tolerence the values can differ and still stand as equal.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the values are approximately equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsApproximate( this float thisValue, float value, float tolerance )
        {
            return System.Math.Abs( value - thisValue ) <= tolerance;
        }

        /// <summary>
        /// Returns whether the value is approximately equal to the given <paramref name="value"/>.
        /// </summary>
        /// <remarks>
        /// A tolerence of 0.00001 is used.
        /// </remarks>
        /// <param name="thisValue">The first value to compare.</param>
        /// <param name="value">The second value to compare.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the values are approximately equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsApproximate( this double thisValue, double value )
        {
            const double Tolerance = .00001;
            return System.Math.Abs( value - thisValue ) <= Tolerance;
        }

        /// <summary>
        /// Returns whether the value is approximately equal to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="thisValue">The first value to compare.</param>
        /// <param name="value">The second value to compare.</param>
        /// <param name="tolerance">
        /// The tolerence the values can differ and still stand as equal.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the values are approximately equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsApproximate( this double thisValue, double value, double tolerance )
        {
            return System.Math.Abs( value - thisValue ) <= tolerance;
        }

        /// <summary>
        /// Returns whether the value is approximately equal to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="thisValue">The first value to compare.</param>
        /// <param name="value">The second value to compare.</param>
        /// <param name="tolerance">
        /// The tolerence the values can differ and still stand as equal.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the values are approximately equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsApproximate( this Vector2 thisValue, Vector2 value, float tolerance )
        {
            return 
                thisValue.X.IsApproximate( value.X, tolerance ) && 
                thisValue.Y.IsApproximate( value.Y, tolerance );
        }
    }
}
