// <copyright file="IndexedEdge.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.IndexedEdge class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Defines an Edge structure that stores the indices (that point into the Vertex Data)
    /// of the points that make up the Edge.
    /// This is a sealed class.
    /// </summary>
    public sealed class IndexedEdge : IEquatable<IndexedEdge>, ICultureSensitiveToStringProvider
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the first index of the Edge, which is usually the starting point.
        /// </summary>
        public int IndexA
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second index of the Edge, which is usually the ending point.
        /// </summary>
        public int IndexB 
        { 
            get;
            set;
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedEdge"/> class.
        /// </summary>
        /// <param name="indexA">The first index of the new Edge. Usually the starting point.</param>
        /// <param name="indexB">The second index of the new Edge. Usually the ending point.</param>
        public IndexedEdge( int indexA, int indexB )
        {
            this.IndexA = indexA;
            this.IndexB = indexB;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedEdge"/> class
        /// whos indices are set to 0.
        /// </summary>
        public IndexedEdge()
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns whether the given <see cref="IndexedEdge"/> has the same indices as this <see cref="IndexedEdge"/>.
        /// </summary>
        /// <param name="other">The <see cref="IndexedEdge"/> to test with. </param>
        /// <returns>
        /// Returns <see langword="true"/> if the indices are equal; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( IndexedEdge other )
        {
            if( other == null )
                return false;

            return
                ((this.IndexA == other.IndexB) && (this.IndexB == other.IndexA)) ||
                ((this.IndexA == other.IndexA) && (this.IndexB == other.IndexB));
        }

        /// <summary>
        /// Returns whether the given <see cref="Object"/> is equal to this IndexedEdge.
        /// </summary>
        /// <param name="obj">The Object to test against.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            IndexedEdge edge = obj as IndexedEdge;

            if( edge != null )
            {
                return Equals( edge );
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Overriden to return a human-readable text representation of the IndexedEdge.
        /// </summary>
        /// <returns>A human-readable text representation of the IndexedEdge.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Overriden to return a human-readable text representation of the IndexedEdge.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the IndexedEdge.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider,
                "IndexedEdge= [{0}; {1}]",
                IndexA.ToString( formatProvider ),
                IndexB.ToString( formatProvider )
            );
        }

        /// <summary>
        /// Overriden to return the hashcode of the <see cref="IndexedEdge"/>.
        /// </summary>
        /// <returns>The hashcode.</returns>
        public override int GetHashCode()
        {
            var builder = new HashCodeBuilder();

            builder.AppendStruct( this.IndexA );
            builder.AppendStruct( this.IndexB );

            return builder.GetHashCode();
        }

        #endregion
    }
}
