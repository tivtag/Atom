// <copyright file="RotationModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Modifiers.RotationModifier class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Xna.Particles.Modifiers
{
    /// <summary>
    /// Defines a <see cref="Modifier"/> which alters the rotation of a <see cref="Particle"/> over its lifetime.
    /// This class can't be inherited.
    /// </summary>
    public sealed class RotationModifier : Modifier
    {
        /// <summary>
        /// Gets or sets the rate of rotation in radians per second.
        /// </summary>
        public float RotationRate 
        { 
            get;
            set;
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
        public override void Process( float totalSeconds, float elapsedSeconds, ref Particle particle )
        {
            particle.Rotation += this.RotationRate * elapsedSeconds;
        }
    }
}