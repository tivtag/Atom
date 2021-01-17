// <copyright file="ArrayExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.ArrayExtensions class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Defines extension methods for arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the elements of the specified arrays are equal; 
        /// using the default EqualityComparer{T}.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements the arrays contain.
        /// </typeparam>
        /// <param name="array">
        /// The first array.
        /// </param>
        /// <param name="otherArray">
        /// The second sequence.
        /// </param>
        /// <returns>
        /// true if the elements are equal;
        /// otherwise false.
        /// </returns>
        public static bool ElementsEqual<T>( this T[] array, T[] otherArray )
        {
            Contract.Requires<ArgumentNullException>( array != null );
            Contract.Requires<ArgumentNullException>( otherArray != null );

            return ElementsEqual<T>( array, otherArray, EqualityComparer<T>.Default );
        }

        /// <summary>
        /// Gets a value indicating whether the elements of the specified arrays are equal; 
        /// using the default EqualityComparer{T}.s
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements the arrays contain.
        /// </typeparam>
        /// <param name="array">
        /// The first array.
        /// </param>
        /// <param name="otherArray">
        /// The second sequence.
        /// </param>
        /// <param name="comparer">
        /// The IEqualityComparer{T} that should be used when comparing elements of the given arrays.
        /// </param>
        /// <returns>
        /// true if the elements are equal;
        /// otherwise false.
        /// </returns>
        public static bool ElementsEqual<T>( this T[] array, T[] otherArray, IEqualityComparer<T> comparer )
        {
            Contract.Requires<ArgumentNullException>( array != null );
            Contract.Requires<ArgumentNullException>( otherArray != null );
            Contract.Requires<ArgumentNullException>( comparer != null );

            if( array.Length != otherArray.Length )
                return false;

            for( int i = 0; i < array.Length; ++i )
            {
                if( !comparer.Equals( array[i], otherArray[i] ) )
                {
                    return false;
                }
            }

            return true;
        }
    }
}
