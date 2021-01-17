// <copyright file="ReverseComparer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Comparers.ReverseComparer{T} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections.Comparers
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// A comparer that wraps the IComparer interface to reproduce the opposite comparison result.
    /// This class can't be inherited.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the item the comparer can compare.
    /// </typeparam>
    [Serializable]
    public sealed class ReverseComparer<T> : IComparer<T>
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the comparer used in this instance.
        /// </summary>
        /// <value>The comparer.</value>
        public IComparer<T> Comparer
        {
            get
            {
                return this.comparer;
            }

            set
            {
                Contract.Requires<ArgumentNullException>( value != null );

                this.comparer = value;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseComparer{T}"/> class.
        /// </summary>
        public ReverseComparer()
            : this( Comparer<T>.Default )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseComparer{T}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// The comparer to reverse.
        /// </param>
        public ReverseComparer( IComparer<T> comparer )
        {
            Contract.Requires<ArgumentNullException>( comparer != null );

            this.comparer = comparer;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Less than zero, if x is less than y.
        /// Zero if x equals y.
        /// Greater than zero if x is greater than y.
        /// </returns>
        public int Compare( T x, T y )
        {
            return this.comparer.Compare( y, x );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The comparer that is reversed by this ReverseComparer.
        /// </summary>
        private IComparer<T> comparer;

        #endregion
    }
}
