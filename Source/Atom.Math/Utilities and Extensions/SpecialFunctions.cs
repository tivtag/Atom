// <copyright file="SpecialFunctions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.SpecialFunctions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
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
    /// Static helper class that contains special math functions.
    /// </summary>
    /// <remarks>
    /// Those functions have been taken from the Math.NET Iridium library.
    /// Full credit goes to Christoph Rüegg and Contributors. See list on the top.
    /// </remarks>
    public static class SpecialFunctions
    {
        #region - Gamma Functions -

        /// <summary>
        /// Returns the natural logarithm of Gamma for a real value &gt; 0.
        /// </summary>
        /// <param name="value">
        /// The input value.
        /// </param>
        /// <returns>
        /// A value ln|Gamma(value))| for value &gt; 0.
        /// </returns>
        public static double GammaLN( double value )
        {
            double[] coefficient = new double[] {
                76.18009172947146,
                -86.50532032941677,
                24.01409824083091,
                -1.231739572450155,
                0.1208650973866179e-2,
                -0.5395239384953e-5
            };

            double x = value;
            double y = value;
            double ser = 1.000000000190015;

            double temp = (x + 5.5);
            temp -= ((x + 0.5) * System.Math.Log( temp ));

            for( int j = 0; j <= 5; ++j )
            {
                ser += coefficient[j] / ++y;
            }

            return -temp + System.Math.Log( 2.5066282746310005 * ser / x );
        }

        /// <summary>
        /// Returns the gamma function for real values (except at 0, -1, -2, ...).
        /// For numeric stability, consider to use GammaLn for positive values.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>A value Gamma(value) for value != 0,-1,-2,...</returns>
        public static double Gamma( double value )
        {
            if( value > 0.0 )
                return System.Math.Exp( GammaLN( value ) );

            double reflection = 1.0 - value;
            double s          = System.Math.Sin( System.Math.PI * reflection );

            // singularity, undefined
            if( s.IsApproximate( 0.0 ) )
                return double.NaN;

            return System.Math.PI / (s * System.Math.Exp( GammaLN( reflection ) ));
        }

        /// <summary>
        /// Returns the regularized lower incomplete gamma function
        /// P(a,x) = 1/Gamma(a) * int(exp(-t)t^(a-1),t=0..x) for real a &gt; 0, x &gt; 0.
        /// </summary>
        /// <param name="a">The first input value.</param>
        /// <param name="x">The second input value.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="a"/> or <paramref name="x"/> is less than zero.
        /// </exception>
        /// <returns>
        /// The regularized lower incomplete gamma function.
        /// </returns>
        public static double GammaRegularized( double a, double x )
        {
            const int MaxIterations = 100;
            double eps   = Constantsd.RelativeAccuracy;
            double fpmin = double.Epsilon / eps;

            if( a < 0.0 )
                throw new ArgumentOutOfRangeException( "a", a, Atom.ErrorStrings.SpecifiedValueIsNegative );
            if( x < 0.0 )
                throw new ArgumentOutOfRangeException( "x", x, Atom.ErrorStrings.SpecifiedValueIsNegative );

            double gln = GammaLN( a );

            if( x < a + 1.0 )
            {
                if( x == 0.0 )
                {
                    return 0.0;
                }
                else
                {
                    double ap = a;
                    double del, sum = del = 1.0 / a;

                    for( int n = 0; n < MaxIterations; ++n )
                    {
                        ++ap;
                        del *= x / ap;
                        sum += del;

                        if( System.Math.Abs( del ) < System.Math.Abs( sum ) * eps )
                        {
                            return sum * System.Math.Exp( -x + (a * System.Math.Log( x )) - gln );
                        }
                    }
                }
            }
            else
            {
                // Continued fraction representation:
                double b = x + 1.0 - a;
                double c = 1.0 / fpmin;
                double d = 1.0 / b;
                double h = d;

                for( int i = 1; i <= MaxIterations; ++i )
                {
                    double an = -i * (i - a);
                    b += 2.0;
                    d = (an * d) + b;

                    if( System.Math.Abs( d ) < fpmin )
                        d = fpmin;

                    c = b + (an / c);

                    if( System.Math.Abs( c ) < fpmin )
                        c = fpmin;

                    d = 1.0 / d;

                    double del = (d * c);
                    h *= del;

                    if( System.Math.Abs( del - 1.0 ) <= eps )
                        return 1.0 - (System.Math.Exp( -x + (a * System.Math.Log( x )) - gln ) * h);
                }
            }

            throw new ArgumentException( MathErrorStrings.ArgumentTooLargeForIterationLimit, "a" );
        }

        #endregion

        #region - Digamma Functions -

        /// <summary>
        /// Returns the digamma (psi) function of real values (except at 0, -1, -2, ...).
        /// Digamma is the logarithmic derivative of the <see cref="Gamma(Double)"/> function.
        /// </summary>
        /// <param name="x">
        /// The input value.
        /// </param>
        /// <returns>
        /// The digamma (psi) function.
        /// </returns>
        public static double Digamma( double x )
        {
            double y  = 0;
            double nz = 0.0;
            bool isNegative = (x <= 0);

            if( isNegative )
            {
                double q = x;
                double p = System.Math.Floor( q );
                isNegative = true;

                if( p.IsApproximate( q ) )
                    return double.NaN; // singularity, undefined

                nz = q - p;

                if( nz != 0.5 )
                {
                    if( nz > 0.5 )
                    {
                        p = p + 1.0;
                        nz = q - p;
                    }

                    nz = System.Math.PI / System.Math.Tan( System.Math.PI * nz );
                }
                else
                {
                    nz = 0.0;
                }

                x = 1.0 - x;
            }

            if( (x <= 10.0) && (x == System.Math.Floor( x )) )
            {
                y = 0.0;
                int n = (int)System.Math.Floor( x );

                for( int i = 1; i <= n - 1; ++i )
                    y = y + (1.0 / i);

                y = y - Constantsd.EulerGamma;
            }
            else
            {
                double s = x;
                double w = 0.0;

                while( s < 10.0 )
                {
                    w = w + (1.0 / s);
                    s = s + 1.0;
                }

                if( s < 1.0e17 )
                {
                    double z    = 1.0 / (s * s);
                    double polv = 8.33333333333333333333e-2;

                    polv = (polv * z) - 2.10927960927960927961e-2;
                    polv = (polv * z) + 7.57575757575757575758e-3;
                    polv = (polv * z) - 4.16666666666666666667e-3;
                    polv = (polv * z) + 3.96825396825396825397e-3;
                    polv = (polv * z) - 8.33333333333333333333e-3;
                    polv = (polv * z) + 8.33333333333333333333e-2;

                    y = z * polv;
                }
                else
                {
                    y = 0.0;
                }

                y = System.Math.Log( s ) - (0.5 / s) - y - w;
            }

            if( isNegative )
            {
                return y - nz;
            }

            return y;
        }

        #endregion

        #region - Beta Functions -

        /// <summary>
        /// Returns the Euler Beta function of real valued z &gt; 0, w &gt; 0.
        /// </summary>
        /// <remarks>
        /// Beta(z,w) = Beta(w,z).
        /// </remarks>
        /// <param name="z">The first input value.</param>
        /// <param name="w">The second input value.</param>
        /// <returns>
        /// The Euler Beta function of <paramref name="w"/> and <paramref name="z"/>.
        /// </returns>
        public static double Beta( double z, double w )
        {
            return System.Math.Exp( GammaLN( z ) + GammaLN( w ) - GammaLN( z + w ) );
        }

        /// <summary>
        /// Returns the natural logarithm of the Euler Beta function of real valued z &gt; 0, w &gt; 0.
        /// </summary>
        /// <remarks>
        /// BetaLn(z,w) = BetaLn(w,z).
        /// </remarks>
        /// <param name="z">The first input value.</param>
        /// <param name="w">The second input value.</param>
        /// <returns>
        /// The natural logarithm of the Euler Beta function of <paramref name="w"/> and <paramref name="z"/>.
        /// </returns>
        public static double BetaLN( double z, double w )
        {
            return GammaLN( z ) + GammaLN( w ) - GammaLN( z + w );
        }

        /// <summary>
        /// Returns the regularized lower incomplete beta function
        /// I_x(a,b) = 1/Beta(a,b) * int(t^(a-1)*(1-t)^(b-1),t=0..x) for real a &gt; 0, b &gt; 0, 1 &gt;= x &gt;= 0.
        /// </summary>
        /// <param name="a">The first positive input value.</param>
        /// <param name="b">The second positive input value.</param>
        /// <param name="x">The third input value; must be in |0..1|.</param>
        /// <returns>
        /// The regularized lower incomplete beta function.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If any of the parameters is out of valid range.
        /// </exception>
        public static double BetaRegularized(
                double a,
                double b,
                double x
            )
        {
            if( a < 0.0 )
                throw new ArgumentOutOfRangeException( "a", a, Atom.ErrorStrings.SpecifiedValueIsNegative );
            if( b < 0.0 )
                throw new ArgumentOutOfRangeException( "b", b, Atom.ErrorStrings.SpecifiedValueIsNegative );

            if( x < 0.0 || x > 1.0 )
            {
                string errorMessage = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    MathErrorStrings.ArgumentInIntervalXYInclusive,
                    "0.0",
                    "1.0"
                );

                throw new ArgumentOutOfRangeException( "x", x, errorMessage );
            }

            double bt = (x == 0.0 || x == 1.0) ? 0.0
                : System.Math.Exp( 
                    GammaLN( a + b ) - 
                    GammaLN( a ) - 
                    GammaLN( b ) + 
                    (a * System.Math.Log( x )) +
                    (b * System.Math.Log( 1.0 - x ))
                  );

            bool isSymmetryTransformation = (x >= (a + 1.0) / (a + b + 2.0));

            // Continued fraction representation
            const int MaxIterations = 100;
            double eps   = Constantsd.RelativeAccuracy;
            double fpmin = double.Epsilon / eps;

            if( isSymmetryTransformation )
            {
                x = 1.0 - x;

                double swap = a;
                a = b;
                b = swap;
            }

            double qab = a + b;
            double qap = a + 1.0;
            double qam = a - 1.0;
            double c = 1.0;
            double d = 1.0 - (qab * x / qap);

            if( System.Math.Abs( d ) < fpmin )
            {
                d = fpmin;
            }

            d = 1.0 / d;
            double h = d;

            for( int m = 1, m2 = 2; m <= MaxIterations; ++m, m2 += 2 )
            {
                double aa = m * (b - m) * x / ((qam + m2) * (a + m2));
                d = 1.0 + (aa * d);

                if( System.Math.Abs( d ) < fpmin )
                    d = fpmin;

                c = 1.0 + (aa / c);

                if( System.Math.Abs( c ) < fpmin )
                    c = fpmin;

                d  = 1.0 / d;
                h *= d * c;
                aa = -(a + m) * (qab + m) * x / ((a + m2) * (qap + m2));
                d  = 1.0 + (aa * d);

                if( System.Math.Abs( d ) < fpmin )
                    d = fpmin;

                c = 1.0 + (aa / c);

                if( System.Math.Abs( c ) < fpmin )
                {
                    c = fpmin;
                }

                d = 1.0 / d;
                double del = d * c;
                h *= del;

                if( System.Math.Abs( del - 1.0 ) <= eps )
                {
                    if( isSymmetryTransformation )
                    {
                        return 1.0 - (bt * h / a);
                    }

                    return bt * h / a;
                }
            }

            throw new ArgumentException( MathErrorStrings.ArgumentTooLargeForIterationLimit, "a" );
        }

        #endregion

        #region - Error Functions -

        /// <summary>
        /// Returns the error function of <paramref name="x"/>.
        /// </summary>
        /// <remarks>
        /// erf(x) = 2/sqrt(pi) * int(exp(-t^2),t=0..x);
        /// </remarks>
        /// <param name="x">
        /// The input value.
        /// </param>
        /// <returns>
        /// The error function of <paramref name="x"/>.
        /// </returns>
        public static double Error( double x )
        {
            if( double.IsNegativeInfinity( x ) )
                return -1.0;

            if( double.IsPositiveInfinity( x ) )
                return 1.0;

            return x < 0.0
                ? -GammaRegularized( 0.5, x * x )
                : GammaRegularized( 0.5, x * x );
        }

        /// <summary>
        /// Returns the inverse error function erf^-1(x).
        /// </summary>
        /// <remarks>
        /// <p>The algorithm uses a minimax approximation by rational functions
        /// and the result has a relative error whose absolute value is less
        /// than 1.15e-9.</p>
        /// <p>See the page <see href="http://home.online.no/~pjacklam/notes/invnorm/"/>
        /// for more details.</p>
        /// </remarks>
        /// <param name="x">
        /// The input value.
        /// </param>
        /// <returns>
        /// The inverse error function of <paramref name="x"/>.
        /// </returns>
        public static double ErrorInv( double x )
        {
            if( x < -1.0 || x > 1.0 )
            {
                string errorMessage = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    MathErrorStrings.ArgumentInIntervalXYInclusive,
                    "-1.0",
                    "1.0"
                );

                throw new ArgumentOutOfRangeException( "x", x, errorMessage );
            }

            x = 0.5 * (x + 1.0);

            // Define break-points.
            double plow = 0.02425;
            double phigh = 1 - plow;

            double q;

            // Rational approximation for lower region:
            if( x < plow )
            {
                q = System.Math.Sqrt( -2 * System.Math.Log( x ) );

                double factorA = (((((((((ErrorInvC[0] * q) + ErrorInvC[1]) * q) + ErrorInvC[2]) * q) + ErrorInvC[3]) * q) + ErrorInvC[4]) * q) + ErrorInvC[5];
                double factorB =   (((((((ErrorInvD[0] * q) + ErrorInvD[1]) * q) + ErrorInvD[2]) * q) + ErrorInvD[3]) * q) + 1.0;

                return (factorA / factorB) * Constantsd.SqrtOneHalf;
            }

            // Rational approximation for upper region:
            if( phigh < x )
            {
                q = System.Math.Sqrt( -2 * System.Math.Log( 1 - x ) );

                double factorA = (((((((((ErrorInvC[0] * q) + ErrorInvC[1]) * q) + ErrorInvC[2]) * q) + ErrorInvC[3]) * q) + ErrorInvC[4]) * q) + ErrorInvC[5];
                double factorB =   (((((((ErrorInvD[0] * q) + ErrorInvD[1]) * q) + ErrorInvD[2]) * q) + ErrorInvD[3]) * q) + 1.0;

                return -(factorA / factorB) * Constantsd.SqrtOneHalf;
            }

            // Rational approximation for central region:
            q = x - 0.5;

            double r = q * q;

            double first  = (((((((((ErrorInvA[0] * r) + ErrorInvA[1]) * r) + ErrorInvA[2]) * r) + ErrorInvA[3]) * r) + ErrorInvA[4]) * r) + ErrorInvA[5];
            double second = (((((((((ErrorInvB[0] * r) + ErrorInvB[1]) * r) + ErrorInvB[2]) * r) + ErrorInvB[3]) * r) + ErrorInvB[4]) * r) + 1.0;

            return (first * q) / (second * Constantsd.SqrtOneHalf);
        }

        /// <summary>
        /// Error function contants.
        /// </summary>
        private static readonly double[] ErrorInvA = {
            -3.969683028665376e+01, 2.209460984245205e+02,
            -2.759285104469687e+02, 1.383577518672690e+02,
            -3.066479806614716e+01, 2.506628277459239e+00
        };

        /// <summary>
        /// Error function contants.
        /// </summary>
        private static readonly double[] ErrorInvB = {
            -5.447609879822406e+01, 1.615858368580409e+02,
            -1.556989798598866e+02, 6.680131188771972e+01,
            -1.328068155288572e+01
        };

        /// <summary>
        /// Error function contants.
        /// </summary>
        private static readonly double[] ErrorInvC = {
            -7.784894002430293e-03, -3.223964580411365e-01,
            -2.400758277161838e+00, -2.549732539343734e+00,
            4.374664141464968e+00, 2.938163982698783e+00
        };

        /// <summary>
        /// Error function contants.
        /// </summary>
        private static readonly double[] ErrorInvD = {
            7.784695709041462e-03, 3.224671290700398e-01,
            2.445134137142996e+00, 3.754408661907416e+00
        };

        #endregion

        /// <summary>
        /// Returns the gamma function of a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="z">
        /// The input value.
        /// </param>
        /// <returns>
        /// The gamma function of <paramref name="z"/>.
        /// </returns>
        public static Complex Gamma( Complex z )
        {
            Complex c = 1.0f / z;

            for( float k = 1.0f; k <= 50000.0; ++k )
            {
                c = c * ((z / k).Exp / (1.0f + (z / k)));
            }

            return (-Constants.EulerGamma * z).Exp * c;
        }

        /// <summary>
        /// Returns the beta function of a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="x">
        /// The first input value.
        /// </param>
        /// <param name="y">
        /// The second input value.
        /// </param>
        /// <returns>
        /// The gamma function of <paramref name="x"/> and <paramref name="y"/>.
        /// </returns>
        public static Complex Beta( Complex x, Complex y )
        {
            return (Gamma( x ) * Gamma( y )) / Gamma( x + y );
        }
    }
}
