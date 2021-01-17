// <copyright file="IConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.IConverter{T1, T2} interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    /// <summary>
    /// Provides a mechanism for converting values between two types.
    /// </summary>
    /// <typeparam name="Source">
    /// The first type.
    /// </typeparam>
    /// <typeparam name="Target">
    /// The second type.
    /// </typeparam>
    public interface IConverter<Source, Target>
    {
        /// <summary>
        /// Attempts to convert the given <typeparamref name="Source"/> value into
        /// the <typeparamref name="Target"/> value.
        /// </summary>
        /// <param name="value">
        /// The input <typeparamref name="Source"/> value.
        /// </param>
        /// <returns>
        /// The output <typeparamref name="Target"/> value.
        /// </returns>
        Target ConvertTo( Source value );

        /// <summary>
        /// Attempts to convert the given <typeparamref name="Target"/> value into
        /// the <typeparamref name="Source"/> value.
        /// </summary>
        /// <param name="value">
        /// The input <typeparamref name="Target"/> value.
        /// </param>
        /// <returns>
        /// The output <typeparamref name="Source"/> value.
        /// </returns>
        Source ConvertFrom( Target value );
    }
}
