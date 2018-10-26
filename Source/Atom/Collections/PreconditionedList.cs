// <copyright file="PreconditionedList.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.PreconditionedList{T} class.
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
    /// Represents a <see cref="RedirectingList{T}"/> that requires items added or inserted into
    /// the list to fulfill a <see cref="Predicate{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the items in the list.
    /// </typeparam>
    public class PreconditionedList<T> : RedirectingList<T>
    {
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// If the specified item does not fulfill the predicate of this PreconditionedList{T}.
        /// </exception>
        /// <param name="index">
        /// The zero-based index of the element to get or set.
        /// </param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public override T this[int index]
        {
            get
            {
                return base[index];
            }

            set
            {
                if( this.predicate( value ) )
                {
                    base[index] = value;
                }
                else
                {
                    this.OnPredicateFailed( value, "value" );
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the PreconditionedList{T} class.
        /// </summary>
        /// <param name="predicate">
        /// The predicate that must be true for an element allowed to be inserted
        /// or added to the new PreconditionedList{T}.
        /// </param>
        public PreconditionedList( Predicate<T> predicate )
            : this( predicate, new List<T>() )
        {
        }

        /// <summary>
        /// Initializes a new instance of the PreconditionedList{T} class.
        /// </summary>
        /// <param name="predicate">
        /// The predicate that must be true for an element allowed to be inserted
        /// or added to the new PreconditionedList{T}.
        /// </param>
        /// <param name="list">
        /// The IList{T} the new PreconditionedList{T} uses internally.
        /// Existing items are -not- validated by the specified <paramref name="predicate"/>.
        /// </param>
        public PreconditionedList( Predicate<T> predicate, IList<T> list )
            : base( list )
        {
            Contract.Requires<ArgumentNullException>( predicate != null );

            this.predicate = predicate;
        }
        
        /// <summary>
        /// Adds the specified item to this PreconditionedList{T}.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// If the specified item does not fulfill the predicate of this PreconditionedList{T}.
        /// </exception>
        /// <param name="item">
        /// The item to add.
        /// </param>
        public override void Add( T item )
        {
            if( this.predicate( item ) )
            {
                base.Add( item );
            }
            else
            {
                this.OnPredicateFailed( item, "item" );
            }
        }

        /// <summary>
        /// Inserts the specified item into this PreconditionedList{T}.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// If the specified item does not fulfill the predicate of this PreconditionedList{T}.
        /// </exception>
        /// <param name="index">
        /// The zero-based index where the item should be inserted.
        /// </param>
        /// <param name="item">
        /// The item to insert.
        /// </param>
        public override void Insert( int index, T item )
        {
            if( this.predicate( item ) )
            {
                base.Insert( index, item );
            }
            else
            {
                this.OnPredicateFailed( item, "item" );
            }
        }

        /// <summary>
        /// Called when the predicate has returned false upon examining an item.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// By default an exception is thrown.
        /// </exception>
        /// <param name="item">
        /// The item that has been examined.
        /// </param>
        /// <param name="argumentName">
        /// The name of the argument from which the value has been received.
        /// </param>
        protected virtual void OnPredicateFailed( T item, string argumentName )
        {
            throw new ArgumentException( this.GetPredicateFailedErrorMessage( item ), argumentName );
        }

        /// <summary>
        /// Gets the error message that is shown when adding or inserting an item
        /// into this PreconditionedList{T} does not fulfill its predicate.
        /// </summary>
        /// <param name="item">
        /// The item that was attempted to be added or inserted.
        /// </param>
        /// <returns>
        /// The error message.
        /// </returns>
        protected virtual string GetPredicateFailedErrorMessage( T item )
        {
            // Contract.Ensures( Contract.Result<string>() != null );

            return ErrorStrings.ItemDoesNotFulfillPredicate;
        }

        /// <summary>
        /// The predicate that must be true for an element allowed to be inserted
        /// or added to this PreconditionedList{T}.
        /// </summary>        
        private readonly Predicate<T> predicate;
    }
}
