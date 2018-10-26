// <copyright file="ObjectProviderExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ObjectProviderExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Patterns.Provider;

    /// <summary>
    /// Defines extension methods for the <see cref="IObjectProvider{TObject}"/>,
    /// <see cref="IObjectProviderContainer"/> and <see cref="IObjectProviderContainerRegistrar"/> interfacse.
    /// </summary>
    public static class ObjectProviderExtensions
    {
        /// <summary>
        /// Attempts to receive the IObjectProvider for the specified object type.
        /// </summary>
        /// <typeparam name="TObject">
        /// The type of object for which an IObjectProvider should be requested.
        /// </typeparam>
        /// <returns>
        /// The associated IObjectProvider; -or- null if no IObjectProvider has been registered
        /// at this IObjectProviderContainer for the specified <typeparamref name="TObject"/>.
        /// </returns>
        public static IObjectProvider<TObject> TryGetObjectProvider<TObject>( this IObjectProviderContainer container )
            where TObject : class
        {
            return (IObjectProvider<TObject>)container.TryGetObjectProvider( typeof( TObject ) );
        }

        /// <summary>
        /// Gets the object this IObjectProvider{TObject}.
        /// </summary>
        /// <exception cref="ServiceNotFoundException">
        /// If no object could be resolved.
        /// </exception>
        /// <param name="provider">
        /// The IObjectProvider to resolve the object on.
        /// </param>
        /// <returns>
        /// The object this IObjectProvider{TObject} provides.
        /// </returns>
        public static TObject Resolve<TObject>( this IObjectProvider<TObject> provider )
            where TObject : class
        {
            Contract.Requires<ArgumentNullException>( provider != null );

            TObject obj = provider.TryResolve();
            ThrowHelper.IfServiceNull( obj );

            return obj;            
        }

        /// <summary>
        /// Gets the IObjectProvider for the specified object type.
        /// </summary>
        /// <exception cref="ServiceNotFoundException">
        /// If no IObjectProvider{TObject} has been registered.
        /// </exception>
        /// <typeparam name="TObject">
        /// The type of object for which an IObjectProvider should be requested.
        /// </typeparam>
        /// <returns>
        /// The associated IObjectProvider.
        /// </returns>
        public static IObjectProvider<TObject> GetObjectProvider<TObject>( this IObjectProviderContainer container )
            where TObject : class
        {
            Contract.Requires<ArgumentNullException>( container != null );

            var provider = container.TryGetObjectProvider<TObject>();
            ThrowHelper.IfServiceNull( provider );

            return provider;
        }

        /// <summary>
        /// Attempts to directly resolve the object of the specified type.
        /// </summary>
        /// <exception cref="ServiceNotFoundException">
        /// If no IObjectProvider{TObject} could be found or if the IObjectProvider{TObject} didn't return an object.
        /// </exception>
        /// <typeparam name="TObject">
        /// The type of the object to resolve.
        /// </typeparam>
        /// <param name="container">
        /// The IObjectProviderContainer that contains the IObjectProvider that resolves the object.
        /// </param>
        /// <returns>
        /// The requested object.
        /// </returns>
        public static TObject Resolve<TObject>( this IObjectProviderContainer container )
            where TObject : class
        {
            Contract.Requires<ArgumentNullException>( container != null );

            var provider = container.GetObjectProvider<TObject>();
            ThrowHelper.IfServiceNull( provider );

            TObject obj = provider.TryResolve();
            ThrowHelper.IfServiceNull( obj );

            return obj;
        }

        /// <summary>
        /// Attempts to directly resolve the object of the specified type.
        /// </summary>
        /// <param name="container">
        /// The IObjectProviderContainer that contains the IObjectProvider that resolves the object.
        /// </param>
        /// <param name="type">
        /// The type of the object to resolve.
        /// </param>
        /// <returns>
        /// The requested object
        /// -or- null if the object could not be resolved.
        /// </returns>
        public static object TryResolve( this IObjectProviderContainer container, Type type )
        {
            Contract.Requires<ArgumentNullException>( container != null );

            var provider = container.TryGetObjectProvider( type );
            if( provider == null )
                return null;

            return provider.TryResolve();
        }

        /// <summary>
        /// Attempts to directly resolve the object of the specified type.
        /// </summary>
        /// <typeparam name="TObject">
        /// The type of the object to resolve.
        /// </typeparam>
        /// <param name="container">
        /// The IObjectProviderContainer that contains the IObjectProvider that resolves the object.
        /// </param>
        /// <returns>
        /// The requested object
        /// -or- null if the object could not be resolved.
        /// </returns>
        public static TObject TryResolve<TObject>( this IObjectProviderContainer container )
            where TObject : class
        {
            return container.TryResolve( typeof( TObject ) ) as TObject;
        }

        /// <summary>
        /// Registers a new LambdaObjectProvider{TObject} at this IObjectProviderContainerRegistrar that
        /// uses the specified lambda to resolve the object.
        /// </summary>
        /// <typeparam name="TObject">
        /// The type of object for which an IObjectProvider should be registered.
        /// </typeparam>
        /// <param name="container">
        /// The IObjectProviderContainerRegistrar that stores the IObjectProviders.
        /// </param>
        /// <param name="lambda">
        /// The lambda to register.
        /// </param>
        public static void Register<TObject>( this IObjectProviderContainerRegistrar container, Func<TObject> lambda )
            where TObject : class
        {
            Contract.Requires<ArgumentNullException>( container != null );
            Contract.Requires<ArgumentNullException>( lambda != null );

            container.Register<TObject>( new LambdaObjectProvider<TObject>( lambda ) );
        }

        /// <summary>
        /// Registers a new LambdaObjectProvider{TObject} at this IObjectProviderContainerRegistrar that
        /// uses the specified lambda to resolve the object.
        /// </summary>
        /// <typeparam name="TObject">
        /// The type of object for which an IObjectProvider should be registered.
        /// </typeparam>
        /// <param name="container">
        /// The IObjectProviderContainerRegistrar that stores the IObjectProviders.
        /// </param>
        /// <param name="lambda">
        /// The lambda to register.
        /// </param>
        public static void Register<TObject>( 
            this IObjectProviderContainerRegistrar container,
            Func<IObjectProviderContainer, TObject> lambda )
                where TObject : class
        {
            Contract.Requires<ArgumentNullException>( container != null );
            Contract.Requires<ArgumentNullException>( lambda != null );

            container.Register<TObject>( new LambdaObjectProvider<TObject>( lambda, container ) );
        }
    }
}
