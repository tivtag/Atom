// <copyright file="EmptyVisitor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.EmptyVisitor{T} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Patterns.Visitor
{
    /// <summary>
    /// Represents an <see cref="IVisitor{T}"/> that does absolutely nothing.
    /// This class can't be inherited.
    /// </summary>
    /// <typeparam name="T">
    /// The type of items visitable by the visitor.
    /// </typeparam>
    /// <remarks>
    /// The <see cref="Instance"/> property can be used to avoid creation.
    /// </remarks>
    public sealed class EmptyVisitor<T> : IVisitor<T>
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="EmptyVisitor{T}"/> class.
        /// </summary>
        public static EmptyVisitor<T> Instance
        {
            get
            {
                return EmptyVisitor<T>.instance; 
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyVisitor{T}"/> class.
        /// </summary>
        public EmptyVisitor()
        {
        }

        /// <summary>
        /// Gets a value indicating whether this IVisitor{T} is done performing it's work..
        /// </summary>
        /// <value>
        /// Always returns <see langword="false"/>.
        /// </value>
        public bool HasCompleted
        {
            get
            { 
                return true;
            }
        }

        /// <summary>
        /// Visits the specified object.
        /// </summary>
        /// <param name="obj">The object to visit.</param>
        public void Visit( T obj )
        {
        }

        /// <summary>
        /// The generic static instance of the <see cref="EmptyVisitor{T}"/> class.
        /// </summary>
        private static readonly EmptyVisitor<T> instance = new EmptyVisitor<T>();
    }
}
