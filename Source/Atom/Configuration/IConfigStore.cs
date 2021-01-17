// <copyright file="IConfigStore.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Configuration.IConfigStore interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Configuration
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Manages saving and loading of configuration properties.
    /// </summary>
    // [ContractClass( typeof( IConfigStoreContracts ) )]
    public interface IConfigStore
    {
        /// <summary>
        /// Gets the properties that have been saved, by loading them from this IConfigStore.
        /// </summary>
        /// <returns>
        /// The properties that this IConfigStore contains; where the first strings
        /// represents the name of the property and the second string the value of the property.
        /// </returns>
        IEnumerable<Tuple<string, string>> Load();
        
        /// <summary>
        /// Saves the specified properties to this IConfigStore.
        /// </summary>
        /// <param name="properties">
        /// The properties to save in this IConfigStore; where the first string
        /// represents the name of the property and the second string the value of the property.
        /// </param>
        void Save( IEnumerable<Tuple<string, string, IConfigPropertyAttribute>> properties );
    }

    /////// <summary>
    /////// Defines the code contracts for the <see cref="IConfigStore"/> interface.
    /////// </summary>
    ////[ContractClassFor(typeof(IConfigStore))]
    ////internal abstract class IConfigStoreContracts : IConfigStore
    ////{
    ////    IEnumerable<Tuple<string, string>> IConfigStore.Load()
    ////    {
    ////        Contract.Ensures( Contract.Result<IEnumerable<Tuple<string, string>>>() != null );

    ////        return default( IEnumerable<Tuple<string, string>> );
    ////    }

    ////    void IConfigStore.Save( IEnumerable<Tuple<string, string, IConfigPropertyAttribute>> properties )
    ////    {
    ////        Contract.Requires<ArgumentNullException>( properties != null );
    ////    }
    ////}
}
