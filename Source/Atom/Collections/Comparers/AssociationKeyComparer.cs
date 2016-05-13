// <copyright file="AssociationKeyComparer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Comparers.AssociationKeyComparer{TKey, TValue} class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections.Comparers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a comparer for comparing keys using the <see cref="Association{TKey, TValue}"/> class.
    /// This class can't be inherited.
    /// </summary>
    /// <typeparam name="TKey">The type of the key for the association. Must inherit from <see cref="IComparable"/>. </typeparam>
    /// <typeparam name="TValue">The type of the value for the association.</typeparam>
    [Serializable]
    public sealed class AssociationKeyComparer<TKey, TValue> : IComparer<Association<TKey, TValue>> 
        where TKey : IComparable
    {
        /// <summary>
        /// Compares two Associations and returns a value indicating whether one is less than,
        /// equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first Association{Key, TValue} to compare.</param>
        /// <param name="y">The second Association{Key, TValue} to compare.</param>
        /// <returns>
        /// Value Condition Less than zero x is less than y. 
        /// Zero x equals y.
        /// Greater than zero x is greater than y.
        /// </returns>
        public int Compare( Association<TKey, TValue> x, Association<TKey, TValue> y )
        {
            if( x == null )
            {
                if( y == null )
                    return 0;
                else
                    return -1;
            }
            else if( y == null )
            {
                return 1;
            }

            if( x.Key == null )
            {
                return y.Key == null ? 0 : -1;
            }

            return x.Key.CompareTo( y.Key );
        }
    }
}
