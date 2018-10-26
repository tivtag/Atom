////// <copyright file="ICultureSensitiveToStringProvider.Contract.cs" company="federrot Software">
//////     Copyright (c) federrot Software. All rights reserved.
////// </copyright>
////// <summary>
//////     Defines the Atom.Contracts.ICultureSensitiveToStringProviderContract class.
////// </summary>
////// <author>
//////     Paul Ennemoser (Tick)
////// </author>

////namespace Atom
////{
////    using System;
////    using Atom.Diagnostics.Contracts;

////    /// <summary>
////    /// Defines the contracts for the ICultureSensitiveToStringProvider interface.
////    /// </summary>
////    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
////    [ContractClassFor( typeof( ICultureSensitiveToStringProvider ) )]
////    internal abstract class ICultureSensitiveToStringProviderContract : ICultureSensitiveToStringProvider
////    {
////        /// <summary>
////        /// Returns a human-readable version of the object
////        /// using culture sensitive formatting.
////        /// </summary>
////        /// <param name="formatProvider">
////        /// Provides access to culture sensitive formatting information.
////        /// </param>
////        /// <returns>
////        /// A string that represents this object.
////        /// </returns>
////        [Pure]
////        string ICultureSensitiveToStringProvider.ToString( IFormatProvider formatProvider )
////        {
////            Contract.Requires( formatProvider != null );
////            Contract.Ensures( Contract.Result<string>() != null );

////            return default( string );
////        }
////    }
////}
