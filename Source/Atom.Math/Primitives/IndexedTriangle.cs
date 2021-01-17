// <copyright file="IndexedTriangle.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.IndexedTriangle structure.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Defines a Triangle structure that stores the indices (that point into the Vertex Data)
    /// of the points that make up the Triangle.
    /// </summary>
    public struct IndexedTriangle : IEquatable<IndexedTriangle>
    {
        #region [ Fields ]

        /// <summary>
        /// First vertex index in triangle.
        /// </summary>
        public int IndexA;

        /// <summary>
        /// Second vertex index in triangle.
        /// </summary>
        public int IndexB;

        /// <summary>
        /// Third vertex index in triangle.
        /// </summary>
        public int IndexC;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedTriangle"/> structure.
        /// </summary>
        /// <param name="indexA">The index of the the first point of the triangle.</param>
        /// <param name="indexB">The index of the the second point of the triangle.</param>
        /// <param name="indexC">The index of the the third point of the triangle.</param>
        public IndexedTriangle( int indexA, int indexB, int indexC )
        {
            this.IndexA = indexA;
            this.IndexB = indexB;
            this.IndexC = indexC;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns whether the given <see cref="IndexedTriangle"/> has the
        /// same indices set as this IndexedTriangle.
        /// </summary>
        /// <param name="other">The IndexedTriangle to test against.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the indices are equal; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( IndexedTriangle other )
        {
            return this.IndexA == other.IndexA &&
                   this.IndexB == other.IndexB &&
                   this.IndexC == other.IndexC;
        }

        /// <summary>
        /// Returns whether the given <see cref="Object"/> is equal to this IndexedTriangle.
        /// </summary>
        /// <param name="obj">The Object to test against.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is IndexedTriangle )
            {
                return Equals( (IndexedTriangle)obj );
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Overriden to return a human-readable text representation of the IndexedTriangle.
        /// </summary>
        /// <returns>A human-readable text representation of the IndexedTriangle.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Overriden to return a human-readable text representation of the IndexedTriangle.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the IndexedTriangle.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider,
                "IndexedTriangle= [{0}; {1}; {2}]",
                IndexA.ToString( formatProvider ),
                IndexB.ToString( formatProvider ),
                IndexC.ToString( formatProvider )
            );
        }

        /// <summary>
        /// Overriden to return the hash code of the <see cref="IndexedTriangle"/>.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var builder = new HashCodeBuilder();

            builder.AppendStruct( this.IndexA );
            builder.AppendStruct( this.IndexB );
            builder.AppendStruct( this.IndexC );

            return builder.GetHashCode();
        }

        #endregion

        #region [ Operators ]

        /// <summary>
        /// Returns whether the given <see cref="IndexedTriangle"/>s have the
        /// same indices.
        /// </summary>
        /// <param name="left">The IndexedTriangle on the left side of the equation.</param>
        /// <param name="right">The IndexedTriangle on the right side of the equation.</param>
        /// <returns>true if the indices are equal; otherwise false.</returns>
        public static bool operator ==( IndexedTriangle left, IndexedTriangle right )
        {
            return left.IndexA == right.IndexA &&
                   left.IndexB == right.IndexB &&
                   left.IndexC == right.IndexC;
        }

        /// <summary>
        /// Returns whether the given <see cref="IndexedTriangle"/>s don't have the
        /// same indices.
        /// </summary>
        /// <param name="left">The IndexedTriangle on the left side of the equation.</param>
        /// <param name="right">The IndexedTriangle on the right side of the equation.</param>
        /// <returns>true if the indices are not equal; otherwise false.</returns>
        public static bool operator !=( IndexedTriangle left, IndexedTriangle right )
        {
            return left.IndexA != right.IndexA ||
                   left.IndexB != right.IndexB ||
                   left.IndexC != right.IndexC;
        }

        #endregion
    }
}
