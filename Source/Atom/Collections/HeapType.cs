// <copyright file="HeapType.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.HeapType enumeration.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections
{
    /// <summary>
    /// Enumerates the different types of a <see cref="Heap{T}"/>.
    /// </summary>
    public enum HeapType
    {
        /// <summary>
        /// The smallest item is kept at the root of the Heap.
        /// </summary>
        Minimum,

        /// <summary>
        /// The largest item is kept at the root of the Heap.
        /// </summary>
        Maximum
    }
}
