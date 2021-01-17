// <copyright file="PositionalSpline3.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.PositionalSpline3 class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// A Catmull-Rom spline that can be used for interpolating translation movements in 3 dimensional space.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A Catmull-Rom spline is a derivitive of the Hermite spline. The difference is that the Hermite spline
    /// allows you to specifiy 2 endpoints and 2 tangents, then the spline is generated. A Catmull-Rom spline
    /// allows you to just supply 1-n number of points and the tangents will be automatically calculated.
    /// </para><para>
    /// Derivation of the hermite polynomial can be found here:
    /// <a href="http://www.cs.unc.edu/~hoff/projects/comp236/curves/papers/hermite.html">Hermite splines.</a>
    /// </para>
    /// </remarks>
    public class PositionalSpline3
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets a value indicating whether the tangents are automatically
        /// recalculated when a new control point is added.
        /// </summary>
        /// <value>
        /// When <see langword="true"/> the tangents are automatically
        /// recalculated when a control point is added or removed.
        /// This property may be set to <see langword="false"/> for perfomance reasons.
        /// </value>
        public bool AutoCalculate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the number of control points in thisPositionalSpline3.
        /// </summary>
        /// <value>The number of control this.points.</value>
        public int PointCount
        {
            get
            {
                return this.points.Count;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Adds a new control point to the end of thisPositionalSpline3.
        /// </summary>
        /// <param name="point"> The point to add. </param>
        public void AddPoint( Vector3 point )
        {
            this.points.Add( point );

            if( this.AutoCalculate )
            {
                this.RecalculateTangents();
            }
        }

        /// <summary>
        /// Removes all current control points from thisPositionalSpline3.
        /// </summary>
        public void Clear()
        {
            this.points.Clear();
            this.tangents.Clear();

            if( this.AutoCalculate )
            {
                this.RecalculateTangents();
            }
        }

        /// <summary>
        /// Gets the control point at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index at which to retreive a point.</param>
        /// <returns>The Vector3 containing the point data. </returns>
        public Vector3 GetPoint( int index )
        {
            return this.points[index];
        }

        /// <summary>
        /// Returns an interpolated point based on a parametric value over the whole series.
        /// </summary>
        /// <param name="distanceOnSpline">
        /// A value between 0 and 1 representing the parametric distance along the
        /// whole length of the spline, this method returns an interpolated point.
        /// </param>
        /// <returns>An interpolated point along the spline.</returns>
        public Vector3 Interpolate( float distanceOnSpline )
        {
            // This does not take into account that points may not be evenly spaced.
            // This will cause a change in velocity for interpolation.

            // What segment this is in?
            float segment = distanceOnSpline * this.points.Count;
            int segIndex  = (int)segment;

            // apportion t
            distanceOnSpline = segment - segIndex;

            return this.Interpolate( segIndex, distanceOnSpline );
        }

        /// <summary>
        /// Interpolates a single segment of the spline given a parametric value.
        /// </summary>
        /// <param name="segmentIndex">
        /// The point index to treat as t=0. segmentIndex + 1 is deemed to be index=1.
        /// </param>
        /// <param name="distanceOnSpline">
        /// The parametric input value.
        /// </param>
        /// <returns>
        /// An interpolated point along the spline.
        /// </returns>
        public Vector3 Interpolate( int segmentIndex, float distanceOnSpline )
        {
            Contract.Requires<ArgumentOutOfRangeException>( segmentIndex >= 0 );
            Contract.Requires<ArgumentOutOfRangeException>( segmentIndex < this.PointCount );

            if( (segmentIndex + 1) == this.points.Count )
            {
                // cant interpolate past the end of the list, just return the last point
                return this.points[segmentIndex];
            }

            // quick special cases
            if( distanceOnSpline == 0.0f )
                return this.points[segmentIndex];
            else if( distanceOnSpline == 1.0f )
                return this.points[segmentIndex + 1];

            // Time for real interpolation
            // Construct a Vector4 of powers of 2
            float t2 = distanceOnSpline  * distanceOnSpline;  // t^2
            float t3 = t2 * distanceOnSpline; // t^3

            Vector4 powers = new Vector4( t3, t2, distanceOnSpline, 1 );

            // Algorithm is result = powers * hermitePoly * Matrix4(point1, point2, tangent1, tangent2)
            Vector3 point1 = this.points[segmentIndex];
            Vector3 point2 = this.points[segmentIndex + 1];
            Vector3 tangent1 = this.tangents[segmentIndex];
            Vector3 tangent2 = this.tangents[segmentIndex + 1];

            Matrix4 point;
            point.M11 = point1.X;
            point.M12 = point1.Y;
            point.M13 = point1.Z;
            point.M14 = 1.0f;

            point.M21 = point2.X;
            point.M22 = point2.Y;
            point.M23 = point2.Z;
            point.M24 = 1.0f;

            point.M31 = tangent1.X;
            point.M32 = tangent1.Y;
            point.M33 = tangent1.Z;
            point.M34 = 1.0f;

            point.M41 = tangent2.X;
            point.M42 = tangent2.Y;
            point.M43 = tangent2.Z;
            point.M44 = 1.0f;

            Vector4 result = Vector4.Transform( powers, (HermitePoly * point) );
            return new Vector3( result.X, result.Y, result.Z );
        }

        /// <summary>
        /// Recalculates the tangents associated with this spline.
        /// </summary>
        /// <remarks>
        /// If you tell the spline not to update on demand by setting AutoCalculate to false,
        /// then you must call this after completing your updates to the spline this.points.
        /// </remarks>
        public void RecalculateTangents()
        {
            // Catmull-Rom approach
            // tangent[i] = 0.5 * (point[i+1] - point[i-1])
            this.tangents.Clear();

            bool isClosed;
            int pointCount = this.points.Count;

            // if there arent at least 2 points, there is nothing to inerpolate
            if( pointCount < 2 )
                return;

            // closed or open?
            if( points[0] == this.points[pointCount - 1] )
                isClosed = true;
            else
                isClosed = false;

            // loop through the points and generate the tangents
            for( int i = 0; i < pointCount; ++i )
            {
                // special cases for first and last point in list
                if( i == 0 )
                {
                    if( isClosed )
                    {
                        // Use numPoints-2 since numPoints-1 is the last point and == [0]
                        this.tangents.Add( 0.5f * (this.points[1] - this.points[pointCount - 2]) );
                    }
                    else
                    {
                        this.tangents.Add( 0.5f * (this.points[1] - this.points[0]) );
                    }
                }
                else if( i == pointCount - 1 )
                {
                    if( isClosed )
                    {
                        // Use same tangent as already calculated for [0]
                        this.tangents.Add( this.tangents[0] );
                    }
                    else
                    {
                        this.tangents.Add( 0.5f * (this.points[i] - this.points[i - 1]) );
                    }
                }
                else
                {
                    this.tangents.Add( 0.5f * (this.points[i + 1] - this.points[i - 1]) );
                }
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The collection of control this.points.
        /// </summary>
        private readonly List<Vector3> points = new List<Vector3>();

        /// <summary>
        /// The collection of generated tangents for the spline controls this.points.
        /// </summary>
        private readonly List<Vector3> tangents = new List<Vector3>();

        /// <summary>
        /// The hermit poly constant matrix.
        /// </summary>
        private static readonly Matrix4 HermitePoly = new Matrix4(
             2.0f, -2.0f, 1.0f, 1.0f,
            -3.0f, 3.0f, -2.0f, -1.0f,
             0.0f, 0.0f, 1.0f, 0.0f,
             1.0f, 0.0f, 0.0f, 0.0f
        );

        #endregion
    }
}
