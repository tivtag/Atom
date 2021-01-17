// <copyright file="Point4.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Point4 structure.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Represents a point in four dimensional space.
    /// </summary>
    [Serializable]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.Point4Converter ) )]
    public struct Point4 : IEquatable<Point4>, ICultureSensitiveToStringProvider
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

        /// <summary>
        /// The W-coordinate of the Point.
        /// </summary>
        public int W;

        #endregion

        #region [ Constants ]

        /// <summary>
        /// Gets a <see cref="Point4"/> with the coordinates (0, 0).
        /// </summary>
        /// <value>The point (0, 0, 0, 0).</value>
        public static Point4 Zero
        { 
            get 
            { 
                return new Point4();
            }
        }

        /// <summary>
        /// Gets a <see cref="Point4"/> with the coordinates (1, 1, 1, 1).
        /// </summary>
        /// <value>The point (1, 1, 1, 1).</value>
        public static Point4 One
        {
            get 
            {
                return new Point4( 1, 1, 1, 1 );
            }
        }

        /// <summary>
        /// Gets a <see cref="Point4"/> with the coordinates (1, 0, 0, 0).
        /// </summary>
        /// <value>The point (1, 0, 0, 0).</value>
        public static Point4 UnitX 
        { 
            get
            { 
                return new Point4( 1, 0, 0, 0 ); 
            }
        }

        /// <summary>
        /// Gets a <see cref="Point4"/> with the coordinates (0, 1, 0, 0).
        /// </summary>
        /// <value>The point (0, 1, 0, 0).</value>
        public static Point4 UnitY 
        {
            get 
            {
                return new Point4( 0, 1, 0, 0 ); 
            }
        }

        /// <summary>
        /// Gets a <see cref="Point4"/> with the coordinates (0, 0, 1, 0).
        /// </summary>
        /// <value>The point (0, 0, 1, 0).</value>
        public static Point4 UnitZ
        {
            get 
            { 
                return new Point4( 0, 0, 1, 0 );
            } 
        }

        /// <summary>
        /// Gets a <see cref="Point4"/> with the coordinates (0, 0, 0, 1).
        /// </summary>
        /// <value>The point (0, 0, 0, 1).</value>
        public static Point4 UnitW
        {
            get
            { 
                return new Point4( 0, 0, 0, 1 );
            } 
        }

        #endregion

        #region [ Properties ]
        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Point4"/> structure.
        /// </summary>
        /// <param name="x">The X-coordinate of the new Point.</param>
        /// <param name="y">The Y-coordinate of the new Point.</param>
        /// <param name="z">The Z-coordinate of the new Point.</param>
        /// <param name="w">The W-coordinate of the new Point.</param>
        public Point4( int x, int y, int z, int w )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
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
        public static Point4 Add( Point4 left, Point4 right )
        {
            Point4 result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the <paramref name="right"/> Vector to the <paramref name="left"/> Vector
        /// in the given Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Point4 left, ref Point4 right, out Point4 result )
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 Add( Point4 point, int scalar )
        {
            Point4 result;

            result.X = point.X + scalar;
            result.Y = point.Y + scalar;
            result.Z = point.Z + scalar;
            result.W = point.W + scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the given <paramref name="scalar"/> to the given <paramref name="point"/>
        /// in the given Vector.
        /// </summary>
        /// <param name="point">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation. </param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Point4 point, int scalar, out Point4 result )
        {
            result.X = point.X + scalar;
            result.Y = point.Y + scalar;
            result.Z = point.Z + scalar;
            result.W = point.W + scalar;
        }

        /// <summary>
        /// This method returns the specified Vector.
        /// </summary>
        /// <remarks>
        /// Is equal to "+point".
        /// </remarks>
        /// <param name="point">The point.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 Plus( Point4 point )
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
        public static void Plus( ref Point4 point, out Point4 result )
        {
            result.X = point.X;
            result.Y = point.Y;
            result.Z = point.Z;
            result.W = point.W;
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Vector from the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 Subtract( Point4 left, Point4 right )
        {
            Point4 result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the <paramref name="right"/> Vector frpm the <paramref name="left"/> Vector
        /// in the given Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Point4 left, ref Point4 right, out Point4 result )
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 Subtract( Point4 point, int scalar )
        {
            Point4 result;

            result.X = point.X - scalar;
            result.Y = point.Y - scalar;
            result.Z = point.Z - scalar;
            result.W = point.W - scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="point"/>
        /// in the given Vector.
        /// </summary>
        /// <param name="point">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Point4 point, int scalar, out Point4 result )
        {
            result.X = point.X - scalar;
            result.Y = point.Y - scalar;
            result.Z = point.Z - scalar;
            result.W = point.W - scalar;
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
        public static Point4 Negate( Point4 point )
        {
            Point4 result;

            result.X = -point.X;
            result.Y = -point.Y;
            result.Z = -point.Z;
            result.W = -point.W;

            return result;
        }

        /// <summary>
        /// Stores the result of negating the elements of the given <paramref name="point"/> in the given Vector.
        /// </summary>
        /// <param name="point">
        /// The point to negate. This value will not be modified by this method.
        /// </param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Negate( ref Point4 point, out Point4 result )
        {
            result.X = -point.X;
            result.Y = -point.Y;
            result.Z = -point.Z;
            result.W = -point.W;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Returns the result of multiplying the given <paramref name="point"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 Multiply( Point4 point, int scalar )
        {
            Point4 result;

            result.X = point.X * scalar;
            result.Y = point.Y * scalar;
            result.Z = point.Z * scalar;
            result.W = point.W * scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the given <paramref name="point"/> by the given <paramref name="scalar"/>.
        /// in the given Vector.
        /// </summary>
        /// <param name="point">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Multiply( ref Point4 point, int scalar, out Point4 result )
        {
            result.X = point.X * scalar;
            result.Y = point.Y * scalar;
            result.Z = point.Z * scalar;
            result.W = point.W * scalar;
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The point on the left side of the equation.</param>
        /// <param name="right">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 Multiply( Point4 left, Point4 right )
        {
            Point4 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
            result.W = left.W * right.W;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the left Vector by the right Vector component-by-component.
        /// in the given result Vector.
        /// </summary>
        /// <param name="left">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The point on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Multiply( ref Point4 left, ref Point4 right, out Point4 result )
        {
            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
            result.W = left.W * right.W;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Returns the result of dividing the given <paramref name="point"/> through the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 Divide( Point4 point, int scalar )
        {
            Point4 result;

            result.X = point.X / scalar;
            result.Y = point.Y / scalar;
            result.Z = point.Z / scalar;
            result.W = point.W / scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the given <paramref name="point"/> through the given <paramref name="scalar"/>
        /// in the given result Vector.
        /// </summary>
        /// <param name="point">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Divide( ref Point4 point, int scalar, out Point4 result )
        {
            result.X = point.X / scalar;
            result.Y = point.Y / scalar;
            result.Z = point.Z / scalar;
            result.W = point.W / scalar;
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The point on the left side of the equation.</param>
        /// <param name="right">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 Divide( Point4 left, Point4 right )
        {
            Point4 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
            result.W = left.W / right.W;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the left Vector through the right Vector component-by-component.
        /// in the given result Vector.
        /// </summary>
        /// <param name="left">The point on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The point on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will store the result of the operation.</param>
        public static void Divide( ref Point4 left, ref Point4 right, out Point4 result )
        {
            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
            result.W = left.W / right.W;
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether the given <see cref="Point4"/> has the
        /// same indices set as this Point4.
        /// </summary>
        /// <param name="other">The Point4 to test against.</param>
        /// <returns>true if the indices are equal; otherwise false.</returns>
        public bool Equals( Point4 other )
        {
            return this.X == other.X &&
                   this.Y == other.Y &&
                   this.Z == other.Z &&
                   this.W == other.W;
        }

        /// <summary>
        /// Returns whether the given <see cref="Object"/> is equal to this Point4.
        /// </summary>
        /// <param name="obj">The Object to test against.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public override bool Equals( object obj )
        {
            if( obj is Point4 )
            {
                return Equals( (Point4)obj );
            }
            else if( obj is Vector4 )
            {
                Vector4 point = (Vector4)obj;

                return point.X.IsApproximate( (float)this.X ) &&
                       point.Y.IsApproximate( (float)this.Y ) &&
                       point.Z.IsApproximate( (float)this.Z ) &&
                       point.W.IsApproximate( (float)this.W );
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
                "[{0} {1} {2} {3}]",
                X.ToString( formatProvider ),
                Y.ToString( formatProvider ),
                Z.ToString( formatProvider ),
                W.ToString( formatProvider ) 
            );
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Overriden to return the hashcode of the <see cref="Point4"/>.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.X );
            hashBuilder.AppendStruct( this.Y );
            hashBuilder.AppendStruct( this.Z );
            hashBuilder.AppendStruct( this.W );

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
        public static Point4 operator +( Point4 left, Point4 right )
        {
            Point4 result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;

            return result;
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the thegiven <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 operator +( Point4 point, int scalar )
        {
            Point4 result;

            result.X = point.X + scalar;
            result.Y = point.Y + scalar;
            result.Z = point.Z + scalar;
            result.W = point.W + scalar;

            return result;
        }

        /// <summary>
        /// Returns the original specified point, doing nothing.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 operator +( Point4 point )
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
        public static Point4 operator -( Point4 left, Point4 right )
        {
            Point4 result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 operator -( Point4 point, int scalar )
        {
            Point4 result;

            result.X = point.X - scalar;
            result.Y = point.Y - scalar;
            result.Z = point.Z - scalar;
            result.W = point.W - scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of negating the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 operator -( Point4 point )
        {
            Point4 result;

            result.X = -point.X;
            result.Y = -point.Y;
            result.Z = -point.Z;
            result.W = -point.W;

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
        public static Point4 operator *( Point4 point, int scalar )
        {
            Point4 result;

            result.X = point.X * scalar;
            result.Y = point.Y * scalar;
            result.Z = point.Z * scalar;
            result.W = point.W * scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The point on the left side of the equation.</param>
        /// <param name="right">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 operator *( Point4 left, Point4 right )
        {
            Point4 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
            result.W = left.W * right.W;

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
        public static Point4 operator /( Point4 point, int scalar )
        {
            Point4 result;

            result.X = point.X / scalar;
            result.Y = point.Y / scalar;
            result.Z = point.Z / scalar;
            result.W = point.W / scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector element-by-element.
        /// </summary>
        /// <param name="left">The point on the left side of the equation.</param>
        /// <param name="right">The point on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Point4 operator /( Point4 left, Point4 right )
        {
            Point4 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
            result.W = left.W / right.W;

            return result;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Returns whether the specified <see cref="Point4"/> instances are (approximately) equal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator ==( Point4 left, Point4 right )
        {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z &&
                   left.W == right.W;
        }

        /// <summary>
        /// Returns whether the specified <see cref="Point4"/> instances are (approximately) inequal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator !=( Point4 left, Point4 right )
        {
            return left.X != right.X ||
                   left.Y != right.Y ||
                   left.Z != right.Z ||
                   left.W != right.W;
        }

        #endregion

        #region Cast

        /// <summary>
        /// Implicit cast operator that implements conversion
        /// from a <see cref="Vector4"/> to a <see cref="Point4"/>.
        /// </summary>
        /// <param name="point">
        /// The input point.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static implicit operator Vector4( Point4 point )
        {
            return new Vector4( point.X, point.Y, point.Z, point.W );
        }

        #endregion

        #endregion
    }
}
