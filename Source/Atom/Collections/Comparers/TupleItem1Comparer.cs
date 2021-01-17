// <copyright file="TupleItem1Comparer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Comparers.TupleItem1Comparer{TKey, TValue} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections.Comparers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A comparer for comparing the first object of <see cref="Tuple&lt;TFirst, TSecond&gt;"/>s.
    /// </summary>
    /// <typeparam name="TFirst">
    /// The type of the first object.
    /// </typeparam>
    /// <typeparam name="TSecond">
    /// The type of the second object.
    /// </typeparam>
    [Serializable]
    public sealed class TupleItem1Comparer<TFirst, TSecond> : IComparer<Tuple<TFirst, TSecond>>
        where TFirst : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TupleItem1Comparer{TFirst, TSecond}"/> class.
        /// </summary>
        public TupleItem1Comparer()
        {
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
        /// </returns>
        public int Compare( Tuple<TFirst, TSecond> x, Tuple<TFirst, TSecond> y )
        {
            if( x == null )
            {
                if( y == null )
                    return 0;
                else
                    return -1;
            }

            if( y == null )
                return 1;

            if( x.Item1 == null )
            {
                return y.Item1 == null ? 0 : -1;
            }

            return x.Item1.CompareTo( y.Item1 );
        }
    }
}
