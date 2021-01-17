// <copyright file="ToXnaMathExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ToXnaMathExtensions class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System;

    /// <summary>
    /// Contains extension methods that convert At0k math objects into their Xna equivalents.
    /// </summary>
    public static class ToXnaMathExtensions
    {
        /// <summary>
        /// Converts the <see cref="Atom.Math.Matrix4"/> into a <see cref="Microsoft.Xna.Framework.Matrix"/>.
        /// </summary>
        /// <param name="matrix">The matrix to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Microsoft.Xna.Framework.Matrix ToXna( this Atom.Math.Matrix4 matrix )
        {
            return new Microsoft.Xna.Framework.Matrix(
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44
            );
        }

        /// <summary>
        /// Converts the <see cref="Atom.Math.Rectangle"/> into a <see cref="Microsoft.Xna.Framework.Rectangle"/>.
        /// </summary>
        /// <param name="rectangle">The rectangle to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Microsoft.Xna.Framework.Rectangle ToXna( this Atom.Math.Rectangle rectangle )
        {
            Microsoft.Xna.Framework.Rectangle converted;

            converted.X      = rectangle.X;
            converted.Y      = rectangle.Y;
            converted.Width  = rectangle.Width;
            converted.Height = rectangle.Height;

            return converted;
        }

        /// <summary>
        /// Converts the <see cref="Atom.Math.RectangleF"/> into a <see cref="Microsoft.Xna.Framework.Rectangle"/>.
        /// </summary>
        /// <param name="rectangle">The rectangle to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Microsoft.Xna.Framework.Rectangle ToXna( this Atom.Math.RectangleF rectangle )
        {
            Microsoft.Xna.Framework.Rectangle converted;

            converted.X      = (int)rectangle.X;
            converted.Y      = (int)rectangle.Y;
            converted.Width  = (int)rectangle.Width;
            converted.Height = (int)rectangle.Height;

            return converted;
        }

        /// <summary>
        /// Converts the <see cref="Atom.Math.Point2"/> into a <see cref="Microsoft.Xna.Framework.Point"/>.
        /// </summary>
        /// <param name="point">The point to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Microsoft.Xna.Framework.Point ToXna( this Atom.Math.Point2 point )
        {
            return new Microsoft.Xna.Framework.Point( point.X, point.Y );
        }

        /// <summary>
        /// Converts the <see cref="Atom.Math.Vector2"/> into a <see cref="Microsoft.Xna.Framework.Vector2"/>.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Microsoft.Xna.Framework.Vector2 ToXna( this Atom.Math.Vector2 vector )
        {
            return new Microsoft.Xna.Framework.Vector2( vector.X, vector.Y );
        }

        /// <summary>
        /// Converts the <see cref="Atom.Math.Vector3"/> into a <see cref="Microsoft.Xna.Framework.Vector3"/>.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Microsoft.Xna.Framework.Vector3 ToXna( this Atom.Math.Vector3 vector )
        {
            return new Microsoft.Xna.Framework.Vector3( vector.X, vector.Y, vector.Z );
        }

        /// <summary>
        /// Converts the <see cref="Atom.Math.Vector4"/> into a <see cref="Microsoft.Xna.Framework.Vector4"/>.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Microsoft.Xna.Framework.Vector4 ToXna( this Atom.Math.Vector4 vector )
        {
            return new Microsoft.Xna.Framework.Vector4( vector.X, vector.Y, vector.Z, vector.W );
        }
    }
}
