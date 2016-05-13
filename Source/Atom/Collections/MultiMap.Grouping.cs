// <copyright file="MultiMap.Grouping.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.MultiMap{Tkey, TElement}.Grouping class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections
{
    using System.Collections.Generic;

    /// <content>
    /// Defines the default IMutableGrouping{TKey, TElement} implementation
    /// used by the MultiMap{TKey, TElement} class.
    /// </content>
    public partial class MultiMap<TKey, TElement>
    {
        /// <summary>
        /// Defines the default implementation of the <see cref="IMutableGrouping{TKey, TElement}"/> interface
        /// that is used by the MultiMap{TKey, TElement} class.
        /// </summary>
        private sealed class Grouping : IMutableGrouping<TKey, TElement>
        {
            /// <summary>
            /// Gets the key that is associated with this Grouping.
            /// </summary>
            public TKey Key
            {
                get
                {
                    return this.key;
                }
            }

            /// <summary>
            /// Gets the number of elements in this Grouping.
            /// </summary>
            public int Count
            {
                get 
                {
                    return this.items.Count;
                }
            }

            /// <summary>
            /// Gets a value indicating whether this Grouping is read-only.
            /// </summary>
            /// <value>Always returns false.</value>
            public bool IsReadOnly
            {
                get
                {
                    return false; 
                }
            }

            /// <summary>
            /// Initializes a new instance of the Grouping class.
            /// </summary>
            /// <param name="key">
            /// The key the new Grouping is associated with.
            /// </param>
            public Grouping( TKey key )
            {
                this.key = key;
            }

            /// <summary>
            /// Adds the specified <paramref name="element"/> to this IMutableGrouping{Tkey, TElement}.
            /// </summary>
            /// <param name="element">
            /// The element to add.
            /// </param>
            public void Add( TElement element )
            {
                this.items.Add( element );
            }

            /// <summary>
            /// Tries to remove the specified <paramref name="element"/>
            /// from this IMutableGrouping{Tkey, TElement}.
            /// </summary>
            /// <param name="element">
            /// The element to remove.
            /// </param>
            /// <returns>
            /// true if the specified element was removed;
            /// otherwise false.
            /// </returns>
            public bool Remove( TElement element )
            {
                return this.items.Remove( element );
            }

            /// <summary>
            /// Gets a value indicating whether this Grouping contains the specified <paramref name="element"/>.
            /// </summary>
            /// <param name="element">
            /// The element to look for.
            /// </param>
            /// <returns>
            /// true if this grouping contains the specified element;
            /// otherwise false.
            /// </returns>
            public bool Contains( TElement element )
            {
                return this.items.Contains( element );
            }

            /// <summary>
            /// Removes all elements from this Grouping.
            /// </summary>
            public void Clear()
            {
                this.items.Clear();
            }

            /// <summary>
            /// Copies this entire Grouping to a compatible one-dimensional
            /// rray, starting at the specified index of the target array.
            /// </summary>
            /// <param name="array">
            /// The one-dimensional System.Array that is the destination of the elements in this Grouping.
            /// </param>
            /// <param name="arrayIndex">
            /// The zero-based index in array at which copying begins.
            /// </param>
            public void CopyTo( TElement[] array, int arrayIndex )
            {
                this.items.CopyTo( array, arrayIndex );
            }

            /// <summary>
            /// Returns an enumerator that iterates through the item associated with this Grouping.
            /// </summary>
            /// <returns>
            /// A new IEnumerator.
            /// </returns>
            public IEnumerator<TElement> GetEnumerator()
            {
                return this.items.GetEnumerator();
            }

            /// <summary>
            /// Returns an enumerator that iterates through the item associated with this Grouping.
            /// </summary>
            /// <returns>
            /// A new IEnumerator.
            /// </returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.items.GetEnumerator();
            }

            /// <summary>
            /// The key this grouping is associated with.
            /// </summary>
            private readonly TKey key;

            /// <summary>
            /// The list that holds the items that are part of this Grouping.
            /// </summary>
            private readonly List<TElement> items = new List<TElement>();
        }
    }
}
