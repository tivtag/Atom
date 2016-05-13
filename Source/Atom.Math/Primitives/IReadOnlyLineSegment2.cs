// <copyright file="IReadOnlyLineSegment2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.IReadOnlyLineSegment2 interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Provides read-only access to line ( ax + by + c = 0 ) object that has a start point and an end point.
    /// </summary>
    public interface IReadOnlyLineSegment2 : IReadOnlyLine2, IEquatable<IReadOnlyLineSegment2>, ICloneable
    {     
        /// <summary>
        /// Returns the distance between this IReadOnlyLineSegment2 and the specfied Point.
        /// </summary>
        /// <param name="point">
        /// The point to get the distance to.
        /// </param>
        /// <returns>
        /// The distance to the point.
        /// </returns>
        float DistanceTo( Vector2 point );       
        
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
        int GetPointLocation( Vector2 point );

        /// <summary> 
        /// Gets a point on this <see cref="LineSegment2"/>. 
        /// </summary>
        /// <param name="time">
        /// The amount to travel on the segment.
        /// </param>
        /// <returns>
        /// A point along this LineSegment2.
        /// </returns>
        Vector2 GetPointOnSegment( float time );

        /// <summary> 
        /// Gets the start point of this IReadOnlyLineSegment2.
        /// </summary>
        /// <value>The start point of this IReadOnlyLineSegment2.</value>
        Vector2 Start { get; }

        /// <summary> 
        /// Gets the end point of this IReadOnlyLineSegment2.
        /// </summary>
        /// <value>The end point of this IReadOnlyLineSegment2.</value>
        Vector2 End { get; }
        
        /// <summary> 
        /// Gets the length of this IReadOnlyLineSegment2.
        /// </summary>
        /// <value>The length (also called magnitude) from the StartPoint to the EndPoint.</value>
        float Length { get; }
        
        /// <summary> 
        /// Gets the squared length of the IReadOnlyLineSegment2.
        /// </summary>
        /// <value>The squared length (also called magnitude) from the StartPoint to the EndPoint.</value>
        float LengthSquared { get; }
        
        /// <summary>
        /// Gets the maximum value of the points in the IReadOnlyLineSegment2.
        /// </summary>
        /// <value>The maximum value of the points in the IReadOnlyLineSegment2.</value>
        Vector2 Maximum { get; }

        /// <summary>
        /// Gets the minumum value of the points in the IReadOnlyLineSegment2.
        /// </summary>
        /// <value>The minumum value of the points in the IReadOnlyLineSegment2.</value>
        Vector2 Minimum { get; }
    }
}
