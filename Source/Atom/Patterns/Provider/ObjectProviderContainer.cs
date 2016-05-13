// <copyright file="ObjectProviderContainer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Patterns.Provider.ObjectProviderContainer class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Patterns.Provider
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements a mechanism for receiving and registering <see cref="IObjectProvider{TObject}"/> instances.
    /// </summary>
    public class ObjectProviderContainer : IObjectProviderContainerRegistrar
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
        public IObjectProvider<object> TryGetObjectProvider( Type type )
        {
            IObjectProvider<object> provider;
            this.map.TryGetValue( type, out provider );

            return provider;
        }

        /// <summary>
        /// Registers the specified IObjectProvider for the specified <typeparamref name="TObject"/>
        /// </summary>
        /// <typeparam name="TObject">
        /// The type of object for which an IObjectProvider should be registered.
        /// </typeparam>
        /// <param name="provider">
        /// The provider to register.
        /// </param>
        public void Register<TObject>( IObjectProvider<TObject> provider )
            where TObject : class
        {
            this.map[typeof( TObject )] = provider;
        }

        /// <summary>
        /// The dictionary that maps object types onto IObjectProviders.
        /// </summary>
        private readonly Dictionary<Type, IObjectProvider<object>> map = new Dictionary<Type, IObjectProvider<object>>();
    }
}
