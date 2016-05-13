// <copyright file="Triangle2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Triangle2 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary> 
    /// Represents a triangle in two dimensional space.
    /// </summary>
    [System.ComponentModel.TypeConverter( typeof( System.ComponentModel.ExpandableObjectConverter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct Triangle2 : IEquatable<Triangle2>, ICultureSensitiveToStringProvider
    {
        #region [ Properties ]

        #region Points

        /// <summary>
        /// Gets or sets the location of point A of the triangle.
        /// </summary>
        /// <value>The first point that makes up the triangle.</value>
        public Vector2 A
        {
            get
            {
                return this.a;
            }

            set
            { 
                this.a = value;
            }
        }

        /// <summary>
        /// Gets or sets the location of point B of the triangle.
        /// </summary>
        /// <value>The second point that makes up the triangle.</value>
        public Vector2 B
        {
            get
            {
                return this.b;
            }

            set
            {
                this.b = value;
            }
        }

        /// <summary>
        /// Gets or sets the location of point C of the triangle.
        /// </summary>
        /// <value>The third point that makes up the triangle.</value>
        public Vector2 C
        {
            get 
            {
                return this.c;
            }

            set
            { 
                this.c = value;
            }
        }

        #endregion

        #region Lines

        /// <summary>
        /// Gets the line between B and C that is opposite of A.
        /// </summary>
        /// <value>The line from B to C.</value>
        public Vector2 LineA
        {
            get
            {
                return this.c - this.b;
            }
        }

        /// <summary>
        /// Gets the line between C and A that is opposite of B.
        /// </summary>
        /// <value>The line from C to A.</value>
        public Vector2 LineB
        {
            get 
            {
                return this.a - this.c;
            }
        }

        /// <summary>
        /// Gets the line between A and B that is opposite of C.
        /// </summary>
        /// <value>The line from A to B.</value>
        public Vector2 LineC
        {
            get 
            {
                return this.b - this.a;
            }
        }

        #endregion

        #region Angles

        /// <summary>
        /// Gets the alpha angle (in radians), which is at the A point.
        /// </summary>
        /// <value>
        /// The angle in radians.
        /// </value>
        public float Alpha
        {
            get
            {
                return Constants.Pi - Vector2.Angle( this.LineB, this.LineC );
            }
        }

        /// <summary>
        /// Gets the beta angle (in radians), which is at the B point.
        /// </summary>
        /// <value>
        /// The angle in radians.
        /// </value>
        public float Beta
        {
            get
            {
                return Constants.Pi -  Vector2.Angle( this.LineC, this.LineA );
            }
        }

        /// <summary>
        /// Gets the gamma angle (in radians), which is at the C point.
        /// </summary>
        /// <value>
        /// The angle in radians.
        /// </value>
        public float Gamma
        {
            get
            {
                return Constants.Pi -  Vector2.Angle( this.LineA, this.LineB );
            }
        }

        #endregion

        #region TriangleType

        /// <summary>
        /// Gets the type of this <see cref="Triangle2"/>.
        /// </summary>
        /// <remarks> 
        /// This is a quite expensive operation.
        /// </remarks>
        /// <value>
        /// The type of this Triangle.
        /// </value>
        public TriangleTypes TriangleType
        {
            get
            {
                float lineA = this.LineA.SquaredLength;
                float lineB = this.LineB.SquaredLength;
                float lineC = this.LineC.SquaredLength;
                float halfPi = Constants.PiOver2;

                #region if( lineA == lineB )
                if( lineA == lineB )
                {
                    if( lineA == lineC ) // isosceles triangles are always acute-angled
                    {
                        return TriangleTypes.Isosceles | TriangleTypes.AcuteAngled;
                    }
                    else
                    {
                        float gamma  = this.Gamma;

                        if( gamma.IsApproximate( halfPi ) )
                            return TriangleTypes.Equilateral | TriangleTypes.Perpendicular;
                        else if( gamma < halfPi )
                            return TriangleTypes.Equilateral | TriangleTypes.AcuteAngled;
                        else
                            return TriangleTypes.Equilateral | TriangleTypes.OptuseAngled;
                    }
                }
                #endregion

                #region if( lineA == lineC )
                if( lineA == lineC )
                {
                    if( lineA == lineB ) // isosceles triangles are always acute-angled
                    {
                        return TriangleTypes.Isosceles | TriangleTypes.AcuteAngled;
                    }
                    else
                    {
                        float alpha = this.Alpha;

                        if( alpha.IsApproximate( halfPi ) )
                            return TriangleTypes.Equilateral | TriangleTypes.Perpendicular;
                        else if( alpha < halfPi )
                            return TriangleTypes.Equilateral | TriangleTypes.AcuteAngled;
                        else
                            return TriangleTypes.Equilateral | TriangleTypes.OptuseAngled;
                    }
                }
                #endregion

                #region if( lineB == lineC )
                if( lineB == lineC )
                {
                    if( lineB == lineA ) // isosceles triangles are always acute-angled
                    {
                        return TriangleTypes.Isosceles | TriangleTypes.AcuteAngled;
                    }
                    else
                    {
                        float beta = this.Beta;

                        if( beta.IsApproximate( halfPi ) )
                            return TriangleTypes.Equilateral | TriangleTypes.Perpendicular;
                        else if( beta < halfPi )
                            return TriangleTypes.Equilateral | TriangleTypes.AcuteAngled;
                        else
                            return TriangleTypes.Equilateral | TriangleTypes.OptuseAngled;
                    }
                }

                #endregion

                float alphaA = this.Alpha;
                float betaA = this.Beta;
                float gammaA = this.Gamma;

                if( alphaA.IsApproximate( halfPi ) ||
                     betaA.IsApproximate( halfPi ) ||
                    gammaA.IsApproximate( halfPi ) )
                {
                    return TriangleTypes.Perpendicular;
                }

                if( alphaA > halfPi || betaA > halfPi || gammaA > halfPi )
                {
                    return TriangleTypes.OptuseAngled;
                }

                return TriangleTypes.AcuteAngled;
            }
        }

        #endregion

        #region Heights

        /// <summary>
        /// Gets the height of the base a.
        /// </summary>
        /// <value>The length of the line that goes from point A to line a.</value>
        public float HeightA
        {
            get
            {
                float lengthA = this.LineA.Length;
                float lengthB = this.LineB.Length;
                float lengthC = this.LineC.Length;
                
                float squaredA = lengthA * lengthA;
                float squaredB = lengthB * lengthB;
                float squaredC = lengthC * lengthC;

                float gamma = this.Gamma;

                if( gamma < Constants.Pi / 2.0f )
                {
                    float v1 = squaredA + squaredB - squaredC;
                    return (float)System.Math.Sqrt( squaredB - (v1 * v1 / 4.0f * squaredA) );
                }

                if( gamma > Constants.Pi / 2.0f )
                {
                    float v2 = squaredA - squaredB - squaredC;
                    return (float)System.Math.Sqrt( squaredC - (v2 * v2 / 4.0f * squaredB) );
                }

                return lengthC; // beta is pi/2
            }
        }

        /// <summary>
        /// Gets the height of the base b.
        /// </summary>
        /// <value>The length of the line that goes from point B to line b.</value>
        public float HeightB
        {
            get
            {
                float lengthA = this.LineA.Length;
                float lengthB = this.LineB.Length;
                float lengthC = this.LineC.Length;

                float squaredA = lengthA * lengthA;
                float squaredB = lengthB * lengthB;
                float squaredC = lengthC * lengthC;

                float alpha = this.Alpha;

                if( alpha < Constants.Pi / 2.0f )
                {
                    float v1 = squaredB + squaredC - squaredA;
                    return (float)System.Math.Sqrt( squaredC - (v1 * v1 / 4 * squaredB) );
                }

                if( alpha > Constants.Pi / 2.0f )
                {
                    float v2 = squaredC - squaredA - squaredB;
                    return (float)System.Math.Sqrt( squaredB - (v2 * v2 / 4 * squaredA) );
                }

                return lengthB; // beta is pi/2
            }
        }

        /// <summary>
        /// Gets the height of the base c.
        /// </summary>
        /// <value>The length of the line that goes from point C to line c.</value>
        public float HeightC
        {
            get
            {
                float lengthA = this.LineA.Length;
                float lengthB = this.LineB.Length;
                float lengthC = this.LineC.Length;

                float squaredA = lengthA * lengthA;
                float squaredB = lengthB * lengthB;
                float squaredC = lengthC * lengthC;

                float beta = this.Beta;

                if( beta < Constants.Pi / 2.0f )
                {
                    float v1 = squaredA + squaredC - squaredB;
                    return (float)System.Math.Sqrt( squaredA - (v1 * v1 / 4 * squaredC) );
                }

                if( beta > Constants.Pi / 2.0f )
                {
                    float v2 = squaredB - squaredA - squaredC;
                    return (float)System.Math.Sqrt( squaredA - (v2 * v2 / 4 * squaredC) );
                }

                return lengthA; // beta is pi/2
            }
        }

        #endregion

        #region Area

        /// <summary>
        /// Gets the area of this Triangle2.
        /// </summary>
        /// <value>The area of this Triangle2.</value>
        public float Area
        {
            get
            {
                return (this.HeightC * this.LineC.Length) / 2.0f;
            }
        }

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the Triangle2 struct.
        /// </summary>
        /// <param name="pointA">
        /// The location of the first point that makes up the new Triangle2.
        /// </param>
        /// <param name="pointB">
        /// The location of the second point that makes up the new Triangle2.
        /// </param>
        /// <param name="pointC">
        /// The location of the third point that makes up the new Triangle2.
        /// </param>
        public Triangle2( Vector2 pointA, Vector2 pointB, Vector2 pointC )
        {
            this.a = pointA;
            this.b = pointB;
            this.c = pointC;
        }

        #endregion

        #region [ Methods ]

        #region ToVertices

        /// <summary>
        /// Converts the triangles represenation into an ordered list of vertices (A, B, C).
        /// </summary>
        /// <returns>
        /// A new array of size three that contains the vertices of this Triangle.
        /// </returns>
        public Vector2[] ToVertices()
        {
            return new Vector2[] { this.a, this.b, this.c };
        }

        #endregion

        #region ToPolygon

        /// <summary>
        /// Converts the triangles represenation into an ordered list of vertices representing a polygon (A, B, C). 
        /// </summary>
        /// <returns>
        /// A polygon with three vertices.
        /// </returns>
        public Polygon2 ToPolygon()
        {
            return new Polygon2( this.ToVertices() );
        }

        #endregion

        #region Operators

        /// <summary>
        /// Gets whether all elements of the two specified <see cref="Triangle2"/>s are equal.
        /// </summary>
        /// <param name="left"> The <see cref="Triangle2"/> on the left side of the equation. </param>
        /// <param name="right"> The <see cref="Triangle2"/> on the right side of the equation. </param>
        /// <returns>
        /// Returns <see langword="true"/> if all elements are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==( Triangle2 left, Triangle2 right )
        {
            return left.a == right.a && left.b == right.b && left.c == right.c;
        }

        /// <summary>
        /// Gets whether any of the elements of the two specified <see cref="Triangle2"/>s are not equal.
        /// </summary>
        /// <param name="left"> The <see cref="Triangle2"/> on the left side of the equation. </param>
        /// <param name="right"> The <see cref="Triangle2"/> on the right side of the equation. </param>
        /// <returns>
        /// Returns <see langword="true"/> if any of the elements is not equal equal;
        /// otherwise <see langword="false"/> if all are equal.
        /// </returns>
        public static bool operator !=( Triangle2 left, Triangle2 right )
        {
            return left.a != right.a || left.b != right.b || left.c != right.c;
        }

        #endregion

        #region Impls/Overrides

        #region ToString

        /// <summary>
        /// Gets a human readable representation of this <see cref="Triangle2"/> instance.
        /// </summary>
        /// <returns> A string that contains the representation created via the current culture settings. </returns>
        public override string ToString()
        {
            return this.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Gets a human readable representation of this <see cref="Triangle2"/> instance.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns> A string that contains the representation created via the current culture settings. </returns>
        public string ToString( IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider,
                "Tri2[ {0}|{1}|{2} ]",
                this.a.ToString( formatProvider ),
                this.b.ToString( formatProvider ), 
                this.c.ToString( formatProvider ) 
            );
        }

        #endregion

        #region Equals

        /// <summary>
        /// Returns whether this <see cref="Triangle2"/> is equal to the specified object.
        /// </summary>
        /// <param name="obj"> The object to test against. </param>
        /// <returns> 
        /// Returns <see langword="true"/> if they are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj == null )
                return false;

            if( !(obj is Triangle2) )
                return false;

            Triangle2 tri = (Triangle2)obj;
            return this.a == tri.a && this.b == tri.b && this.c == tri.c;
        }

        /// <summary>
        /// Returns whether this <see cref="Triangle2"/> is equal to the specified <see cref="Triangle2"/>.
        /// </summary>
        /// <param name="other"> The triangle to test against. </param>
        /// <returns> 
        /// Returns <see langword="true"/> if all elements are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( Triangle2 other )
        {
            return this.a == other.a && this.b == other.b && this.c == other.c;
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets the hash code of this <see cref="Triangle2"/> instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.a );
            hashBuilder.AppendStruct( this.b );
            hashBuilder.AppendStruct( this.c );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #endregion

        #region IntersectsPoint

        /// <summary>
        /// Checks wether a given point is inside a triangle,
        /// in 2-dimensional (Cartesian) space.
        /// </summary>
        /// <remarks>
        /// The vertices of the triangle must be given in either
        /// trigonometrical (anticlockwise) or inverse trigonometrical
        /// (clockwise) order.
        /// </remarks>
        /// <param name="pointX">
        /// The X-coordinate of the point.
        /// </param>
        /// <param name="pointY">
        /// The Y-coordinate of the point.
        /// </param>
        /// <param name="trianglePointAx">
        /// The X-coordinate of the triangle's first vertex.
        /// </param>
        /// <param name="trianglePointAy">
        /// The Y-coordinate of the triangle's first vertex.
        /// </param>
        /// <param name="trianglePointBx">
        /// The X-coordinate of the triangle's second vertex.
        /// </param>
        /// <param name="trianglePointBy">
        /// The Y-coordinate of the triangle's second vertex.
        /// </param>
        /// <param name="trianglePointCx">
        /// The X-coordinate of the triangle's third vertex.
        /// </param>
        /// <param name="trianglePointCy">
        /// The Y-coordinate of the triangle's third vertex.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the point resides in the triangle;
        /// or otherwise <see langword="false"/> if the point is outside the triangle.
        /// </returns>
        public static bool IntersectsPoint( 
            float pointX, 
            float pointY, 
            float trianglePointAx,
            float trianglePointAy, 
            float trianglePointBx, 
            float trianglePointBy, 
            float trianglePointCx, 
            float trianglePointCy )
        {
            float v1x = trianglePointBx - trianglePointAx;
            float v1y = trianglePointBy - trianglePointAy;

            float v2x = pointX - trianglePointBx;
            float v2y = pointY - trianglePointBy;

            bool isClockwise = ((v1x * v2y) - (v1y * v2x)) >= 0.0f;

            v1x = trianglePointCx - trianglePointBx;
            v1y = trianglePointCy - trianglePointBy;

            v2x = pointX - trianglePointCx;
            v2y = pointY - trianglePointCy;

            if( ((v1x * v2y) - (v1y * v2x) >= 0.0f) != isClockwise )
                return false;

            v1x = trianglePointAx - trianglePointCx;
            v1y = trianglePointAy - trianglePointCy;

            v2x = pointX - trianglePointAx;
            v2y = pointY - trianglePointAy;

            if( ((v1x * v2y) - (v1y * v2x) >= 0.0f) != isClockwise )
                return false;

            return true;
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The first point that makes up this Triangle2.
        /// </summary>
        private Vector2 a;

        /// <summary>
        /// The second point that makes up this Triangle2.
        /// </summary>
        private Vector2 b;

        /// <summary>
        /// The third point that makes up this Triangle2.
        /// </summary>
        private Vector2 c;

        #endregion
    }

    #region enum TriangleTypes

    /// <summary> 
    /// Enumerates the different types of Triangles. 
    /// </summary>
    [Flags]
    public enum TriangleTypes
    {
        /// <summary> 
        /// No type has been specified. 
        /// </summary>
        None = 0,

        /// <summary> 
        /// The triangle is equilateral (de: gleichschenklig) .
        /// </summary>
        Equilateral = 32,

        /// <summary> 
        /// The triangle is isosceles (de: gleichseitig). 
        /// </summary>
        Isosceles = 64,

        /// <summary>
        /// The triangle is perpendicular (de: rechtwinklig).  
        /// </summary>
        Perpendicular = 256,

        /// <summary>
        /// The triangle is optuse-angled (de: stumpfwinklig).  
        /// </summary>
        OptuseAngled = 1024,

        /// <summary>
        /// The triangle is acute-angled (de: spitzwinklig). 
        /// </summary>
        AcuteAngled = 2048
    }

    #endregion
}
