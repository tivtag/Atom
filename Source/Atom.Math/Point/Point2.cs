// <copyright file="Point2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Point2 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Represents a point in two dimensional space.
    /// </summary>
    [Serializable]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.Point2Converter ) )]
    public struct Point2 : IEquatable<Point2>, ICultureSensitiveToStringProvider
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

        #endregion

        #region [ Constants ]
        
        /// <summary>
        /// Gets a <see cref="Point2"/> with the coordinates (0, 0).
        /// </summary>
        /// <value>The point (0, 0).</value>
        public static Point2 Zero 
        {
            get
            { 
                return new Point2(); 
            } 
        }

        /// <summary>
        /// Gets a <see cref="Point2"/> with the coordinates (1, 1).
        /// </summary>
        /// <value>The point (1, 1).</value>
        public static Point2 One 
        {
            get 
            { 
                return new Point2( 1, 1 );
            } 
        }

        /// <summary>
        /// Gets a <see cref="Point2"/> with the coordinates (1, 0).
        /// </summary>
        /// <value>The point (1, 0).</value>
        public static Point2 UnitX 
        { 
            get
            { 
                return new Point2( 1, 0 );
            } 
        }

        /// <summary>
        /// Gets a <see cref="Point2"/> with the coordinates (0, 1).
        /// </summary>
        /// <value>The point (0, 1).</value>
        public static Point2 UnitY
        {
            get 
            { 
                return new Point2( 0, 1 );
            }
        }

        #endregion

        #region [ Properties ]
        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Point2"/> structure.
        /// </summary>
        /// <param name="x">The X-coordinate of the new Point.</param>
        /// <param name="y">The Y-coordinate of the new Point.</param>
        public Point2( int x, int y )
        {
            this.X = x;
            this.Y = y;
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
        public static Point2 Add( Point2 left, Point2 right )
        {
            Point2 result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the <paramref name="right"/> Vector to the <paramref name="left"/> Vector
        /// in the given Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Point2 left, ref Point2 right, out Point2 result )
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 Add( Point2 point, int scalar )
        {
            Point2 result;

            result.X = point.X + scalar;
            result.Y = point.Y + scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the given <paramref name="scalar"/> to the given <paramref name="point"/>
        /// in the given Vector.
        /// </summary>
        /// <param name="point">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation. </param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Point2 point, int scalar, out Point2 result )
        {
            result.X = point.X + scalar;
            result.Y = point.Y + scalar;
        }

        /// <summary>
        /// This method returns the specified Vector.
        /// </summary>
        /// <remarks>
        /// Is equal to "+point".
        /// </remarks>
        /// <param name="point">The point.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 Plus( Point2 point )
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
        public static void Plus( ref Point2 point, out Point2 result )
        {
            result.X = point.X;
            result.Y = point.Y;
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Vector from the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 Subtract( Point2 left, Point2 right )
        {
            Point2 result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the <paramref name="right"/> Vector frpm the <paramref name="left"/> Vector
        /// in the given Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Point2 left, ref Point2 right, out Point2 result )
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 Subtract( Point2 point, int scalar )
        {
            Point2 result;

            result.X = point.X - scalar;
            result.Y = point.Y - scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="point"/>
        /// in the given Vector.
        /// </summary>
        /// <param name="point">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Point2 point, int scalar, out Point2 result )
        {
            result.X = point.X - scalar;
            result.Y = point.Y - scalar;
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
        public static Point2 Negate( Point2 point )
        {
            return new Point2( -point.X, -point.Y );
        }

        /// <summary>
        /// Stores the result of negating the elements of the given <paramref name="point"/> in the given Vector.
        /// </summary>
        /// <param name="point">
        /// The point to negate. This value will not be modified by this method.
        /// </param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Negate( ref Point2 point, out Point2 result )
        {
            result.X = -point.X;
            result.Y = -point.Y;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Returns the result of multiplying the given <paramref name="point"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 Multiply( Point2 point, int scalar )
        {
            Point2 result;

            result.X = point.X * scalar;
            result.Y = point.Y * scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the given <paramref name="point"/> by the given <paramref name="scalar"/>.
        /// in the given Vector.
        /// </summary>
        /// <param name="point">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Multiply( ref Point2 point, int scalar, out Point2 result )
        {
            result.X = point.X * scalar;
            result.Y = point.Y * scalar;
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The point on the left side of the equation.</param>
        /// <param name="right">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 Multiply( Point2 left, Point2 right )
        {
            Point2 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the left Vector by the right Vector component-by-component.
        /// in the given result Vector.
        /// </summary>
        /// <param name="left">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The point on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Multiply( ref Point2 left, ref Point2 right, out Point2 result )
        {
            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Returns the result of dividing the given <paramref name="point"/> through the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 Divide( Point2 point, int scalar )
        {
            Point2 result;

            result.X = point.X / scalar;
            result.Y = point.Y / scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the given <paramref name="point"/> through the given <paramref name="scalar"/>
        /// in the given result Vector.
        /// </summary>
        /// <param name="point">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Divide( ref Point2 point, int scalar, out Point2 result )
        {
            result.X = point.X / scalar;
            result.Y = point.Y / scalar;
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The point on the left side of the equation.</param>
        /// <param name="right">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 Divide( Point2 left, Point2 right )
        {
            Point2 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the left Vector through the right Vector component-by-component.
        /// in the given result Vector.
        /// </summary>
        /// <param name="left">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The point on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will store the result of the operation.</param>
        public static void Divide( ref Point2 left, ref Point2 right, out Point2 result )
        {
            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether the given <see cref="Point2"/> has the
        /// same indices set as this Point2.
        /// </summary>
        /// <param name="other">The Point2 to test against.</param>
        /// <returns>true if the indices are equal; otherwise false.</returns>
        public bool Equals( Point2 other )
        {
            return this.X == other.X &&
                   this.Y == other.Y;
        }

        /// <summary>
        /// Returns whether the given <see cref="Object"/> is equal to this Point2.
        /// </summary>
        /// <param name="obj">The Object to test against.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public override bool Equals( object obj )
        {
            if( obj is Point2 )
            {
                return Equals( (Point2)obj );
            }
            else if( obj is Vector2 )
            {
                Vector2 point = (Vector2)obj;

                return point.X.IsApproximate( (float)this.X ) &&
                       point.Y.IsApproximate( (float)this.Y );
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
                "[{0} {1}]",
                X.ToString( formatProvider ),
                Y.ToString( formatProvider ) 
            );
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Overriden to return the hashcode of the <see cref="Point2"/>.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.X );
            hashBuilder.AppendStruct( this.Y );

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
        public static Point2 operator +( Point2 left, Point2 right )
        {
            return new Point2( left.X + right.X, left.Y + right.Y );
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the thegiven <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 operator +( Point2 point, int scalar )
        {
            return new Point2( point.X + scalar, point.Y + scalar );
        }

        /// <summary>
        /// Returns the original specified point, doing nothing.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 operator +( Point2 point )
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
        public static Point2 operator -( Point2 left, Point2 right )
        {
            return new Point2( left.X - right.X, left.Y - right.Y );
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 operator -( Point2 point, int scalar )
        {
            return new Point2( point.X - scalar, point.Y - scalar );
        }

        /// <summary>
        /// Returns the result of negating the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 operator -( Point2 point )
        {
            return new Point2( -point.X, -point.Y );
        }

        #endregion

        #region *

        /// <summary>
        /// Returns the result of multiplcing the given <paramref name="point"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 operator *( Point2 point, int scalar )
        {
            return new Point2( point.X * scalar, point.Y * scalar );
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The point on the left side of the equation.</param>
        /// <param name="right">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 operator *( Point2 left, Point2 right )
        {
            Point2 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;

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
        public static Point2 operator /( Point2 point, int scalar )
        {
            return new Point2( point.X / scalar, point.Y / scalar );
        }

        /// <summary>
        /// Returns the result of dividing the given <paramref name="point"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 operator /( Point2 point, float scalar )
        {
            return new Vector2( point.X / scalar, point.Y / scalar );
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector element-by-element.
        /// </summary>
        /// <param name="left">The point on the left side of the equation.</param>
        /// <param name="right">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point2 operator /( Point2 left, Point2 right )
        {
            Point2 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;

            return result;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Returns whether the specified <see cref="Point2"/> instances are (approximately) equal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator ==( Point2 left, Point2 right )
        {
            return left.X == right.X &&
                   left.Y == right.Y;
        }

        /// <summary>
        /// Returns whether the specified <see cref="Point2"/> instances are (approximately) inequal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator !=( Point2 left, Point2 right )
        {
            return left.X != right.X ||
                   left.Y != right.Y;
        }

        #endregion

        #region Cast

        /// <summary>
        /// Implicit cast operator that implements conversion
        /// from a <see cref="Vector2"/> to a <see cref="Point2"/>.
        /// </summary>
        /// <param name="point">
        /// The input point.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static implicit operator Vector2( Point2 point )
        {
            return new Vector2( point.X, point.Y );
        }

        #endregion

        #endregion
    }
}
