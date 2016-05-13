// <copyright file="ServiceProviderExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ServiceProviderExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;

    /// <summary>
    /// Defines extension methods for the <see cref="IServiceProvider"/> interface.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Gets the service object of the specified type <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">
        /// The type of the service to receive.
        /// </typeparam>
        /// <param name="serviceProvider">
        /// The <see cref="IServiceProvider"/> from which the service should be received.
        /// </param>
        /// <returns>
        /// A service object of type serviceType. -or-
        /// null if there is no service object of type serviceType.
        /// </returns>
        public static TService GetService<TService>( this IServiceProvider serviceProvider )
            where TService : class
        {
            return serviceProvider.GetService( typeof( TService ) ) as TService;
        }
    }
}
