// <copyright file="InOrderVisitor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.InOrderVisitor{T} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Patterns.Visitor
{
    /// <summary>
    /// Represents an <see cref="IOrderedVisitor{T}"/> that redirects calls <see cref="IOrderedVisitor{T}.VisitInOrder"/> calls
    /// to another <see cref="IVisitor{T}"/>.
    /// This class can't be inherited.
    /// </summary>
    /// <typeparam name="T"> The type of items that can be visited by this visitor. </typeparam>
    public sealed class InOrderVisitor<T> : OrderedVisitor<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InOrderVisitor{T}"/> class.
        /// </summary>
        /// <param name="visitor">
        /// The IVisitor{T} the new InOrderVisitor{T} redirects calls to.
        /// </param>
        public InOrderVisitor( IVisitor<T> visitor )
            : base( visitor )
        {
        }

        /// <summary>
        /// Has been overriden to do nothing.
        /// </summary>
        /// <param name="obj">
        /// The object to visit.
        /// </param>
        public override void VisitPostOrder( T obj )
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
