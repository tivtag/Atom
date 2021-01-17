// <copyright file="CollectionExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.CollectionExtensions class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Atom.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Defines extension methods for the ICollection{T} interface.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds the specified elements to the end of the System.Collections.Generic.ICollection&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to which items should be added.
        /// </param>
        /// <param name="items"> 
        /// The elements that should be added to the end of the ICollection&lt;T&gt;.
        /// </param>
        [DebuggerStepThrough]
        public static void AddRange<T>( this ICollection<T> collection, IEnumerable<T> items )
        {
            Contract.Requires<ArgumentNullException>( collection != null );
            Contract.Requires<ArgumentNullException>( items != null );

            foreach( T item in items )
            {
                collection.Add( item );
            }
        }

        /// <summary>
        /// Replaces the items in the specified collections with the specified items.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to modify.
        /// </param>
        /// <param name="items">
        /// The items the collection should contain.
        /// </param>
        public static void Replace<T>( this ICollection<T> collection, IEnumerable<T> items )
        {
            Contract.Requires<ArgumentNullException>( collection != null );
            Contract.Requires<ArgumentNullException>( items != null );
            // Contract.Ensures( collection.Count == items.Count() );

            collection.Clear();

            foreach( T item in items )
            {
                collection.Add( item );
            }
        }
    }
}
