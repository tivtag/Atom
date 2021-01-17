// <copyright file="Swap.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Swap class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    /// <summary>
    /// Defines a mechanism for swapping two objects.
    /// </summary>
    public static class Swap
    {
        /// <summary>
        /// Swaps the the specified items.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the items to swap.
        /// </typeparam>
        /// <param name="first">
        /// The first item.
        /// </param>
        /// <param name="second">
        /// The second item.
        /// </param>
        public static void Them<T>( ref T first, ref T second )
        {
            T temp = first;
            first = second;
            second = temp;
        }
    }
}