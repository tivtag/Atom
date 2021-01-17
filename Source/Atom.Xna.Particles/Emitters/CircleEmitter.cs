// <copyright file="CircleEmitter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Emitters.CircleEmitter class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles.Emitters
{
    using System;
    using Atom.Math;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Defines an <see cref="Emitter"/> which releases <see cref="Particle"/>s in a circle or ring shape.
    /// This class can't be inherited.
    /// </summary>
    public sealed class CircleEmitter : Emitter
    {
        /// <summary>
        /// Gets or sets the radius of the Circle.
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Particle"/>s should only be spawned on the edge of the Circle.
        /// </summary>
        /// <value>The default value is false.</value>
        public bool IsRing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Particle"/>s 
        /// should radiate away from the center of the Circle.
        /// </summary>
        /// <value>The default value is false.</value>
        public bool IsRadiating { get; set; }

        /// <summary>
        /// Generates an offset vector for a Particle as it is released.
        /// </summary>
        /// <param name="totalSeconds">
        /// The total game time in whole and fractional seconds.
        /// </param>
        /// <param name="triggerPosition">
        /// The position a twhich the Emitter has been triggered.
        /// </param>
        /// <param name="offset">
        /// The value to populate with the generated offset.
        /// </param>
        protected override void GenerateParticleOffset( float totalSeconds, ref Xna.Vector2 triggerPosition, out Xna.Vector2 offset )
        {
            float radians = this.Rand.RandomRange( 0.0f, Constants.TwoPi );

            offset = new Xna.Vector2( 
                (float)Math.Cos( radians ) * this.Radius, 
                (float)Math.Sin( radians ) * this.Radius 
            );

            if( !this.IsRing )
            {
                Xna.Vector2.Multiply( ref offset, this.Rand.RandomSingle, out offset );
            }
        }

        /// <summary>
        /// Generates a normalised force vector for a Particle as it is released.
        /// </summary>
        /// <param name="offset">
        /// The offset that has been created for the Particle. <seealso cref="GenerateParticleOffset"/>
        /// </param>
        /// <param name="force">
        /// The value to populate with the generated force.
        /// </param>
        protected override void GenerateParticleForce( ref Xna.Vector2 offset, out Xna.Vector2 force )
        {
            if( this.IsRadiating )
            {
                Xna.Vector2.Normalize( ref offset, out force );
            }
            else
            {
                base.GenerateParticleForce( ref offset, out force );
            }
        }
    }
}