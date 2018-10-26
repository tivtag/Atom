// <copyright file="Line2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Line2 class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Globalization;
    using Atom.Diagnostics.Contracts;

    /// <summary> 
    /// Represents a line in 2D space, descriped using the general line equation: Ax + By - c = 0.
    /// </summary>
    [System.Serializable]
    [System.ComponentModel.TypeConverter( typeof( System.ComponentModel.ExpandableObjectConverter ) )]
    public class Line2 : IEquatable<Line2>, IReadOnlyLine2
    {
        #region [ Basic Properties ]

        /// <summary> 
        /// Gets or sets the A parameter.
        /// </summary>
        /// <value>The A parameter as used in the 'Ax + By - c = 0' equation.</value>
        public float A 
        {
            get; 
            set; 
        }

        /// <summary> 
        /// Gets or sets the B parameter.
        /// </summary>
        /// <value>The B parameter as used in the 'Ax + By - c = 0' equation.</value>
        public float B 
        { 
            get;
            set; 
        }

        /// <summary> 
        /// Gets or sets the C constant.
        /// </summary>
        /// <value>The c constant as used in the 'Ax + By - c = 0' equation.</value>
        public float C 
        { 
            get; 
            set;
        }

        #endregion

        #region [ Properties ]

        /// <summary> 
        /// Gets the angle of the line in radians.
        /// </summary>
        /// <value>
        /// The angle in radians.
        /// </value>
        public float Angle
        {
            get
            {
                if( this.B == 0 )
                    return Constants.PiOver2;
                else
                    return (float)System.Math.Atan( (double)(-A / B) );
            }
        }

        /// <summary>
        /// Gets the direction this Line2 points.
        /// </summary>
        public Vector2 Direction
        {
            get
            {
                float angle = this.Angle;
                return new Vector2( (float)Math.Cos( angle ), (float)Math.Sin( angle ) );
            }
        }

        /// <summary> 
        /// Gets a value indicating whether this <see cref="Line2"/> is a vertical line.
        /// </summary>
        /// <value> 
        /// Is <see langword="true"/> if the line is vertical; otherwise <see langword="false"/>. 
        /// </value>
        public bool IsVertical
        {
            get
            {
                if( System.Math.Abs( B ) < Constants.Epsilon )
                    return true;
                else
                    return false;
            }
        }

        /// <summary> 
        /// Gets a value indicating whether this <see cref="Line2"/> is a horizontal line. 
        /// </summary>
        /// <value> 
        /// Is <see langword="true"/> if the line is horizontal; otherwise <see langword="false"/>. 
        /// </value>
        public bool IsHorizontal
        {
            get
            {
                if( System.Math.Abs( A ) < Constants.Epsilon )
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Line2"/> class.
        /// </summary>
        public Line2()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Line2"/> class.
        /// </summary>
        /// <param name="a">The parameter a.</param> 
        /// <param name="b">The parameter b.</param>
        /// <param name="c">The constant c.</param>
        public Line2( float a, float b, float c )
        {
            this.A = a;
            this.B = b;
            this.C = c;
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="Line2"/> class.
        /// </summary>
        /// <exception cref="ArgumentException"> If the angle is invalid. </exception>
        /// <param name="angle"> The angle in radiants. Must be less than two Pi. </param>
        /// <param name="point"> The start point. </param>
        public Line2( float angle, Vector2 point )
        {
            this.Initialize( angle, point );
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="Line2"/> class.
        /// It creates a line between the two specified points.
        /// </summary>
        /// <exception cref="ArgumentException">If the given points are equal. </exception>
        /// <param name="start"> The start point. </param>
        /// <param name="end"> The end point.  </param>
        public Line2( Vector2 start, Vector2 end )
        {
            this.Initialize( start, end );
        }

        /// <summary> Initializes a new instance of the <see cref="Line2"/> class. </summary>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="line"/> is null.
        /// </exception>
        /// <param name="line"> The line to copy. </param>
        public Line2( Line2 line )
        {
            if( line == null )
                throw new System.ArgumentNullException( "line" );

            this.A = line.A;
            this.B = line.B;
            this.C = line.C;
        }

        /// <summary> 
        /// Initializes this <see cref="Line2"/>.
        /// </summary>
        /// <exception cref="ArgumentException"> 
        /// If the specified <paramref name="angle"/> is greater than two Pi.
        /// </exception>
        /// <param name="angle"> The angle of the line in radiants. Must be less than two Pi. </param>
        /// <param name="point"> The start point. </param>
        protected void Initialize( float angle, Vector2 point )
        {
            if( angle > Constants.TwoPi )
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        MathErrorStrings.SpecifiedAngleXGreaterTwoPi,
                        angle.ToString( CultureInfo.CurrentCulture )
                    ),
                    "angle"
                );
            }

            // A vertical line ?
            if( System.Math.Abs( angle - Constants.PiOver2 ) < Constants.Epsilon )
            {
                A = 1.0f;
                B = 0.0f;
                C = -point.X;
            }
            else
            {
                A = -((float)System.Math.Tan( (double)angle ));
                B = 1.0f;
                C = (-A * point.X) - point.Y;
            }
        }

        /// <summary> 
        /// Initializes the <see cref="Line2"/>.
        /// </summary>
        /// <exception cref="ArgumentException">If the given points are equal. </exception>
        /// <param name="start"> The start point. </param>
        /// <param name="end"> The end point.  </param>
        protected void Initialize( Vector2 start, Vector2 end )
        {
            if( start == end )
                throw new ArgumentException( Atom.ErrorStrings.ArgumentsMayNotBeEqual, "start" );

            if( System.Math.Abs( start.X - end.X ) < Constants.Epsilon )
            {
                // A vertical line.
                Initialize( Constants.PiOver2, start );
            }
            else if( System.Math.Abs( start.Y - end.Y ) < Constants.Epsilon )
            {
                // A horizontal line.
                Initialize( 0.0f, start );
            }
            else
            {
                // A normal line. :D
                float slope = (end.Y - start.Y) / (end.X - start.X);
                float alpha = (float)System.Math.Atan( (double)slope );

                Initialize( alpha, start );
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Determines whether the specified <see cref="Line2"/> is parallel to this <see cref="Line2"/>.
        /// </summary>
        /// <param name="line"> 
        /// The line to test against.
        /// </param>
        /// <returns> 
        /// Returns <see langword="true"/> if they are parallel; otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        public bool IsParallelTo( Line2 line )
        {
            Contract.Requires<ArgumentNullException>( line != null );

            return (A / B).IsApproximate( line.A / line.B );
        }

        /// <summary> 
        /// Calculates the intersection point of two lines. 
        /// </summary>
        /// <param name="line"> 
        /// The line to test against.
        /// </param>
        /// <param name="point"> 
        /// Will contain the intersection point if the function returns true.
        /// </param>
        /// <returns> 
        /// Returns <see langword="true"/> if the lines intersect; 
        /// or otherwise <see langword="false"/> if the lines are parallel (=> don't intersect ever).
        /// </returns>
        [Pure]
        public bool Intersects( Line2 line, out Vector2 point )
        {
            Contract.Requires<ArgumentNullException>( line != null );

            if( this.IsParallelTo( line ) == false )
            {
                point.X = ((B * line.C) - (C * line.B)) / ((A * line.B) - (B * line.A));
                point.Y = ((A * line.C) - (C * line.A)) / ((B * line.B) - (A * line.A));
                return true;
            }

            point = Vector2.Zero;
            return false;
        }

        #region Distance

        /// <summary> 
        /// Returns the distance from a given point to the line. 
        /// </summary>
        /// <param name="point">
        /// The point to get the distance to.
        /// </param>
        /// <returns>
        /// The distance.
        /// </returns>
        [Pure]
        public float Distance( Vector2 point )
        {
            float d = System.Math.Abs( (A * point.X) + (B * point.Y) + C );
            return d / (float)System.Math.Sqrt( (A * A) + (B * B) );
        }

        #endregion

        #region GetX

        /// <summary>
        /// Calculates X given <paramref name="y"/>.
        /// </summary>
        /// <exception cref="DivideByZeroException">
        /// Thrown if <see cref="A"/> is 0.
        /// </exception>
        /// <param name="y">
        /// The y coordinate.
        /// </param>
        /// <returns>
        /// The x coordinate.
        /// </returns>
        [Pure]
        public float GetX( float y )
        {
            return -((B * y) + C) / A;
        }

        #endregion

        #region GetY

        /// <summary>
        /// Calculates Y given <paramref name="x"/>.
        /// </summary>
        /// <exception cref="DivideByZeroException">
        /// Thrown if <see cref="B"/> is 0.
        /// </exception>
        /// <param name="x">
        /// The x coordinate.
        /// </param>
        /// <returns>
        /// The y coordinate.
        /// </returns>
        [Pure]
        public float GetY( float x )
        {
            return -((A * x) + C) / B;
        }

        #endregion

        #region > Overrides/Impls <

        /// <summary> 
        /// Returns the hash code for this Line2 instance.
        /// </summary>
        /// <returns>
        /// The hash code.
        /// </returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.A );
            hashBuilder.AppendStruct( this.B );
            hashBuilder.AppendStruct( this.C );

            return hashBuilder.GetHashCode();
        }

        #region ToString

        /// <summary> Returns a string representation of this <see cref="Line2"/> object. </summary>
        /// <returns> A string representation. </returns>
        public override string ToString()
        {
            return ToString( CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a string representation of this <see cref="Line2"/> object.
        /// </summary>
        /// <param name="formatProvider">
        /// The formating information provider to use.
        /// </param>
        /// <returns> A string representation. </returns>
        public virtual string ToString( IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider,
                "Line2[ a={0} b={1} c={2} ]",
                A.ToString( formatProvider ),
                B.ToString( formatProvider ),
                C.ToString( formatProvider )
            );
        }

        #endregion

        #region Equals

        /// <summary>
        /// Gets whether the given Object is equal to the <see cref="Line2"/>.
        /// </summary>
        /// <param name="obj">The object to test against.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public override bool Equals( object obj )
        {
            Line2 other = obj as Line2;
            if( other == null )
                return false;

            return this.A.IsApproximate( other.A ) &&
                   this.B.IsApproximate( other.B ) &&
                   this.C.IsApproximate( other.C );
        }

        /// <summary>
        /// Gets whether the given <see cref="Line2"/> is equal to the <see cref="Line2"/>.
        /// </summary>
        /// <param name="other">The Line2 to test against.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public bool Equals( Line2 other )
        {
            if( other == null )
                return false;

            return this.A.IsApproximate( other.A ) &&
                   this.B.IsApproximate( other.B ) &&
                   this.C.IsApproximate( other.C );
        }

        #endregion

        #region Clone

        /// <summary>
        /// Returns a clone of the <see cref="Line2"/>.
        /// </summary>
        /// <returns>A new Line2 instance.</returns>
        public Line2 Clone()
        {
            return new Line2( this );
        }

        /// <summary>
        /// Returns a clone of the <see cref="Line2"/>.
        /// </summary>
        /// <returns>A new Line2 instance.</returns>
        object ICloneable.Clone()
        {
            return new Line2( this );
        }

        #endregion

        #endregion

        #endregion
    }
}
