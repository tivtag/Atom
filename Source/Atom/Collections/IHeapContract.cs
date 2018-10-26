// <copyright file="IHeapContract.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.IHeapContract{T} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Defines the contracts for the IHeap{T} interface.
    /// </summary>
    /// <typeparam name="T">
    /// The type of item stored in the IHeap{T}.
    /// </typeparam>
    // [ContractClassFor( typeof( IHeap<> ) )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Contract classes for interfaces aren't required to be documented." )] 
    internal abstract class IHeapContract<T> : IHeap<T>
    {
        /// <summary>
        /// Gets the <see cref="HeapType"/> of this IHeap{T}.
        /// </summary>
        public HeapType HeapType
        {
            get
            {
                return default( HeapType );   
            }
        }

        /// <summary>
        /// Gets the item at the root of this IHeap{T}.
        /// </summary>
        /// <value>
        /// The smallest or greatest value of this Heap{T}; depending on its <see cref="HeapType"/>.
        /// </value>
        public T Root
        {
            get
            {
                var @this = (IHeap<T>)this;
                Contract.Requires<InvalidOperationException>( @this.Count > 0 );

                return default( T );
            }
        }
        
        /// <summary>
        /// Gets the IComparer{T} that is used to compare the objects stored in this IHeap{T}.
        /// </summary>
        public IComparer<T> Comparer
        {
            get
            {
                //  Contract.Ensures( Contract.Result<IComparer<T>>() != null );

                return default( IComparer<T> );
            }
        }

        /// <summary>
        /// Removes and returns the item at the root of this Heap{T}.
        /// </summary>
        /// <returns>
        /// The smallest or greatest value of this Heap{T}; depending on its <see cref="HeapType"/>.
        /// </returns>
        public T Pop()
        {
            var @this = (IHeap<T>)this;
            Contract.Requires<InvalidOperationException>( @this.Count > 0 );
            // Contract.Ensures( @this.Count == (Contract.OldValue( @this.Count ) - 1) );
            // Contract.Ensures( object.Equals( Contract.Result<T>(), Contract.OldValue<T>( @this.Root ) ) );
            // Contract.Ensures(
            //     Contract.ForAll( @this, ( element ) => {
            //         return @this.Comparer.Compare( @this.Root, element ) <= 0;                
            //     })
            // );

            return default( T );
        }

        #region ICollection<T> Members

        void ICollection<T>.Add( T item )
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Contains( T item )
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.CopyTo( T[] array, int arrayIndex )
        {
            throw new NotImplementedException();
        }

        int ICollection<T>.Count
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<T>.Remove( T item )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
