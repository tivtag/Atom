// <copyright file="ToAtomMathExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ToAtomMathExtensions class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System;

    /// <summary>
    /// Contains extension methods that convert Xna math objects into their Atom equivalents.
    /// </summary>
    public static class ToAtomMathExtensions
    {
        /// <summary>
        /// Converts the <see cref="Microsoft.Xna.Framework.Matrix"/> into a <see cref="Atom.Math.Matrix4"/>.
        /// </summary>
        /// <param name="matrix">The matrix to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Atom.Math.Matrix4 ToAtom( this Microsoft.Xna.Framework.Matrix matrix )
        {
            return new Atom.Math.Matrix4(
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44
            );
        }

        /// <summary>
        /// Converts the <see cref="Microsoft.Xna.Framework.Point"/> into a <see cref="Atom.Math.Point2"/>.
        /// </summary>
        /// <param name="point">The point to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Atom.Math.Point2 ToAtom( this Microsoft.Xna.Framework.Point point )
        {
            return new Atom.Math.Point2( point.X, point.Y );
        }

        /// <summary>
        /// Converts the <see cref="Microsoft.Xna.Framework.Vector2"/> into a <see cref="Atom.Math.Vector2"/>.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Atom.Math.Vector2 ToAtom( this Microsoft.Xna.Framework.Vector2 vector )
        {
            return new Atom.Math.Vector2( vector.X, vector.Y );
        }

        /// <summary>
        /// Converts the <see cref="Microsoft.Xna.Framework.Vector3"/> into a <see cref="Atom.Math.Vector3"/>.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Atom.Math.Vector3 ToAtom( this Microsoft.Xna.Framework.Vector3 vector )
        {
            return new Atom.Math.Vector3( vector.X, vector.Y, vector.Z );
        }

        /// <summary>
        /// Converts the <see cref="Microsoft.Xna.Framework.Vector4"/> into a <see cref="Atom.Math.Vector4"/>.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Atom.Math.Vector4 ToAtom( this Microsoft.Xna.Framework.Vector4 vector )
        {
            return new Atom.Math.Vector4( vector.X, vector.Y, vector.Z, vector.W );
        }

        /// <summary>
        /// Converts the <see cref="Microsoft.Xna.Framework.Rectangle"/> into a <see cref="Atom.Math.Rectangle"/>.
        /// </summary>
        /// <param name="rectangle">The vector to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Atom.Math.Rectangle ToAtom( this Microsoft.Xna.Framework.Rectangle rectangle )
        {
            return new Atom.Math.Rectangle( rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height );
        }
    }
}
