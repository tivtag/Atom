// <copyright file="MathSerializationContextExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.MathSerializationContextExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Storage;

    /// <summary>
    /// Defines extension methods that allow serialization of Atom.Math types using the <see cref="ISerializationContext"/> class.
    /// </summary>
    public static class MathSerializationContextExtensions
    {
        /// <summary>
        /// Writes/Serializes the given <see cref="Point2"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="point">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Point2 point )
        {
            context.Write( point.X );
            context.Write( point.Y );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Point3"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="point">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Point3 point )
        {
            context.Write( point.X );
            context.Write( point.Y );
            context.Write( point.Z );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Point4"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="point">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Point4 point )
        {
            context.Write( point.X );
            context.Write( point.Y );
            context.Write( point.Z );
            context.Write( point.W );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Vector2"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="vector">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Vector2 vector )
        {
            context.Write( vector.X );
            context.Write( vector.Y );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Vector3"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="vector">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Vector3 vector )
        {
            context.Write( vector.X );
            context.Write( vector.Y );
            context.Write( vector.Z );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Vector4"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="vector">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Vector4 vector )
        {
            context.Write( vector.X );
            context.Write( vector.Y );
            context.Write( vector.Z );
            context.Write( vector.W );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="ComplexVector2"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="vector">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, ComplexVector2 vector )
        {
            context.Write( vector.X );
            context.Write( vector.Y );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Vector"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="vector">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Vector vector )
        {
            Contract.Requires<ArgumentNullException>( vector != null );
            
            context.Write( vector.DimensionCount );

            for( int index = 0; index < vector.DimensionCount; ++index )
            {
                context.Write( vector[index] );
            }
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="ComplexVector"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="vector">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, ComplexVector vector )
        {
            Contract.Requires<ArgumentNullException>( vector != null );

            context.Write( vector.DimensionCount );

            for( int index = 0; index < vector.DimensionCount; ++index )
            {
                context.Write( vector[index] );
            }
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Rectangle"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="rectangle">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Rectangle rectangle )
        {
            context.Write( rectangle.X );
            context.Write( rectangle.Y );
            context.Write( rectangle.Width );
            context.Write( rectangle.Height );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="RectangleF"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="rectangle">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, RectangleF rectangle )
        {
            context.Write( rectangle.X );
            context.Write( rectangle.Y );
            context.Write( rectangle.Width );
            context.Write( rectangle.Height );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Matrix2"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="matrix">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Matrix2 matrix )
        {
            context.Write( matrix.M11 );
            context.Write( matrix.M12 );
            context.Write( matrix.M21 );
            context.Write( matrix.M22 );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Matrix3"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="matrix">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Matrix3 matrix )
        {
            context.Write( matrix.M11 );
            context.Write( matrix.M12 );
            context.Write( matrix.M13 );

            context.Write( matrix.M21 );
            context.Write( matrix.M22 );
            context.Write( matrix.M23 );

            context.Write( matrix.M31 );
            context.Write( matrix.M32 );
            context.Write( matrix.M33 );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Matrix4"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="matrix">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Matrix4 matrix )
        {
            context.Write( matrix.M11 );
            context.Write( matrix.M12 );
            context.Write( matrix.M13 );
            context.Write( matrix.M14 );

            context.Write( matrix.M21 );
            context.Write( matrix.M22 );
            context.Write( matrix.M23 );
            context.Write( matrix.M24 );

            context.Write( matrix.M31 );
            context.Write( matrix.M32 );
            context.Write( matrix.M33 );
            context.Write( matrix.M34 );
            
            context.Write( matrix.M41 );
            context.Write( matrix.M42 );
            context.Write( matrix.M43 );
            context.Write( matrix.M44 );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Matrix"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="matrix">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Matrix matrix )
        {
            Contract.Requires<ArgumentNullException>( matrix != null );

            context.Write( matrix.RowCount );
            context.Write( matrix.ColumnCount );

            for( int row = 0; row < matrix.RowCount; ++row )
            {
                for( int column = 0; column < matrix.ColumnCount; ++column )
                {
                    context.Write( matrix[row, column] );
                }
            }
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="ComplexMatrix"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="matrix">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, ComplexMatrix matrix )
        {
            Contract.Requires<ArgumentNullException>( matrix != null );

            context.Write( matrix.RowCount );
            context.Write( matrix.ColumnCount );

            for( int row = 0; row < matrix.RowCount; ++row )
            {
                for( int column = 0; column < matrix.ColumnCount; ++column )
                {
                    context.Write( matrix[row, column] );
                }
            }
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Complex"/> number using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="complex">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Complex complex )
        {
            context.Write( complex.Real );
            context.Write( complex.Imag );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Circle"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="circle">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Circle circle )
        {
            context.Write( circle.Center );
            context.Write( circle.Radius );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Ray2"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="ray">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Ray2 ray )
        {
            context.Write( ray.Origin );
            context.Write( ray.Direction );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Ray3"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="ray">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Ray3 ray )
        {
            context.Write( ray.Origin );
            context.Write( ray.Direction );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Plane3"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="plane">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Plane3 plane )
        {
            context.Write( plane.Normal );
            context.Write( plane.Distance );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="FloatRange"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="range">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, FloatRange range )
        {
            context.Write( range.Minimum );
            context.Write( range.Maximum );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="IntegerRange"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="range">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, IntegerRange range )
        {
            context.Write( range.Minimum );
            context.Write( range.Maximum );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="VariableFloat"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="variableFloat">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, VariableFloat variableFloat )
        {
            context.Write( variableFloat.Anchor );
            context.Write( variableFloat.Variation );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Triangle2"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="triangle">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Triangle2 triangle )
        {
            context.Write( triangle.A );
            context.Write( triangle.B );
            context.Write( triangle.C );
        }

        /// <summary>
        /// Writes/Serializes the given <see cref="Quaternion"/> using this ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="quaternion">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Quaternion quaternion )
        {
            context.Write( quaternion.X );
            context.Write( quaternion.Y );
            context.Write( quaternion.Z );
            context.Write( quaternion.W );
        }
    }
}
