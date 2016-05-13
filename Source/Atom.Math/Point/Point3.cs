// <copyright file="Point3.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Point3 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Represents a point in three dimensional space.
    /// </summary>
    [Serializable]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.Point3Converter ) )]
    public struct Point3 : IEquatable<Point3>, ICultureSensitiveToStringProvider
    {
        #region [ Fields ]

        /// <summary>
        /// The X-coordinate of the Point.
        /// </summary>
        public int X;

        /// <summary>
        /// The Y-coordinate of the Point.
        /// </summary>
        public int Y;

        /// <summary>
        /// The Z-coordinate of the Point.
        /// </summary>
        public int Z;

        #endregion

        #region [ Constants ]

        /// <summary>
        /// Gets a <see cref="Point3"/> with the coordinates (0, 0).
        /// </summary>
        /// <value>The point (0, 0, 0).</value>
        public static Point3 Zero 
        { 
            get 
            { 
                return new Point3(); 
            } 
        }

        /// <summary>
        /// Gets a <see cref="Point3"/> with the coordinates (1, 1, 1).
        /// </summary>
        /// <value>The point (1, 1, 1).</value>
        public static Point3 One 
        { 
            get
            { 
                return new Point3( 1, 1, 1 );
            }
        }

        /// <summary>
        /// Gets a <see cref="Point3"/> with the coordinates (1, 0, 0).
        /// </summary>
        /// <value>The point (1, 0, 0).</value>
        public static Point3 UnitX 
        { 
            get
            { 
                return new Point3( 1, 0, 0 );
            }
        }

        /// <summary>
        /// Gets a <see cref="Point3"/> with the coordinates (0, 1, 0).
        /// </summary>
        /// <value>The point (0, 1, 0).</value>
        public static Point3 UnitY 
        { 
            get 
            { 
                return new Point3( 0, 1, 0 );
            } 
        }

        /// <summary>
        /// Gets a <see cref="Point3"/> with the coordinates (0, 0, 1).
        /// </summary>
        /// <value>The point (0, 0, 1).</value>
        public static Point3 UnitZ 
        { 
            get 
            { 
                return new Point3( 0, 0, 1 ); 
            } 
        }

        #endregion

        #region [ Properties ]
        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Point3"/> structure.
        /// </summary>
        /// <param name="x">The X-coordinate of the new Point.</param>
        /// <param name="y">The Y-coordinate of the new Point.</param>
        /// <param name="z">The Z-coordinate of the new Point.</param>
        public Point3( int x, int y, int z )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        #endregion

        #region [ Methods ]

        #region > Operators <

        #region Add

        /// <summary>
        /// Returns the result of adding the <paramref name="right"/> Vector to the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 Add( Point3 left, Point3 right )
        {
            Point3 result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the <paramref name="right"/> Vector to the <paramref name="left"/> Vector
        /// in the given Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Point3 left, ref Point3 right, out Point3 result )
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 Add( Point3 point, int scalar )
        {
            Point3 result;

            result.X = point.X + scalar;
            result.Y = point.Y + scalar;
            result.Z = point.Z + scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the given <paramref name="scalar"/> to the given <paramref name="point"/>
        /// in the given Vector.
        /// </summary>
        /// <param name="point">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation. </param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Point3 point, int scalar, out Point3 result )
        {
            result.X = point.X + scalar;
            result.Y = point.Y + scalar;
            result.Z = point.Z + scalar;
        }

        /// <summary>
        /// This method returns the specified Vector.
        /// </summary>
        /// <remarks>
        /// Is equal to "+point".
        /// </remarks>
        /// <param name="point">The point.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 Plus( Point3 point )
        {
            return point;
        }

        /// <summary>
        /// This method stores the specified Vector in the specified result value.
        /// </summary>
        /// <remarks>
        /// Is equal to "+point".
        /// </remarks>
        /// <param name="point">The point.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Plus( ref Point3 point, out Point3 result )
        {
            result.X = point.X;
            result.Y = point.Y;
            result.Z = point.Z;
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Vector from the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 Subtract( Point3 left, Point3 right )
        {
            Point3 result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the <paramref name="right"/> Vector frpm the <paramref name="left"/> Vector
        /// in the given Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Point3 left, ref Point3 right, out Point3 result )
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 Subtract( Point3 point, int scalar )
        {
            Point3 result;

            result.X = point.X - scalar;
            result.Y = point.Y - scalar;
            result.Z = point.Z - scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="point"/>
        /// in the given Vector.
        /// </summary>
        /// <param name="point">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Point3 point, int scalar, out Point3 result )
        {
            result.X = point.X - scalar;
            result.Y = point.Y - scalar;
            result.Z = point.Z - scalar;
        }

        #endregion

        #region Negate

        /// <summary>
        /// Returns the result of negating the elements of the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">
        /// The point to negate.
        /// </param>
        /// <returns>The result of the operation.</returns>
        public static Point3 Negate( Point3 point )
        {
            Point3 result;

            result.X = -point.X;
            result.Y = -point.Y;
            result.Z = -point.Z;

            return result;
        }

        /// <summary>
        /// Stores the result of negating the elements of the given <paramref name="point"/> in the given Vector.
        /// </summary>
        /// <param name="point">
        /// The point to negate. This value will not be modified by this method.
        /// </param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Negate( ref Point3 point, out Point3 result )
        {
            result.X = -point.X;
            result.Y = -point.Y;
            result.Z = -point.Z;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Returns the result of multiplying the given <paramref name="point"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 Multiply( Point3 point, int scalar )
        {
            Point3 result;

            result.X = point.X * scalar;
            result.Y = point.Y * scalar;
            result.Z = point.Z * scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the given <paramref name="point"/> by the given <paramref name="scalar"/>.
        /// in the given Vector.
        /// </summary>
        /// <param name="point">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Multiply( ref Point3 point, int scalar, out Point3 result )
        {
            result.X = point.X * scalar;
            result.Y = point.Y * scalar;
            result.Z = point.Z * scalar;
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The point on the left side of the equation.</param>
        /// <param name="right">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 Multiply( Point3 left, Point3 right )
        {
            Point3 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the left Vector by the right Vector component-by-component.
        /// in the given result Vector.
        /// </summary>
        /// <param name="left">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The point on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Multiply( ref Point3 left, ref Point3 right, out Point3 result )
        {
            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Returns the result of dividing the given <paramref name="point"/> through the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 Divide( Point3 point, int scalar )
        {
            Point3 result;

            result.X = point.X / scalar;
            result.Y = point.Y / scalar;
            result.Z = point.Z / scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the given <paramref name="point"/> through the given <paramref name="scalar"/>
        /// in the given result Vector.
        /// </summary>
        /// <param name="point">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Divide( ref Point3 point, int scalar, out Point3 result )
        {
            result.X = point.X / scalar;
            result.Y = point.Y / scalar;
            result.Z = point.Z / scalar;
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The point on the left side of the equation.</param>
        /// <param name="right">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 Divide( Point3 left, Point3 right )
        {
            Point3 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the left Vector through the right Vector component-by-component.
        /// in the given result Vector.
        /// </summary>
        /// <param name="left">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The point on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will store the result of the operation.</param>
        public static void Divide( ref Point3 left, ref Point3 right, out Point3 result )
        {
            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether the given <see cref="Point3"/> has the
        /// same indices set as this Point3.
        /// </summary>
        /// <param name="other">The Point3 to test against.</param>
        /// <returns>true if the indices are equal; otherwise false.</returns>
        public bool Equals( Point3 other )
        {
            return this.X == other.X &&
                   this.Y == other.Y &&
                   this.Z == other.Z;
        }

        /// <summary>
        /// Returns whether the given <see cref="Object"/> is equal to this Point3.
        /// </summary>
        /// <param name="obj">The Object to test against.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public override bool Equals( object obj )
        {
            if( obj is Point3 )
            {
                return Equals( (Point3)obj );
            }
            else if( obj is Vector3 )
            {
                Vector3 point = (Vector3)obj;

                return point.X.IsApproximate( (float)this.X ) &&
                       point.Y.IsApproximate( (float)this.Y ) &&
                       point.Z.IsApproximate( (float)this.Z );
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overriden to return a human-readable text representation of the Point.
        /// </summary>
        /// <returns>A human-readable text representation of the Point.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Overriden to return a human-readable text representation of the Point
        /// using the given formatting information provider.
        /// </summary>
        /// <param name="formatProvider">Provides culture-sensitive formatting information.</param>
        /// <returns>A human-readable text representation of the Point.</returns>
        public string ToString( IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider,
                "[{0} {1} {2}]",
                X.ToString( formatProvider ),
                Y.ToString( formatProvider ),
                 Z.ToString( formatProvider ) 
            );
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Overriden to return the hashcode of the <see cref="Point3"/>.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.X );
            hashBuilder.AppendStruct( this.Y );
            hashBuilder.AppendStruct( this.Z );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        #region +

        /// <summary>
        /// Returns the result of adding the <paramref name="right"/> Vector to the the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 operator +( Point3 left, Point3 right )
        {
            Point3 result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;

            return result;
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the thegiven <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 operator +( Point3 point, int scalar )
        {
            Point3 result;

            result.X = point.X + scalar;
            result.Y = point.Y + scalar;
            result.Z = point.Z + scalar;

            return result;
        }

        /// <summary>
        /// Returns the original specified point, doing nothing.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 operator +( Point3 point )
        {
            return point;
        }

        #endregion

        #region -

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Vector from the the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 operator -( Point3 left, Point3 right )
        {
            Point3 result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 operator -( Point3 point, int scalar )
        {
            Point3 result;

            result.X = point.X - scalar;
            result.Y = point.Y - scalar;
            result.Z = point.Z - scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of negating the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 operator -( Point3 point )
        {
            Point3 result;

            result.X = -point.X;
            result.Y = -point.Y;
            result.Z = -point.Z;

            return result;
        }

        #endregion

        #region *

        /// <summary>
        /// Returns the result of multiplcing the given <paramref name="point"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 operator *( Point3 point, int scalar )
        {
            Point3 result;

            result.X = point.X * scalar;
            result.Y = point.Y * scalar;
            result.Z = point.Z * scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of multiplcing the given <paramref name="scalar"/> by the given <paramref name="point"/>.
        /// </summary>
        /// <param name="scalar">The scalar on the left side of the equation.</param>
        /// <param name="point">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 operator *( int scalar, Point3 point )
        {
            Point3 result;

            result.X = point.X * scalar;
            result.Y = point.Y * scalar;
            result.Z = point.Z * scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The point on the left side of the equation.</param>
        /// <param name="right">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 operator *( Point3 left, Point3 right )
        {
            Point3 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;

            return result;
        }

        #endregion

        #region /

        /// <summary>
        /// Returns the result of dividing the given <paramref name="point"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 operator /( Point3 point, int scalar )
        {
            Point3 result;

            result.X = point.X / scalar;
            result.Y = point.Y / scalar;
            result.Z = point.Z / scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector element-by-element.
        /// </summary>
        /// <param name="left">The point on the left side of the equation.</param>
        /// <param name="right">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point3 operator /( Point3 left, Point3 right )
        {
            Point3 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;

            return result;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Returns whether the specified <see cref="Point3"/> instances are (approximately) equal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator ==( Point3 left, Point3 right )
        {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        /// <summary>
        /// Returns whether the specified <see cref="Point3"/> instances are (approximately) inequal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator !=( Point3 left, Point3 right )
        {
            return left.X != right.X ||
                   left.Y != right.Y ||
                   left.Z != right.Z;
        }

        #endregion

        #region Cast

        /// <summary>
        /// Implicit cast operator that implements conversion
        /// from a <see cref="Vector3"/> to a <see cref="Point3"/>.
        /// </summary>
        /// <param name="point">
        /// The input point.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static implicit operator Vector3( Point3 point )
        {
            return new Vector3( point.X, point.Y, point.Z );
        }

        #endregion

        #endregion
    }
}
