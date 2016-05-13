// <copyright file="Modifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Modifier class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Xna.Particles
{
    /// <summary>
    /// Represents the abstract base class of all <see cref="Particle"/> modifiers.
    /// </summary>
    public abstract class Modifier
    {
        /// <summary>
        /// Gets an instance of a Random Number Generator.
        /// </summary>
        protected Atom.Math.RandMT Rand
        {
            get { return RandomNumberGeneratorProvider.Instance; }
        }

        /// <summary>
        /// Processes the specified <see cref="Particle"/>.
        /// </summary>
        /// <param name="totalSeconds">
        /// The total number of seconds that have been elapsed.
        /// </param>
        /// <param name="elapsedSeconds">
        /// The number of seconds that have been elapsed since the last update.
        /// </param>
        /// <param name="particle">
        /// The <see cref="Particle"/> to process.
        /// </param>
        public abstract void Process( float totalSeconds, float elapsedSeconds, ref Particle particle );
    }
}