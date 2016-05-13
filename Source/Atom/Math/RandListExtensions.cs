// <copyright file="RandListExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.RandListExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines extension methods related to randomly selecting elements from IList{T}s.
    /// </summary>
    public static class RandListExtensions
    {
        /// <summary>
        /// Gets a random element of the given collection.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element in the list.
        /// </typeparam>
        /// <param name="list">
        /// The list to query.
        /// </param>
        /// <param name="rand">
        /// The random number generator to use.
        /// </param>
        /// <returns>
        /// A random item from the given list or the default value of T if the given list is empty.
        /// </returns>
        public static T RandomOrDefault<T>( this IList<T> list, IRand rand )
        {
            if( list.Count == 0 )
            {
                return default(T);
            }

            int index = rand.RandomRange(0, list.Count - 1);
            return list[index];
        }
    }
}
