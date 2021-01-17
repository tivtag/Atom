// <copyright file="FastMultiMap.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.FastMultiMap{Tkey, TElement} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a dictionary that maps keys onto lists of elements.
    /// </summary>
    /// <remarks>
    /// The FastMultiMap exposes some implementation details and provides less flexability
    /// compared to the <see cref="MultiMap&lt;TKey, TElement&gt;"/> class.
    /// In exchange the amount of virtual function calls are reduced/eleminated.
    /// </remarks>
    /// <typeparam name="TKey">
    /// The type of the key that is associated with a list of elements.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of elements.
    /// </typeparam>
    public class FastMultiMap<TKey, TElement> : IMultiMap<TKey, TElement>, IEnumerable<List<TElement>>
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the number of key/value association pairs in this FastMultiMap&lt;TKey,TElement&gt;.
        /// </summary>
        /// <value>
        /// The number of key/value association pairs in this FastMultiMap&lt;TKey,TElement&gt;.
        /// </value>
        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        /// <summary>
        /// Gets the elements that are associated with the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">
        /// The key of the desired sequence of values.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable&lt;TElement&gt;"/> sequence of values 
        /// indexed by the specified key.
        /// </returns>
        public List<TElement> this[TKey key]
        {
            get
            {
                return this.dictionary[key];
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="FastMultiMap&lt;TKey, TElement&gt;"/> class.
        /// </summary>
        public FastMultiMap()
        {
            this.dictionary = new Dictionary<TKey, List<TElement>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastMultiMap&lt;TKey, TElement&gt;"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The initial number of elements the new FastMultiMap{TKey, TElement} can contain.
        /// </param>
        public FastMultiMap( int capacity )
        {
            this.dictionary = new Dictionary<TKey, List<TElement>>( capacity );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Adds the specified <paramref name="value"/> under the specified <paramref name="key"/>
        /// to this FastMultiMap{TKey, TElement}.
        /// </summary>
        /// <param name="key">
        /// The key the specified <paramref name="value"/> should be associated with.
        /// </param>
        /// <param name="value">
        /// The value to add to this FastMultiMap{TKey, TElement}.
        /// </param>
        public void Add( TKey key, TElement value )
        {
            List<TElement> list;

            if( !this.dictionary.TryGetValue( key, out list ) )
            {
                list = new List<TElement>();
                this.dictionary.Add( key, list );
            }

            list.Add( value );
        }

        /// <summary>
        /// Tries to remove the specified key/value pair from this FastMultiMap{TKey, TElement}.
        /// </summary>
        /// <param name="key">
        /// The key that is associated with the specified <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        /// The value to remove.
        /// </param>
        /// <returns>
        /// true if the specified key/value pair was removed from this FastMultiMap{TKey, TElement};
        /// otherwise false.
        /// </returns>
        public bool Remove( TKey key, TElement value )
        {
            List<TElement> list;

            if( this.dictionary.TryGetValue( key, out list ) )
            {
                return list.Remove( value );
            }

            return false;
        }

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
        public bool TryGet( TKey key, out List<TElement> elements )
        {
            Contract.Requires<ArgumentNullException>( key != null );
            // Contract.Ensures( !(Contract.Result<bool>() == false) || Contract.ValueAtReturn( out elements ) == null );
            // Contract.Ensures( !(Contract.Result<bool>() == true) || Contract.ValueAtReturn( out elements ) != null );

            return this.dictionary.TryGetValue( key, out elements );
        }
        
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
        bool IMultiMap<TKey, TElement>.TryGet( TKey key, out IEnumerable<TElement> elements )
        {
            List<TElement> list;

            if( this.dictionary.TryGetValue( key, out list ) )
            {
                elements = list;
                return true;
            }
            else
            {
                elements = null;
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this FastMultiMap{TKey, TElement} 
        /// contains the specified value with the specified key.
        /// </summary>
        /// <param name="key">
        /// The key to locate.
        /// </param>
        /// <param name="value">
        /// The value that is associated with the specified <paramref name="key"/> to locate.
        /// </param>
        /// <returns>
        /// true if this FastMultiMap{TKey, TElement} contains the specified key/value;
        /// otherwise false.
        /// </returns>
        public bool Contains( TKey key, TElement value )
        {
            List<TElement> list;

            if( this.dictionary.TryGetValue( key, out list ) )
            {
                return list.Contains( value );
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a List exists for the 
        /// specified <paramref name="key"/> in this FastMultiMap{TKey, TElement}.
        /// </summary>
        /// <param name="key">
        /// The key to search for in this FastMultiMap{TKey, TElement}.
        /// </param>
        /// <returns>
        /// true if key is in this MultiMap; otherwise, false.
        /// </returns>
        public bool Contains( TKey key )
        {
            return this.dictionary.ContainsKey( key );
        }
              
        /// <summary>
        /// Returns an enumerator that iterates through the lists of this FastMultiMap.
        /// </summary>
        /// <returns>
        /// A new IEnumerator.
        /// </returns>
        public IEnumerator<List<TElement>> GetEnumerator()
        {
            return this.dictionary.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the lists of this FastMultiMap.
        /// </summary>
        /// <returns>
        /// A new IEnumerator.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The dictionary of elements.
        /// </summary>
        private readonly Dictionary<TKey, List<TElement>> dictionary;

        #endregion
    }
}