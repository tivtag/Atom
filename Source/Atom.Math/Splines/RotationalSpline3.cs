// <copyright file="RotationalSpline3.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.RotationalSpline3 class.
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
    /// A class used to interpolate orientations (rotations) along a spline
    /// using derivatives of quaternions.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Like the <see cref="PositionalSpline3"/> class, this class is about interpolating values smoothly over a spline.
    /// Whilst <see cref="PositionalSpline3"/> deals with positions (the normal sense we think about splines),
    /// this class interpolates orientations.
    /// The theory is identical, except we're now in 4-dimensional space instead of 3.
    /// </para><para>
    /// In positional splines, we use the points and tangents on those points to generate
    /// control points for the spline. In this case, we use quaternions and derivatives
    /// of the quaternions (i.e. the rate and direction of change at each point). This is the
    /// same as PositionalSpline since a tangent is a derivative of a position. 
    /// We effectively generate an extra quaternion in between each actual quaternion 
    /// which when take with the original quaternion forms the 'tangent' of that quaternion.
    /// </para>
    /// </remarks>
    public class RotationalSpline3
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets a value indicating whether the tangents 
        /// are automatically recalculated when a control point is added or removed.
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
        /// Gets the number of control points in this <see cref="RotationalSpline3"/>.
        /// </summary>
        /// <value>The number of control points.</value>
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
        /// Adds a control point to the end of the <see cref="RotationalSpline3"/>.
        /// </summary>
        /// <param name="point"> The control point to add. </param>
        public void AddPoint( Quaternion point )
        {
            this.points.Add( point );

            if( this.AutoCalculate )
            {
                this.RecalculateTangents();
            }
        }

        /// <summary>
        /// Removes all control points from this <see cref="RotationalSpline3"/>.
        /// </summary>
        public void Clear()
        {
            this.points.Clear();
            this.tangents.Clear();
        }

        /// <summary>
        /// Returns an interpolated point based on a parametric value over the whole series.
        /// </summary>
        /// <param name="distanceAlongSpline">
        /// A value between 0 and 1 representing the parametric distance along the
        /// whole length of the spline.
        /// </param>
        /// <returns>
        /// An interpolated point along the spline.
        /// </returns>
        public Quaternion Interpolate( float distanceAlongSpline )
        {
            return this.Interpolate( distanceAlongSpline, true );
        }

        /// <summary>
        /// Returns an interpolated point based on a parametric value over the whole series.
        /// </summary>
        /// <param name="index">
        /// The point index to treat as t=0. index + 1 is deemed to be distanceAlongSpline=1.
        /// </param>
        /// <param name="distanceAlongSpline">
        /// A value between 0 and 1 representing the parametric distance along the
        /// whole length of the spline.
        /// </param>
        /// <returns>
        /// An interpolated point along the spline.
        /// </returns>
        public Quaternion Interpolate( int index, float distanceAlongSpline )
        {
            return this.Interpolate( index, distanceAlongSpline, true );
        }

        /// <summary>
        /// Returns an interpolated point based on a parametric value over the whole series.
        /// </summary>
        /// <param name="distanceAlongSpline">
        /// A value between 0 and 1 representing the parametric distance along the
        /// whole length of the spline.
        /// </param>
        /// <param name="useShortestPath">
        /// True forces rotations to use the shortest path.
        /// </param>
        /// <returns>
        /// An interpolated point along the spline.
        /// </returns>
        public Quaternion Interpolate( float distanceAlongSpline, bool useShortestPath )
        {
            // This does not take into account that points may not be evenly spaced.
            // This will cause a change in velocity for interpolation.

            // What segment this is in?
            float segment = distanceAlongSpline * this.points.Count;
            int segIndex = (int)segment;

            // apportion t
            distanceAlongSpline = segment - segIndex;

            return this.Interpolate( segIndex, distanceAlongSpline, useShortestPath );
        }

        /// <summary>
        /// Interpolates a single segment of the spline given a parametric value.
        /// </summary>
        /// <param name="index">
        /// The point index to treat as t=0. index + 1 is deemed to be distanceAlongSpline=1.
        /// </param>
        /// <param name="distanceAlongSpline">
        /// The parametric input value.
        /// </param>
        /// <param name="useShortestPath">
        /// States whether the spline should take the shortest path between two control points.
        /// </param>
        /// <returns>
        /// An interpolated point along the spline.
        /// </returns>
        public Quaternion Interpolate( int index, float distanceAlongSpline, bool useShortestPath )
        {
            Contract.Requires<ArgumentOutOfRangeException>( index >= 0 );
            Contract.Requires<ArgumentOutOfRangeException>( index < this.PointCount );

            if( (index + 1) == this.points.Count )
            {
                // can't interpolate past the end of the list, just return the last point
                return this.points[index];
            }

            // quick special cases
            if( distanceAlongSpline == 0.0f )
            {
                return this.points[index];
            }
            else if( distanceAlongSpline == 1.0f )
            {
                return this.points[index + 1];
            }

            // Time for real interpolation
            // Algorithm uses spherical quadratic interpolation

            // return the final result
            return Quaternion.Squad(
                this.points[index],
                this.tangents[index],
                this.tangents[index + 1],
                this.points[index + 1],
                useShortestPath,
                distanceAlongSpline
            );
        }

        /// <summary>
        /// Recalculates the tangents associated with this spline.
        /// </summary>
        /// <remarks>
        /// If you tell the spline not to update on demand by setting AutoCalculate to false,
        /// then you must call this after completing your updates to the spline points.
        /// </remarks>
        public void RecalculateTangents()
        {
            // let p = point[i], pInv = p.Inverse
            // tangent[i] = p * exp( -0.25 * ( log(pInv * point[i+1]) + log(pInv * point[i-1]) ) )
            int pointCount = this.points.Count;

            // if there arent at least 2 points, there is nothing to inerpolate
            if( pointCount < 2 )
                return;

            // closed or open?
            bool isClosed;
            if( points[0] == this.points[pointCount - 1] )
                isClosed = true;
            else
                isClosed = false;

            Quaternion invPoint, part1, part2, preExp;

            // loop through the points and generate the tangents
            for( int i = 0; i < pointCount; ++i )
            {
                Quaternion point = this.points[i];

                // Get the inverse of p
                invPoint = point.Inverse;

                // special cases for first and last point in list
                if( i == 0 )
                {
                    part1 = (invPoint * this.points[i + 1]).Log;
                    if( isClosed )
                    {
                        // Use numPoints-2 since numPoints-1 is the last point and == [0]
                        part2 = (invPoint * this.points[pointCount - 2]).Log;
                    }
                    else
                        part2 = (invPoint * point).Log;
                }
                else if( i == pointCount - 1 )
                {
                    if( isClosed )
                    {
                        // Use same tangent as already calculated for [0]
                        part1 = (invPoint * this.points[1]).Log;
                    }
                    else
                        part1 = (invPoint * point).Log;

                    part2 = (invPoint * this.points[i - 1]).Log;
                }
                else
                {
                    part1 = (invPoint * this.points[i + 1]).Log;
                    part2 = (invPoint * this.points[i - 1]).Log;
                }

                preExp = -0.25f * (part1 + part2);
                tangents.Add( point * preExp.Exp );
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The list of control points.
        /// </summary>
        private readonly List<Quaternion> points = new List<Quaternion>();

        /// <summary>
        /// The list of generated tangents for the spline controls points.
        /// </summary>
        private readonly List<Quaternion> tangents = new List<Quaternion>();

        #endregion
    }
}
