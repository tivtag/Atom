// <copyright file="CountingVisitor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.CountingVisitor{T} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Patterns.Visitor
{
    /// <summary>
    /// An <see cref="IVisitor{T}"/> that counts the items it visits.
    /// This class can't be inherited.
    /// </summary>
    /// <typeparam name="T">
    /// The type of items that can be visited by this visitor.
    /// </typeparam>
    public sealed class CountingVisitor<T> : IVisitor<T>
    {
        /// <summary>
        /// Gets a value indicating whether this CountingVisitor{T} is done performing its work.
        /// </summary>
        /// <value>
        /// Always return <see langword="false"/>.
        /// </value>
        public bool HasCompleted
        {
            get 
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the number of items that have been visited by this CountingVisitor{T}.
        /// </summary>
        /// <value>
        /// The number of items that have been visited.
        /// </value>
        public int Count
        {
            get 
            {
                return count; 
            }
        }

        /// <summary>
        /// Visits the specified object.
        /// </summary>
        /// <param name="obj">The object to visit.</param>
        public void Visit( T obj )
        {
            ++this.count;
        }

        /// <summary>
        /// Resets the number of items that have been
        /// visited by this CountingVisitor{T}.
        /// </summary>
        public void Reset()
        {
            this.count = 0;
        }

        /// <summary>
        /// The number of items that have been visited by this CountingVisitor{T}.
        /// </summary>
        private int count;
    }
}
