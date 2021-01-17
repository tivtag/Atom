// <copyright file="OrientedRectangleF.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.OrientedRectangleF structure.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an oriented rectangle in 2-D space.
    /// </summary>
    public struct OrientedRectangleF
    {
        /// <summary>
        /// Gets the corners of this OrientedRectangleF.
        /// </summary>
        public IEnumerable<Vector2> Corners
        {
            get
            {
                yield return this.upperLeft;
                yield return this.upperRight;
                yield return this.lowerRight;
                yield return this.lowerLeft;
            }
        }

        /// <summary>
        /// Gets the upper-left corner of this OrientedRectangleF.
        /// </summary>
        public Vector2 UpperLeft
        {
            get
            {
                return this.upperLeft;
            }
        }

        /// <summary>
        /// Gets the upper-right corner of this OrientedRectangleF.
        /// </summary>
        public Vector2 UpperRight
        {
            get
            {
                return this.upperRight;
            }
        }

        /// <summary>
        /// Gets the lower-left corner of this OrientedRectangleF.
        /// </summary>
        public Vector2 LowerLeft
        {
            get
            {
                return this.lowerLeft;
            }
        }

        /// <summary>
        /// Gets the lower-right corner of this OrientedRectangleF.
        /// </summary>
        public Vector2 LowerRight
        {
            get
            {
                return this.lowerRight;
            }
        }

        /// <summary>
        /// Gets the line segments of this OrientedRectangleF.
        /// </summary>
        public IEnumerable<FastLineSegment2> LineSegments
        {
            get
            {
                yield return new FastLineSegment2( this.upperLeft, this.upperRight );
                yield return new FastLineSegment2( this.upperRight, this.lowerRight );
                yield return new FastLineSegment2( this.lowerRight, this.lowerLeft );
                yield return new FastLineSegment2( this.lowerLeft, this.upperLeft );
            }
        }

        /// <summary>
        /// Initializes a new instance of the OrientedRectangleF class.
        /// </summary>
        /// <param name="upperLeft">
        /// The 'upper left' corner of the new OrientedRectangleF.
        /// </param>
        /// <param name="upperRight">
        /// The 'upper right' corner of the new OrientedRectangleF.
        /// </param>
        /// <param name="lowerLeft">
        /// The 'lower left' corner of the new OrientedRectangleF.
        /// </param>
        public OrientedRectangleF( Vector2 upperLeft, Vector2 upperRight, Vector2 lowerLeft )
        {
            this.upperLeft = upperLeft;
            this.upperRight = upperRight;
            this.lowerLeft = lowerLeft;

            this.lowerRight = new Vector2( 
                lowerLeft.X - (upperLeft.X - upperRight.X),
                lowerLeft.Y - (upperLeft.Y - upperRight.Y)
            );
        }

        /// <summary>
        /// Creates a new OrientedRectangleF given a RectangleF and an angle.
        /// </summary>
        /// <param name="rectangle">
        /// The rectangle to rotate.
        /// </param>
        /// <param name="angle">
        /// The angle to rotate in radian.
        /// The center of the rectangle is the origin of rotation.
        /// </param>
        /// <returns>
        /// The newly created OrientedRectangleF.
        /// </returns>
        public static OrientedRectangleF FromRectangle( RectangleF rectangle, float angle )
        {
            Vector2 center = rectangle.Center;

            Vector2 upperLeft = rectangle.Minimum;
            Vector2 upperRight = new Vector2( rectangle.X + rectangle.Width, rectangle.Y );
            Vector2 lowerLeft = new Vector2( rectangle.X, rectangle.Y + rectangle.Height );

            float cos = (float)System.Math.Cos( (double)angle );
            float sin = (float)System.Math.Sin( (double)angle );
            upperLeft.Rotate( center, cos, sin );
            upperRight.Rotate( center, cos, sin );
            lowerLeft.Rotate( center, cos, sin );

            return new OrientedRectangleF( upperLeft, upperRight, lowerLeft );
        }

        /// <summary>
        /// Creates a new OrientedRectangleF that lies on the specified line.
        /// </summary>
        /// <param name="line">
        /// The line the new OrientedRectangleF lies on.
        /// </param>
        /// <param name="width">
        /// The width of the rectangle; extending for half width into both directions perpendicular of
        /// the input line.
        /// </param>
        /// <returns>
        /// The newly created v.
        /// </returns>
        public static OrientedRectangleF FromLine( FastLineSegment2 line, float width )
        {            
            Vector2 delta = line.Start - line.End;
            Vector2 perp = new Vector2( delta.Y, -delta.X );
            perp.Normalize();
                        
            float halfWidth = width * 0.5f;
            Vector2 change = perp * halfWidth;

            Vector2 upperLeft = line.Start - change;
            Vector2 upperRight = line.Start + change;
            Vector2 lowerLeft = line.End - change;

            return new OrientedRectangleF( upperLeft, upperRight, lowerLeft );
        }

        /// <summary>
        /// Gets the minimum and maximum points of this OrientedRectangleF.
        /// </summary>
        /// <param name="min">
        /// Will contain the minimum point.
        /// </param>
        /// <param name="max">
        /// Will contain the maximum point.
        /// </param>
        public void GetMinMax( out Vector2 min, out Vector2 max )
        {
            min = this.upperLeft;
            max = this.upperLeft;

            // upperRight
            if( this.upperRight.X < min.X )
            {
                min.X = this.upperRight.X;
            }
            if( this.upperRight.X > max.X )
            {
                max.X = this.upperRight.X;
            }

            if( this.upperRight.Y < min.Y )
            {
                min.Y = this.upperRight.Y;
            }
            if( this.upperRight.Y > max.Y )
            {
                max.Y = this.upperRight.Y;
            }
            
            // lowerRight
            if( this.lowerRight.X < min.X )
            {
                min.X = this.lowerRight.X;
            }
            if( this.lowerRight.X > max.X )
            {
                max.X = this.lowerRight.X;
            }

            if( this.lowerRight.Y < min.Y )
            {
                min.Y = this.lowerRight.Y;
            }
            if( this.lowerRight.Y > max.Y )
            {
                max.Y = this.lowerRight.Y;
            }

            // lowerLeft
            if( this.lowerLeft.X < min.X )
            {
                min.X = this.lowerLeft.X;
            }
            if( this.lowerLeft.X > max.X )
            {
                max.X = this.lowerLeft.X;
            }

            if( this.lowerLeft.Y < min.Y )
            {
                min.Y = this.lowerLeft.Y;
            }
            if( this.lowerLeft.Y > max.Y )
            {
                max.Y = this.lowerLeft.Y;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this OrientedRectangleF contains
        /// the specified point.
        /// </summary>
        /// <param name="point">
        /// The point to test for containment.
        /// </param>
        /// <returns>
        /// true if this OrientedRectangleF contains the specified point;
        /// otherwise false.
        /// </returns>
        public bool Contains( Vector2 point )
        {
            // 0 <= dot(AB,AM) <= dot(AB,AB) &&
            // 0 <= dot(AC,AM) <= dot(AC,AC)

            Vector2 am = point - this.upperLeft;
            Vector2 ab = this.upperRight - this.upperLeft;
            
            float abDotAm;
            Vector2.Dot( ref ab, ref am, out abDotAm );

            // 0 <= dot(AB,AM)
            if( 0.0f <= abDotAm )
            {
                float abDotAb;
                Vector2.Dot( ref ab, ref ab, out abDotAb );

                // dot(AB,AM) <= dot(AB,AB)
                if( abDotAm <= abDotAb )
                {
                    Vector2 ac = this.lowerLeft - this.upperLeft;

                    float acDotAm;
                    Vector2.Dot( ref ac, ref am, out acDotAm );

                    // 0 <= dot(AC,AM)
                    if( 0.0f <= acDotAm )
                    {
                        float acDotAc;
                        Vector2.Dot( ref ac, ref ac, out acDotAc );

                        // dot(AC,AM) <= dot(AC,AC)
                        if( acDotAm <= acDotAc )
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether this OrientedRectangleF intersects
        /// with the specified axis-aligned RectangleF.
        /// </summary>
        /// <param name="rectangle">
        /// The rectangle to check for intersection with.
        /// </param>
        /// <returns>
        /// true if they intersect; -or- otherwise false.
        /// </returns>
        public bool Intersects( RectangleF rectangle )
        {
            // Does the rectangle intersect with any of the outer line segements of the oriented rectangle?
            foreach( var segment in this.LineSegments )
            {
                if( rectangle.Intersects( segment ) )
                {
                    return true;
                }
            }

            // Does the axis-aligned rectangle fully contain the oriented rectangle?
            if( rectangle.Contains( this.upperLeft ) )
            {
                return true;
            }

            // Does the oriented rectangle fully contain the axis-aligned rectangle?
            return this.Contains( rectangle.Position );
        }

        /// <summary>
        /// The upper-left corner of this OrientedRectangleF.
        /// </summary>
        private Vector2 upperLeft;

        /// <summary>
        /// The upper-right corner of this OrientedRectangleF.
        /// </summary>
        private Vector2 upperRight;

        /// <summary>
        /// The lower-right corner of this OrientedRectangleF.
        /// Is calculated from the other three corners.
        /// </summary>
        private Vector2 lowerRight;

        /// <summary>
        /// The lower-left corner of this OrientedRectangleF.
        /// </summary>
        private Vector2 lowerLeft;
    }
}
