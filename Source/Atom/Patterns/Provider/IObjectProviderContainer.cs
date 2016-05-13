// <copyright file="IObjectProviderContainer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.IObjectProviderContainer interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;

    /// <summary>
    /// Provides a mechanism for receiving <see cref="IObjectProvider{TObject}"/> instances.
    /// </summary>
    public interface IObjectProviderContainer
    {
        /// <summary>
        /// Attempts to receive the IObjectProvider for the specified object type.
        /// </summary>
        /// <param name="type">
        /// The type of object for which an IObjectProvider should be requested.
        /// </param>
        /// <returns>
        /// The associated IObjectProvider; -or- null if no IObjectProvider has been registered
        /// at this IObjectProviderContainer for the specified <see cref="Type"/>.
        /// </returns>
        IObjectProvider<object> TryGetObjectProvider( Type type );
    }
}
