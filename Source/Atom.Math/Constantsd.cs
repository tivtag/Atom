// <copyright file="Constantsd.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Constantsd class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    /// <summary>
    /// Static class that contains double-precision mathematical constants.
    /// </summary>
    public static class Constantsd
    {
        /// <summary>
        /// The number 2^(-53).
        /// </summary>
        public static readonly double RelativeAccuracy = NumberUtilities.EpsilonOf( 1.0 );

        /// <summary>
        /// The number 2^(-52).
        /// </summary>
        public static readonly double PositiveRelativeAccuracy = NumberUtilities.PositiveEpsilonOf( 1.0 );

        /// <summary>
        /// The number 10 * 2^(-52).
        /// </summary>
        public static readonly double DefaultRelativeAccuracy = 10 * PositiveRelativeAccuracy;

        /// <summary>
        /// The square root of 0.5.
        /// As in sqrt(1/2) = 1/sqrt(2) = sqrt(2)/2.
        /// </summary>
        public const double SqrtOneHalf = 0.70710678118654752440084436210484903928483593768845d;

        /// <summary>
        /// The square root of five.
        /// </summary>
        public const double SqrtFive = 2.23606797749979;

        /// <summary>The catalan constant.</summary>
        /// <remarks>Sum(k=0 -> inf){ (-1)^k/(2*k + 1)2 }</remarks>
        public const double Catalan = 0.9159655941772190150546035149323841107741493742816721342664981196217630197762547694794d;

        /// <summary>The Euler-Mascheroni constant.</summary>
        /// <remarks>lim(n -> inf){ Sum(k=1 -> n) { 1/k - log(n) } }</remarks>
        public const double EulerGamma = 0.5772156649015328606065120900824024310421593359399235988057672348849d;

        /// <summary>The golden ratio constant '(1+sqrt(5))/2'. </summary>
        public const double GoldenRatio = 1.6180339887498948482045868343656381177203091798057628621354486227052604628189024497072d;

        /// <summary>The glaisher constant.</summary>
        /// <remarks>e^(1/12 - Zeta(-1))</remarks>
        public const double Glaisher = 1.2824271291006226368753425688697917277676889273250011920637400217404063088588264611297d;

        /// <summary>The khinchin constant.</summary>
        /// <remarks>prod(k=1 -> inf){1+1/(k*(k+2))^log(k,2)}</remarks>
        public const double Khinchin = 2.6854520010653064453097148354817956938203822939944629530511523455572188595371520028011d;
    }
}
