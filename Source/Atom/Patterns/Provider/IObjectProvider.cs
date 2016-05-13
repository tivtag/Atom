// <copyright file="IObjectProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.IObjectProvider{TObject} interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    /// <summary>
    /// Provides a simple mechanism for receiving an object of a specific type.
    /// </summary>
    /// <typeparam name="TObject">
    /// The type of the object this IObjectProvider{TObject} provides.
    /// </typeparam>
    public interface IObjectProvider<out TObject>
        where TObject : class
    {
        /// <summary>
        /// Attempts to get the object this IObjectProvider{TObject}
        /// provides.
        /// </summary>
        /// <returns>
        /// The object this IObjectProvider{TObject} provides;
        /// -or- null if this IObjectProvider{TObject} does not provide any object at the moment.
        /// </returns>
        TObject TryResolve();
    }
}
