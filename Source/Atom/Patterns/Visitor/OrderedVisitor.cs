// <copyright file="OrderedVisitor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.OrderedVisitor{T} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Patterns.Visitor
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Implements a visitor that visits objects in order (PreOrder, PostOrder, or InOrder).
    /// </summary>
    /// <remarks>
    /// Used primarily as a base class for Visitors specializing in a specific order type.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of items that can be visited by this OrderedVisitor{T}.
    /// </typeparam>
    public class OrderedVisitor<T> : IOrderedVisitor<T>
    {
        #region [ Properties ]

        /// <summary>
        /// Gets a value indicating whether this OrderedVisitor{T} is done visiting.
        /// </summary>
        /// <value>Whether the underlying <see cref="Visitor"/> has completed.</value>
        public bool HasCompleted
        {
            get
            {
                return this.visitor.HasCompleted;
            }
        }

        /// <summary>
        /// Gets the <see cref="IVisitor{T}"/> this OrderedVisitor{T} uses internally.
        /// </summary>
        /// <value>The <see cref="IVisitor{T}"/> this OrderedVisitor{T} uses internally.</value>
        public IVisitor<T> Visitor
        {
            get
            {
                // Contract.Ensures( Contract.Result<IVisitor<T>>() != null );

                return this.visitor;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedVisitor{T}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="visitor"/> is null.
        /// </exception>
        /// <param name="visitor">
        /// The visitor to use when visiting the object.
        /// </param>
        public OrderedVisitor( IVisitor<T> visitor )
        {
            Contract.Requires<ArgumentNullException>( visitor != null );

            this.visitor = visitor;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Visits the given object in pre-order.
        /// </summary>
        /// <param name="obj">
        /// The object to virst.
        /// </param>
        public virtual void VisitPreOrder( T obj )
        {
            this.visitor.Visit( obj );
        }

        /// <summary>
        /// Visits the given object in post-order.
        /// </summary>
        /// <param name="obj">
        /// The object to virst.
        /// </param>
        public virtual void VisitPostOrder( T obj )
        {
            this.visitor.Visit( obj );
        }

        /// <summary>
        /// Visits the given object in order.
        /// </summary>
        /// <param name="obj">
        /// The object to virst.
        /// </param>
        public virtual void VisitInOrder( T obj )
        {
            this.visitor.Visit( obj );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The <see cref="IVisitor{T}"/> this OrderedVisitor{T} uses internally.
        /// </summary>
        private readonly IVisitor<T> visitor;

        #endregion
    }
}
