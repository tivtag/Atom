// <copyright file="Heap.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.Heap{T} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Collections
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using Atom.Collections.Comparers;

    /// <summary>
    /// Represents a simple heap of items.
    /// </summary>
    /// <seealso cref="HeapType"/>
    /// <typeparam name="T">
    /// The type of item stored in the Heap{T}.
    /// </typeparam>
    [Serializable]
    public class Heap<T> : IHeap<T>
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the type of heap represented by this instance.
        /// </summary>
        /// <value>The type of heap.</value>
        public HeapType HeapType
        {
            get 
            {
                return this.heapType;
            }
        }

        /// <summary>
        /// Gets the smallest item in the heap (located at the root).
        /// </summary>
        /// <value>
        /// The smallest or greatest value of this Heap{T}; depending on its <see cref="HeapType"/>.
        /// </value>
        public T Root
        {
            get
            {
                return this.data[1];
            }
        }

        /// <summary>
        /// Gets the IComparer{T} that is used to compare the objects stored in this IHeap{T}.
        /// </summary>
        public IComparer<T> Comparer
        {
            get
            {
                return this.comparer;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</value>
        public int Count
        {
            get
            {
                return this.data.Count - 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// Always returns <c>false</c>.
        /// </value>
        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class.
        /// </summary>
        /// <param name="type">
        /// The type of new Heap.
        /// </param>
        public Heap( HeapType type )
            : this( type, Comparer<T>.Default )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class.
        /// </summary>
        /// <param name="type">
        /// The type of new Heap.
        /// </param>
        /// <param name="capacity">
        /// The initial number of objects the new Heap can store.
        /// </param>
        public Heap( HeapType type, int capacity )
            : this( type, capacity, Comparer<T>.Default )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="comparer"/> is null.
        /// </exception>
        /// <param name="type">
        /// The type of new Heap.
        /// </param>
        /// <param name="comparer">
        /// The comparer the new Heap should use.
        /// </param>
        public Heap( HeapType type, IComparer<T> comparer )
            : this( type, 1, comparer )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="comparer"/> is null.
        /// </exception>
        /// <param name="type">
        /// The type of new Heap.
        /// </param>
        /// <param name="capacity">
        /// The initial number of objects the new Heap can store.
        /// </param>
        /// <param name="comparer">
        /// The comparer the new Heap should use.
        /// </param>
        public Heap( HeapType type, int capacity, IComparer<T> comparer )
        {
            Contract.Requires<ArgumentNullException>( comparer != null );
            
            this.data = new List<T>( capacity );
            this.data.Add( default( T ) );  // Add a dummy item so our indexing starts at 1

            if( type == HeapType.Minimum )
                this.comparer = comparer;
            else
                this.comparer = new ReverseComparer<T>( comparer );
            this.heapType = type;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Adds an item to the <see cref="Heap{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="Heap{T}"/>.</param>
        public void Add( T item )
        {
            // Add a dummy to the end of the list (it will be replaced)
            this.data.Add( default( T ) );

            int counter = this.data.Count - 1;

            while( (counter > 1) && (this.comparer.Compare( this.data[counter / 2], item ) > 0) )
            {
                int halfCounter = counter / 2;

                this.data[counter] = this.data[halfCounter];
                counter = halfCounter;
            }

            this.data[counter] = item;
        }

        /// <summary>
        /// Determines whether this <see cref="Heap{T}"/> contains the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="Heap{T}"/>.</param>
        /// <returns>
        /// Returns <c>true</c> if item is found in the <see cref="Heap{T}"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Contains( T item )
        {
            if( item == null )
            {
                for( int j = 1; j < this.data.Count; ++j )
                {
                    if( this.data[j] == null )
                    {
                        return true;
                    }
                }

                return false;
            }

            var equalityComparer = EqualityComparer<T>.Default;

            for( int i = 1; i < this.data.Count; ++i )
            {
                if( equalityComparer.Equals( this.data[i], item ) )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes and returns the item at the root of this Heap{T}.
        /// </summary>
        /// <returns>
        /// The smallest or greatest value of this Heap{T}; depending on its <see cref="HeapType"/>.
        /// </returns>
        public T Pop()
        {
            // The minimum item to return.
            T min = this.data[1];

            // The last item in the heap
            T last = data[this.Count];
            this.data.RemoveAt( this.Count );

            // If there's still items left in this heap, reheapify it.
            int dataCount = this.data.Count;

            if( dataCount > 1 )
            {
                // Re-heapify the binary tree to conform to the heap property 
                int counter = 1;

                while( (counter * 2) < dataCount )
                {
                    int child = counter * 2;

                    if( ((child + 1) < dataCount) && (this.comparer.Compare( this.data[child + 1], this.data[child] ) < 0) )
                    {
                        ++child;
                    }

                    if( this.comparer.Compare( last, this.data[child] ) <= 0 )
                    {
                        break;
                    }

                    this.data[counter] = this.data[child];
                    counter = child;
                }

                this.data[counter] = last;
            }

            return min;
        }

        /// <summary>
        /// Copies the elements of the <see cref="Heap{T}"/> to an <see cref="T:System.Array"></see>, 
        /// starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="T:System.ArgumentNullException">array is null.</exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-TriangleType T cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo( T[] array, int arrayIndex )
        {
            for( int i = 1; i < this.data.Count; ++i )
            {
                array[arrayIndex++] = this.data[i];
            }
        }

        /// <summary>
        /// Clears all the objects in this instance.
        /// </summary>
        public void Clear()
        {
            // Clears all objects in this instance except the first dummy one.
            this.data.RemoveRange( 1, this.data.Count - 1 );
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            for( int i = 1; i < this.data.Count; ++i )
            {
                yield return this.data[i];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the items of the heap.
        /// </summary>
        /// <returns>An enumerator for enumerating though the colleciton.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region > Not Supported <

        /// <summary>
        /// This operation is not supported.
        /// </summary>
        /// <param name="item">This parameter is not used.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <returns>This method never returns.</returns>
        bool ICollection<T>.Remove( T item )
        {
            throw new NotSupportedException();
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The type of this Heap{T}.
        /// </summary>
        private readonly HeapType heapType;

        /// <summary>
        /// The internal list that stores the actual data stored in this Heap{T}.
        /// </summary>
        private readonly List<T> data;

        /// <summary>
        /// The IComparer that is used when sorting.
        /// </summary>
        private readonly IComparer<T> comparer;

        #endregion
    }
}
