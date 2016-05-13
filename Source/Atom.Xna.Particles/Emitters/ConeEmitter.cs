// <copyright file="ConeEmitter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Emitters.ConeEmitter class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Xna.Particles.Emitters
{
    using System;
    using Atom.Math;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Defines an <see cref="Emitter"/> which releases <see cref="Particle"/>s in 
    /// a beam which gradually becomes wider.
    /// This class can't be inherited.
    /// </summary>
    public sealed class ConeEmitter : Emitter
    {
        /// <summary>
        /// Gets or sets the direction (in radians) this ConeEmitter is emitting <see cref="Particle"/>s into.
        /// </summary>
        public float Direction { get; set; }

        /// <summary>
        /// Gets or sets the angle (in radians) from edge to edge of the ConeEmitter beam.
        /// </summary>
        public float ConeAngle { get; set; }

        /// <summary>
        /// Generates a normalised force vector for a Particle as it is released.
        /// </summary>
        /// <param name="offset">
        /// The offset that has been created for the Particle. <seealso cref="Emitter.GenerateParticleOffset"/>
        /// </param>
        /// <param name="force">
        /// The value to populate with the generated force.
        /// </param>
        protected override void GenerateParticleForce( ref Xna.Vector2 offset, out Xna.Vector2 force )
        {
            float halfAngle = this.ConeAngle * 0.5f;
            float radians = this.Rand.RandomRange(
                this.Direction - halfAngle,
                this.Direction + halfAngle
            );

            force = new Xna.Vector2 {
                X = (float)Math.Cos( radians ),
                Y = (float)Math.Sin( radians )
            };
        }
    }
}