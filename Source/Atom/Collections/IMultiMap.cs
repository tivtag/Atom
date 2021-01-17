// <copyright file="IMultiMap.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.IMultiMap{Tkey, TElement} interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections
{
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a generic collection that maps keys onto multiple values.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of the key that identifies a group of elements.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of the elements stored in the IMultiMap.
    /// </typeparam>
    // [ContractClass( typeof( IMultiMapContract<,> ) )]
    public interface IMultiMap<TKey, TElement>
    {
        /// <summary>
        /// Adds the specified <paramref name="value"/> under the specified <paramref name="key"/>
        /// to this IMultiMap{TKey, TElement}.
        /// </summary>
        /// <param name="key">
        /// The key the specified <paramref name="value"/> should be associated with.
        /// </param>
        /// <param name="value">
        /// The value to add to this IMultiMap{TKey, TElement}.
        /// </param>
        void Add( TKey key, TElement value );

        /// <summary>
        /// Tries to remove the specified key/value pair from this IMultiMap{TKey, TElement}.
        /// </summary>
        /// <param name="key">
        /// The key that is associated with the specified <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        /// The value to remove.
        /// </param>
        /// <returns>
        /// true if the specified key/value pair was removed from this IMultiMap{TKey, TElement};
        /// otherwise false.
        /// </returns>
        bool Remove( TKey key, TElement value );
        
        /// <summary>
        /// Gets the elements that are associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">
        /// The key to lookup.
        /// </param>
        /// <param name="elements">
        /// When this method returns, contains the elements associated
        /// with the specified <paramref name="key"/>.
        /// </param>
        /// <returns>
        /// true if there exists any element under the specified <paramref name="key"/>;
        /// otherwise false.
        /// </returns>
        [Pure]
        bool TryGet( TKey key, out IEnumerable<TElement> elements );

        /// <summary>
        /// Gets a value indicating whether this IMultiMap{TKey, TElement}
        /// contains the specified value with the specified key.
        /// </summary>
        /// <param name="key">
        /// The key to locate.
        /// </param>
        /// <param name="value">
        /// The value that is associated with the specified <paramref name="key"/> to locate.
        /// </param>
        /// <returns>
        /// true if this IMultiMap{TKey, TElement} contains the specified key/value;
        /// otherwise false.
        /// </returns>
        [Pure]
        bool Contains( TKey key, TElement value );
    }
}
