// <copyright file="IRand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Math.IRand interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Math
{
    /// <summary>
    /// Provides a mechanism to receive various pseudo-random numbers.
    /// </summary>
    /// <remarks>
    /// The default constructor of an object implementing this interface
    /// should seed itself with the current time.
    /// </remarks>
    public interface IRand
    {
        /// <summary>
        /// Gets a random boolean state value.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> 50% of the time,
        /// and <see langword="false"/> the other 50%.
        /// </value>
        bool RandomBoolean { get; }

        /// <summary>
        /// Gets a random number in the interval [0,0x7fffffff].
        /// </summary>
        /// <value>A random integer.</value>
        int RandomInteger { get; }

        /// <summary>
        /// Gets a random number in the interval [0.0, 1.0].
        /// </summary>
        /// <value>A random single-precision floating point value.</value>
        float RandomSingle { get; }

        /// <summary>
        /// Gets a random number in the interval [0.0, 1.0].
        /// </summary>
        /// <value>A random double-precision floating point value.</value>
        double RandomDouble { get; }

        /// <summary>
        /// Gets a random number in the interval [0.0, 1.0].
        /// </summary>
        /// <value>A random decimal value.</value>
        decimal RandomDecimal { get; }
    }
}
