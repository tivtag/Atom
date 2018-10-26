// <copyright file="NonNullList.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.NonNullList{T} class.
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
    /// Represents a <see cref="PreconditionedList{T}"/> that requires its items to be non-null.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the items in the list.
    /// </typeparam>
    public class NonNullList<T> : PreconditionedList<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the NonNullList{T} class.
        /// </summary>
        public NonNullList()
            : this( new List<T>() )
        {
        }

        /// <summary>
        /// Initializes a new instance of the NonNullList{T} class.
        /// </summary>
        /// <param name="list">
        /// The IList{T} the new NonNullList{T} uses internally.
        /// </param>
        public NonNullList( IList<T> list )
            : base( NonNullPredicate, list )
        {
            Contract.Requires<ArgumentException>( Contract.ForAll( list, element => element != null ) );
        }

        /// <summary>
        /// Defines the object invariant of the NonNullList class.
        /// </summary>
        // [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant( Contract.ForAll<T>( this, NonNullPredicate ) );
        }

        /// <summary>
        /// The predicate that must hold true for every element of the NonNullList class.
        /// </summary>
        /// <param name="item">
        /// The item to verify.
        /// </param>
        /// <returns>
        /// true if the predicate holds;
        /// otherwise false.
        /// </returns>
        [Pure]
        private static bool NonNullPredicate( T item )
        {
            return item != null;
        }

        /// <summary>
        /// Gets the error message that is shown when adding or inserting an item
        /// into this NonNullList{T} does not fulfill its predicate.
        /// </summary>
        /// <param name="item">
        /// The item that was attempted to be added or inserted.
        /// </param>
        /// <returns>
        /// The error message.
        /// </returns>
        protected override string GetPredicateFailedErrorMessage( T item )
        {
            return ErrorStrings.SpecifiedItemIsNull;
        }
    }
}
