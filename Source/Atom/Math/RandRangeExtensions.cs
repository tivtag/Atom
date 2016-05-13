// <copyright file="RandRangeExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.RandRangeExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Defines extension methods for the <see cref="IRand"/> interface
    /// that make it easier to get a random value within a specific range.
    /// </summary>
    public static class RandRangeExtensions
    {
        #region RandomRange

        /// <summary>
        /// Returns a random integer within the specified range.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="minimumValue"> The lower bound. </param>
        /// <param name="maximumValue"> The upper bound. </param>
        /// <exception cref="OverflowException">If an overflow has occured.</exception>
        /// <returns> 
        /// A random integer greater than or equal to <c>minimumValue</c>, and less than
        /// or equal to <c>maximumValue</c>. 
        /// </returns>
        public static int RandomRange( this IRand rand, int minimumValue, int maximumValue )
        {
            if( minimumValue > maximumValue )
            {
                int temp = maximumValue;
                maximumValue = minimumValue;
                minimumValue = temp;
            }

            checked
            {
                return (int)System.Math.Round( minimumValue + ((maximumValue - minimumValue) * rand.RandomSingle) );
            }
        }

        /// <summary>
        /// Returns a random long integer within the specified range.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="minimumValue"> The lower bound. </param>
        /// <param name="maximumValue"> The upper bound. </param>
        /// <exception cref="OverflowException">If an overflow has occured.</exception>
        /// <returns> 
        /// A random integer greater than or equal to <c>minimumValue</c>, and less than
        /// or equal to <c>maximumValue</c>. 
        /// </returns>
        public static long RandomRange( this IRand rand, long minimumValue, long maximumValue )
        {
            if( minimumValue > maximumValue )
            {
                long temp = maximumValue;
                maximumValue = minimumValue;
                minimumValue = temp;
            }

            checked
            {
                return (long)System.Math.Round( minimumValue + ((maximumValue - minimumValue) * rand.RandomDouble) );
            }
        }

        /// <summary>
        /// Returns a random integer within the specified range.
        /// </summary>
        /// <exception cref="OverflowException">If an overflow has occured.</exception>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="minimumValue"> The lower bound. </param>
        /// <param name="maximumValue"> The upper bound. </param>
        /// <returns> 
        /// A random integer greater than or equal to <c>minimumValue</c>, and less than
        /// or equal to <c>maximumValue</c>. 
        /// </returns>
        [System.CLSCompliant( false )]
        public static uint RandomRange( this IRand rand, uint minimumValue, uint maximumValue )
        {
            if( minimumValue > maximumValue )
            {
                uint temp = maximumValue;
                maximumValue = minimumValue;
                minimumValue = temp;
            }

            checked
            {
                return (uint)System.Math.Round( minimumValue + ((maximumValue - minimumValue) * rand.RandomDouble) );
            }
        }

        /// <summary>
        /// Returns a random floating point value within the specified range.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="minimumValue"> The lower bound. </param>
        /// <param name="maximumValue"> The upper bound. </param>
        /// <returns>
        /// A random single-precision floating-point greater than or equal to <c>minimumValue</c>, and less than
        /// or equal to <c>maximumValue</c>. 
        /// </returns>
        public static float RandomRange( this IRand rand, float minimumValue, float maximumValue )
        {
            if( minimumValue > maximumValue )
            {
                float temp = maximumValue;
                maximumValue = minimumValue;
                minimumValue = temp;
            }

            return minimumValue + ((maximumValue - minimumValue) * rand.RandomSingle);
        }

        /// <summary>
        /// Returns a random floating point value within the specified range.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="minimumValue"> The lower bound. </param>
        /// <param name="maximumValue"> The upper bound. </param>
        /// <returns> 
        /// A random double-precision floating-point value greater than or equal to <c>minimumValue</c>, and less than
        /// or equal to <c>maximumValue</c>. 
        /// </returns>
        public static double RandomRange( this IRand rand, double minimumValue, double maximumValue )
        {
            if( minimumValue > maximumValue )
            {
                double temp = maximumValue;
                maximumValue = minimumValue;
                minimumValue = temp;
            }

            return minimumValue + ((maximumValue - minimumValue) * rand.RandomDouble);
        }

        /// <summary>
        /// Returns a random floating point value within the specified range.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="minimumValue"> The lower bound. </param>
        /// <param name="maximumValue"> The upper bound. </param>
        /// <returns> 
        /// A random decimal greater than or equal to <c>minimumValue</c>, and less than
        /// or equal to <c>maximumValue</c>.
        /// </returns>
        public static decimal RandomRange( this IRand rand, decimal minimumValue, decimal maximumValue )
        {
            if( minimumValue > maximumValue )
            {
                decimal temp = maximumValue;
                maximumValue = minimumValue;
                minimumValue = temp;
            }

            return minimumValue + ((maximumValue - minimumValue) * rand.RandomDecimal);
        }

        #endregion

        #region UncheckedRandomRange

        /// <summary>
        /// Returns a random floating point value within the specified range,
        /// doesn't check whether the given minimumValue and maximumValue are in the right order of minimumValue &lt;= maximumValue.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="minimumValue"> The lower bound. </param>
        /// <param name="maximumValue"> The upper bound. </param>
        /// <returns>
        /// A random integer greater than or equal to <c>minimumValue</c>, and less than
        /// or equal to <c>maximumValue</c>. 
        /// </returns>
        public static float UncheckedRandomRange( this IRand rand, float minimumValue, float maximumValue )
        {
            return minimumValue + ((maximumValue - minimumValue) * rand.RandomSingle);
        }

        /// <summary>
        /// Returns a random floating point value within the specified range,
        /// doesn't check whether the given minimumValue and maximumValue are in the right order of minimumValue &lt;= maximumValue.
        /// </summary>
        /// <param name="rand">The random number generator to use.</param>
        /// <param name="minimumValue"> The lower bound. </param>
        /// <param name="maximumValue"> The upper bound. </param>
        /// <returns>
        /// A random integer greater than or equal to <c>minimumValue</c>, and less than
        /// or equal to <c>maximumValue</c>. 
        /// </returns>
        public static int UncheckedRandomRange( this IRand rand, int minimumValue, int maximumValue )
        {
            return (int)System.Math.Round( minimumValue + ((maximumValue - minimumValue) * rand.RandomSingle) );
        }

        #endregion
    }
}