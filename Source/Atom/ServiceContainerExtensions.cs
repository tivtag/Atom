// <copyright file="ServiceContainerExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ServiceContainerExtensions class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    using System.ComponentModel.Design;

    /// <summary>
    /// Defines extension methods for the <see cref="IServiceContainer"/> interface.
    /// </summary>
    public static class ServiceContainerExtensions
    {
        /// <summary>
        /// Adds the specified service to the IServiceContainer.
        /// </summary>
        /// <typeparam name="TService">
        /// The type to add the service under.
        /// </typeparam>
        /// <param name="container">
        /// The container to modify.
        /// </param>
        /// <param name="service">
        /// The service to add.
        /// </param>
        public static void AddService<TService>( this IServiceContainer container, TService service )
        {
            container.AddService( typeof( TService ), service );
        }
    }
}
