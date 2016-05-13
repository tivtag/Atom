// <copyright file="IStringConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.IStringConverter interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;

    /// <summary>
    /// Provides a mechanism for converting values to and from strings.
    /// </summary>
    public interface IStringConverter
    {
        /// <summary>
        /// Attempts to convert the given source object into a string.
        /// </summary>
        /// <param name="value">
        /// The input source value.
        /// </param>
        /// <returns>
        /// The output value.
        /// </returns>
        string ConvertToString( object value );

        /// <summary>
        /// Attempts to convert the given target string value into
        /// the a source value.
        /// </summary>
        /// <param name="value">
        /// The input target value, encoded in a string.
        /// </param>
        /// <param name="targetType">
        /// The type the target value encodes.
        /// </param>
        /// <returns>
        /// The output source value.
        /// </returns>
        object ConvertFromString( string value, Type targetType );
    }
}
