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
    using System.Diagnostics.Contracts;
    using Atom.Storage;

    /// <summary>
    /// Defines extension methods that allow serialization of Atom.Math types using the <see cref="IDeserializationContext"/> class.
    /// </summary>
    public static class MathDeserializationContextExtensions
    {      
        /// <summary>
        /// Reads a <see cref="Point2"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Point2 value that has been read.
        /// </returns>
        public static Point2 ReadPoint2( this IDeserializationContext context )
        {
            int x = context.ReadInt32();
            int y = context.ReadInt32();

            return new Point2( x, y );
        }

        /// <summary>
        /// Reads a <see cref="Point3"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Point3 value that has been read.
        /// </returns>
        public static Point3 ReadPoint3( this IDeserializationContext context )
        {
            int x = context.ReadInt32();
            int y = context.ReadInt32();
            int z = context.ReadInt32();

            return new Point3( x, y, z );
        }

        /// <summary>
        /// Reads a <see cref="Point4"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Point4 value that has been read.
        /// </returns>
        public static Point4 ReadPoint4( this IDeserializationContext context )
        {
            int x = context.ReadInt32();
            int y = context.ReadInt32();
            int z = context.ReadInt32();
            int w = context.ReadInt32();

            return new Point4( x, y, z, w );
        }

        /// <summary>
        /// Reads a <see cref="Vector2"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Vector2 value that has been read.
        /// </returns>
        public static Vector2 ReadVector2( this IDeserializationContext context )
        {
            float x = context.ReadSingle();
            float y = context.ReadSingle();

            return new Vector2( x, y );
        }

        /// <summary>
        /// Reads a <see cref="Vector3"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Vector3 value that has been read.
        /// </returns>
        public static Vector3 ReadVector3( this IDeserializationContext context )
        {
            float x = context.ReadSingle();
            float y = context.ReadSingle();
            float z = context.ReadSingle();

            return new Vector3( x, y, z );
        }

        /// <summary>
        /// Reads a <see cref="Vector4"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Vector4 value that has been read.
        /// </returns>
        public static Vector4 ReadVector4( this IDeserializationContext context )
        {
            float x = context.ReadSingle();
            float y = context.ReadSingle();
            float z = context.ReadSingle();
            float w = context.ReadSingle();

            return new Vector4( x, y, z, w );
        }

        /// <summary>
        /// Reads a <see cref="ComplexVector2"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The ComplexVector2 value that has been read.
        /// </returns>
        public static ComplexVector2 ReadComplexVector2( this IDeserializationContext context)
        {
            Complex x = context.ReadComplex();
            Complex y = context.ReadComplex();

            return new ComplexVector2( x, y );
        }

        /// <summary>
        /// Reads a <see cref="Vector"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Vector value that has been read.
        /// </returns>
        public static Vector ReadVector( this IDeserializationContext context )
        {
            int dimensionCount = context.ReadInt32();

            Vector vector = new Vector( dimensionCount );
            
            for( int index = 0; index < dimensionCount; ++index )
            {
                vector[index] = context.ReadSingle();
            }

            return vector;
        }

        /// <summary>
        /// Reads a <see cref="ComplexVector"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The ComplexVector value that has been read.
        /// </returns>
        public static ComplexVector ReadComplexVector( this IDeserializationContext context )
        {
            int dimensionCount = context.ReadInt32();

            ComplexVector vector = new ComplexVector( dimensionCount );

            for( int index = 0; index < dimensionCount; ++index )
            {
                vector[index] = context.ReadComplex();
            }

            return vector;
        }

        /// <summary>
        /// Reads a <see cref="Rectangle"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Rectangle value that has been read.
        /// </returns>
        public static Rectangle ReadRectangle( this IDeserializationContext context )
        {
            int x = context.ReadInt32();
            int y = context.ReadInt32();
            int width = context.ReadInt32();
            int height = context.ReadInt32();

            return new Rectangle( x, y, width, height );
        }

        /// <summary>
        /// Reads a <see cref="RectangleF"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The RectangleF value that has been read.
        /// </returns>
        public static RectangleF ReadRectangleF( this IDeserializationContext context )
        {
            float x = context.ReadSingle();
            float y = context.ReadSingle();
            float width = context.ReadSingle();
            float height = context.ReadSingle();

            return new RectangleF( x, y, width, height );
        }

        /// <summary>
        /// Reads a <see cref="Matrix2"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Matrix2 value that has been read.
        /// </returns>
        public static Matrix2 ReadMatrix2( this IDeserializationContext context )
        {
            float m11 = context.ReadSingle();
            float m12 = context.ReadSingle();
            float m21 = context.ReadSingle();
            float m22 = context.ReadSingle();

            return new Matrix2( m11, m12, m21, m22 );
        }

        /// <summary>
        /// Reads a <see cref="Matrix3"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Matrix3 value that has been read.
        /// </returns>
        public static Matrix3 ReadMatrix3( this IDeserializationContext context )
        {
            float m11 = context.ReadSingle();
            float m12 = context.ReadSingle();
            float m13 = context.ReadSingle();

            float m21 = context.ReadSingle();
            float m22 = context.ReadSingle();
            float m23 = context.ReadSingle();
            
            float m31 = context.ReadSingle();
            float m32 = context.ReadSingle();
            float m33 = context.ReadSingle();

            return new Matrix3(
                m11,
                m12,
                m13,
                m21,
                m22,
                m23,
                m31,
                m32,
                m33
            );
        }

        /// <summary>
        /// Reads a <see cref="Matrix4"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Matrix4 value that has been read.
        /// </returns>
        public static Matrix4 ReadMatrix4( this IDeserializationContext context )
        {
            float m11 = context.ReadSingle();
            float m12 = context.ReadSingle();
            float m13 = context.ReadSingle();
            float m14 = context.ReadSingle();

            float m21 = context.ReadSingle();
            float m22 = context.ReadSingle();
            float m23 = context.ReadSingle();
            float m24 = context.ReadSingle();

            float m31 = context.ReadSingle();
            float m32 = context.ReadSingle();
            float m33 = context.ReadSingle();
            float m34 = context.ReadSingle();

            float m41 = context.ReadSingle();
            float m42 = context.ReadSingle();
            float m43 = context.ReadSingle();
            float m44 = context.ReadSingle();

            return new Matrix4(
                m11,
                m12,
                m13,
                m14,
                m21,
                m22,
                m23,
                m24,
                m31,
                m32,
                m33,
                m34,
                m41,
                m42,
                m43,
                m44
            );
        }
        
        /// <summary>
        /// Reads a <see cref="Matrix"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Matrix value that has been read.
        /// </returns>
        public static Matrix ReadMatrix( this IDeserializationContext context )
        {
            int rowCount = context.ReadInt32();
            int columnCount = context.ReadInt32();

            Matrix matrix = new Matrix( rowCount, columnCount );

            for( int row = 0; row < rowCount; ++row )
            {
                for( int column = 0; column < columnCount; ++column )
                {
                    matrix[row, column] = context.ReadSingle();
                }
            }

            return matrix;
        }

        /// <summary>
        /// Reads a <see cref="ComplexMatrix"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The ComplexMatrix value that has been read.
        /// </returns>
        public static ComplexMatrix ReadComplexMatrix( this IDeserializationContext context )
        {
            int rowCount = context.ReadInt32();
            int columnCount = context.ReadInt32();

            ComplexMatrix matrix = new ComplexMatrix( rowCount, columnCount );

            for( int row = 0; row < rowCount; ++row )
            {
                for( int column = 0; column < columnCount; ++column )
                {
                    matrix[row, column] = context.ReadComplex();
                }
            }

            return matrix;
        }

        /// <summary>
        /// Reads a <see cref="Complex"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Complex value that has been read.
        /// </returns>
        public static Complex ReadComplex( this IDeserializationContext context )
        {
            float real = context.ReadSingle();
            float imag = context.ReadSingle();

            return new Complex( real, imag );
        }

        /// <summary>
        /// Reads a <see cref="Circle"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Circle value that has been read.
        /// </returns>
        public static Circle ReadCircle( this IDeserializationContext context )
        {
            Vector2 center = context.ReadVector2();
            float radius = context.ReadSingle();

            return new Circle( center, radius );
        }

        /// <summary>
        /// Reads a <see cref="Ray2"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Ray2 value that has been read.
        /// </returns>
        public static Ray2 ReadRay2( this IDeserializationContext context )
        {
            Vector2 origin = context.ReadVector2();
            Vector2 direction = context.ReadVector2();

            return new Ray2( origin, direction );
        }

        /// <summary>
        /// Reads a <see cref="Ray3"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Ray3 value that has been read.
        /// </returns>
        public static Ray3 ReadRay3( this IDeserializationContext context )
        {
            Vector3 origin = context.ReadVector3();
            Vector3 direction = context.ReadVector3();

            return new Ray3( origin, direction );
        }

        /// <summary>
        /// Reads a <see cref="Plane3"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Plane3 value that has been read.
        /// </returns>
        public static Plane3 ReadPlane3( this IDeserializationContext context )
        {
            Vector3 normal = context.ReadVector3();
            float distance = context.ReadSingle();

            return new Plane3( normal, distance );
        }

        /// <summary>
        /// Reads a <see cref="FloatRange"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The FloatRange value that has been read.
        /// </returns>
        public static FloatRange ReadFloatRange( this IDeserializationContext context )
        {
            float minimum = context.ReadSingle();
            float maximum = context.ReadSingle();

            return new FloatRange( minimum, maximum );
        }

        /// <summary>
        /// Reads a <see cref="IntegerRange"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The IntegerRange value that has been read.
        /// </returns>
        public static IntegerRange ReadIntegerRange( this IDeserializationContext context )
        {
            int minimum = context.ReadInt32();
            int maximum = context.ReadInt32();

            return new IntegerRange( minimum, maximum );
        }

        /// <summary>
        /// Reads a <see cref="VariableFloat"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The VariableFloat value that has been read.
        /// </returns>
        public static VariableFloat ReadVariableFloat( this IDeserializationContext context )
        {
            float anchor = context.ReadSingle();
            float variation = context.ReadSingle();

            return new VariableFloat( anchor, variation );
        }

        /// <summary>
        /// Reads a <see cref="Triangle2"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Triangle2 value that has been read.
        /// </returns>
        public static Triangle2 ReadTriangle2( this IDeserializationContext context )
        {
            Vector2 a = context.ReadVector2();
            Vector2 b = context.ReadVector2();
            Vector2 c = context.ReadVector2();

            return new Triangle2( a, b, c );
        }

        /// <summary>
        /// Reads a <see cref="Quaternion"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Quaternion value that has been read.
        /// </returns>
        public static Quaternion ReadQuaternion( this IDeserializationContext context )
        {
            float x = context.ReadSingle();
            float y = context.ReadSingle();
            float z = context.ReadSingle();
            float w = context.ReadSingle();

            return new Quaternion( x, y, z, w );
        }
    }
}
