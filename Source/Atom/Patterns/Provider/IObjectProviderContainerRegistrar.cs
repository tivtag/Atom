// <copyright file="IObjectProviderContainerRegistrar.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Patterns.Provider.IObjectProviderContainerRegistrar interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Patterns.Provider
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Provides a mechanism for receiving and registering <see cref="IObjectProvider{TObject}"/> instances.
    /// </summary>
    // [ContractClass( typeof( IObjectProviderContainerRegistrarContract ) )]
    public interface IObjectProviderContainerRegistrar : IObjectProviderContainer
    {
        /// <summary>
        /// Registers the specified IObjectProvider for the specified <typeparamref name="TObject"/>
        /// </summary>
        /// <typeparam name="TObject">
        /// The type of object for which an IObjectProvider should be registered.
        /// </typeparam>
        /// <param name="provider">
        /// The provider to register.
        /// </param>
        void Register<TObject>( IObjectProvider<TObject> provider )
            where TObject : class;
    }

    /////// <summary>
    /////// Defines the contracts for the <see cref="IObjectProviderContainer"/> interface.
    /////// </summary>
    ////[ContractClassFor( typeof( IObjectProviderContainerRegistrar ) )]
    ////internal abstract class IObjectProviderContainerRegistrarContract : IObjectProviderContainerRegistrar
    ////{
    ////    IObjectProvider<object> IObjectProviderContainer.TryGetObjectProvider( Type type )
    ////    {
    ////        return default( IObjectProvider<object> );
    ////    }

    ////    void IObjectProviderContainerRegistrar.Register<TObject>( IObjectProvider<TObject> provider )
    ////    {
    ////        Contract.Requires<ArgumentNullException>( provider != null );
    ////    }
    ////}
}
