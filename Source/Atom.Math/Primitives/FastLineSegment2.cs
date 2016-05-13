// <copyright file="FastLineSegment2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.FastLineSegment2 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    /// <summary>
    /// Represents an inmutable representation of a line segment in 2D space.
    /// </summary>
    /// <remarks>
    /// The speed and perfomance characteristics are, 
    /// in return of greatly reduces feature richness, 
    /// much better than of the <see cref="LineSegment2"/> class.
    /// </remarks>
    /// <seealso cref="LineSegment2"/>
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct FastLineSegment2
    {
        /// <summary>
        /// The starting point of the line segment.
        /// </summary>
        public readonly Vector2 Start;

        /// <summary>
        /// The ending point of the line segment.
        /// </summary>
        public readonly Vector2 End;

        /// <summary>
        /// Initializes a new instance of the FastLineSegment2 structure.
        /// </summary>
        /// <param name="start">
        /// The starting point of the new line segment.
        /// </param>
        /// <param name="end">
        /// The ending point of the new line segment.
        /// </param>
        public FastLineSegment2( Vector2 start, Vector2 end )
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Determines whether a specified Rectanglet intersects with this FastLineSegment2.
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
        
        /// <summary>
        /// Explicit cast operator that implements conversion
        /// from a FastLineSegment2 to a <see cref="LineSegment2"/>.
        /// </summary>
        /// <remarks>
        /// This operation is explicit because the creationg of a LineSegment2 instance
        /// is rather expensive.
        /// </remarks>
        /// <param name="segment">
        /// The input segment.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static explicit operator LineSegment2( FastLineSegment2 segment )
        {
            return new LineSegment2( segment.Start, segment.End );
        }
    }
}
