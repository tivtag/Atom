// <copyright file="ArrayUtilities.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.ArrayUtilities class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Provides static array-related utility methods.
    /// </summary>
    public static class ArrayUtilities
    {
        /// <summary>
        /// Creates a multi-dimensional array that contains the same elements as the 
        /// given an one-dimensional array of elements and the size of the array to create.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements the arrays contain.
        /// </typeparam>
        /// <param name="rows">
        /// The number of rows the new multi-dimensional array should have.
        /// </param>
        /// <param name="columns">
        /// The number of columns the new multi-dimensional array should have.
        /// </param>
        /// <param name="elements">
        /// The values that should be copied over into the new multi dimensional array.
        /// </param>
        /// <returns>
        /// The newly created multi-dimensional array.
        /// </returns>
        public static T[,] CreateMultiDimensional<T>( int rows, int columns, T[] elements )
        {
            Contract.Requires<ArgumentException>( (columns * rows) == elements.Length );
            Contract.Requires<ArgumentNullException>( elements != null );

            // Contract.Ensures( Contract.Result<T[,]>() != null );
            // Contract.Ensures( Contract.Result<T[,]>().GetLowerBound( 0 ) == 0 );
            // Contract.Ensures( Contract.Result<T[,]>().GetUpperBound( 0 ) == (rows - 1) );
            // Contract.Ensures( Contract.Result<T[,]>().GetLowerBound( 1 ) == 0 );
            // Contract.Ensures( Contract.Result<T[,]>().GetUpperBound( 1 ) == (columns - 1) );

            var multiArray = new T[rows, columns];

            for( int row = 0; row < rows; ++row )
            {
                for( int column = 0; column < columns; ++column )
                {
                    int index = (row * columns) + column;
                    T element = elements[index];

                    multiArray[row, column] = element;
                }
            }

            return multiArray;
        }

        /// <summary>
        /// Removes the array element at the given <paramref name="index"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements the arrays contain.
        /// </typeparam>
        /// <param name="array">
        /// The array to manipulate.
        /// </param>
        /// <param name="index">
        /// The zero-based index of the element to remove.
        /// </param>
        public static void RemoveAt<T>( ref T[] array, int index )
        {
            Contract.Requires<ArgumentNullException>( array != null );
            Contract.Requires<ArgumentOutOfRangeException>( index >= 0 );
            Contract.Requires<ArgumentOutOfRangeException>( index < array.Length );
            
            T[] original = (T[])array.Clone();
            Array.Resize( ref array, array.Length - 1 );

            for( int i = 0; i < index; ++i )
            {
                array[i] = original[i];
            }

            for( int i = index; i < array.Length; ++i )
            {
                array[i] = original[i + 1];
            }
        }

        /// <summary>
        /// Insters an <paramref name="item"/> at the given zero-based <paramref name="index"/> into the given <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements the arrays contain.
        /// </typeparam>
        /// <param name="array">
        /// The array to manipulate.
        /// </param>
        /// <param name="index">
        /// The zero-based index into the array.
        /// </param>
        /// <param name="item">
        /// The item to insert.
        /// </param>
        public static void InsertAt<T>( ref T[] array, int index, T item )
        {
            Contract.Requires<ArgumentNullException>( array != null );
            Contract.Requires<ArgumentOutOfRangeException>( index >= 0 );
            Contract.Requires<ArgumentOutOfRangeException>( index < array.Length );

            Array.Resize( ref array, array.Length + 1 );

            T last = array[index];
            array[index] = item;

            for( int i = index + 1; i < array.Length; ++i )
            {
                T next = array[i];
                array[i] = last;
                last = next;
            }
        }
    }
}
