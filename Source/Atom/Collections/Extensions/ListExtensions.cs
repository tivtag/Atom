// <copyright file="ListExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.ListExtensions class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Defines extension methods for the IList{T} interface.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Swaps the items at the given zero-based indices of this IList{T}.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the list.
        /// </typeparam>
        /// <param name="list">
        /// The list to modify.
        /// </param>
        /// <param name="indexA">
        /// The zero-based index of the first element to swap.
        /// </param>
        /// <param name="indexB">
        /// The zero-based index of the second element to swap.
        /// </param>
        public static void SwapItems<T>( this IList<T> list, int indexA, int indexB )
        {
            Contract.Requires<ArgumentNullException>( list != null );
            Contract.Requires<ArgumentOutOfRangeException>( indexA >= 0 );
            Contract.Requires<ArgumentOutOfRangeException>( indexB >= 0 );
            Contract.Requires<ArgumentOutOfRangeException>( indexA < list.Count );
            Contract.Requires<ArgumentOutOfRangeException>( indexB < list.Count );

            T itemA = list[indexA];
            T itemB = list[indexB];

            list[indexA] = itemB;
            list[indexB] = itemA;
        }

        /// <summary>
        /// Returns a read-only instance of this <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the list.
        /// </typeparam>
        /// <param name="list">
        /// The list to wrap.
        /// </param>
        /// <returns>
        /// A new read-only instance of this <see cref="IList{T}"/>.
        /// </returns>
        public static IList<T> AsReadOnly<T>( this IList<T> list )
        {
            Contract.Requires<ArgumentNullException>( list != null );
            // Contract.Ensures( Contract.Result<IList<T>>() != null );
            // Contract.Ensures( Contract.Result<IList<T>>().IsReadOnly );

            return new ReadOnlyCollection<T>( list );
        }
    }
}
