// <copyright file="IConfigPropertySearcher.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Configuration.IConfigPropertySearcher interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Provides a mechanism that finds all configuration properties of a type.
    /// </summary>
    // [ContractClass( typeof( IConfigPropertySearcherContracts ) )]
    public interface IConfigPropertySearcher
    {
        /// <summary>
        /// Finds the configuation properties of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// The type to query.
        /// </param>
        /// <returns>
        /// The properties that have been found.
        /// </returns>
        IEnumerable<Tuple<PropertyInfo, IConfigPropertyAttribute>> Search( Type type );
    }

    /////// <summary>
    /////// Defines the contracts for the IConfigPropertySearcher interface.
    /////// </summary>
    ////[ContractClassFor(typeof(IConfigPropertySearcher))]
    ////internal abstract class IConfigPropertySearcherContracts : IConfigPropertySearcher
    ////{
    ////    IEnumerable<Tuple<PropertyInfo, IConfigPropertyAttribute>> IConfigPropertySearcher.Search( Type type )
    ////    {
    ////        Contract.Requires( type != null );
    ////        Contract.Ensures( Contract.Result<IEnumerable<Tuple<PropertyInfo, IConfigPropertyAttribute>>>() != null );

    ////        return default( IEnumerable<Tuple<PropertyInfo, IConfigPropertyAttribute>> );
    ////    }
    ////}
}
