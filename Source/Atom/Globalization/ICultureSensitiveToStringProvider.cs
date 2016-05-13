// <copyright file="ICultureSensitiveToStringProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ICultureSensitiveToStringProvider interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides a mechanism that allows to receive a
    /// culture-sensitive string representation of the object.
    /// </summary>
    [ContractClass( typeof( ICultureSensitiveToStringProviderContract ) )]
    public interface ICultureSensitiveToStringProvider
    {
        /// <summary>
        /// Returns a human-readable version of the object
        /// using culture sensitive formatting.
        /// </summary>
        /// <param name="formatProvider">
        /// Provides access to culture sensitive formatting information.
        /// </param>
        /// <returns>
        /// A string that represents this object.
        /// </returns>
        string ToString( System.IFormatProvider formatProvider );
    }
}
