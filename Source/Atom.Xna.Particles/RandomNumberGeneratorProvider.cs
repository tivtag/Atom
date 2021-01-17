// <copyright file="RandomNumberGeneratorProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.RandomNumberGeneratorProvider class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles
{
    using Atom.Math;

    /// <summary>
    /// Provides singleton access to a <see cref="IRand"/> instance.
    /// </summary>
    internal static class RandomNumberGeneratorProvider
    {
        /// <summary>
        /// Gets an instance of a Random Number Generator.
        /// </summary>
        public static RandMT Instance
        {
            get { return rand; }
        }

        /// <summary>
        /// Stores the random number generator singleton instance.
        /// </summary>
        private static RandMT rand = new RandMT();
    }
}
