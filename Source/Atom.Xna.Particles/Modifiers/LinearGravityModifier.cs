// <copyright file="LinearGravityModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Modifiers.LinearGravityModifier class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles.Modifiers
{
    using System.ComponentModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a <see cref="Modifier"/> that applies a constant force vector 
    /// to <see cref="Particle"/>s over their lifetime.
    /// This class can't be inherited.
    /// </summary>
    public sealed class LinearGravityModifier : Modifier
    {
        /// <summary>
        /// Gets or sets the gravity vector applied by this LinearGravityModifier.
        /// </summary>
        public Vector2 Gravity;

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
            Vector2 force;
            Vector2.Multiply( ref this.Gravity, elapsedSeconds, out force );

            particle.ApplyForce( ref force );
        }
    }
}