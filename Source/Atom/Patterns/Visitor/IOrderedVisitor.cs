// <copyright file="IOrderedVisitor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.IOrderedVisitor{T} interface.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Patterns.Visitor
{
    /// <summary>
    /// Represents a visitor that visits objects in order (PreOrder, PostOrder, or InOrder).
    /// </summary>
    /// <typeparam name="T">
    /// The type of items that can be visited by this IOrderedVisitor{T}.
    /// </typeparam>
    public interface IOrderedVisitor<in T>
    {
        /// <summary>
        /// Gets a value indicating whether this OrderedVisitor{T} is done visiting.
        /// </summary>
        /// <value>Whether the underlying <see cref="Visitor"/> has completed.</value>
        bool HasCompleted
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="IVisitor{T}"/> this OrderedVisitor{T} uses internally.
        /// </summary>
        /// <value>The <see cref="IVisitor{T}"/> this OrderedVisitor{T} uses internally.</value>
        IVisitor<T> Visitor
        {
            get;
        }

        /// <summary>
        /// Visits the given object in pre-order.
        /// </summary>
        /// <param name="obj">
        /// The object to virst.
        /// </param>
        void VisitPreOrder( T obj );

        /// <summary>
        /// Visits the given object in post-order.
        /// </summary>
        /// <param name="obj">
        /// The object to virst.
        /// </param>
        void VisitPostOrder( T obj );

        /// <summary>
        /// Visits the given object in order.
        /// </summary>
        /// <param name="obj">
        /// The object to virst.
        /// </param>
        void VisitInOrder( T obj );
    }
}
