// <copyright file="LineEmitter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Emitters.LineEmitter class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Xna.Particles.Emitters
{
    using System;
    using System.ComponentModel;
    using Atom.Math;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Defines an <see cref="Emitter"/> which releases Particles at a random point along a line.
    /// </summary>
    public class LineEmitter : Emitter
    {
        /// <summary>
        /// Gets or sets the length of the line.
        /// </summary>
        public int Length
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the rotation of the line around its middle point.
        /// </summary>
        public float Angle
        {
            get { return (float)Math.Atan2( this.rotationMatrix.M12, this.rotationMatrix.M11 ); }
            set { this.rotationMatrix = Xna.Matrix.CreateRotationZ( value ); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this LineEmitter should spawn
        /// ...
        /// </summary>
        public bool IsRectilinear
        {
            get;
            set;
        }

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
            float halfLength = this.Length * 0.5f;
            offset = new Xna.Vector2 {
                X = this.Rand.RandomRange( -halfLength, halfLength ),
                Y = 0f
            };

            Xna.Vector2.Transform( ref offset, ref this.rotationMatrix, out offset );
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
            if( this.IsRectilinear )
            {
                Xna.Vector3 up = this.rotationMatrix.Up;
                force = new Xna.Vector2( up.X, up.Y );
            }
            else
            {
                base.GenerateParticleForce( ref offset, out force );
            }
        }

        /// <summary>
        /// The orientation of the line.
        /// </summary>
        private Xna.Matrix rotationMatrix = Xna.Matrix.CreateRotationZ( 0f );
    }
}