// <copyright file="LineSegment2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.LineSegment2 class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary> 
    /// Represents a line( ax + by + c = 0 ) with a start point and an end point.
    /// </summary>
    [System.Serializable]
    [System.ComponentModel.TypeConverter( typeof( System.ComponentModel.ExpandableObjectConverter ) )]
    public class LineSegment2 : Line2, IReadOnlyLineSegment2, IEquatable<LineSegment2>
    {
        #region [ Properties ]

        /// <summary> 
        /// Gets or sets the start point of this LineSegment2.
        /// </summary>
        /// <value>The start point of this LineSegment2.</value>
        public Vector2 Start
        {
            get
            {
                return this.start;
            }

            set
            {
                this.Initialize( value, this.end );
                this.start = value;
            }
        }

        /// <summary> 
        /// Gets or sets the end point of this LineSegment2.
        /// </summary>
        /// <value>The end point of this LineSegment2.</value>
        public Vector2 End
        {
            get
            {
                return this.end;
            }

            set
            {
                this.Initialize( this.start, value );
                this.end = value;
            }
        }

        /// <summary> 
        /// Gets the length of this LineSegment2.
        /// </summary>
        /// <value>The length (also called magnitude) from the StartPoint to the EndPoint.</value>
        public float Length
        {
            get
            {
                float deltaX = this.end.X - this.start.X;
                float deltaY = this.end.Y - this.start.Y;

                return (float)System.Math.Sqrt( (deltaX * deltaX) + (deltaY * deltaY) );
            }
        }

        /// <summary> 
        /// Gets the squared length of the LineSegment2.
        /// </summary>
        /// <value>The squared length (also called magnitude) from the StartPoint to the EndPoint.</value>
        public float LengthSquared
        {
            get
            {
                float deltaX = this.end.X - this.start.X;
                float deltaY = this.end.Y - this.start.Y;

                return (deltaX * deltaX) + (deltaY * deltaY);
            }
        }
        
        /// <summary>
        /// Gets the maximum value of the points in the LineSegment2.
        /// </summary>
        /// <value>The maximum value of the points in the LineSegment2.</value>
        public Vector2 Maximum
        {
            get
            {
                Vector2 maximum;
                Vector2.Max( ref this.start, ref this.end, out maximum );

                return maximum;
            }
        }

        /// <summary>
        /// Gets the minumum value of the points in the LineSegment2.
        /// </summary>
        /// <value>The minumum value of the points in the LineSegment2.</value>
        public Vector2 Minimum
        {
            get
            {
                Vector2 minimum;
                Vector2.Min( ref this.start, ref this.end, out minimum );

                return minimum;
            }
        }
        
        #endregion

        #region [ Constructors ]

        /// <summary> 
        /// Initializes a new instance of the <see cref="LineSegment2"/> class.
        /// </summary>
        public LineSegment2()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSegment2"/> class.
        /// </summary>
        /// <param name="startPoint">
        /// The start-point of the <see cref="LineSegment2"/>.
        /// </param>
        /// <param name="endPoint">
        /// The end-point of the <see cref="LineSegment2"/>.
        /// </param>
        public LineSegment2( Vector2 startPoint, Vector2 endPoint )
            : base( startPoint, endPoint )
        {
            this.start = startPoint;
            this.end = endPoint;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Sets both the start and end point of this LineSegment2 at the same time.
        /// </summary>
        /// <param name="startPoint">
        /// The new start point of this LineSegment2.
        /// </param>
        /// <param name="endPoint">
        /// The new end point of this LineSegment2.
        /// </param>
        public void Set( Vector2 startPoint, Vector2 endPoint )
        {
            this.start = startPoint;
            this.end = endPoint;
            this.Initialize( startPoint, endPoint );
        }

        /// <summary> 
        /// Gets a point on this <see cref="LineSegment2"/>. 
        /// </summary>
        /// <param name="time">
        /// The amount to travel on the segment.
        /// </param>
        /// <returns>
        /// A point along this LineSegment2.
        /// </returns>
        [Pure]
        public Vector2 GetPointOnSegment( float time )
        {
            time = MathUtilities.Clamp( time, 0.0f, 1.0f );

            Vector2 delta = this.end - this.start;
            float distance = time * delta.Length;

            return this.start + (delta.Direction * distance);
        }

        /// <summary>
        /// Implicit cast operator that implements conversion
        /// from a LineSegment2 to a <see cref="FastLineSegment2"/>.
        /// </summary>
        /// <param name="segment">
        /// The input segment.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static implicit operator FastLineSegment2( LineSegment2 segment )
        {
            // Not checking for null because of performance reasons.
            return new FastLineSegment2( segment.start, segment.end );
        }

        #region GetPointLocation

        /// <summary>
        /// Gets the location of the given point (Is it left or right?).
        /// </summary>
        /// <param name="point">
        /// The point to test.
        /// </param>
        /// <returns>
        /// Returns:
        /// <para>
        /// -1: point at the left of the line (or above the line if the line is horizontal).
        /// </para>
        /// <para>
        /// 0: point in the line segment or in the line segment's extension.
        /// </para>
        /// <para>
        /// 1: point at right of the line (or below the line if the line is horizontal).
        /// </para>
        /// </returns>
        [Pure]
        public int GetPointLocation( Vector2 point )
        {
            // Is it a horizontal line ?
            if( this.IsHorizontal )
            {
                if( (start.Y - point.Y).IsApproximate( 0.0f ) )
                    return 0;    // They are equal.
                else if( start.Y > point.Y )
                    return -1;   // The point is above the line.
                else
                    return 1;    // The point is below the line.
            }
            else
            {
                // Make it so that the line's direction is bottom -> up
                if( end.Y > start.Y )
                    this.ChangeDirection();

                float length = this.Length;
                float s =
                        (((start.Y - point.Y) * (end.X - start.X)) -
                          ((start.X - point.X) * (end.Y - start.Y))
                        ) / length * length;

                if( s.IsApproximate( 0.0f ) )
                    return 0;   // The point is in the line or line extension.
                else if( s > 0.0f )
                    return -1;  // The point is left of the line or above the horizontal line. 
                else
                    return 1;
            }
        }

        #endregion

        #region DistanceTo

        /// <summary>
        /// Returns the distance between the LineSegment and the specfied Point.
        /// </summary>
        /// <param name="point">
        /// The point to get the distance to.
        /// </param>
        /// <returns>
        /// The distance to the point.
        /// </returns>
        [Pure]
        public float DistanceTo( Vector2 point )
        {
            Vector2 pointOnLine;
            return DistanceTo( point, start, end, out pointOnLine );
        }
        
        /// <summary>
        /// Returns the distance between the LineSegment and the specified Point.
        /// </summary>
        /// <param name="point">
        /// The point to get the distance to.
        /// </param>
        /// <param name="pointOnLine">
        /// The projected point on the line.
        /// </param>
        /// <param name="start">
        /// The start point of the line segement.
        /// </param>
        /// <param name="end">
        /// The end point of the line segement.
        /// </param>
        /// <returns>
        /// The distance to the point.
        /// </returns>
        public static float DistanceTo( Vector2 point, Vector2 start, Vector2 end, out Vector2 pointOnLine )
        {
            Vector2 v;
            Vector2.Subtract( ref end, ref start, out v );

            Vector2 w;
            Vector2.Subtract( ref point, ref start, out w );

            float c1;
            Vector2.Dot( ref w, ref v, out c1 );

            if( c1 <= 0.0f )
            {
                float result;
                Vector2.Distance( ref point, ref start, out result );

                pointOnLine = start;
                return result;
            }

            float c2;
            Vector2.Dot( ref v, ref v, out c2 );

            if( c2 <= c1 )
            {
                float result;
                Vector2.Distance( ref point, ref end, out result );

                pointOnLine = end;
                return result;
            }

            float ratio = c1 / c2;

            Vector2 vb;
            vb.X = v.X * ratio;
            vb.Y = v.Y * ratio;

            Vector2.Add( ref start, ref vb, out pointOnLine );

            float distance;
            Vector2.Distance( ref point, ref pointOnLine, out distance );

            return distance;
        }

        #endregion

        #region ChangeDirection

        /// <summary> 
        /// Changes the direction of the line,
        /// so that the startpoint becomes the endpoint and visaversa.
        /// </summary>
        public void ChangeDirection()
        {
            Vector2 temp = start;
            start = end;
            end = temp;
        }

        #endregion

        #region OffsetLine

        /// <summary>
        /// Creates a line that is similiar to this line but has an offset (distance).</summary>
        /// <param name="distance">
        /// The distance to move.
        /// </param>
        /// <param name="rightOrDown">
        /// States whether this line goes right/down or left/up.
        /// </param>
        /// <returns>
        /// The newly created LineSegment2.
        /// </returns>
        [Pure]
        public LineSegment2 OffsetLine( float distance, bool rightOrDown )
        {
            // Contract.Ensures( Contract.Result<LineSegment2>() != null );

            Vector2 newStartPoint = new Vector2();
            Vector2 newEndPoint = new Vector2();
            float alpha = this.Angle;

            if( rightOrDown )
            {
                if( this.IsHorizontal )
                {
                    newStartPoint.X = start.X;
                    newStartPoint.Y = start.Y + distance;

                    newEndPoint.X = end.X;
                    newEndPoint.Y = end.Y + distance;
                    return new LineSegment2( newStartPoint, newEndPoint );
                }
                else
                {
                    float sin = (float)System.Math.Sin( (double)alpha );
                    float cos = (float)System.Math.Cos( (double)alpha );

                    float absDisSin = System.Math.Abs( distance * sin );
                    float absDisCos = System.Math.Abs( distance * cos );

                    if( sin >= 0.0f )
                    {
                        newStartPoint.X = start.X + absDisCos;
                        newStartPoint.Y = start.Y + absDisSin;

                        newEndPoint.X = end.X + absDisCos;
                        newEndPoint.Y = end.Y + absDisSin;

                        return new LineSegment2( newStartPoint, newEndPoint );
                    }
                    else
                    {
                        newStartPoint.X = start.X + absDisCos;
                        newStartPoint.Y = start.Y - absDisSin;

                        newEndPoint.X = end.X + absDisCos;
                        newEndPoint.Y = end.Y - absDisSin;

                        return new LineSegment2( newStartPoint, newEndPoint );
                    }
                }
            }
            else
            {
                if( this.IsHorizontal )
                {
                    newStartPoint.X = start.X;
                    newStartPoint.Y = start.Y - distance;

                    newEndPoint.X = end.X;
                    newEndPoint.Y = end.Y - distance;

                    return new LineSegment2( newStartPoint, newEndPoint );
                }
                else
                {
                    float sin = (float)System.Math.Sin( (double)alpha );
                    float cos = (float)System.Math.Cos( (double)alpha );

                    float absDisSin = System.Math.Abs( distance * sin );
                    float absDisCos = System.Math.Abs( distance * cos );

                    if( sin >= 0.0f )
                    {
                        newStartPoint.X = start.X - absDisCos;
                        newStartPoint.Y = start.Y - absDisSin;
                        newEndPoint.X = end.X - absDisCos;
                        newEndPoint.Y = end.Y - absDisSin;

                        return new LineSegment2( newStartPoint, newEndPoint );
                    }
                    else
                    {
                        newStartPoint.X = start.X - absDisCos;
                        newStartPoint.Y = start.Y + absDisSin;
                        newEndPoint.X = end.X - absDisCos;
                        newEndPoint.Y = end.Y + absDisSin;

                        return new LineSegment2( newStartPoint, newEndPoint );
                    }
                }
            }
        }

        #endregion OffsetLine

        #region > Intersection <

        /// <summary>
        /// Determines whether a specified Rectangle intersects with this FastLineSegment2.
        /// </summary>
        /// <param name="rectangle">
        /// The Rectangle to test for intersection with.
        /// </param>
        /// <returns>
        /// true if they intersect; -or-
        /// otherwise false.
        /// </returns>
        public bool Intersects( Rectangle rectangle )
        {
            return rectangle.Intersects( this );
        }

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether the given other <see cref="Object"/> 
        /// is equal to the <see cref="LineSegment2"/>.
        /// </summary>
        /// <param name="obj">The Object to test against.</param>
        /// <returns>
        /// True if they are equal; otherwise false.
        /// </returns>
        public override bool Equals( object obj )
        {
            return this.Equals( obj as LineSegment2 );
        }

        /// <summary>
        /// Returns whether the given other <see cref="LineSegment2"/> 
        /// is equal to the <see cref="LineSegment2"/>.
        /// </summary>
        /// <param name="other">The LineSegment2 to test against.</param>
        /// <returns>
        /// Whether the segments are equal.
        /// </returns>
        public bool Equals( LineSegment2 other )
        {
            return this.Equals( (IReadOnlyLineSegment2)other );
        }

        /// <summary>
        /// Returns whether the given <see cref="IReadOnlyLineSegment2"/> 
        /// is equal to the <see cref="LineSegment2"/>.
        /// </summary>
        /// <param name="other">
        /// The IReadOnlyLineSegment2 to test against.
        /// </param>
        /// <returns>
        /// Whether the segments are equal.
        /// </returns>
        public bool Equals( IReadOnlyLineSegment2 other )
        {
            if( other == null )
                return false;

            return this.start == other.Start && this.end == other.End;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a string representation of this <see cref="LineSegment2"/> object.
        ///  </summary>
        /// <returns> A string representation. </returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a string representation of this <see cref="LineSegment2"/> object.
        /// </summary>
        /// <param name="formatProvider">
        /// Provides formating information.
        /// </param>
        /// <returns> A string representation. </returns>
        public override string ToString( IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider, 
                "LineSegment2[ Start={0} End={1}\nA={2} B={3} C={4} ]",
                start.ToString( formatProvider ), 
                end.ToString( formatProvider ),
                A.ToString( formatProvider ),
                B.ToString( formatProvider ), 
                C.ToString( formatProvider )
            );
        }

        #endregion

        #region Clone

        /// <summary>
        /// Returns a clone of the <see cref="LineSegment2"/>.
        /// </summary>
        /// <returns>The cloned LineSegment2.</returns>
        public new LineSegment2 Clone()
        {
            // Contract.Ensures( Contract.Result<LineSegment2>() != null );

            return new LineSegment2( this.start, this.end );
        }

        /// <summary>
        /// Returns a clone of the <see cref="LineSegment2"/>.
        /// </summary>
        /// <returns>The cloned LineSegment2.</returns>
        object ICloneable.Clone()
        {
            return new LineSegment2( this.start, this.end );
        }

        #endregion

        /// <summary> 
        /// Returns the hash code of the <see cref="LineSegment2"/> instance. 
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
            hashBuilder.AppendStruct( this.start );
            hashBuilder.AppendStruct( this.end );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary> 
        /// The starting point of this LineSegment2.
        /// </summary>
        private Vector2 start;

        /// <summary>
        /// The ending point of this LineSegment2.
        /// </summary>
        private Vector2 end;

        #endregion
    }
}
