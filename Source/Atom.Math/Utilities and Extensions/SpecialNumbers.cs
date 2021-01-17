// <copyright file="SpecialNumbers.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.SpecialNumbers class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

#region Math.NET Iridium (LGPL) by Ruegg + Contributors
// Math.NET Iridium, part of the Math.NET Project
// http://mathnet.opensourcedotnet.info
//
// Copyright (c) 2002-2008, Christoph Rüegg, http://christoph.ruegg.name
//
// Contribution: Fn.IntLog2 by Ben Houston, http://www.exocortex.org
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public 
// License along with this program; if not, write to the Free Software
// Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
#endregion
#region Some algorithms based on: Copyright 2000 Moshier, Bochkanov
// Cephes Math Library
// Copyright by Stephen L. Moshier
//
// Contributors:
//    * Sergey Bochkanov (ALGLIB project). Translation from C to
//      pseudocode.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
// - Redistributions of source code must retain the above copyright
//   notice, this list of conditions and the following disclaimer.
//
// - Redistributions in binary form must reproduce the above copyright
//   notice, this list of conditions and the following disclaimer listed
//   in this license in the documentation and/or other materials
//   provided with the distribution.
//
// - Neither the name of the copyright holders nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Static helper class that contains methods to calcualte
    /// various special numbers.
    /// </summary>
    public static class SpecialNumbers
    {
        #region - Fibonacci Numbers -

        /// <summary>
        /// Calculates the <paramref name="n"/>-th Fibonacci number
        /// using a single formula.
        /// </summary>
        /// <remarks>
        /// Keep in mind that Fibonacci numbers get very big quite fast.
        /// http://www.mcs.surrey.ac.uk/Personal/R.Knott/Fibonacci/fibFormula.html
        /// </remarks>
        /// <param name="n">The 'index' of the fibonacci number to get. </param>
        /// <returns>The <paramref name="n"/>th fibonacci number. </returns>
        public static int Fibonacci( int n )
        {
            double fib = 
                Math.Pow( Constantsd.GoldenRatio, n ) - 
                Math.Pow( (1.0 - Constantsd.GoldenRatio), n );

            return (int)Math.Floor( fib / Constantsd.SqrtFive );
        }

        /// <summary>
        /// Calculates the <paramref name="n"/>-th Fibonacci number
        /// using a single formula.
        /// </summary>
        /// <remarks>
        /// Keep in mind that Fibonacci numbers get very big quite fast.
        /// http://www.mcs.surrey.ac.uk/Personal/R.Knott/Fibonacci/fibFormula.html
        /// </remarks>
        /// <param name="n">The 'index' of the fibonacci number to get. </param>
        /// <returns>The <paramref name="n"/>th fibonacci number. </returns>
        public static long Fibonacci( long n )
        {
            double fib = 
                Math.Pow( Constantsd.GoldenRatio, n ) - 
                Math.Pow( (1.0 - Constantsd.GoldenRatio), n );

            return (long)Math.Floor( fib / Constantsd.SqrtFive );
        }

        #endregion

        #region - Harmonic Numbers -

        /// <summary>
        /// Returns the n-th harmonic number; Hn = sum(1/k,k=1..n).
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="n"/> is less than zero.
        /// </exception>
        /// <param name="n">
        /// The input number; must be greater or equal 0.
        /// </param>
        /// <returns>
        /// The n-th harmonic number.
        /// </returns>
        public static double Harmonic( int n )
        {
            if( n < 0 )
                throw new ArgumentOutOfRangeException( "n", Atom.ErrorStrings.SpecifiedValueIsNegative );

            const int PrecomputedCount = 32;

            if( n >= PrecomputedCount )
            {
                double n2 = n * n;
                double n4 = n2 * n2;

                return Constantsd.EulerGamma
                    + Math.Log( n )
                    + (0.5 / n)
                    - (1.0 / (12.0 * n2))
                    + (1.0 / (120.0 * n4));
            }

            return harmonicNumbers[n];
        }

        /// <summary>
        /// Stores the first 32 harmonic numbers.
        /// </summary>
        private static double[] harmonicNumbers = new double[] {
            0.0,
            1.0,
            1.5,
            1.833333333333333333333333,
            2.083333333333333333333333,
            2.283333333333333333333333,
            2.45,
            2.592857142857142857142857,
            2.717857142857142857142857,
            2.828968253968253968253968,
            2.928968253968253968253968,
            3.019877344877344877344877,
            3.103210678210678210678211,
            3.180133755133755133755134,
            3.251562326562326562326562,
            3.318228993228993228993229,
            3.380728993228993228993229,
            3.439552522640757934875582,
            3.495108078196313490431137,
            3.547739657143681911483769,
            3.597739657143681911483769,
            3.645358704762729530531388,
            3.690813250217274985076843,
            3.734291511086840202468147,
            3.775958177753506869134814,
            3.815958177753506869134814,
            3.854419716215045330673275,
            3.891456753252082367710312,
            3.927171038966368081996027,
            3.961653797587057737168440,
            3.994987130920391070501774,
            4.027245195436520102759838
        };

        #endregion

        #region - Factorial Numbers -

        /// <summary>
        /// Returns the factorial (n!) of an integer number > 0.
        /// </summary>
        /// <param name="value">
        /// The input value.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="value"/> is less than zero.
        /// </exception>
        /// <returns>
        /// The factorial (value!).
        /// </returns>
        public static double Factorial( int value )
        {
            if( value < 0 )
                throw new ArgumentOutOfRangeException( "value", Atom.ErrorStrings.SpecifiedValueIsNegative );

            const int PrecomputedCount = 32;
            if( value >= PrecomputedCount )
                return Math.Exp( SpecialFunctions.GammaLN( value + 1.0 ) );

            return factorialNumbers[value];
        }

        /// <summary>
        /// Returns the natural logarithm of the factorial (n!) for an integer value > 0.
        /// </summary>
        /// <param name="value">
        /// The input value.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="value"/> is less than zero.
        /// </exception>
        /// <returns>
        /// The natural logarithm of the factorial (value!).
        /// </returns>
        public static double FactorialLN( int value )
        {
            if( value < 0 )
                throw new ArgumentOutOfRangeException( "value", Atom.ErrorStrings.SpecifiedValueIsNegative );

            if( value <= 1 )
                return 0.0d;

            const int CacheSize = 64;
            if( value >= CacheSize )
                return SpecialFunctions.GammaLN( value + 1.0 );

            if( factorialLnCache == null )
                factorialLnCache = new double[CacheSize];

            double result = factorialLnCache[value];
            return result != 0.0 ? result : (factorialLnCache[value] = SpecialFunctions.GammaLN( value + 1.0 ));
        }

        /// <summary>
        /// The first 32 factorial numbers.
        /// </summary>
        private static readonly double[] factorialNumbers = new double[] {
            1d,
            1d,
            2d,
            6d,
            24d,
            120d,
            720d,
            5040d,
            40320d,
            362880d,
            3628800d,
            39916800d,
            479001600d,
            6227020800d,
            87178291200d,
            1307674368000d,
            20922789888000d,
            355687428096000d,
            6402373705728000d,
            121645100408832000d,
            2432902008176640000d,
            51090942171709440000d,
            1124000727777607680000d,
            25852016738884976640000d,
            620448401733239439360000d,
            15511210043330985984000000d,
            403291461126605635584000000d,
            10888869450418352160768000000d,
            304888344611713860501504000000d,
            8841761993739701954543616000000d,
            265252859812191058636308480000000d,
            8222838654177922817725562880000000d
        };

        /// <summary>
        /// Stores cached values of the natural logarithm of factorial numbers.
        /// </summary>
        private static double[] factorialLnCache;

        #endregion

        #region - Binomial Coefficient -

        /// <summary>
        /// Returns the binomial coefficient of n and k as a double precision number.
        /// </summary>
        /// <remarks>
        /// If you need to multiply or divide various such coefficients, consider
        /// using the logarithmic version <see cref="BinomialCoefficientLN"/> instead
        /// so you can add instead of multiply and subtract instead of divide, and
        /// then exponentiate the result using <see cref="Math.Exp"/>.
        /// </remarks>
        /// <param name="n">
        /// The first input index.
        /// </param>
        /// <param name="k">
        /// The second input index.
        /// </param>
        /// <returns>
        /// The binomial coefficient of <paramref name="n"/> and <paramref name="k"/>.
        /// </returns>
        public static double BinomialCoefficient( int n, int k )
        {
            if( k < 0 || n < 0 || k > n )
                return 0.0;

            return Math.Floor( 0.5 + Math.Exp( FactorialLN( n ) - FactorialLN( k ) - FactorialLN( n - k ) ) );
        }

        /// <summary>
        /// Returns the natural logarithm of the binomial coefficient of n and k as a double precision number.
        /// </summary>
        /// <param name="n">
        /// The first input index.
        /// </param>
        /// <param name="k">
        /// The second input index.
        /// </param>
        /// <returns>
        /// The natural logarithm of the binomial coefficient of <paramref name="n"/> and <paramref name="k"/>.
        /// </returns>
        public static double BinomialCoefficientLN( int n, int k )
        {
            if( k < 0 || n < 0 || k > n )
                return 1.0;

            return FactorialLN( n ) - FactorialLN( k ) - FactorialLN( n - k );
        }

        #endregion
    }
}
