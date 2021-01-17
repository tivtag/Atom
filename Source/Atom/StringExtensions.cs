// <copyright file="StringExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.StringExtensions class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    using System;

    /// <summary>
    /// Defines extension methods for the <see cref="String"/> class.
    /// </summary>
    public static class StringExtensions
    {    
        /// <summary>
        /// Returns a value indicating whether the specified System.String object occurs within this string,
        /// using the specified <see cref="StringComparison"/>.
        /// </summary>
        /// <param name="this">
        /// The original string to operate on.
        /// </param>
        /// <param name="value">
        /// The string to seek.
        /// </param>
        /// <param name="comparisonType">
        /// Specifies the rule of the search.
        /// </param>
        /// <returns>
        /// true if the value parameter occurs within this string,
        /// or if value is the empty string (""); otherwise, false.
        /// </returns>
        public static bool Contains(this string @this, string value, StringComparison comparisonType)
        {
            return @this.IndexOf(value, comparisonType) >= 0;
        }
    }
}