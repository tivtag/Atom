// <copyright file="TrackingVisitor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.TrackingVisitor{T} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Patterns.Visitor
{
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// A visitor that tracks (stores) objects in the order they were visited.
    /// Handy for demonstrating and testing different ordered visits implementations on
    /// data structures. This class can't be inherited.
    /// </summary>
    /// <typeparam name="T">
    /// The type of objects to be visited.
    /// </typeparam>
    public sealed class TrackingVisitor<T> : IVisitor<T>
    {
        #region [ Properties ]

        /// <summary>
        /// Gets a value indicating whether this instance is done performing its work.
        /// </summary>
        /// <value>
        /// Always returns <see langword="false"/>.
        /// </value>
        public bool HasCompleted
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the reference of the list that contains the objects
        /// this <see cref="TrackingVisitor{T}"/> has visisted.
        /// </summary>
        /// <value>The tracking list.</value>        
        public IList<T> TrackingList
        {
            get
            {
                // Contract.Ensures( Contract.Result<IList<T>>() != null );

                return this.tracks;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingVisitor{T}"/> class.
        /// </summary>
        public TrackingVisitor()
        {
            this.tracks = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingVisitor{T}"/> class.
        /// </summary>
        /// <param name="initialCapacity">
        /// The initial number of elements the new <see cref="TrackingVisitor{T}"/> can contain.
        /// </param>
        public TrackingVisitor( int initialCapacity )
        {
            this.tracks = new List<T>( initialCapacity );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Visits the specified object.
        /// </summary>
        /// <param name="obj">
        /// The object to visit.
        /// </param>
        public void Visit( T obj )
        {
            // Contract.Ensures( this.TrackingList.Count == ( 1 + Contract.OldValue( this.TrackingList.Count ) ) );
            // Contract.Ensures( this.TrackingList.Contains( obj ) );

            this.tracks.Add( obj );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The list that is internally used by this TrackingVisitor{T} 
        /// to keep track of the items that have been visited.
        /// </summary>
        private readonly List<T> tracks;

        #endregion
    }
}
