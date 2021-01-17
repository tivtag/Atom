// <copyright file="RectangleF.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Math.RectangleF structure.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Represents a floating-point rectangle that is
    /// defined by a position and a dimension value.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.RectangleFConverter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct RectangleF : IEquatable<RectangleF>, ICultureSensitiveToStringProvider
    {
        #region [ Fields ]

        /// <summary>
        /// The position of the upper left corner of this RectangleF on the x-axis.
        /// </summary>
        public float X;

        /// <summary>
        /// The position of the upper left corner of this RectangleF on the y-axis.
        /// </summary>
        public float Y;

        /// <summary>
        /// The width of this RectangleF.
        /// </summary>
        public float Width;

        /// <summary>
        /// The height of this RectangleF.
        /// </summary>
        public float Height;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the position of this <see cref="RectangleF"/>.
        /// </summary>
        /// <value>The position of this rectangle.</value>
        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Position
        {
            get
            { 
                return new Vector2( this.X, this.Y );
            }

            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref="RectangleF"/>.
        /// </summary>
        /// <value>The size of this rectangle.</value>
        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Size
        {
            get 
            { 
                return new Vector2( this.Width, this.Height );
            }

            set
            {
                this.Width  = value.X;
                this.Height = value.Y;
            }
        }
        
        /// <summary>
        /// Gets or sets the center of this <see cref="RectangleF"/>.
        /// </summary>
        /// <value>The center of this rectangle.</value>
        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Center
        {
            get
            { 
                return new Vector2( this.X + (this.Width / 2.0f), this.Y + (this.Height / 2.0f) );
            }

            set
            {
                this.X = value.X - (this.Width / 2.0f);
                this.Y = value.Y - (this.Height / 2.0f);
            }
        }

        /// <summary>
        /// Gets the x-coordinate of the left side of the rectangle.
        /// </summary>
        /// <value>The x-coordinate of the left side of the rectangle.</value>
        public float Left
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
        public float Right
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
        public float Top
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
        public float Bottom
        {
            get
            {
                return this.Y + this.Height;
            }
        }

        /// <summary>
        /// Gets or sets the minimum point of this <see cref="RectangleF"/>.
        /// </summary>
        /// <value>This value is the same as the <see cref="Position"/>.</value>
        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Minimum
        {
            get 
            { 
                return new Vector2( this.X, this.Y );
            }

            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the maximum point of this <see cref="RectangleF"/>.
        /// </summary>
        /// <value>The maximum point.</value>
        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Maximum
        {
            get 
            { 
                return new Vector2( this.X + this.Width, this.Y + this.Height );
            }

            set
            {
                this.Size = value - this.Minimum;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="position">The position of the new RectangleF.</param>
        /// <param name="size">The size of the new RectangleF.</param>>
        public RectangleF( Vector2 position, Vector2 size )
        {
            this.X      = position.X;
            this.Y      = position.Y;
            this.Width  = size.X;
            this.Height = size.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="position">The position of the new RectangleF.</param>
        /// <param name="width">The width of the new RectangleF.</param>
        /// <param name="height">The height of the new RectangleF.</param>
        public RectangleF( Vector2 position, float width, float height )
        {
            this.X      = position.X;
            this.Y      = position.Y;
            this.Width  = width;
            this.Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the new RectangleF.</param>
        /// <param name="y">The y-coordinate of the new RectangleF.</param>
        /// <param name="width">The width of the new RectangleF.</param>
        /// <param name="height">The height of the new RectangleF.</param>
        public RectangleF( float x, float y, float width, float height )
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Changes the position of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="amount">The values to adjust the position of the RectangleF by.</param>
        public void Offset( Vector2 amount )
        {
            this.Position += amount;
        }

        /// <summary>
        /// Changes the position of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="offsetX">Change in the x-position.</param>
        /// <param name="offsetY">Change in the y-position.</param>
        public void Offset( float offsetX, float offsetY )
        {
            this.X += offsetX;
            this.Y += offsetY;
        }

        /// <summary>
        /// Pushes the edges of this <see cref="RectangleF"/> out by the horizontal and vertical values specified.
        /// </summary>
        /// <param name="horizontalAmount">Value to push the sides out by.</param>
        /// <param name="verticalAmount">Value to push the top and bottom out by.</param>
        public void Inflate( float horizontalAmount, float verticalAmount )
        {
            this.X      -= horizontalAmount;
            this.Y      -= verticalAmount;
            this.Width  += horizontalAmount * 2;
            this.Height += verticalAmount * 2;
        }
        
        /// <summary>
        /// Gets the closest point <paramref name="from"/> the given point on this RectangleF.
        /// </summary>
        /// <param name="from">
        /// The point to project onto this RectangleF.
        /// </param>
        /// <returns>
        /// The closest point on this RectangleF.
        /// </returns>
        public Vector2 GetClosestPoint( Vector2 from )
        {
            Vector2 result;

            float left = this.Left;

            if( from.X < left )
            {
                result.X = left;
            }
            else
            {
                float right = this.Right;
                if( from.X > right )
                    result.X = right;
                else
                    result.X = from.X;
            }

            float top = this.Top;

            if( from.Y < top )
            {
                result.Y = top;
            }
            else
            {
                float bottom = this.Bottom;

                if( from.Y > bottom )
                    result.Y = bottom;
                else
                    result.Y = from.Y;
            }

            return result;
        }
        
        /// <summary>
        /// Receives the corners of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="cornerA">Will contain the first corner.</param>
        /// <param name="cornerB">Will contain the second corner.</param>
        /// <param name="cornerC">Will contain the third corner.</param>
        /// <param name="cornerD">Will contain the fourth corner.</param>
        public void GetCorners( out Vector2 cornerA, out Vector2 cornerB, out Vector2 cornerC, out Vector2 cornerD )
        {
            cornerA = new Vector2( X, Y );
            cornerB = new Vector2( X + Width, Y );
            cornerC = new Vector2( X + Width, Y + Height );
            cornerD = new Vector2( X, Y + Height );
        }

        #region > Containment Tests <

        /// <summary>
        /// Determines whether this RectangleF contains a specified vector represented by its x- and y-coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the specified vector.</param>
        /// <param name="y">The y-coordinate of the specified vector.</param>
        /// <returns>true if the specified vector is contained within this RectangleF; false otherwise.
        /// </returns>
        public bool Contains( float x, float y )
        {
            return this.X <= x && x < (this.X + this.Width) &&
                   this.Y <= y && y < (this.Y + this.Height);
        }

        /// <summary>
        /// Determines whether this RectangleF contains a specified Vector2.
        /// </summary>
        /// <param name="vector">The vector to evaluate.</param>
        /// <returns>true if the specified vector is contained within this RectangleF; false otherwise.
        /// </returns>
        public bool Contains( Vector2 vector )
        {
            return this.X <= vector.X && vector.X < (this.X + this.Width) &&
                   this.Y <= vector.Y && vector.Y < (this.Y + this.Height);
        }

        /// <summary>
        /// Determines whether this RectangleF contains a specified Point2.
        /// </summary>
        /// <param name="point">The point to evaluate.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified point is contained within this RectangleF;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Contains( Point2 point )
        {
            return this.X <= point.X && point.X < (this.X + this.Width) &&
                   this.Y <= point.Y && point.Y < (this.Y + this.Height);
        }

        /// <summary>
        /// Determines whether this RectangleF entirely contains a specified RectangleF.
        /// </summary>
        /// <param name="rect">The RectangleF to evaluate.</param>
        /// <returns>
        /// Returns <see langword="true"/> if this RectangleF entirely contains the specified RectangleF;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Contains( RectangleF rect )
        {
            return (this.X <= rect.X) && ((rect.X + rect.Width) <= (this.X + this.Width)) &&
                   (this.Y <= rect.Y) && ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        /// <summary>
        /// Determines whether this RectangleF entirely contains a specified RectangleF.
        /// </summary>
        /// <param name="rect">The RectangleF to evaluate.</param>
        /// <param name="result">
        /// Will contain whether this RectangleF entirely contains the specified RectangleF.
        /// </param>
        public void Contains( ref RectangleF rect, out bool result )
        {
            result = (this.X <= rect.X) && ((rect.X + rect.Width) <= (this.X + this.Width)) &&
                     (this.Y <= rect.Y) && ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        /// <summary>
        /// Determines whether this RectangleF entirely contains a specified RectangleF.
        /// </summary>
        /// <param name="rect">The RectangleF to evaluate.</param>
        /// <returns>
        /// Returns <see langword="true"/> if this RectangleF entirely contains the specified RectangleF; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Contains( Rectangle rect )
        {
            return (this.X <= rect.X) && ((rect.X + rect.Width) <= (this.X + this.Width)) &&
                   (this.Y <= rect.Y) && ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        #endregion

        #region > Intersection Tests <

        /// <summary>
        /// Determines whether a specified RectangleF floatersects with this RectangleF.
        /// </summary>
        /// <param name="rect">The RectangleF to evaluate.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified Rectangle intersects with this one; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Intersects( Rectangle rect )
        {
            return (rect.X < (this.X + this.Width)) && (this.X < (rect.X + rect.Width)) &&
                   (rect.Y < (this.Y + this.Height)) && (this.Y < (rect.Y + rect.Height));
        }

        /// <summary>
        /// Determines whether a specified RectangleF floatersects with this RectangleF.
        /// </summary>
        /// <param name="rect">The RectangleF to evaluate.</param>
        /// <param name="result">
        /// Will be <see langword="true"/> if the specified Rectangle intersects with this one; 
        /// otherwise <see langword="false"/>.
        /// </param>
        public void Intersects( ref Rectangle rect, out bool result )
        {
            result = (rect.X < (this.X + this.Width)) && (this.X < (rect.X + rect.Width)) &&
                     (rect.Y < (this.Y + this.Height)) && (this.Y < (rect.Y + rect.Height));
        }

        /// <summary>
        /// Determines whether a specified RectangleF intersects with this RectangleF.
        /// </summary>
        /// <param name="rect">The RectangleF to evaluate.</param>
        /// <returns>true if the specified RectangleF intersects with this one; false otherwise.
        /// </returns>
        public bool Intersects( RectangleF rect )
        {
            return (rect.X < (this.X + this.Width)) && (this.X < (rect.X + rect.Width)) &&
                   (rect.Y < (this.Y + this.Height)) && (this.Y < (rect.Y + rect.Height));
        }

        /// <summary>
        /// Determines whether a specified RectangleF intersects with this RectangleF.
        /// </summary>
        /// <param name="rect">
        /// The RectangleF to evaluate.
        /// </param>
        /// <param name="result">
        /// Will be <see langword="true"/> if the specified RectangleF intersects with this one; 
        /// otherwise <see langword="false"/>.
        /// </param>
        public void Intersects( ref RectangleF rect, out bool result )
        {
            result = (rect.X < (this.X + this.Width)) && (this.X < (rect.X + rect.Width)) &&
                     (rect.Y < (this.Y + this.Height)) && (this.Y < (rect.Y + rect.Height));
        }

        /// <summary>
        /// Gets a value indicating whether this RectangleF intersects with the given <see cref="Circle"/>.
        /// </summary>
        /// <param name="circle">
        /// The circle to test against.
        /// </param>
        /// <returns>
        /// Returns true if this RectangleF intersects with the given <paramref name="circle"/>; 
        /// otherwise false.
        /// </returns>
        public bool Intersects( Circle circle )
        {
            Vector2 point = this.GetClosestPoint( circle.Center );
            return circle.Contains( point );
        }


        /// <summary>
        /// Determines whether a specified line segment intersects with this RectangleF.
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

            Func<float, float, bool> clips = ( float directedProjection, float directedDistance ) =>
            {
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

        #region > Creation Helpers <

        #region FromPolygon

        /// <summary>
        /// Creates a new axis aligned <see cref="RectangleF"/> that covers the specified <see cref="Polygon2"/>.
        /// </summary>
        /// <param name="polygon">The input polygon.</param>
        /// <returns>The created rectangle.</returns>
        public static RectangleF FromPolygon( Polygon2 polygon )
        {
            if( polygon == null )
                throw new ArgumentNullException( "polygon" );

            var vertices = polygon.Vertices;
            if( vertices.Count == 0 )
                throw new ArgumentException( MathErrorStrings.OperationRequiresAtleastOnePoint, "polygon" );

            Vector2 minimum = vertices[0];
            Vector2 maximum = minimum;

            for( int i = 0; i < vertices.Count; ++i )
            {
                Vector2 vertex = vertices[i];

                if( vertex.X < minimum.X ) minimum.X = vertex.X;
                if( vertex.X > maximum.X ) maximum.X = vertex.X;
                if( vertex.Y < minimum.Y ) minimum.Y = vertex.Y;
                if( vertex.Y > maximum.Y ) maximum.Y = vertex.Y;
            }

            RectangleF rectangle;

            rectangle.X      = minimum.X;
            rectangle.Y      = minimum.Y;
            rectangle.Width  = maximum.X - minimum.X;
            rectangle.Height = maximum.Y - minimum.Y;

            return rectangle;
        }

        #endregion

        #region FromOrientedRectangle
        
        /// <summary>
        /// Creates a new axis aligned <see cref="RectangleF"/> that fully covers the given input <see cref="OrientedRectangleF"/>.
        /// </summary>
        /// <param name="rectangle">
        /// The oriented rectangle.
        /// </param>
        /// <returns>The rectangle that has been created.</returns>
        public static RectangleF FromOrientedRectangle( OrientedRectangleF rectangle )
        {
            Vector2 min, max;
            rectangle.GetMinMax( out min, out max );

            return new RectangleF(
                min,
                max - min
            );
        }

        /// <summary>
        /// Creates a new axis aligned <see cref="RectangleF"/> given an input <see cref="RectangleF"/>
        /// and a <paramref name="rotation"/> value.
        /// </summary>
        /// <param name="rectangle">
        /// The un-oriented rectangle.
        /// </param>
        /// <param name="rotation">
        /// The rotation in radians.
        /// </param>
        /// <param name="rotationOrigin">
        /// The origin (relative to to the rectangle) to rotate about.
        /// </param>
        /// <returns>The created rectangle.</returns>
        public static RectangleF FromOrientedRectangle( RectangleF rectangle, float rotation, Vector2 rotationOrigin )
        {
            return FromOrientedRectangle( rectangle, Matrix2.FromAngle( rotation ), rotationOrigin );
        }

        /// <summary>
        /// Creates a new axis aligned <see cref="RectangleF"/> given an input <see cref="RectangleF"/>
        /// and a <paramref name="rotation"/> value.
        /// </summary>
        /// <param name="rectangle">
        /// The un-oriented rectangle.
        /// </param>
        /// <param name="rotation">
        /// The rotation encapsulated in a <see cref="Matrix2"/>.
        /// </param>
        /// <param name="rotationOrigin">
        /// The origin (relative to to the rectangle) to rotate about.
        /// </param>
        /// <returns>The created rectangle.</returns>
        public static RectangleF FromOrientedRectangle( RectangleF rectangle, Matrix2 rotation, Vector2 rotationOrigin )
        {
            // Move rectangle to orgin:
            Vector2 offset     = rectangle.Position + rotationOrigin;
            rectangle.Position = -rotationOrigin;

            // Receive corners of the rectangle:
            Vector2 cornerA, cornerB, cornerC, cornerD;
            rectangle.GetCorners( out cornerA, out cornerB, out cornerC, out cornerD );

            // Transform corners:
            cornerA = Vector2.Transform( cornerA, rotation ) + offset;
            cornerB = Vector2.Transform( cornerB, rotation ) + offset;
            cornerC = Vector2.Transform( cornerC, rotation ) + offset;
            cornerD = Vector2.Transform( cornerD, rotation ) + offset;

            // Find the smallest and the greatest value:
            Vector2 min = cornerA, max = cornerA;

            // B
            if( cornerB.X < min.X )
                min.X = cornerB.X;
            else if( cornerB.X > max.X )
                max.X = cornerB.X;

            if( cornerB.Y < min.Y )
                min.Y = cornerB.Y;
            else if( cornerB.Y > max.Y )
                max.Y = cornerB.Y;

            // C
            if( cornerC.X < min.X )
                min.X = cornerC.X;
            else if( cornerC.X > max.X )
                max.X = cornerC.X;

            if( cornerC.Y < min.Y )
                min.Y = cornerC.Y;
            else if( cornerC.Y > max.Y )
                max.Y = cornerC.Y;

            // D
            if( cornerD.X < min.X )
                min.X = cornerD.X;
            else if( cornerD.X > max.X )
                max.X = cornerD.X;

            if( cornerD.Y < min.Y )
                min.Y = cornerD.Y;
            else if( cornerD.Y > max.Y )
                max.Y = cornerD.Y;

            // Build rectangle:
            return new RectangleF(
                min.X,
                min.Y,
                max.X - min.X,
                max.Y - min.Y
            );
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether this <see cref="RectangleF"/> is equal to the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The Object to compare with this RectangleF.</param>
        /// <returns>true if this RectangleF is equal to the specified object; otherwise false.</returns>
        public override bool Equals( object obj )
        {
            if( obj is RectangleF )
                return this.Equals( (RectangleF)obj );

            return false;
        }

        /// <summary>
        /// Returns whether this <see cref="RectangleF"/> is equal to the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="other">The RectangleF to compare with this RectangleF.</param>
        /// <returns>true if this RectangleF is equal to the specified object; otherwise false.</returns>
        public bool Equals( RectangleF other )
        {
            return this.X == other.X && 
                   this.Y == other.Y &&
                   this.Width      == other.Width && 
                   this.Height     == other.Height;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overriden to return a human-readable text representation of the RectangleF.
        /// </summary>
        /// <returns>A human-readable text representation of the RectangleF.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the RectangleF.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the RectangleF.</returns>
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
        /// Gets the hash code of this <see cref="RectangleF"/>.
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
        /// Returns whether the specified <see cref="RectangleF"/>s are equal.
        /// </summary>
        /// <param name="left">The RectangleF on the left side of the equation.</param>
        /// <param name="right">The RectangleF on the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the RectangleFs are equal; otherwise <see langword="true"/>.
        /// </returns>
        public static bool operator ==( RectangleF left, RectangleF right )
        {
            return left.X == right.X && 
                   left.Y == right.Y &&
                   left.Width      == right.Width      && 
                   left.Height     == right.Height;
        }

        /// <summary>
        /// Returns whether the specified <see cref="RectangleF"/>s are not equal.
        /// </summary>
        /// <param name="left">The RectangleF on the left side of the equation.</param>
        /// <param name="right">The RectangleF on the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the RectangleFs are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=( RectangleF left, RectangleF right )
        {
            return left.X != right.X || 
                   left.Y != right.Y ||
                   left.Width      != right.Width      || 
                   left.Height     != right.Height;
        }
        
        /// <summary>
        /// Explicit cast operator that implements conversion
        /// from a <see cref="RectangleF"/> to a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rectangle">
        /// The input rectangle.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static explicit operator Rectangle( RectangleF rectangle )
        {
            return new Rectangle( (int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height );
        }

        #endregion
    }
}
