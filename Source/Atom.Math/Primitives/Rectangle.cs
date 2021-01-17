// <copyright file="Rectangle.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Rectangle structure.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Represents a rectangle that is defined 
    /// by a position and a dimension value.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.RectangleConverter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct Rectangle : IEquatable<Rectangle>, ICultureSensitiveToStringProvider
    {
        #region [ Fields ]

        /// <summary>
        /// The position of the upper left corner of this Rectangle on the x-axis.
        /// </summary>
        public int X;

        /// <summary>
        /// The position of the upper left corner of this Rectangle on the y-axis.
        /// </summary>
        public int Y;

        /// <summary>
        /// The width of this Rectangle.
        /// </summary>
        public int Width;

        /// <summary>
        /// The height of this Rectangle.
        /// </summary>
        public int Height;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the position of the upper-left corner of this Rectangle.
        /// </summary>
        /// <value>The position of this rectangle.</value>
        [System.Xml.Serialization.XmlIgnore]
        public Point2 Position
        {
            get
            { 
                return new Point2( this.X, this.Y );
            }

            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this Rectangle.
        /// </summary>
        /// <value>The size of this rectangle.</value>
        [System.Xml.Serialization.XmlIgnore]
        public Point2 Size
        {
            get
            { 
                return new Point2( this.Width, this.Height );
            }

            set
            {
                this.Width  = value.X;
                this.Height = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the center of this Rectangle.
        /// </summary>
        /// <value>The center of this rectangle.</value>
        [System.Xml.Serialization.XmlIgnore]
        public Point2 Center
        {
            get 
            {
                return new Point2( this.X + (this.Width / 2), this.Y + (this.Height / 2) );
            }

            set
            {
                this.X = value.X - (this.Width / 2);
                this.Y = value.Y - (this.Height / 2);
            }
        }

        /// <summary>
        /// Gets the x-coordinate of the left side of the rectangle.
        /// </summary>
        /// <value>The x-coordinate of the left side of the rectangle.</value>
        public int Left
        {
            get
            {
                return this.X;
            }
        }

        /// <summary>
        /// Gets the x-coordinate of the right side of the rectangle.
        /// </summary>
        /// <value>The x-coordinate of the right side of the rectangle.</value>
        public int Right
        {
            get
            {
                return this.X + this.Width;
            }
        }

        /// <summary>
        /// Gets the y-coordinate of the top of the rectangle.
        /// </summary>
        /// <value>The y-coordinate of the top of the rectangle.</value>
        public int Top
        {
            get
            {
                return this.Y;
            }
        }

        /// <summary>
        /// Gets the y-coordinate of the bottom of the rectangle.
        /// </summary>
        /// <value>The y-coordinate of the bottom of the rectangle.</value>
        public int Bottom
        {
            get
            {
                return this.Y + this.Height;
            }
        }

        #endregion

        #region [ Constants ]

        /// <summary>
        /// Gets an empty Rectangle.
        /// </summary>
        /// <value>A rectangle at position (0, 0) and of size (0, 0).</value>
        public static Rectangle Empty
        { 
            get { return emtpy; } 
        }

        /// <summary>
        /// Stores the empty rectangle.
        /// </summary>
        private static readonly Rectangle emtpy = new Rectangle();

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the Rectangle structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the new Rectangle.</param>
        /// <param name="y">The y-coordinate of the new Rectangle.</param>
        /// <param name="width">The width of the new Rectangle.</param>
        /// <param name="height">The height of the new Rectangle.</param>
        public Rectangle( int x, int y, int width, int height )
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the Rectangle structure.
        /// </summary>
        /// <param name="position">The position of the new Rectangle.</param>
        /// <param name="size">The size of the Rectangle.</param>
        public Rectangle( Point2 position, Point2 size )
        {
            this.X      = position.X;
            this.Y      = position.Y;
            this.Width  = size.X;
            this.Height = size.Y;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Changes the position of this Rectangle.
        /// </summary>
        /// <param name="amount">The values to adjust the position of the Rectangle by.</param>
        public void Offset( Point2 amount )
        {
            this.Position += amount;
        }

        /// <summary>
        /// Changes the position of this Rectangle.
        /// </summary>
        /// <param name="offsetX">Change in the x-position.</param>
        /// <param name="offsetY">Change in the y-position.</param>
        public void Offset( int offsetX, int offsetY )
        {
            this.X += offsetX;
            this.Y += offsetY;
        }

        /// <summary>
        /// Pushes the edges of this Rectangle out by the horizontal and vertical values specified.
        /// </summary>
        /// <param name="horizontalAmount">Value to push the sides out by.</param>
        /// <param name="verticalAmount">Value to push the top and bottom out by.</param>
        public void Inflate( int horizontalAmount, int verticalAmount )
        {
            checked
            {
                this.X      -= horizontalAmount;
                this.Y      -= verticalAmount;
                this.Width  += horizontalAmount * 2;
                this.Height += verticalAmount * 2;
            }
        }

        /// <summary>
        /// Gets the closest point <paramref name="from"/> the given point on this Rectangle.
        /// </summary>
        /// <param name="from">
        /// The point to project onto this Rectangle.
        /// </param>
        /// <returns>
        /// The closest point on this Rectangle.
        /// </returns>
        public Point2 GetClosestPoint( Point2 from )
        {
            Point2 result;

            int left = this.Left;
            if( from.X < left )
            {
                result.X = left;
            }
            else
            {
                int right = this.Right;
                if( from.X > right )
                    result.X = right;
                else
                    result.X = from.X;
            }

            int top = this.Top;
            if( from.Y < top )
            {
                result.Y = top;
            }
            else
            {
                int bottom = this.Bottom;
                if( from.Y > bottom )
                    result.Y = bottom;
                else
                    result.Y = from.Y;
            }

            return result;
        }

        /// <summary>
        /// Receives the corners of this Rectangle.
        /// </summary>
        /// <param name="cornerA">Will contain the first corner.</param>
        /// <param name="cornerB">Will contain the second corner.</param>
        /// <param name="cornerC">Will contain the third corner.</param>
        /// <param name="cornerD">Will contain the fourth corner.</param>
        public void GetCorners( out Point2 cornerA, out Point2 cornerB, out Point2 cornerC, out Point2 cornerD )
        {
            cornerA = new Point2( X, Y );
            cornerB = new Point2( X + Width, Y );
            cornerC = new Point2( X + Width, Y + Height );
            cornerD = new Point2( X, Y + Height );
        }

        /// <summary>
        /// Gets the shortest distance from the given <paramref name="point"/> to any side of this Rectangle.
        /// </summary>
        /// <param name="point">
        /// The point to test against.
        /// </param>
        /// <param name="pointOnRect">
        /// The projected point on the rectangle.
        /// </param>
        /// <returns>
        /// The distance from the point to the rectangle.
        /// </returns>
        public float DistanceTo( Point2 point, out Point2 pointOnRect )
        {
            Point2 cornerA, cornerB, cornerC, cornerD;
            GetCorners( out cornerA, out cornerB, out cornerC, out cornerD );

            float bestDistance = float.MaxValue;
            Vector2 pointOnRectTest;
            float distance;
            pointOnRect = Point2.Zero;

            // Upper Left -> Upper Right
            distance = LineSegment2.DistanceTo( point, cornerA, cornerB, out pointOnRectTest );
            if( distance < bestDistance )
            {
                pointOnRect = (Point2)pointOnRectTest;
                bestDistance = distance;
            }

            // Upper Right -> Lower Right
            distance = LineSegment2.DistanceTo( point, cornerB, cornerC, out pointOnRectTest );
            if( distance < bestDistance )
            {
                pointOnRect = (Point2)pointOnRectTest;
                bestDistance = distance;
            }
            
            // Lower Right -> Lower Left
            distance = LineSegment2.DistanceTo( point, cornerC, cornerD, out pointOnRectTest );
            if( distance < bestDistance )
            {
                pointOnRect = (Point2)pointOnRectTest;
                bestDistance = distance;
            }

            // Lower Left -> Upper Left
            distance = LineSegment2.DistanceTo( point, cornerD, cornerA, out pointOnRectTest );
            if( distance < bestDistance )
            {
                pointOnRect = (Point2)pointOnRectTest;
                bestDistance = distance;
            }

            return bestDistance;
        }

        #region > Containment Tests <

        /// <summary>
        /// Determines whether this Rectangle contains a specified point represented by its x- and y-coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the specified point.</param>
        /// <param name="y">The y-coordinate of the specified point.</param>
        /// <returns>true if the specified point is contained within this Rectangle; false otherwise.
        /// </returns>
        public bool Contains( int x, int y )
        {
            return this.X <= x && x < (this.X + this.Width) &&
                   this.Y <= y && y < (this.Y + this.Height);
        }

        /// <summary>
        /// Determines whether this Rectangle contains a specified Point.
        /// </summary>
        /// <param name="point">The Point to evaluate.</param>
        /// <returns>true if the specified Point is contained within this Rectangle; false otherwise.
        /// </returns>
        public bool Contains( Point2 point )
        {
            return this.X <= point.X && point.X < (this.X + this.Width) &&
                   this.Y <= point.Y && point.Y < (this.Y + this.Height);
        }

        /// <summary>
        /// Determines whether this Rectangle contains a specified Point.
        /// </summary>
        /// <param name="point">The Point to evaluate.</param>
        /// <returns>true if the specified Point is contained within this Rectangle; false otherwise.
        /// </returns>
        public bool Contains( Vector2 point )
        {
            return this.X <= point.X && point.X < (this.X + this.Width) &&
                   this.Y <= point.Y && point.Y < (this.Y + this.Height);
        }

        /// <summary>
        /// Determines whether this Rectangle entirely contains a specified Rectangle.
        /// </summary>
        /// <param name="rect">The Rectangle to evaluate.</param>
        /// <returns>true if this Rectangle entirely contains the specified Rectangle; false otherwise.
        /// </returns>
        public bool Contains( Rectangle rect )
        {
            return (this.X <= rect.X) && ((rect.X + rect.Width ) <= (this.X + this.Width )) &&
                   (this.Y <= rect.Y) && ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        /// <summary>
        /// Determines whether this Rectangle entirely contains a specified Rectangle.
        /// </summary>
        /// <param name="rect">The Rectangle to evaluate.</param>
        /// <param name="result">
        /// Will contain whether this Rectangle entirely contains the specified Rectangle.
        /// </param>
        public void Contains( ref Rectangle rect, out bool result )
        {
            result = (this.X <= rect.X) && ((rect.X + rect.Width) <= (this.X + this.Width)) &&
                     (this.Y <= rect.Y) && ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        #endregion

        #region > Intersection Tests <

        /// <summary>
        /// Determines whether a specified Rectangle intersects with this Rectangle.
        /// </summary>
        /// <param name="rect">
        /// The Rectangle to evaluate.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified Rectangle intersects with this one; otherwise <see langword="false"/>.
        /// </returns>
        public bool Intersects( Rectangle rect )
        {
            return (rect.X < (this.X + this.Width)) && (this.X < (rect.X + rect.Width)) &&
                   (rect.Y < (this.Y + this.Height)) && (this.Y < (rect.Y + rect.Height));
        }

        /// <summary>
        /// Determines whether a specified Rectangle intersects with this Rectangle.
        /// </summary>
        /// <param name="rect">
        /// The Rectangle to evaluate.
        /// </param>
        /// <param name="result">
        /// Will be <see langword="true"/> if the specified Rectangle intersects with this one; otherwise <see langword="false"/>.
        /// </param>
        public void Intersects( ref Rectangle rect, out bool result )
        {
            result = (rect.X < (this.X + this.Width)) && (this.X < (rect.X + rect.Width)) &&
                     (rect.Y < (this.Y + this.Height)) && (this.Y < (rect.Y + rect.Height));
        }

        /// <summary>
        /// Determines whether a specified <see cref="RectangleF"/> intersects with this Rectangle.
        /// </summary>
        /// <param name="rect">
        /// The Rectangle to evaluate.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified Rectangle intersects with this one; otherwise <see langword="false"/>.
        /// </returns>
        public bool Intersects( RectangleF rect )
        {
            return (rect.X < (this.X + this.Width )) && (this.X < (rect.X + rect.Width )) &&
                   (rect.Y < (this.Y + this.Height)) && (this.Y < (rect.Y + rect.Height));
        }

        /// <summary>
        /// Determines whether a specified <see cref="RectangleF"/> intersects with this Rectangle.
        /// </summary>
        /// <param name="rect">
        /// The Rectangle to evaluate.
        /// </param>
        /// <param name="result">
        /// Will be <see langword="true"/> if the specified Rectangle intersects with this one; otherwise <see langword="false"/>.
        /// </param>
        public void Intersects( ref RectangleF rect, out bool result )
        {
            result = (rect.X < (this.X + this.Width )) && (this.X < (rect.X + rect.Width )) &&
                     (rect.Y < (this.Y + this.Height)) && (this.Y < (rect.Y + rect.Height));
        }

        /// <summary>
        /// Determines whether a specified line segment intersects with this Rectangle.
        /// </summary>
        /// <param name="line">
        /// The line segment to test for intersection with.
        /// </param>
        /// <returns>
        /// true if they intersect; -or-
        /// otherwise false.
        /// </returns>
        public bool Intersects( FastLineSegment2 line )
        {
            Vector2 start = line.Start;
            Vector2 end   = line.End;
            Vector2 delta = end - start;

            Vector2 rectMinimum = this.Position;
            Vector2 rectMaximum = this.Position + this.Size;

            float tMinimum = 0, tMaximum = 1;

            Func<float, float, bool> clips = (float directedProjection, float directedDistance) => {
                if( directedProjection == 0 )
                {
                    if( directedDistance < 0 ) return false;
                }
                else
                {
                    float amount = directedDistance / directedProjection;
                    if( directedProjection < 0 )
                    {
                        if( amount > tMaximum ) return false;
                        else if( amount > tMinimum ) tMinimum = amount;
                    }
                    else
                    {
                        if( amount < tMinimum ) return false;
                        else if( amount < tMaximum ) tMaximum = amount;
                    }
                }

                return true;
            };

            if( clips( -delta.X, start.X - rectMinimum.X ) )
            {
                if( clips( delta.X, rectMaximum.X - start.X ) )
                {
                    if( clips( -delta.Y, start.Y - rectMinimum.Y ) )
                    {
                        if( clips( delta.Y, rectMaximum.Y - start.Y ) )
                        {
                            if( tMaximum < 1 )
                            {
                                end.X = start.X + tMaximum * delta.X;
                                end.Y = start.Y + tMaximum * delta.Y;
                            }
                            if( tMinimum > 0 )
                            {
                                start.X += tMinimum * delta.X;
                                start.Y += tMinimum * delta.Y;
                            }

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether this Rectangle is equal to the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The Object to compare with this Rectangle.</param>
        /// <returns>true if this Rectangle is equal to the specified object; otherwise false.</returns>
        public override bool Equals( object obj )
        {
            if( obj is Rectangle )
                return this.Equals( (Rectangle)obj );

            return false;
        }

        /// <summary>
        /// Returns whether this Rectangle is equal to the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="other">The Rectangle to compare with this Rectangle.</param>
        /// <returns>true if this Rectangle is equal to the specified object; otherwise false.</returns>
        public bool Equals( Rectangle other )
        {
            return this.X == other.X && 
                   this.Y == other.Y &&
                   this.Width      == other.Width && 
                   this.Height     == other.Height;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Return a human-readable text representation of the Rectangle.
        /// </summary>
        /// <returns>A human-readable text representation of the Rectangle.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Rectangle.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Rectangle.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider,
                "[X:{0} Y:{1} Width:{2} Height:{3}]",
                this.X.ToString( formatProvider ),
                this.Y.ToString( formatProvider ),
                this.Width.ToString( formatProvider ),
                this.Height.ToString( formatProvider )
            );
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets the hash code of this Rectangle.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.X );
            hashBuilder.AppendStruct( this.Y );
            hashBuilder.AppendStruct( this.Width );
            hashBuilder.AppendStruct( this.Height );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        /// <summary>
        /// Returns whether the specified Rectangles are equal.
        /// </summary>
        /// <param name="left">The Rectangle on the left side of the equation.</param>
        /// <param name="right">The Rectangle on the right side of the equation.</param>
        /// <returns>true if the Rectangles are equal; false otherwise.</returns>
        public static bool operator ==( Rectangle left, Rectangle right )
        {
            return left.X == right.X && 
                   left.Y == right.Y &&
                   left.Width      == right.Width      && 
                   left.Height     == right.Height;
        }

        /// <summary>
        /// Returns whether the specified Rectangles are not equal.
        /// </summary>
        /// <param name="left">The Rectangle on the left side of the equation.</param>
        /// <param name="right">The Rectangle on the right side of the equation.</param>
        /// <returns>true if the Rectangles are not equal; false otherwise.</returns>
        public static bool operator !=( Rectangle left, Rectangle right )
        {
            return left.X != right.X || 
                   left.Y != right.Y ||
                   left.Width      != right.Width      || 
                   left.Height     != right.Height;
        }

        /// <summary>
        /// Implicit cast operator that implements conversion
        /// from a Rectangle to a <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="rectangle">
        /// The input rectangle.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static implicit operator RectangleF( Rectangle rectangle )
        {
            return new RectangleF( rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height );
        }

        #endregion
    }
}
