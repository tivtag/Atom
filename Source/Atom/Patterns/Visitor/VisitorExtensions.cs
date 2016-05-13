// <copyright file="VisitorExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Patterns.Visitor.VisitorExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Patterns.Visitor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides static extension methods that make the work with <see cref="IVisitor{T}"/>s easier.
    /// </summary>
    public static class VisitorExtensions
    {
        /// <summary>
        /// Accepts the given <see cref="IVisitor{T}"/> to visit elements of this <see cref="IEnumerable{T}"/>
        /// until the <see cref="IVisitor{T}.HasCompleted"/> or all elements have been visited.
        /// </summary>
        /// <typeparam name="T">
        /// The type of items to visit.
        /// </typeparam>
        /// <param name="collection">
        /// The collection whose items to accept. 
        /// </param>
        /// <param name="visitor">
        /// The visitor to use.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="visitor"/> is null.
        /// </exception>
        public static void Visit<T>( this IEnumerable<T> collection, IVisitor<T> visitor )
        {
            Contract.Requires<ArgumentNullException>( collection != null );
            Contract.Requires<ArgumentNullException>( visitor != null );

            foreach( T item in collection )
            {
                if( visitor.HasCompleted )
                    break;

                visitor.Visit( item );
            }
        }

        /// <summary>
        /// Accepts the given <see cref="IVisitor{Object}"/> to visit elements of this <see cref="IEnumerable"/>
        /// until the <see cref="IVisitor{Object}.HasCompleted"/> or all elements have been visited.
        /// </summary>
        /// <param name="collection">
        /// The collection whose items to accept. 
        /// </param>
        /// <param name="visitor">
        /// The visitor to use.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="visitor"/> is null.
        /// </exception>
        public static void Visit( this IEnumerable collection, IVisitor<object> visitor )
        {
            Contract.Requires<ArgumentNullException>( collection != null );
            Contract.Requires<ArgumentNullException>( visitor != null );

            foreach( object item in collection )
            {
                if( visitor.HasCompleted )
                    break;

                visitor.Visit( item );
            }
        }
    }
}
