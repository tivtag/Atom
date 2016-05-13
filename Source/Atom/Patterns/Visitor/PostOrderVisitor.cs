// <copyright file="PostOrderVisitor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.PostOrderVisitor{T} class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Patterns.Visitor
{
    /// <summary>
    /// Represents an <see cref="IOrderedVisitor{T}"/> that redirects calls <see cref="IOrderedVisitor{T}.VisitPostOrder"/> calls
    /// to another <see cref="IVisitor{T}"/>.
    /// This class can't be inherited.
    /// </summary>
    /// <typeparam name="T">
    /// The type of items that can be visited by this visitor.
    /// </typeparam>
    public sealed class PostOrderVisitor<T> : OrderedVisitor<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostOrderVisitor{T}"/> class.
        /// </summary>
        /// <param name="visitor">
        /// The IVisitor{T} the new PostOrderVisitor{T} redirects calls to.
        /// </param>
        public PostOrderVisitor( IVisitor<T> visitor )
            : base( visitor )
        {
        }

        /// <summary>
        /// Has been overriden to do nothing.
        /// </summary>
        /// <param name="obj">
        /// The object to visit.
        /// </param>
        public override void VisitInOrder( T obj )
        {
        }

        /// <summary>
        /// Has been overriden to do nothing.
        /// </summary>
        /// <param name="obj">
        /// The object to visit.
        /// </param>
        public override void VisitPreOrder( T obj )
        {
        }
    }
}
