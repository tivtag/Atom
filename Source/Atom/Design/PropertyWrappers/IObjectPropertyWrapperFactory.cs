// <copyright file="IObjectPropertyWrapperFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.IObjectPropertyWrapperFactory interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Design
{
    using System;

    /// <summary>
    /// Provides a mechanism that allows the central creation of <see cref="IObjectPropertyWrapper"/>s.
    /// </summary>
    // [Atom.Diagnostics.Contracts.ContractClass(typeof(IObjectPropertyWrapperFactoryContracts))]
    public interface IObjectPropertyWrapperFactory
    {
        /// <summary>
        /// Receives an <see cref="IObjectPropertyWrapper"/> for the given <see cref="Object"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the given <paramref name="obj"/> is null.
        /// </exception>
        /// <param name="obj">
        /// The object to receive an IObjectPropertyWrapper for.
        /// </param>
        /// <returns>
        /// The initialized IObjectPropertyWrapper for the given Object,
        /// or null if there exists no IObjectPropertyWrapper for the requested type.
        /// </returns>
        IObjectPropertyWrapper ReceiveWrapper( object obj );
        
        /// <summary>
        /// Receives an <see cref="IObjectPropertyWrapper"/> for the given <see cref="Object"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the given <paramref name="obj"/> is null.
        /// </exception>
        /// <param name="obj">
        /// The object to receive an IObjectPropertyWrapper for.
        /// </param>
        /// <returns>
        /// The initialized IObjectPropertyWrapper for the given Object,
        /// or the original object if there exists no IObjectPropertyWrapper for the given type.
        /// </returns>
        object ReceiveWrapperOrObject( object obj );

        /// <summary>
        /// Gets the types of the objects this IObjectPropertyWrapperFactory
        /// provides an <see cref="IObjectPropertyWrapper"/> for.
        /// </summary>
        /// <returns>A new array that contains the types.</returns>
        Type[] GetObjectTypes();

        /// <summary>
        /// Registers the given <see cref="IObjectPropertyWrapper"/> at this IObjectPropertyWrapperFactory.
        /// </summary>
        /// <param name="wrapper">
        /// The wrapper to register.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If the given <paramref name="wrapper"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the <paramref name="wrapper"/> already has been registered.
        /// </exception>
        void RegisterWrapper( IObjectPropertyWrapper wrapper );

        /// <summary>
        /// Unregisters the <see cref="IObjectPropertyWrapper"/> for the given <see cref="Type"/>
        /// from this IObjectPropertyWrapperFactory
        /// </summary>
        /// <param name="type">
        /// The type of the object the wrapper to unregister wraps around.
        /// </param>
        /// <returns>
        /// Returns true when the <see cref="IObjectPropertyWrapper"/> for the given <see cref="Type"/> has been removed;
        /// otherwise false.
        /// </returns>
        bool UnregisterWrapper( Type type );
    }
}
