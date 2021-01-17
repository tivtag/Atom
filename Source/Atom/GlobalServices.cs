// <copyright file="GlobalServices.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.GlobalServices interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    using System;
    using System.ComponentModel.Design;
    using Atom.Diagnostics.Contracts;
    using Atom.Diagnostics;
    
    /// <summary>
    /// Provides access to globally accessable services.
    /// </summary>
    public static class GlobalServices
    {
        /// <summary>
        /// Gets or sets the object which stores the globally accessable services.
        /// </summary>
        /// <value>A reference to the ServiceContainer object managed by the GlobalServices class.</value>
        public static ServiceContainer Container
        {
            get
            {
                // Contract.Ensures( Contract.Result<ServiceContainer>() != null );

                return container;
            }

            set
            {
                Contract.Requires<ArgumentNullException>( value != null );

                container = value;
            }
        }

        /// <summary>
        /// Gets the requested service.
        /// </summary>
        /// <typeparam name="TService">
        /// The type of the service to receive.
        /// </typeparam>
        /// <exception cref="ServiceNotFoundException">
        /// If the service could not be found.
        /// </exception>
        /// <returns>
        /// The requested service.
        /// </returns>
        public static TService GetService<TService>()
            where TService : class
        {
            // Contract.Ensures( Contract.Result<TService>() != null );

            var service = TryGetService<TService>();
            ThrowHelper.IfServiceNull<TService>( service );
            return service;
        }

        /// <summary>
        /// Gets the requested service.
        /// </summary>
        /// <typeparam name="TService">
        /// The type of the service to receive.
        /// </typeparam>
        /// <exception cref="ServiceNotFoundException">
        /// If the service could not be found.
        /// </exception>
        /// <param name="errorMessage">
        /// The error message to display when the service could not be found.
        /// </param>
        /// <returns>
        /// The requested service.
        /// </returns>
        public static TService GetService<TService>( string errorMessage )
            where TService : class
        {
            // Contract.Ensures( Contract.Result<TService>() != null );

            var service = TryGetService<TService>();
            ThrowHelper.IfServiceNull<TService>( service, errorMessage );
            return service;
        }

        /// <summary>
        /// Tries to get the requested service.
        /// </summary>
        /// <param name="service">The type of the service to get.</param>
        /// <returns>
        /// An instance of the service if it could be found, 
        /// or null if it could not be found.
        /// </returns>
        public static object TryGetService( Type service )
        {
            return container.GetService( service );
        }

        /// <summary>
        /// Tries to get the requested service.
        /// </summary>
        /// <typeparam name="TService">
        /// The type of the service to receive.
        /// </typeparam>
        /// <returns>
        /// An instance of the service if it could be found, 
        /// or null if it could not be found.
        /// </returns>
        public static TService TryGetService<TService>()
            where TService : class
        {
            return container.GetService( typeof( TService ) ) as TService;
        }

        /// <summary>
        /// Helpers method that tries to find an <see cref="Atom.Diagnostics.ILogProvider"/>
        /// to log the given <paramref name="message"/>.
        /// </summary>
        /// <param name="message">
        /// The message to log.
        /// </param>
        public static void TryLog( string message )
        {
            var logService = TryGetService<ILogProvider>();

            if( logService != null )
            {
                var log = logService.Log;

                if( log != null )
                {
                    log.WriteLine( message );
                }
            }
        }

        /// <summary>
        /// The container object.
        /// </summary>
        private static ServiceContainer container = new ServiceContainer();
    }
}
