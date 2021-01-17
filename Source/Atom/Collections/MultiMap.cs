// <copyright file="MultiMap.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.MultiMap{Tkey, TElement} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a dictionary that maps keys onto lists of elements.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of the key that is associated with a list of elements.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of elements.
    /// </typeparam>
    public partial class MultiMap<TKey, TElement> 
        : ILookup<TKey, TElement>, Atom.Collections.IMultiMap<TKey, TElement>
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the number of key/value association pairs in this MultiMap&lt;TKey,TElement&gt;.
        /// </summary>
        /// <value>
        /// The number of key/value association pairs in this MultiMap&lt;TKey,TElement&gt;.
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
        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                return this.dictionary[key];
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiMap&lt;TKey, TElement&gt;"/> class.
        /// </summary>
        public MultiMap()
        {
            this.dictionary = new Dictionary<TKey, IMutableGrouping<TKey, TElement>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiMap&lt;TKey, TElement&gt;"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The initial number of elements the new MultiMap can contain.
        /// </param>
        public MultiMap( int capacity )
        {
            this.dictionary = new Dictionary<TKey, IMutableGrouping<TKey, TElement>>( capacity );
        }

        #endregion

        #region [ Methods ]

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
        /// true if there exists an IMutableGrouping{TKey, TElement} that corresponds to the specified <paramref name="key"/>;
        /// otherwise false.
        /// </returns>
        public bool TryGet( TKey key, out IEnumerable<TElement> elements )
        {
            IMutableGrouping<TKey, TElement> group;

            if( this.dictionary.TryGetValue( key, out group ) )
            {
                elements = group;
                return true;
            }
            else
            {
                elements = null;
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this MultiMap{TKey, TElement} contains
        /// the specified key/value pair.
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
        public bool Contains( TKey key, TElement value )
        {
            IMutableGrouping<TKey, TElement> group;

            if( this.dictionary.TryGetValue( key, out group ) )
            {
                return group.Contains( value );
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether an IMutableGrouping exists for the 
        /// specified <paramref name="key"/> in this MultiMap{TKey, TElement}.
        /// </summary>
        /// <param name="key">
        /// The key to search for in this MultiMap{TKey, TElement}.
        /// </param>
        /// <returns>
        /// true if key is in this MultiMap; otherwise, false.
        /// </returns>
        public bool Contains( TKey key )
        {
            return this.dictionary.ContainsKey( key );
        }

        /// <summary>
        /// Adds the specified <paramref name="value"/> under the specified <paramref name="key"/>
        /// to this MultiMap{TKey, TElement}.
        /// </summary>
        /// <param name="key">
        /// The key the specified <paramref name="value"/> should be associated with.
        /// </param>
        /// <param name="value">
        /// The value to add to this MultiMap{TKey, TElement}.
        /// </param>
        public void Add( TKey key, TElement value )
        {
            IMutableGrouping<TKey, TElement> group;

            if( !this.dictionary.TryGetValue( key, out group ) )
            {
                group = this.CreateGrouping( key );
                this.dictionary.Add( key, group );
            }

            group.Add( value );
        }

        /// <summary>
        /// Tries to remove the specified key/value pair from this MultiMap{TKey, TElement}.
        /// </summary>
        /// <param name="key">
        /// The key that is associated with the specified <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        /// The value to remove.
        /// </param>
        /// <returns>
        /// true if the specified key/value pair was removed from this MultiMap{TKey, TElement};
        /// otherwise false.
        /// </returns>
        public bool Remove( TKey key, TElement value )
        {
            IMutableGrouping<TKey, TElement> group;

            if( this.dictionary.TryGetValue( key, out group ) )
            {
                bool removed = group.Remove( value );

                if( removed && group.Count == 0 )
                {
                    this.dictionary.Remove( key );
                }

                return removed;
            }

            return false;
        }

        /// <summary>
        /// Creates a new <see cref="IMutableGrouping{TKey, TElement}"/> for the
        /// specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">
        /// The key to create a new grouping for.
        /// </param>
        /// <returns>
        /// A newly created IMutableGrouping{TKey, TElement}.
        /// </returns>
        protected virtual IMutableGrouping<TKey, TElement> CreateGrouping( TKey key )
        {
            // Contract.Ensures( Contract.Result<IMutableGrouping<TKey, TElement>>() != null );        
            return new Grouping( key );
        }

        /// <summary>
        /// Returns an enumerator that iterates through the IGroupings that are associated with this MultiMap.
        /// </summary>
        /// <returns>
        /// A new IEnumerator.
        /// </returns>
        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            foreach( var group in this.dictionary )
            {
                yield return group.Value;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the IGroupings that are associated with this MultiMap.
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
        private readonly Dictionary<TKey, IMutableGrouping<TKey, TElement>> dictionary;

        #endregion
    }
}