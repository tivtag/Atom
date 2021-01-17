// <copyright file="IVisitor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.IVisitor{T} interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Patterns.Visitor
{
    /// <summary>
    /// The main interface of the visitor pattern - an <see cref="IVisitor{T}"/> gets called
    /// on all elements of a group of objects, doing defined work. 
    /// A simple example would be the summing up of all intagers in a list of intagers
    /// by visiting all intagers in the list.
    /// </summary>
    /// <remarks>
    /// The framework defines multiple enhanced versions of the build-in collections
    /// to enable support to the Visitor pattern.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of items visitable by the visitor.
    /// </typeparam>
    public interface IVisitor<in T>
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IVisitor{T}"/> instance is done performing its work.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if this instance is done;
        /// otherwise, <see langword="false"/>.
        /// </value>
        bool HasCompleted
        {
            get;
        }

        /// <summary>
        /// Visits the specified object.
        /// </summary>
        /// <param name="obj">
        /// The object to visit.
        /// </param>
        void Visit( T obj );
    }
}
