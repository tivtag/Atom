// <copyright file="RandomColorModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Modifiers.RandomColorModifier class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Xna.Particles.Modifiers
{
    /// <summary>
    /// Represents a <see cref="Modifier"/> that randomly selects a color for the Particle.
    /// </summary>
    public sealed class RandomColorModifier : Modifier
    {
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
        public override void Process( float totalSeconds, float elapsedSeconds, ref Particle particle )
        {
            particle.ColorValue.X = this.Rand.RandomSingle;
            particle.ColorValue.Y = this.Rand.RandomSingle;
            particle.ColorValue.Z = this.Rand.RandomSingle;
        }
    }
}
