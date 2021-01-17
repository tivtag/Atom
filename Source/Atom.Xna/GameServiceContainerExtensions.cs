// <copyright file="GameServiceContainerExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.GameServiceContainerExtensions class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines extension methods for the <see cref="GameServiceContainer"/> class.
    /// </summary>
    public static class GameServiceContainerExtensions
    {
        /// <summary>
        /// Adds a service to the GameServiceContainer.
        /// </summary>
        /// <typeparam name="TService">
        /// The type of service to add.
        /// </typeparam>
        /// <param name="container">
        /// The container to which the service should be added.
        /// </param>
        /// <param name="service">
        /// The service provider to add.
        /// </param>
        public static void AddService<TService>( this GameServiceContainer container, TService service )
        {
            container.AddService( typeof( TService ), service );
        }
    }
}