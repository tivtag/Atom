// <copyright file="DirectObjectProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Patterns.Provider.DirectObjectProvider class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Patterns.Provider
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents an <see cref="IObjectProvider{TObject}"/> that provides an object
    /// specified upon construction.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public sealed class DirectObjectProvider<TObject> : IObjectProvider<TObject>
        where TObject : class
    {
        /// <summary>
        /// Initializes a new instance of the DirectObjectProvider{TObject} class.
        /// </summary>
        /// <param name="obj">
        /// The object the new DirectObjectProvider{TObject} provides.
        /// </param>
        public DirectObjectProvider( TObject obj )
        {
            Contract.Requires<ArgumentNullException>( obj != null );

            this.obj = obj;
        }

        /// <summary>
        /// Attempts to get the object this IObjectProvider{TObject}
        /// provides.
        /// </summary>
        /// <returns>
        /// The object this IObjectProvider{TObject} provides;
        /// -or- null if this IObjectProvider{TObject} does not provide any object at the moment.
        /// </returns>
        public TObject TryResolve()
        {
            return this.obj;
        }

        /// <summary>
        /// The object this DirectObjectProvider{TObject} provides.
        /// </summary>
        private readonly TObject obj;
    }
}
