// <copyright file="SortableList.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.SortableList{T} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="SortableList{T}"/> keeps itself sorted,
    /// using either a specified <see cref="IComparer{T}"/> or the
    /// <see cref="IComparable"/>/<see cref="IComparable{T}"/> implementation
    /// of an object.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the items to store inside the list.
    /// </typeparam>
    public class SortableList<T> : IList<T>, ICloneable, IEquatable<SortableList<T>>
    {
        #region [ Properties ]

        /// <summary> 
        /// Gets a value indicating whether this <see cref="SortableList{T}"/> is currently sorted. 
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if this SortableList{T} is currently sorted;
        /// otherwise false.
        /// </value>
        public bool IsSorted
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SortableList{T}"/> should 
        /// keep itself sorted all the time. 
        /// </summary>
        /// <remarks>
        /// When adding many new items it may be wise to set this value to false
        /// for the duration of adding new items.
        /// </remarks>
        /// <value>The default value is true.</value>
        public bool KeepSorted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allow duplicate items in this <see cref="SortableList{T}"/>.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool AllowDuplicates
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the amount of items in this <see cref="SortableList{T}"/>.
        /// </summary>
        /// <value>
        /// The number of elements actually contained in the SortableList{T}.
        /// </value>
        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        /// <summary>
        /// Gets or sets item at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index"> The index of the item. </param>
        /// <returns> The item at the specified index. </returns>
        public T this[int index]
        {
            get 
            { 
                return this.list[index];
            }

            set 
            { 
                this.list[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the total number of elements the internal 
        /// data structure can hold without resizing.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        /// <value>
        /// The number of elements that the <see cref="SortableList{T}"/> can contain before resizing is required.
        /// </value>
        public int Capacity
        {
            get 
            {
                return this.list.Capacity;
            }

            set
            {
                this.list.Capacity = value; 
            }
        }

        #region Minimum

        /// <summary>
        /// Gets the minimum item in this <see cref="SortableList{T}"/>.
        /// </summary>
        /// <value>The minimum item in this <see cref="SortableList{T}"/>.</value>
        public Tuple<int, T> Minimum
        {
            get
            {
                if( this.Count > 0 )
                {
                    if( this.IsSorted )
                    {
                        return new Tuple<int, T>( 0, this.list[0] );
                    }

                    int index = 0;
                    T item = this.list[0];

                    for( int i = 1; i < this.list.Count; ++i )
                    {
                        T nextItem = this.list[i];

                        if( this.comparer.Compare( item, nextItem ) > 0 )
                        {
                            item = nextItem;
                            index = i;
                        }
                    }

                    return new Tuple<int, T>( index, item );
                }
                else
                {
                    return new Tuple<int, T>( -1, default( T ) );
                }
            }
        }

        #endregion

        #region IndexOfMinimum

        /// <summary>
        /// Gets the index of the object of the list whose value is minimum.
        /// </summary>
        /// <value>The index of the minimum object in the list.</value>
        public int IndexOfMinimum
        {
            get
            {
                if( list.Count > 0 )
                {
                    int index  = 0;
                    T item = this.list[0];

                    if( !this.IsSorted )
                    {
                        for( int i = 1; i < this.list.Count; ++i )
                        {
                            if( this.comparer.Compare( item, list[i] ) > 0 )
                            {
                                item = list[i];
                                index = i;
                            }
                        }
                    }

                    return index;
                }
                else
                {
                    return -1;
                }
            }
        }

        #endregion

        #region Maximum

        /// <summary>
        /// Gets the maximum item in this <see cref="SortableList{T}"/>.
        /// </summary>
        /// <value>
        /// The maximum item in this <see cref="SortableList{T}"/>.
        /// </value>
        public Tuple<int, T> Maximum
        {
            get
            {
                if( this.Count > 0 )
                {
                    int index = this.list.Count - 1;
                    T item    = this.list[index];

                    if( this.IsSorted )
                        return new Tuple<int, T>( index, item );

                    for( int i = this.list.Count - 2; i >= 0; --i )
                    {
                        T nextItem = this.list[i];

                        if( this.comparer.Compare( item, nextItem ) < 0 )
                        {
                            item = nextItem;
                            index = i;
                        }
                    }

                    return new Tuple<int, T>( index, item );
                }
                else
                {
                    return new Tuple<int, T>( -1, default( T ) );
                }
            }
        }

        #endregion

        #region IndexOfMaximum

        /// <summary>
        /// Gets the index of the object of the list whose value is maximum.
        /// </summary>
        /// <value>The index of the maximum object in the list.</value>
        public int IndexOfMaximum
        {
            get
            {
                if( list.Count > 0 )
                {
                    int index = this.list.Count - 1;
                    T item    = this.list[index];

                    if( !this.IsSorted )
                    {
                        for( int i = this.list.Count - 2; i >= 0; --i )
                        {
                            T nextItem = this.list[i];

                            if( this.comparer.Compare( item, nextItem ) < 0 )
                            {
                                item  = nextItem;
                                index = i;
                            }
                        }
                    }

                    return index;
                }
                else
                {
                    return -1;
                }
            }
        }

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableList{T}"/> class.
        /// Since no IComparer is provided here, added <typeparamref name="T"/> must implement the IComparer interface.
        /// </summary>
        public SortableList()
            : this( null, 0 )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableList{T}"/> class.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// If <Tparamref name="T"/> doesn't implement <see cref="IComparable{T}"/> and is not <see cref="Object"/>.
        /// </exception>
        /// <param name="capacity"> The initial number of items the <see cref="SortableList{T}"/> can contain. </param>
        public SortableList( int capacity )
            : this( null, capacity )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableList{T}"/> class.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// If <paramref name="comparer"/> is null: If <Tparamref name="T"/> doesn't implement <see cref="IComparable{T}"/> and is not <see cref="Object"/>.
        /// </exception>
        /// <param name="comparer"> The comparer to use for sorthing this <see cref="SortableList{T}"/>. </param>
        public SortableList( IComparer<T> comparer )
            : this( comparer, 0 )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableList{T}"/> class.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// If <paramref name="comparer"/> is null: If <Tparamref name="T"/> doesn't implement <see cref="IComparable{T}"/> and is not <see cref="Object"/>.
        /// </exception>
        /// <param name="comparer"> The comparer to use for sorthing this <see cref="SortableList{T}"/>. </param>
        /// <param name="capacity"> The initial number of items the <see cref="SortableList{T}"/> can contain. </param>
        public SortableList( IComparer<T> comparer, int capacity )
        {
            if( comparer == null )
            {
                if( typeof( IComparable<T> ).IsAssignableFrom( typeof( T ) ) == false )
                {
                    if( typeof( T ) != typeof( object ) )
                    {
                        throw new ArgumentException(
                            string.Format(
                                System.Globalization.CultureInfo.CurrentCulture,
                                ErrorStrings.TypeXDoesntImplementY,
                                typeof( T ).ToString(),
                                "IComparable<T>"
                            ),
                            "comparer"
                        );
                    }
                }

                this.comparer = new ObjectComparer();
                this.useObjectComparer = true;
            }
            else
            {
                this.comparer = comparer;
                this.useObjectComparer = false;
            }

            this.list = capacity > 0 ? new List<T>( capacity ) : new List<T>();

            this.IsSorted        = true;
            this.KeepSorted      = true;
            this.AllowDuplicates = true;
        }

        #endregion

        #region [ Methods ]

        #region Sort

        /// <summary>
        /// Sorts this <see cref="SortableList{T}"/>.
        /// Does nothing if already sorted.
        /// </summary>
        public void Sort()
        {
            if( this.IsSorted )
                return;

            this.list.Sort( comparer );
            this.IsSorted = true;
        }

        #endregion

        #region LimitOccurrencesOf

        /// <summary>
        /// Limits the number of occurences of the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">
        /// The item to limit.
        /// </param>
        /// <param name="amountToKeep">
        /// The maximum number of items to keep.
        /// </param>
        public void LimitOccurrencesOf( T item, int amountToKeep )
        {
            int index = 0;

            while( (index = IndexOf( item, index )) >= 0 )
            {
                if( amountToKeep <= 0 )
                {
                    list.RemoveAt( index );
                }
                else
                {
                    ++index;
                    --amountToKeep;
                }

                if( IsSorted && comparer.Compare( list[index], item ) > 0 )
                    break;
            }
        }

        #endregion

        #region RemoveDuplicates

        /// <summary>
        /// Removes all duplicate items from this <see cref="SortableList{T}"/>.
        /// </summary>
        public void RemoveDuplicates()
        {
            if( this.IsSorted )
            {
                int index = 0;

                while( index < this.Count - 1 )
                {
                    if( this.comparer.Compare( this[index], this[index + 1] ) == 0 )
                    {
                        this.list.RemoveAt( index );
                    }
                    else
                    {
                        ++index;
                    }
                }
            }
            else
            {
                for( int i = 0; i < this.Count; ++i )
                {
                    T item = this[i];

                    for( int j = 0; j < this.Count; ++j )
                    {
                        if( i != j && this.comparer.Compare( item, this[j] ) == 0 )
                        {
                            this.list.RemoveAt( j );
                        }
                    }
                }
            }
        }

        #endregion

        #region IList<T> Members

        #region Contains

        /// <summary>
        /// Returns whether this <see cref="SortableList{T}"/> contains the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item"> The item to test. </param>
        /// <returns>True if it contains the item, otherwise false. </returns>
        public bool Contains( T item )
        {
            if( this.IsSorted )
                return this.list.BinarySearch( item, comparer ) >= 0;
            else
                return this.list.Contains( item );
        }

        #endregion

        #region IndexOf

        #region IndexOf

        /// <summary>
        /// Returns the index of the specified <paramref name="item"/> in this <see cref="SortableList{T}"/>.
        /// </summary>
        /// <param name="item"> The item to get the index of. </param>
        /// <returns>
        /// A positive value if found,
        /// a negative value if not found.
        /// </returns>
        public int IndexOf( T item )
        {
            if( IsSorted )
            {
                int index = list.BinarySearch( item, comparer );

                while( index > 0 && list[index - 1].Equals( item ) )
                {
                    --index;
                }

                return index;
            }
            else
            {
                return list.IndexOf( item );
            }
        }

        #endregion

        #region IndexOf

        /// <summary> Returns the index of the specified item. </summary>
        /// <exception cref="ArgumentNullException"> If <paramref name="comparer"/> is null. </exception>
        /// <param name="item">The object to locate.</param>
        /// <param name="comparer">Equality function to use for the search.</param>
        /// <returns>The index of the item if found, -1 if not found. </returns>
        public int IndexOf( T item, IEqualityComparer<T> comparer )
        {
            Contract.Requires<ArgumentNullException>( comparer != null );

            for( int i = 0; i < list.Count; ++i )
            {
                if( comparer.Equals( list[i], item ) )
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion

        #region IndexOf

        /// <summary>
        /// Returns the index of the specified <paramref name="item"/> in this <see cref="SortableList{T}"/>.
        /// </summary>
        /// <param name="item"> The item to get the index of. </param>
        /// <param name="startIndex">The zero-based starting index of the search. </param>
        /// <returns>
        /// A positive value if found,
        /// a negative value if not found.
        /// </returns>
        public int IndexOf( T item, int startIndex )
        {
            if( IsSorted )
            {
                int index = list.BinarySearch( startIndex, list.Count - startIndex, item, comparer );

                while( index > startIndex && list[index - 1].Equals( item ) )
                    --index;

                return index;
            }
            else
            {
                return list.IndexOf( item, startIndex );
            }
        }

        #endregion

        #endregion

        #region Add

        /// <summary>
        /// Adds the specified <paramref name="item"/> to this <see cref="SortableList{T}"/>.
        /// </summary>
        /// <param name="item"> The item to add. </param>
        /// <returns> The index of the added item. </returns>
        public int Add( T item )
        {
            if( !this.AllowDuplicates && this.Contains( item ) )
                return -1;

            if( this.KeepSorted )
            {
                int index = this.IndexOf( item );
                int newIndex = index >= 0 ? index : -index - 1;

                if( newIndex >= this.Count )
                    list.Add( item );
                else
                    list.Insert( newIndex, item );
                return newIndex;
            }
            else
            {
                this.IsSorted = false;
                this.list.Add( item );
                return this.list.Count - 1;
            }
        }

        #endregion

        #region AddRange

        /// <summary>
        /// Adds the elements of the specified collection to this <see cref="SortableList{T}"/>.
        /// </summary>
        /// <param name="items"> The items to add. </param>
        public void AddRange( ICollection<T> items )
        {
            if( this.KeepSorted )
            {
                foreach( T item in items )
                {
                    this.Add( item );
                }
            }
            else
            {
                this.list.AddRange( items );
                this.IsSorted = false;
            }
        }

        #endregion

        #region Insert

        /// <summary>
        /// Inserts the specified <paramref name="item"/> at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index at which the <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The item to insert.</param>  
        /// <exception cref="InvalidOperationException">
        /// If <see cref="KeepSorted"/> is set to true. 
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"> 
        /// If the specified <paramref name="index"/> is out of range.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <see cref="AllowDuplicates"/> is true and the list already contains the specified item.
        /// </exception>
        public void Insert( int index, T item )
        {
            if( this.KeepSorted )
                throw new InvalidOperationException( ErrorStrings.CantInsertItemIntoSortableListIfKeepSortedIsTrue );

            if( index >= list.Count || index < 0 )
                throw new ArgumentOutOfRangeException( "index", index, ErrorStrings.SpecifiedIndexIsInvalid );

            if( this.AllowDuplicates == false )
            {
                if( this.Contains( item ) )
                    throw new ArgumentException( ErrorStrings.ListAlreadyConstainsSpecifiedItem );
            }

            T before = index == 0 ? default( T ) : list[index - 1];
            T after  = list[index];

            if( (before != null && comparer.Compare( before, item ) > 0) ||
                (after  != null && comparer.Compare( item, after ) > 0) )
            {
                IsSorted = false;
            }

            list.Insert( index, item );
        }

        #endregion

        #region InsertRange

        /// <summary>
        /// Adds the elements of the specified collection to this <see cref="SortableList{T}"/>
        /// at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The index to start adding at.
        /// </param>
        /// <param name="items">
        /// The items to add.
        /// </param>
        public void InsertRange( int index, ICollection<T> items )
        {
            if( this.KeepSorted )
            {
                foreach( T item in items )
                {
                    this.Insert( index++, item );
                }
            }
            else
            {
                this.list.InsertRange( index, items );
                this.IsSorted = false;
            }
        }

        #endregion

        #region Clear

        /// <summary>
        /// Removes all elements from this <see cref="SortableList{T}"/>.
        /// </summary>
        public void Clear()
        {
            this.list.Clear();
        }

        #endregion

        #region RemoveAt

        /// <summary>
        /// Removes the item at the specified <paramref name="index"/>.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"> If the specified <paramref name="index"/> is invalid. </exception>
        /// <param name="index"> The index of the item to remove. </param>
        public void RemoveAt( int index )
        {
            this.list.RemoveAt( index );
        }

        #endregion

        #endregion

        #region Object Members

        /// <summary>
        /// Returns the hash-code of this instance.
        /// </summary>
        /// <returns>The hash-code.</returns>
        public override int GetHashCode()
        {
            return this.list.GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of this <see cref="SortableList{T}"/> instance.
        /// </summary>
        /// <returns>A string representation of this <see cref="SortableList{T}"/> instance.</returns>
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            foreach( T item in list )
            {
                sb.AppendLine( item.ToString() );
            }

            return sb.ToString();
        }

        /// <summary>
        /// Determines whether all elements of this <see cref="SortableList{T}"/> are
        /// equal to the specified <see cref="SortableList{T}"/>.
        /// </summary>
        /// <param name="other">The other SortableList to compare against.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the lists are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( SortableList<T> other )
        {
            if( other == null )
                return false;

            if( this.list.Count != other.Count )
                return false;

            for( int i = 0; i < other.Count; ++i )
            {
                if( this.list[i].Equals( other[i] ) == false )
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether all elements of this <see cref="SortableList{T}"/> are
        /// equal to the specified Object.
        /// </summary>
        /// <param name="obj">
        /// The object to compare to.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            return this.Equals( obj as SortableList<T> );
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Returns a clone of this SortableList{T} instance.
        /// </summary>
        /// <returns>
        /// The cloned SortableList{T}.
        /// </returns>
        public object Clone()
        {
            var clone = new SortableList<T>( comparer, list.Count ) {
                IsSorted          = this.IsSorted,
                KeepSorted        = this.KeepSorted,
                useObjectComparer = this.useObjectComparer
            };

            clone.list.AddRange( this.list.ToArray() );
            return clone;
        }

        #endregion
        
        #region ICollection<T> Members

        /// <summary>
        /// Adds the specified <paramref name="item"/> to this <see cref="SortableList{T}"/>.
        /// </summary>
        /// <param name="item"> The item to add. </param>
        void ICollection<T>.Add( T item )
        {
            this.Add( item );
        }

        /// <summary>
        /// Copies the elements of this <see cref="SortableList{T}"/> to the specified <paramref name="array"/>
        /// starting at the specified <paramref name="arrayIndex"/>.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="arrayIndex"/> is equal to or greater than the length of array.-or-The number
        /// of elements in the source SortavbleList{T} is greater than
        /// the available space from arrayIndex to the end of the destination array.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If the specified <paramref name="arrayIndex"/> is invalid.
        /// </exception>
        /// <param name="array">
        /// The one-dimensional System.Array that is the destination of the elements
        /// copied from SortableList{T}. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in array at which copying begins.
        /// </param>
        public void CopyTo( T[] array, int arrayIndex )
        {
            this.list.CopyTo( array, arrayIndex );
        }

        /// <summary>
        /// Gets a value indicating whether from this collection can only be readen.
        /// </summary>
        /// <value>Always returns <see langword="false"/>.</value>
        public bool IsReadOnly
        {
            get
            { 
                return false;
            }
        }

        /// <summary>
        /// Removes the first occurence of the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item"> The item to remove. </param>
        /// <returns>
        /// Returns <see langword="true"/> if the given <paramref name="item"/> has been removed;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Remove( T item )
        {
            return this.list.Remove( item );
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates over through this <see cref="SortableList{T}"/>.
        /// </summary>
        /// <returns>A new IEnumerator{T} over the items in this SortableList{T}.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates over through this <see cref="SortableList{T}"/>.
        /// </summary>
        /// <returns>A new IEnumerator{T} over the items in this SortableList{T}.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary> 
        /// The internal list that is kept sorted. 
        /// </summary>
        private readonly List<T> list;

        /// <summary> 
        /// The comparer used for sorting. May be null.
        /// </summary>
        private readonly IComparer<T> comparer;

        /// <summary> 
        /// States whether to use the ICompareable interface of the objects for comparing.
        /// </summary>
        private bool useObjectComparer;

        #endregion

        #region > class ObjectComparer <

        /// <summary>
        /// Implements an <see cref="IComparer{T}"/>
        /// that is used when the user doesn't specifiy
        /// a comparer.
        /// </summary>
        private sealed class ObjectComparer : IComparer<T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ObjectComparer"/> class.
            /// </summary>
            public ObjectComparer()
            {
            }

            /// <summary>
            /// Compares the objects.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// Value Condition 
            /// Less than zero x is less than y.
            /// Zero x equals y.
            /// Greater than zero x is greater than y.
            /// </returns>
            public int Compare( T x, T y )
            {
                IComparable<T> c = x as IComparable<T>;
                if( c != null )
                    return c.CompareTo( y );

                IComparable c2 = x as IComparable;
                if( c2 != null )
                    return c2.CompareTo( y );

                throw new InvalidOperationException(
                    string.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        ErrorStrings.ObjectXDoesntImplementY,
                        "x",
                        "IComparable<T>/IComparable"
                    )
                );
            }
        }

        #endregion
    }
}