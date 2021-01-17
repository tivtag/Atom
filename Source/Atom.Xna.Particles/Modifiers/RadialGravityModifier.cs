// <copyright file="RadialGravityModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Modifiers.RadialGravityModifier class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles.Modifiers
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a <see cref="Modifier"/> which pulls <see cref="Particle"/>s towards it.
    /// This class can't be inherited.
    /// </summary>
    public sealed class RadialGravityModifier : Modifier, IPositionalModifier
    {
        /// <summary>
        /// Gets or sets the position of the gravity well.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
            }
        }

        /// <summary>
        /// The storage field of the <see cref="Position"/> property.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The strength of the gravity well.
        /// </summary>
        public float Strength;

        /// <summary>
        /// Gets or sets the radius of the gravity well.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the specified value is negetive or zero.
        /// </exception>
        public float Radius
        {
            get
            {
                return this.radius;
            }

            set
            {
                if( value <= float.Epsilon )
                    throw new ArgumentOutOfRangeException( "value" );

                this.radius = value;
                this.radiusSquared = value * value;
            }
        }

        /// <summary>
        /// The storage field of the <see cref="Radius"/> property.
        /// </summary>
        private float radius;

        /// <summary>
        /// The radius, squared.
        /// </summary>
        private float radiusSquared;

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
            Vector2 distance;
            Vector2.Subtract( ref this.position, ref particle.Position, out distance );

            // Check to see if the Particle is within range of the gravity well...
            if( distance.LengthSquared() < this.radiusSquared )
            {
                Vector2 force;
                Vector2.Normalize( ref distance, out force );

                // Adjust the force vector based on the strength of the gravity well and the time delta...
                Vector2.Multiply( ref force, this.Strength * elapsedSeconds, out force );
                particle.ApplyForce( ref force );
            }
        }
    }
}