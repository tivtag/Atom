// <copyright file="RedirectingList.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.RedirectingList{T} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents an list of items that uses another list to 
    /// actually store the elements.
    /// </summary>
    /// <remarks>
    /// This class implements both IList{T}, ICollection{T} and
    /// their non-generic counter-parts.
    /// The other aspect of this class is that all inseration and deletion
    /// methods are virtual and as such overwriteable.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the items in the list.
    /// </typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( 
        "Microsoft.StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented", 
        Justification = "The interfaces IList and ICollection aren't directly exposed to the user." )] 
    public class RedirectingList<T> : IList<T>, ICollection<T>, IList, ICollection
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the list this RedirectingList{T} uses internally.
        /// </summary>
        protected IList<T> List
        {
            get
            {
                Contract.Ensures( Contract.Result<IList<T>>() != null );
                return this.list;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectingList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="list">
        /// The list the new RedirectingList{T} should redirect all calls to.
        /// </param>
        public RedirectingList( IList<T> list )
        {
            Contract.Requires<ArgumentNullException>( list != null );

            this.list = list;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectingList&lt;T&gt;"/> class.
        /// </summary>
        public RedirectingList()
            : this( new List<T>() )
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Defines the contract invariant that always holds true.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant( this.list != null );
        }

        /// <summary>
        /// Casts the specified object to T.
        /// </summary>
        /// <remarks>
        /// Used by the various IList and ICollection methods.
        /// </remarks>
        /// <param name="value">
        /// The input value.
        /// </param>
        /// <returns>
        /// The casted output value.
        /// </returns>
        private static T Cast( object value )
        {
            return (T)value;
        }

        #endregion

        #region [ Implementation ]

        #region IList<T> Members

        /// <summary>
        /// Determines the index of a specific item in the System.Collections.Generic.IList{T}.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the System.Collections.Generic.IList{T}.
        /// </param>
        /// <returns>
        /// The index of item if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf( T item )
        {
            return this.list.IndexOf( item );
        }

        /// <summary>
        /// Inserts an item to the System.Collections.Generic.IList{T} at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the System.Collections.Generic.IList{T}.</param>
        public virtual void Insert( int index, T item )
        {
            this.list.Insert( index, item );
        }

        /// <summary>
        /// Removes the System.Collections.Generic.IList{T} item at the specified index.
        /// </summary>
        /// <param name="index"> The zero-based index of the item to remove.</param>
        public virtual void RemoveAt( int index )
        {
            this.list.RemoveAt( index );
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to get or set.
        /// </param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public virtual T this[int index]
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

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// Adds an item to the System.Collections.Generic.ICollection{T}.
        /// </summary>
        /// <param name="item">
        /// The object to add to the System.Collections.Generic.ICollection{T}.
        /// </param>
        public virtual void Add( T item )
        {
            this.list.Add( item );
        }

        /// <summary>
        /// Removes all items from the System.Collections.Generic.ICollection{T}.
        /// </summary>
        public virtual void Clear()
        {
            this.list.Clear();
        }

        /// <summary>
        /// Determines whether the System.Collections.Generic.ICollection{T} contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the System.Collections.Generic.ICollection{T}.
        /// </param>
        /// <returns>
        /// true if item is found in the System.Collections.Generic.ICollection{T}>;
        /// otherwise, false.
        /// </returns>
        public bool Contains( T item )
        {
            return this.list.Contains( item );
        }

        /// <summary>
        /// Copies the elements of the System.Collections.Generic.ICollection{T} to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional System.Array that is the destination of the elements
        /// copied from System.Collections.Generic.ICollection{T}. The System.Array must
        /// have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in array at which copying begins.
        /// </param>
        public void CopyTo( T[] array, int arrayIndex )
        {
            this.list.CopyTo( array, arrayIndex );
        }

        /// <summary>
        /// Gets the number of elements contained in the System.Collections.Generic.ICollection{T}.
        /// </summary>
        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the System.Collections.Generic.ICollection{T} is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get 
            {
                return this.list.IsReadOnly;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the System.Collections.Generic.ICollection{T}.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the System.Collections.Generic.ICollection{T}.
        /// </param>
        /// <returns>
        /// true if item was successfully removed from the System.Collections.Generic.ICollection{T};
        /// otherwise, false. This method also returns false if item is not found in
        /// the original System.Collections.Generic.ICollection{T}.
        /// </returns>
        public virtual bool Remove( T item )
        {
            return this.list.Remove( item );
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A System.Collections.Generic.IEnumerator{T} that can be used to iterate through
        /// the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A System.Collections.Generic.IEnumerator that can be used to iterate through
        /// the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IList Members

        int IList.Add( object value )
        {
            this.Add( Cast( value ) );
            return this.Count - 1;
        }

        bool IList.Contains( object value )
        {
            return this.Contains( Cast( value ) );
        }

        int IList.IndexOf( object value )
        {
            return this.IndexOf( Cast( value ) );
        }

        void IList.Insert( int index, object value )
        {
            this.Insert( index, Cast( value ) );
        }

        void IList.Remove( object value )
        {
            this.Remove( Cast( value ) );
        }

        void IList.RemoveAt( int index )
        {
            this.RemoveAt( index );
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                this[index] = Cast( value );
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                var list = this.list as IList;

                if( list != null )
                    return list.IsFixedSize;
                else
                    return false;
            }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo( Array array, int index )
        {
            throw new NotImplementedException();
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false; 
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return null; 
            }
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The list this RedirectingList{T} is redirecting all calls to.
        /// </summary>
        private readonly IList<T> list;

        #endregion
    }
}