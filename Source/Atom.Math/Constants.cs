// <copyright file="Constants.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Constants class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    /// <summary>
    /// Static class that contains single-precision mathematical constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Represents the mathematical constant e.
        /// </summary>
        public const float E = 2.71828f;

        /// <summary> 
        /// Represents the log base ten of e.
        /// </summary>
        public const float Log10E = 0.434294f;

        /// <summary>
        /// Represents the log base two of e.
        /// </summary>
        public const float Log2E = 1.4427f;

        /// <summary>
        /// Represents the value of pi.
        /// </summary>
        public const float Pi = 3.14159f;

        /// <summary>
        /// Represents the value of pi divided by two.
        /// </summary>
        public const float PiOver2 = 1.5708f;

        /// <summary>
        /// Represents the value of pi divided by four.
        /// </summary>
        public const float PiOver4 = 0.785398f;

        /// <summary>
        /// Represents the value of pi times two.
        /// </summary>
        public const float TwoPi = 6.28319f;

        /// <summary> 
        ///  Epsilon, a fairly small value for a single precision floating point (0.00001f).
        /// </summary>
        public const float Epsilon = 0.00001f;

        /// <summary>
        /// The square root of five.
        /// </summary>
        public const float SqrtOfFive = 2.23606797749979f;

        /// <summary> 
        /// Represents the earth velocity constant.  9.80665 Rows / s².
        /// </summary>
        public const float EarthVelocity = 9.80665f;

        /// <summary>
        /// Represents the gravity constant. (6.6742e-11f) Unit: Rows³ / km*s².
        /// </summary>
        public const float Gravity = 6.6742e-11f;

        /// <summary>
        /// Represents the avogadro constant. 6.0221367e26fkml^-1.
        /// </summary>
        public const float Avogadro = 6.0221367e26f;

        /// <summary>
        /// Represents the boltzmann constant. 1.3807 * 10 ^ -23.
        /// </summary>
        public const float Boltzmann = 1.3807e-23f;

        /// <summary>
        /// Represents the catalan constant.
        /// </summary>
        /// <remarks>Sum(k=0 -> inf){ (-1)^k/(2*k + 1)2 }</remarks>
        public const float Catalan = 0.91596559417721901505460351493238411077f;

        /// <summary>
        /// Represents the Euler-Mascheroni constant.
        /// </summary>
        /// <remarks>lim(n -> inf){ Sum(k=1 -> n) { 1/k - log(n) } }</remarks>
        public const float EulerGamma = 0.57721566490153286060651209008f;

        /// <summary>
        /// Represents the golden ratio constant. (1+sqrt(5))/2.
        /// </summary>
        public const float GoldenRatio = 1.618033988749894848204586834365638f;

        /// <summary>
        /// Represents the glaisher constant.
        /// </summary>
        /// <remarks>e^(1/12 - Zeta(-1))</remarks>
        public const float Glaisher = 1.2824271291006226368753425688698f;

        /// <summary>
        /// Represents the khinchin constant.
        /// </summary>
        /// <remarks>prod(k=1 -> inf){1+1/(k*(k+2))^log(k,2)}</remarks>
        public const float Khinchin = 2.6854520010653064453097148354818f;
    }
}
