// <copyright file="RandExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.RandExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides extension methods for the <see cref="IRand"/> interface.
    /// </summary>
    public static class RandExtensions
    {
        /// <summary>
        /// Randomly shuffles the specified IList{T}.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements in the list.
        /// </typeparam>
        /// <param name="list">
        /// The list to shuffle.
        /// </param>
        /// <param name="rand">
        /// The random number generator to use.
        /// </param>
        public static void Shuffle<T>( this IList<T> list, IRand rand )
        {
            int n = list.Count;

            while( n > 1 )
            {
                --n;
                int k = rand.UncheckedRandomRange( 0, n );
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Randomly selects one of the specified weighted items;
        /// where a greater weight increases the chance for an item to be picked.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the items to select from.
        /// </typeparam>
        /// <param name="rand">
        /// The random number generator to use.
        /// </param>
        /// <param name="weightedItems">
        /// The weighted items. The weights don't have to add up to any value.
        /// </param>
        /// <returns>
        /// The item that has been selected;
        /// or the default value of T if none has been selected.
        /// </returns>
        /// <example>
        ///    var titles = new Tuple&lt;float, string&gt;[] {
        ///        Tuple.Create( 70.0f, "Link" ),
        ///        Tuple.Create( 30.0f, "Garret" )
        ///    };
        ///    
        ///    string hero = rand.Select( titles );
        ///    
        ///    // 70% chance for hero to be "Link"
        ///    // 30% chance for hero to be "Garret"
        /// </example>
        public static T Select<T>( this IRand rand, IEnumerable<Tuple<float, T>> weightedItems )
        {
            Contract.Requires<ArgumentNullException>( rand != null );
            Contract.Requires<ArgumentNullException>( weightedItems != null );

            float totalWeight = weightedItems.Select( item => item.Item1 ).Sum();
            float randomWeight = rand.RandomRange( 0.0f, totalWeight );

            float cursor = 0.0f;

            foreach( var weightedItem in weightedItems )
            {
                float weight = weightedItem.Item1;
                float cursorEnd = cursor + weight;

                if( cursor <= randomWeight && randomWeight <= cursorEnd )
                {
                    return weightedItem.Item2;
                }

                cursor = cursorEnd;
            }

            return default( T );
        }

        /// <summary>
        /// Gets a random <see cref="Direction4"/>.
        /// </summary>
        /// <param name="rand">
        /// The random number generator to use.
        /// </param>
        /// <returns>
        /// The random <see cref="Direction4"/>, each direction has the same chance to be picked.
        /// </returns>
        public static Direction4 RandomDirection4( this IRand rand )
        {
            return (Direction4)rand.RandomRange( 0, 4 );
        }

        /// <summary>
        /// Gets a random <see cref="Direction4"/> that is not the given <paramref name="directionToExclude"/>.
        /// </summary>
        /// <param name="rand">
        /// The random number generator to use.
        /// </param>
        /// <param name="directionToExclude">
        /// The <see cref="Direction4"/> that should not be picked.
        /// </param>
        /// <returns>
        /// The random <see cref="Direction4"/>, each direction has the same chance to be picked.
        /// </returns>
        public static Direction4 RandomDirection4But( this IRand rand, Direction4 directionToExclude )
        {
            Direction4 direction = (Direction4)rand.RandomRange( 0, 4 );

            if( direction == directionToExclude )
                return rand.RandomDirection4But( directionToExclude );

            return direction;
        }

        /// <summary>
        /// Gets a random <see cref="Direction4"/> that is not the given <paramref name="directionToExclude"/>
        /// nor <see cref="Direction4.None"/>.
        /// </summary>
        /// <param name="rand">
        /// The random number generator to use.
        /// </param>
        /// <param name="directionToExclude">
        /// The <see cref="Direction4"/> that should not be picked.
        /// </param>
        /// <returns>
        /// The random <see cref="Direction4"/>, each direction has the same chance to be picked.
        /// </returns>
        public static Direction4 RandomActualDirection4But( this IRand rand, Direction4 directionToExclude )
        {
            Direction4 direction = (Direction4)rand.RandomRange( 1, 4 );

            if( direction == directionToExclude )
            {
                return rand.RandomActualDirection4But( directionToExclude );
            }

            return direction;
        }

        /// <summary>
        /// Gets a random <see cref="Direction8"/>.
        /// </summary>
        /// <param name="rand">
        /// The random number generator to use.
        /// </param>
        /// <returns>
        /// The random <see cref="Direction8"/>, each direction has the same chance to be picked.
        /// </returns>
        public static Direction8 RandomDirection8( this IRand rand )
        {
            return (Direction8)rand.RandomRange( 0, 7 );
        }

        /// <summary>
        /// Gets a random <see cref="Direction8"/> that is not the given <paramref name="directionToExclude"/>.
        /// </summary>
        /// <param name="rand">
        /// The random number generator to use.
        /// </param>
        /// <param name="directionToExclude">
        /// The <see cref="Direction4"/> that should not be picked.
        /// </param>
        /// <returns>
        /// The random <see cref="Direction4"/>, each direction has the same chance to be picked.
        /// </returns>
        public static Direction8 RandomDirection8But( this IRand rand, Direction8 directionToExclude )
        {
            Direction8 direction = (Direction8)rand.RandomRange( 0, 7 );

            if( direction == directionToExclude )
                return rand.RandomDirection8But( directionToExclude );

            return direction;
        }

        /// <summary>
        /// Gets a random <see cref="Direction8"/> that is not the given <paramref name="directionToExclude"/> nor <see cref="Direction8.None"/>.
        /// </summary>
        /// <param name="rand">
        /// The random number generator to use.
        /// </param>
        /// <param name="directionToExclude">
        /// The <see cref="Direction4"/> that should not be picked.
        /// </param>
        /// <returns>
        /// The random <see cref="Direction4"/>, each direction has the same chance to be picked.
        /// </returns>
        public static Direction8 RandomActualDirection8But( this IRand rand, Direction8 directionToExclude )
        {
            Direction8 direction = (Direction8)rand.RandomRange( 1, 7 );

            if( direction == directionToExclude )
                return rand.RandomActualDirection8But( directionToExclude );

            return direction;
        }

        /// <summary>
        /// Returns a random Vector2 value within the specified range.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="minimumValue"> The lower bound. </param>
        /// <param name="maximumValue"> The upper bound. </param>
        /// <returns>
        /// A random Vector2 greater than or equal to <c>minimumValue</c>, and less than
        /// or equal to <c>maximumValue</c>. 
        /// </returns>
        public static Vector2 RandomRange( this IRand rand, Vector2 minimumValue, Vector2 maximumValue )
        {
            Vector2 result;

            result.X = rand.RandomRange( minimumValue.X, maximumValue.X );
            result.Y = rand.RandomRange( minimumValue.Y, maximumValue.Y );

            return result;
        }

        /// <summary>
        /// Returns a random Vector3 value within the specified range.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="minimumValue"> The lower bound. </param>
        /// <param name="maximumValue"> The upper bound. </param>
        /// <returns>
        /// A random Vector3 greater than or equal to <c>minimumValue</c>, and less than
        /// or equal to <c>maximumValue</c>. 
        /// </returns>
        public static Vector3 RandomRange( this IRand rand, Vector3 minimumValue, Vector3 maximumValue )
        {
            Vector3 result;

            result.X = rand.RandomRange( minimumValue.X, maximumValue.X );
            result.Y = rand.RandomRange( minimumValue.Y, maximumValue.Y );
            result.Z = rand.RandomRange( minimumValue.Z, maximumValue.Z );

            return result;
        }

        /// <summary>
        /// Returns a random Vector4 value within the specified range.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="minimumValue"> The lower bound. </param>
        /// <param name="maximumValue"> The upper bound. </param>
        /// <returns>
        /// A random Vector4 greater than or equal to <c>minimumValue</c>, and less than
        /// or equal to <c>maximumValue</c>. 
        /// </returns>
        public static Vector4 RandomRange( this IRand rand, Vector4 minimumValue, Vector4 maximumValue )
        {
            Vector4 result;

            result.X = rand.RandomRange( minimumValue.X, maximumValue.X );
            result.Y = rand.RandomRange( minimumValue.Y, maximumValue.Y );
            result.Z = rand.RandomRange( minimumValue.Z, maximumValue.Z );
            result.W = rand.RandomRange( minimumValue.W, maximumValue.W );

            return result;
        }
    }
}

/*
#region RandomMatrix

        /// <summary>
        /// Generates a random MxN <see cref="Matrix"/> whose elements range between 0 and 1.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="rowCount">The number of rows the new random Matrix should have.</param>
        /// <param name="columnCount">The number of columns the new random Matrix should have.</param>
        /// <returns>The new random Matrix.</returns>
        public static Matrix RandomMatrix( this IRand rand, int rowCount, int columnCount )
        {
            Matrix matrix = new Matrix( rowCount, columnCount );

            for( int row = 0; row < rowCount; ++row )
                for( int column = 0; column < columnCount; ++column )
                    matrix[row, column] = rand.RandomSingle;

            return matrix;
        }

        /// <summary>
        /// Generates a random MxN <see cref="Matrix"/> whose elements range between 
        /// the specified <paramref name="minimumValue"/> and <paramref name="maximumValue"/>.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="rowCount">The number of rows the new random Matrix should have.</param>
        /// <param name="columnCount">The number of columns the new random Matrix should have.</param>
        /// <param name="minimumValue">The minimum value any element of the new random Matrix may have.</param>
        /// <param name="maximumValue">The maximum value any element of the new random Matrix may have.</param>
        /// <returns>The new random Matrix.</returns>
        public static Matrix RandomMatrix( this IRand rand, int rowCount, int columnCount, float minimumValue, float maximumValue )
        {
            if( minimumValue > maximumValue )
            {
                float temp = maximumValue;
                maximumValue = minimumValue;
                minimumValue = temp;
            }

            Matrix matrix = new Matrix( rowCount, columnCount );

            for( int row = 0; row < rowCount; ++row )
                for( int column = 0; column < columnCount; ++column )
                    matrix[row, column] = rand.UncheckedRandomRange( minimumValue, maximumValue );

            return matrix;
        }

        /// <summary>
        /// Generates a random <see cref="Matrix2"/> whose elements range between 0 and 1.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <returns>The new random Matrix2.</returns>
        public static Matrix2 RandomMatrix2( this IRand rand )
        {
            Matrix2 matrix;

            matrix.M11 = rand.RandomSingle;
            matrix.M12 = rand.RandomSingle;

            matrix.M21 = rand.RandomSingle;
            matrix.M22 = rand.RandomSingle;

            return matrix;
        }

        /// <summary>
        /// Generates a random <see cref="Matrix3"/> whose elements range between 0 and 1.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <returns>The new random Matrix3.</returns>
        public static Matrix3 RandomMatrix3( this IRand rand )
        {
            Matrix3 matrix;

            matrix.M11 = rand.RandomSingle;
            matrix.M12 = rand.RandomSingle;
            matrix.M13 = rand.RandomSingle;

            matrix.M21 = rand.RandomSingle;
            matrix.M22 = rand.RandomSingle;
            matrix.M23 = rand.RandomSingle;

            matrix.M31 = rand.RandomSingle;
            matrix.M32 = rand.RandomSingle;
            matrix.M33 = rand.RandomSingle;

            return matrix;
        }

        /// <summary>
        /// Generates a random <see cref="Matrix4"/> whose elements range between 0 and 1.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <returns>The new random Matrix4.</returns>
        public static Matrix4 RandomMatrix4( this IRand rand )
        {
            Matrix4 matrix;

            matrix.M11 = rand.RandomSingle;
            matrix.M12 = rand.RandomSingle;
            matrix.M13 = rand.RandomSingle;
            matrix.M14 = rand.RandomSingle;

            matrix.M21 = rand.RandomSingle;
            matrix.M22 = rand.RandomSingle;
            matrix.M23 = rand.RandomSingle;
            matrix.M24 = rand.RandomSingle;

            matrix.M31 = rand.RandomSingle;
            matrix.M32 = rand.RandomSingle;
            matrix.M33 = rand.RandomSingle;
            matrix.M34 = rand.RandomSingle;

            matrix.M41 = rand.RandomSingle;
            matrix.M42 = rand.RandomSingle;
            matrix.M43 = rand.RandomSingle;
            matrix.M44 = rand.RandomSingle;

            return matrix;
        }

        #endregion

        #region RandomVector

        /// <summary>
        /// Generates a random N-dimensional <see cref="Vector"/> whose elements range between 0 and 1.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="dimensionCount">
        /// The number of dimensions the new random Vector should have.
        /// </param>
        /// <returns>The new random Vector.</returns>
        public static Vector RandomVector( this IRand rand, int dimensionCount )
        {
            Vector vector = new Vector( dimensionCount );

            for( int i = 0; i < dimensionCount; ++i )
                vector[i] = rand.RandomSingle;

            return vector;
        }

        /// <summary>
        /// Generates a random N-dimensional <see cref="Vector"/> whose elements range between
        /// the specified <paramref name="minimumValue"/> and <paramref name="maximumValue"/>.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="dimensionCount">
        /// The number of dimensions the new random Vector should have.
        /// </param>
        /// <param name="minimumValue">The minimum value any element of the new random Vector may have.</param>
        /// <param name="maximumValue">The maximum value any element of the new random Vector may have.</param>
        /// <returns>The new random Vector.</returns>
        public static Vector RandomVector( this IRand rand, int dimensionCount, float minimumValue, float maximumValue )
        {
            if( minimumValue > maximumValue )
            {
                float temp   = maximumValue;
                maximumValue = minimumValue;
                minimumValue = temp;
            }

            Vector vector = new Vector( dimensionCount );

            for( int i = 0; i < dimensionCount; ++i )
                vector[i] = rand.UncheckedRandomRange( minimumValue, maximumValue );

            return vector;
        }

        /// <summary>
        /// Generates a random <see cref="Vector2"/> whose elements range between 0 and 1.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <returns>The new random Vector2.</returns>
        public static Vector2 RandomVector2( this IRand rand )
        {
            Vector2 vector;

            vector.X = rand.RandomSingle;
            vector.Y = rand.RandomSingle;

            return vector;
        }

        /// <summary>
        /// Generates a random <see cref="Vector3"/> whose elements range between 0 and 1.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <returns>The new random Vector3.</returns>
        public static Vector3 RandomVector3( this IRand rand )
        {
            Vector3 vector;

            vector.X = rand.RandomSingle;
            vector.Y = rand.RandomSingle;
            vector.Z = rand.RandomSingle;

            return vector;
        }

        /// <summary>
        /// Generates a random <see cref="Vector4"/> whose elements range between 0 and 1.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <returns>The new random Vector4.</returns>
        public static Vector4 RandomVector4( this IRand rand )
        {
            Vector4 vector;

            vector.X = rand.RandomSingle;
            vector.Y = rand.RandomSingle;
            vector.Z = rand.RandomSingle;
            vector.W = rand.RandomSingle;

            return vector;
        }

        #endregion
*/