// <copyright file="Circle.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Circle structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a Circle that is defined by a center point and a radius value.
    /// </summary>
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.CircleConverter ) )]
    [System.Serializable]
    public struct Circle : IEquatable<Circle>, ICultureSensitiveToStringProvider
    {
        #region [ Fields ]

        /// <summary>
        /// The position of the center of the Circle.
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// The radius of the Circle.
        /// </summary>
        public float Radius;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Circle"/> structure.
        /// </summary>
        /// <param name="center">
        /// The position of the center of the Circle.
        /// </param>
        /// <param name="radius">
        /// The radius of the Circle.
        /// </param>
        public Circle( Vector2 center, float radius )
        {
            this.Center = center;
            this.Radius = radius;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Circle"/> structure.
        /// </summary>
        /// <param name="center">
        /// The position of the center of the Circle.
        /// </param>
        /// <param name="radius">
        /// The radius of the Circle.
        /// </param>
        public Circle( Point2 center, float radius )
        {
            this.Center = new Vector2( center.X, center.Y );
            this.Radius = radius;
        }

        #endregion

        #region [ Methods ]

        #region Contains

        /// <summary>
        /// Returns whether the given <paramref name="point"/>
        /// is contained by the <see cref="Circle"/>.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns>true if the Circle contains the given point; otherwise false.</returns>
        [Pure]
        public bool Contains( Vector2 point )
        {
            Vector2 delta = new Vector2( Center.X - point.X, Center.Y - point.Y );

            float hypo = delta.Length; // hypothenuse
            return (Radius + 1.0f - hypo) >= float.Epsilon;
        }

        /// <summary>
        /// Returns whether the given <paramref name="point"/>
        /// is contained by the <see cref="Circle"/>.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns>true if the Circle contains the given point; otherwise false.</returns>
        [Pure]
        public bool Contains( Point2 point )
        {
            Vector2 delta = new Vector2( Center.X - point.X, Center.Y - point.Y );

            float hypo = delta.Length; // hypothenuse
            return (Radius + 1.0f - hypo) >= float.Epsilon;
        }

        #endregion

        #region IsInside

        /// <summary>
        /// Returns whether the given <paramref name="point"/> lies inside the circumcircle made up by points (A,B,C).
        /// </summary>
        /// <remarks>
        /// NOTE: A point on the edge is inside the circumcircle.
        /// </remarks>
        /// <param name="point">The point to check.</param>
        /// <param name="circlePointA">First point on the circle.</param>
        /// <param name="circlePointB">Second point on the circle.</param>
        /// <param name="circlePointC">Third point on the circle.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the <paramref name="point"/> is inside circle;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsInside( Vector2 point, Vector2 circlePointA, Vector2 circlePointB, Vector2 circlePointC )
        {
            // Return true if the point (xp,yp) lies inside the circumcircle
            // made up by points (x1,y1) (x2,y2) (x3,y3).
            // NOTE: A point on the edge is inside the circumcircle.
            if( System.Math.Abs( circlePointA.Y - circlePointB.Y ) < float.Epsilon &&
                System.Math.Abs( circlePointB.Y - circlePointC.Y ) < float.Epsilon )
            {
                // In Circum; the points are coincident!
                return false;
            }

            float m1, m2;
            float mx1, mx2;
            float my1, my2;
            float xc, yc;

            if( System.Math.Abs( circlePointB.Y - circlePointA.Y ) < float.Epsilon )
            {
                m2 = -(circlePointC.X - circlePointB.X) / (circlePointC.Y - circlePointB.Y);
                mx2 = (circlePointB.X + circlePointC.X) * 0.5f;
                my2 = (circlePointB.Y + circlePointC.Y) * 0.5f;

                // Calculate CircumCircle center (xc,yc).
                xc = (circlePointB.X + circlePointA.X) * 0.5f;
                yc = (m2 * (xc - mx2)) + my2;
            }
            else if( System.Math.Abs( circlePointC.Y - circlePointB.Y ) < float.Epsilon )
            {
                m1 = -(circlePointB.X - circlePointA.X) / (circlePointB.Y - circlePointA.Y);
                mx1 = (circlePointA.X + circlePointB.X) * 0.5f;
                my1 = (circlePointA.Y + circlePointB.Y) * 0.5f;

                // Calculate CircumCircle center (xc,yc).
                xc = (circlePointC.X + circlePointB.X) * 0.5f;
                yc = (m1 * (xc - mx1)) + my1;
            }
            else
            {
                m1 = -(circlePointB.X - circlePointA.X) / (circlePointB.Y - circlePointA.Y);
                m2 = -(circlePointC.X - circlePointB.X) / (circlePointC.Y - circlePointB.Y);
                mx1 = (circlePointA.X + circlePointB.X) * 0.5f;
                mx2 = (circlePointB.X + circlePointC.X) * 0.5f;
                my1 = (circlePointA.Y + circlePointB.Y) * 0.5f;
                my2 = (circlePointB.Y + circlePointC.Y) * 0.5f;

                // Calculate CircumCircle center (xc,yc).
                xc = ((m1 * mx1) - (m2 * mx2) + my2 - my1) / (m1 - m2);
                yc = (m1 * (xc - mx1)) + my1;
            }

            float deltaX       = circlePointB.X - xc;
            float deltaY       = circlePointB.Y - yc;
            float squaredRange = (deltaX * deltaX) + (deltaY * deltaY);

            deltaX = point.X - xc;
            deltaY = point.Y - yc;

            return ((deltaX * deltaX) + (deltaY * deltaY)) <= squaredRange;
        }

        #endregion

        #region > Overrides/Impls <
        
        /// <summary>
        /// Gets the hash code of the Circle object.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return this.Radius.GetHashCode() + this.Center.GetHashCode();
        }

        #region Equals

        /// <summary>
        /// Returns whether the given Object is equal to the Circle.
        /// </summary>
        /// <param name="obj">The object to test against.</param>
        /// <returns>true if they are equal, otherwise false.</returns>
        public override bool Equals( object obj )
        {
            if( !(obj is Circle) )
                return false;

            Circle other = (Circle)obj;
            return this.Radius.IsApproximate( other.Radius ) && this.Center.Equals( other.Center );
        }

        /// <summary>
        /// Returns whether the given Circle is equal to the Circle.
        /// </summary>
        /// <param name="other">The Circle to test against.</param>
        /// <returns>true if they are equal, otherwise false.</returns>
        public bool Equals( Circle other )
        {
            return this.Radius.IsApproximate( other.Radius ) && this.Center.Equals( other.Center );
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overriden to return a human-readable string that descripes the Circle.
        /// </summary>
        /// <returns>A string that descripes the Circle.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Circle.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Circle.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider,
                "[{0} {1}]",
                Center.ToString( formatProvider ),
                Radius.ToString( formatProvider )
            );
        }

        #endregion

        #endregion

        #region > Creation Helpers <

        /// <summary>
        /// Creates a <see cref="Circle"/> given an axis aligned <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rectangle">
        /// The input rectangle.
        /// </param>
        /// <returns>
        /// The converted circle.
        /// </returns>
        public static Circle FromRectangle( Rectangle rectangle )
        {
            Circle circle;

            Point2 center;
            center.X = rectangle.X + (rectangle.Width / 2);
            center.Y = rectangle.Y + (rectangle.Height / 2);

            circle.Center = center;
            circle.Radius = System.Math.Max( rectangle.Width, rectangle.Height ) / 2;

            return circle;
        }

        /// <summary>
        /// Creates a <see cref="Circle"/> given an axis aligned <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="rectangle">
        /// The input rectangle.
        /// </param>
        /// <returns>
        /// The converted circle.
        /// </returns>
        public static Circle FromRectangle( RectangleF rectangle )
        {
            Circle circle;

            Vector2 center;
            center.X = rectangle.X + (rectangle.Width / 2.0f);
            center.Y = rectangle.Y + (rectangle.Height / 2.0f);

            circle.Center = center;
            circle.Radius = System.Math.Max( rectangle.Width, rectangle.Height ) / 2.0f;

            return circle;
        }

        #endregion

        #endregion

        #region [ Operators ]

        /// <summary>
        /// Returns whether the given Circles are equal in position and radius.
        /// </summary>
        /// <param name="left">The Circle on the left side of the equation.</param>
        /// <param name="right">The Circle on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator ==( Circle left, Circle right )
        {
            return left.Radius.IsApproximate( right.Radius ) && left.Center.Equals( right.Center );
        }
            
        /// <summary>
        /// Returns whether the given Circles are inequal in position or radius.
        /// </summary>
        /// <param name="left">The Circle on the left side of the equation.</param>
        /// <param name="right">The Circle on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator !=( Circle left, Circle right )
        {
            return !left.Radius.IsApproximate( right.Radius ) || !left.Center.Equals( right.Center );
        }

        #endregion
    }
}
