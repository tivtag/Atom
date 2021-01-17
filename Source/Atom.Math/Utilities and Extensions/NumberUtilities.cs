// <copyright file="NumberUtilities.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.NumberUtilities class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Provides static number related utility methods.
    /// </summary>
    /// <remarks>
    /// This utility class differs from the <see cref="MathUtilities"/> class
    /// in that it contains more (number-) specialized methods.
    /// </remarks>
    public static class NumberUtilities
    {
        #region - GCD / LCM -
        
        #region GreatestCommonDivisor

        /// <summary>
        /// Finds the greatest common divisor.
        /// </summary>
        /// <param name="firstNumber">The first number.</param>
        /// <param name="secondNumber">The second number.</param>
        /// <returns>The greatest common divisor between the two integers supplied.</returns>
        /// <exception cref="ArgumentException"> 
        /// If the specified <paramref name="firstNumber"/> or <paramref name="secondNumber"/> is less than 0.
        /// </exception>
        public static int GreatestCommonDivisor( int firstNumber, int secondNumber )
        {
            if( secondNumber < 0 )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "secondNumber" );
            if( firstNumber < 0 )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "firstNumber" );

            // The used algorithm only works if firstNumber >= secondNumber.
            if( firstNumber < secondNumber )
            {
                int temp     = firstNumber;
                firstNumber  = secondNumber;
                secondNumber = temp;
            }

            return GreatestCommonDivisorRecursive( firstNumber, secondNumber );
        }

        /// <summary>
        /// Finds the greatest common divisor.
        /// </summary>
        /// <param name="firstNumber">The first number.</param>
        /// <param name="secondNumber">The second number.</param>
        /// <returns>The greatest common divisor between the two integers supplied.</returns>
        private static int GreatestCommonDivisorRecursive( int firstNumber, int secondNumber )
        {
            if( secondNumber == 0 )
                return firstNumber;
            else
                return GreatestCommonDivisorRecursive( secondNumber, firstNumber % secondNumber );
        }

        /// <summary>
        /// Finds the greatest common divisor.
        /// </summary>
        /// <param name="firstNumber">The first number.</param>
        /// <param name="secondNumber">The second number.</param>
        /// <returns>The greatest common divisor between the two longegers supplied.</returns>
        /// <exception cref="ArgumentException"> 
        /// If the specified <paramref name="firstNumber"/> or <paramref name="secondNumber"/> is less than 0.
        /// </exception>
        public static long GreatestCommonDivisor( long firstNumber, long secondNumber )
        {
            if( secondNumber < 0 )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "secondNumber" );
            if( firstNumber < 0 )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "firstNumber" );

            // The used algorithm only works if firstNumber >= secondNumber.
            if( firstNumber < secondNumber )
            {
                long temp     = firstNumber;
                firstNumber  = secondNumber;
                secondNumber = temp;
            }

            return GreatestCommonDivisorRecursive( firstNumber, secondNumber );
        }

        /// <summary>
        /// Finds the greatest common divisor.
        /// </summary>
        /// <param name="firstNumber">The first number.</param>
        /// <param name="secondNumber">The second number.</param>
        /// <returns>The greatest common divisor between the two longegers supplied.</returns>
        private static long GreatestCommonDivisorRecursive( long firstNumber, long secondNumber )
        {
            if( secondNumber == 0 )
                return firstNumber;
            else
                return GreatestCommonDivisorRecursive( secondNumber, firstNumber % secondNumber );
        }

        #endregion

        #region LeastCommonMultiple

        /// <summary>
        /// Returns the least common multiple of two integers using euclids algorithm.
        /// </summary>
        /// <param name="firstNumber">
        /// The first value.
        /// </param>
        /// <param name="secondNumber">
        /// The second value.
        /// </param>
        /// <returns>
        /// The least common multiple of the two input values.
        /// </returns>
        public static int LeastCommonMultiple( int firstNumber, int secondNumber )
        {
            if( firstNumber == 0 || secondNumber == 0 )
                return 0;

            return System.Math.Abs( firstNumber * secondNumber ) / GreatestCommonDivisor( firstNumber, secondNumber );
        }

        /// <summary>
        /// Returns the least common multiple of two integers using euclids algorithm.
        /// </summary>
        /// <param name="firstNumber">
        /// The first value.
        /// </param>
        /// <param name="secondNumber">
        /// The second value.
        /// </param>
        /// <returns>
        /// The least common multiple of the two input values.
        /// </returns>
        public static long LeastCommonMultiple( long firstNumber, long secondNumber )
        {
            if( firstNumber == 0 || secondNumber == 0 )
                return 0;

            return System.Math.Abs( firstNumber * secondNumber ) / GreatestCommonDivisor( firstNumber, secondNumber );
        }

        #endregion

        #endregion

        #region - Prime Test -

        /// <summary>
        /// Determines whether the <paramref name="value"/> is a prime number.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <returns><c>true</c> if the number is a prime; otherwise, <c>false</c>.</returns>
        public static bool IsPrime( this long value )
        {
            if( value <= 1 )
                return false;

            long sqrtValue = (long)System.Math.Sqrt( (double)value );

            for( long i = 2; i <= sqrtValue; ++i )
            {
                if( (value % i) == 0 )
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the <paramref name="value"/> is a prime number.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <returns><c>true</c> if the number is a prime; otherwise, <c>false</c>.</returns>
        public static bool IsPrime( this int value )
        {
            if( value <= 1 )
                return false;

            int sqrtValue = (int)System.Math.Sqrt( (double)value );

            for( int i = 2; i <= sqrtValue; ++i )
            {
                if( (value % i) == 0 )
                    return false;
            }

            return true;
        }

        #endregion

        #region - Epsilon Calculation -

        /// <summary>
        /// Evaluates the minimum distance to the next distinguishable number near the argument value.
        /// </summary>
        /// <remarks>
        /// Evaluates the <b>negative</b> epsilon. 
        /// The more common positive epsilon is equal to two times this negative epsilon.
        /// </remarks>
        /// <seealso cref="PositiveEpsilonOf(double)"/>
        /// <param name="value">The input value.</param>
        /// <returns>Relative Epsilon (positive double or NaN).</returns>
        public static double EpsilonOf( double value )
        {
            if( double.IsInfinity( value ) || double.IsNaN( value ) )
                return double.NaN;

            long signed64 = BitConverter.DoubleToInt64Bits( value );

            if( signed64 == 0 )
            {
                ++signed64;
                return BitConverter.Int64BitsToDouble( signed64 ) - value;
            }

            if( signed64-- < 0 )
                return BitConverter.Int64BitsToDouble( signed64 ) - value;

            return value - BitConverter.Int64BitsToDouble( signed64 );
        }

        /// <summary>
        /// Evaluates the minimum distance to the next distinguishable number near the argument value.
        /// </summary>
        /// <remarks>Evaluates the <b>positive</b> epsilon. See also <see cref="EpsilonOf"/></remarks>
        /// <seealso cref="EpsilonOf(double)"/>
        /// <param name="value">The input value.</param>
        /// <returns>Relative Epsilon (positive double or NaN).</returns>
        public static double PositiveEpsilonOf( double value )
        {
            return 2.0 * EpsilonOf( value );
        }

        #endregion         

        #region - Value Conversation -

        /// <summary>
        /// Maps a double to an unsigned long integer which provides lexicographical ordering.
        /// </summary>
        /// <param name="value">
        /// The input value.
        /// </param>
        /// <returns>
        /// The mapped value.
        /// </returns>
        [CLSCompliant( false )]
        public static ulong ToLexicographicalOrderedUInt64( double value )
        {
            long signed64   = BitConverter.DoubleToInt64Bits( value );
            ulong unsigned64 = (ulong)signed64;

            return (signed64 >= 0)
                ? unsigned64
                : 0x8000000000000000 - unsigned64;
        }

        /// <summary>
        /// Maps a double to an signed long integer which provides lexicographical ordering.
        /// </summary>
        /// <param name="value">
        /// The input value.
        /// </param>
        /// <returns>
        /// The mapped value.
        /// </returns>
        public static long ToLexicographicalOrderedInt64( double value )
        {
            long signed64 = BitConverter.DoubleToInt64Bits( value );

            return (signed64 >= 0)
                ? signed64
                : (long)(0x8000000000000000 - (ulong)signed64);
        }

        /// <summary>
        /// Converts a long integer in signed-magnitude format to an unsigned long integer in two-complement format.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The converted value.</returns>
        [CLSCompliant( false )]
        public static ulong SignedMagnitudeToTwosComplementUInt64( long value )
        {
            return (value >= 0)
                ? (ulong)value
                : 0x8000000000000000 - (ulong)value;
        }

        /// <summary>
        /// Converts an unsigned long integer in two-complement to a long integer in signed-magnitude format format.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The converted value.</returns>
        public static long SignedMagnitudeToTwosComplementInt64( long value )
        {
            return (value >= 0)
                ? value
                : (long)(0x8000000000000000 - (ulong)value);
        }

        #endregion

        /// <summary>
        /// Increments a floating point number to the next bigger number representable by the data type.
        /// </summary>
        /// <remarks>
        /// The incrementation step length depends on the provided value.
        /// Increment(double.MaxValue) will return positive infinity.
        /// </remarks>
        /// <param name="value">The input value.</param>
        /// <returns>The incremented value.</returns>
        public static double Increment( double value )
        {
            if( double.IsInfinity( value ) || double.IsNaN( value ) )
                return value;

            long signed64 = BitConverter.DoubleToInt64Bits( value );

            if( signed64 < 0 )
            {
                --signed64;
            }
            else
            {
                ++signed64;
            }

            if( signed64 == -9223372036854775808 )
            {
                // = "-0", make it "+0"
                return 0;
            }

            value = BitConverter.Int64BitsToDouble( signed64 );
            return double.IsNaN( value ) ? double.NaN : value;
        }

        /// <summary>
        /// Decrements a floating point number to the next smaller number representable by the data type.
        /// </summary>
        /// <remarks>
        /// The decrementation step length depends on the provided value.
        /// Decrement(double.MinValue) will return negative infinity.
        /// </remarks>
        /// <param name="value">The input value.</param>
        /// <returns>The decremented value.</returns>
        public static double Decrement( double value )
        {
            if( double.IsInfinity( value ) || double.IsNaN( value ) )
                return value;

            long signed64 = BitConverter.DoubleToInt64Bits( value );
            if( signed64 == 0 )
                return -double.Epsilon;

            if( signed64 < 0 )
            {
                ++signed64;
            }
            else
            {
                --signed64;
            }

            value = BitConverter.Int64BitsToDouble( signed64 );
            return double.IsNaN( value ) ? double.NaN : value;
        }

        /// <summary>
        /// Evaluates the count of numbers between two double numbers.
        /// </summary>
        /// <remarks>
        /// The second number is included in the number, thus two equal numbers evaluate to zero and 
        /// two neighbour numbers evaluate to one. Therefore, what is returned is actually the count of numbers between plus 1.
        /// </remarks>
        /// <param name="a">
        /// The first number.
        /// </param>
        /// <param name="b">
        /// The second number.
        /// </param>
        /// <returns>
        /// The amount of numbers between the two given double numbers.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// If <paramref name="a"/> or <paramref name="b"/> are infinity or NaN.
        /// </exception>
        [CLSCompliant( false )]
        public static ulong NumbersBetween( double a, double b )
        {
            if( double.IsNaN( a ) || double.IsInfinity( a ) )
                throw new ArgumentException( MathErrorStrings.ArgumentIsInfinityNaN, "a" );

            if( double.IsNaN( b ) || double.IsInfinity( b ) )
                throw new ArgumentException( MathErrorStrings.ArgumentIsInfinityNaN, "b" );

            ulong ua = ToLexicographicalOrderedUInt64( a );
            ulong ub = ToLexicographicalOrderedUInt64( b );

            return (a >= b) ? ua - ub : ub - ua;
        }
    }
}
