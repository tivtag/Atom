// <copyright file="IHeap.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.IHeap{T} interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a heap of items.
    /// </summary>
    /// <seealso cref="HeapType"/>
    /// <typeparam name="T">
    /// The type of item stored in the IHeap{T}.
    /// </typeparam>
    // [Atom.Diagnostics.Contracts.ContractClass( typeof( IHeapContract<> ) )]
    public interface IHeap<T> : ICollection<T>
    {
        /// <summary>
        /// Gets the <see cref="HeapType"/> of this IHeap{T}.
        /// </summary>
        HeapType HeapType 
        { 
            get;
        }

        /// <summary>
        /// Gets the item at the root of this IHeap{T}.
        /// </summary>
        /// <value>
        /// The smallest or greatest value of this Heap{T}; depending on its <see cref="HeapType"/>.
        /// </value>
        T Root 
        {
            get;
        }

        /// <summary>
        /// Gets the IComparer{T} that is used to compare the objects stored in this IHeap{T}.
        /// </summary>
        IComparer<T> Comparer
        {
            get;
        }

        /// <summary>
        /// Removes and returns the item at the root of this Heap{T}.
        /// </summary>
        /// <returns>
        /// The smallest or greatest value of this Heap{T}; depending on its <see cref="HeapType"/>.
        /// </returns>
        T Pop();
    }
}
