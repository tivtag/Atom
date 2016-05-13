// <copyright file="Emitter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Particles.Emitter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.Particles
{
    using System;
    using System.Collections.Generic;
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Defines the base class for all Particle Emitter. 
    /// </summary>
    /// <remarks>
    /// The basic implementation releases Particles from a single point.
    /// </remarks>
    public class Emitter
    {
        #region [ Data Fields ]

        /// <summary>
        /// Gets or sets the number of <see cref="Particle"/>s which are available to this Emitter.
        /// </summary>
        public int Budget = 1000;

        /// <summary>
        /// Gets or sets the time that released <see cref="Particle"/>s will remain active for, 
        /// in whole and fractional seconds.
        /// </summary>
        public float Term = 10.0f;

        /// <summary>
        /// Gets or sets the number of Particles which will be released on each trigger.
        /// </summary>
        public int ReleaseQuantity = 50;

        /// <summary>
        /// Gets or sets the speed at which Particles travel when they are released.
        /// </summary>
        public FloatRange ReleaseSpeed;

        /// <summary>
        /// Gets or sets the colour of released Particles.
        /// </summary>
        public Xna.Vector3 ReleaseColorValue;

        /// <summary>
        /// Gets or sets the opacity of released Particles.
        /// </summary>
        public FloatRange ReleaseOpacity;

        /// <summary>
        /// Gets or sets the scale of released particles.
        /// </summary>
        public FloatRange ReleaseScale;

        /// <summary>
        /// Gets or sets the rotation of released Particles.
        /// </summary>
        public FloatRange ReleaseRotation;

        /// <summary>
        /// Gets or sets the array of Particles.
        /// </summary>
        public Particle[] Particles;

        /// <summary>
        /// Gets or sets the Texture2D used to display the Particles.
        /// </summary>
        public Texture2D ParticleTexture;

        /// <summary>
        /// Gets the collection of Modifiers which are acting upon the Emitter.
        /// </summary>
        public readonly List<Modifier> Modifiers;

        /// <summary>
        /// Gets the queue that contains the positions at which this Emitter should trigger <see cref="Particle"/>s.
        /// </summary>
        protected readonly Queue<Xna.Vector2> Triggers;

        /// <summary>
        /// The number of idle particles.
        /// </summary>
        private int idleMarker;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the number of <see cref="Particle"/>s which are currently active in this Emitter.
        /// </summary>
        public int ActiveParticleCount
        {
            get
            {
                return this.idleMarker;
            }
        }

        /// <summary>
        /// Gets an enumeration over the currently active <see cref="Particle"/>s in this Emitter.
        /// </summary>
        public IEnumerable<Particle> ActiveParticles
        {
            get
            {
                for( int i = 0; i < this.idleMarker; ++i )
                {
                    yield return this.Particles[i];
                }
            }
        }

        /// <summary>
        /// Gets or sets the initial Xna.Color of all <see cref="Particle"/>s.
        /// </summary>
        public Xna.Color ReleaseColor
        {
            get
            {
                return new Xna.Color( this.ReleaseColorValue );
            }

            set
            {
                this.ReleaseColorValue = value.ToVector3();
            }
        }

        /// <summary>
        /// Gets an instance of a Random Number Generator.
        /// </summary>
        protected RandMT Rand
        {
            get { return RandomNumberGeneratorProvider.Instance; }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the Emitter class.
        /// </summary>
        /// <param name="butget">
        /// The number of <see cref="Particle"/>s which are available to the new Emitter.
        /// </param>
        /// <param name="term">
        /// The time that released <see cref="Particle"/>s will remain active for, 
        /// in whole and fractional seconds.
        /// </param>
        public Emitter( int butget, float term )
            : this()
        {
            this.Budget = butget;
            this.Term = term;
        }

        /// <summary>
        /// Initializes a new instance of the Emitter class.
        /// </summary>
        public Emitter()
        {
            this.Triggers = new Queue<Xna.Vector2>();
            this.Modifiers = new List<Modifier>();
        }

        /// <summary>
        /// Initialises the Emitter.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown if the Term and/or Budget properties have not been set.
        /// </exception>
        public void Initialize()
        {
            if( this.Term <= 0 || this.Budget <= 0 )
                throw new InvalidOperationException();

            this.Particles = new Particle[this.Budget];
            this.idleMarker = 0;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Moves all active particles of this Emitter by the given scrollValue.
        /// </summary>
        /// <param name="scrollValue">
        /// The value each Particle should be moved for.
        /// </param>
        public void MoveActiveParticles( Microsoft.Xna.Framework.Vector2 scrollValue )
        {
            for( int particleIndex = 0; particleIndex < this.ActiveParticleCount; ++particleIndex )
            {
                this.Particles[particleIndex].Position += scrollValue;
            }
        }

        /// <summary>
        /// Triggers this Emitter at the specified position.
        /// </summary>
        /// <param name="x">
        /// The position on the x-axis at which this Emitter should be triggered.
        /// </param>
        /// <param name="y">
        /// The position on the y-axis at which this Emitter should be triggered.
        /// </param>
        public void Trigger( float x, float y )
        {
            this.Trigger( new Microsoft.Xna.Framework.Vector2( x, y ) );
        }

        /// <summary>
        /// Triggers this Emitter at the specified <paramref name="position"/>.
        /// </summary>
        /// <param name="position">
        /// The position at which this Emitter should be triggered.
        /// </param>
        public void Trigger( Xna.Vector2 position )
        {
            this.Triggers.Enqueue( position );
        }

        /// <summary>
        /// Updates this Emitter and all <see cref="Particle"/>s within.
        /// </summary>
        /// <param name="gameTime">
        /// The current GameTime.
        /// </param>
        public void Update( Microsoft.Xna.Framework.GameTime gameTime )
        {
            var totalSeconds = (float)gameTime.TotalGameTime.TotalSeconds;
            var elpsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.Update( totalSeconds, elpsedSeconds );
        }

        /// <summary>
        /// Updates this Emitter and all <see cref="Particle"/>s within.
        /// </summary>
        /// <param name="totalSeconds">
        /// The total game time in whole and fractional seconds.
        /// </param>
        /// <param name="elapsedSeconds">
        /// The elapsed frame time in whole and fractional seconds.
        /// </param>
        public void Update( float totalSeconds, float elapsedSeconds )
        {
            this.PreUpdate( totalSeconds, elapsedSeconds );

            // Check to see if the Emitter has been triggered...
            while( this.Triggers.Count > 0 )
            {
                Xna.Vector2 triggerPosition = this.Triggers.Dequeue();

                // Release some particles...
                this.ReleaseParticles( totalSeconds, this.ReleaseQuantity, ref triggerPosition );
            }

            // Track the number of Particles which have expired...
            int expiredCount = 0;

            // Begin iterating through all active Particles...
            for( int i = 0; i < this.idleMarker; ++i )
            {
                Particle particle = this.Particles[i];

                // Calculate the age of the Particle...
                particle.Age = (totalSeconds - particle.Inception) / this.Term;

                // If its age is >= 1, we need not do any further processing on it, as it will be retired...
                if( particle.Age >= 1.0f )
                {
                    ++expiredCount;
                    continue;
                }

                this.ProcessModifiers( totalSeconds, elapsedSeconds, ref particle );

                particle.Update( elapsedSeconds );
                this.Particles[i] = particle;
            }

            // If there were Particles which expired, retire them now...
            if( expiredCount > 0 )
            {
                this.RetireParticles( expiredCount );
            }
        }

        /// <summary>
        /// Called when this Emitter is about to be updated.
        /// </summary>
        /// <param name="totalSeconds">
        /// The total game time in whole and fractional seconds.
        /// </param>
        /// <param name="elapsedSeconds">
        /// The elapsed frame time in whole and fractional seconds.
        /// </param>
        protected virtual void PreUpdate( float totalSeconds, float elapsedSeconds )
        {
        }

        /// <summary>
        /// Releases the specified number of Particles, at the specified trigger position.
        /// </summary>
        /// <param name="totalSeconds">Total game time in whole and fractional seconds.</param>
        /// <param name="count">The number of particles to release.</param>
        /// <param name="triggerPosition">The position of the trigger.</param>
        /// <exception cref="System.InvalidOperationException">Thrown if the Emitter has not been initialised.</exception>
        private void ReleaseParticles( float totalSeconds, int count, ref Xna.Vector2 triggerPosition )
        {
            for( int i = 0; i < count; ++i )
            {
                // Check to see that there is an idle Particle available...
                if( this.idleMarker < this.Budget )
                {
                    // Get the next available idle Particle...
                    Particle particle = this.Particles[this.idleMarker];

                    // Generate and offset and force vector for the particle...
                    Xna.Vector2 offset;
                    this.GenerateParticleOffset( totalSeconds, ref triggerPosition, out offset );

                    Xna.Vector2 force;
                    this.GenerateParticleForce( ref offset, out force );

                    // Add the trigger position and offset vector to get the particles release position...
                    Xna.Vector2.Add( ref triggerPosition, ref offset, out particle.Position );

                    // Calculate the velocity of the particle using the force vector and the release velocity...
                    Xna.Vector2.Multiply( ref force, this.ReleaseSpeed.GetRandomValue( this.Rand ), out particle.Velocity );

                    particle.Momentum   = Xna.Vector2.Zero;
                    particle.Inception  = totalSeconds;
                    particle.Age        = 0f;
                    particle.ColorValue = new Xna.Vector4( this.ReleaseColorValue, this.ReleaseOpacity.GetRandomValue( this.Rand ) );
                    particle.Scale      = this.ReleaseScale.GetRandomValue( this.Rand );
                    particle.Rotation   = this.ReleaseRotation.GetRandomValue( this.Rand );

                    this.Particles[this.idleMarker] = particle;
                    ++this.idleMarker;
                }
            }
        }

        /// <summary>
        /// Retires the specified number of <see cref="Particle"/>s.
        /// </summary>
        /// <param name="count">
        /// The number of particles to retire.
        /// </param>
        private void RetireParticles( int count )
        {
            // Move the remaining particles to the front of the array...
            for( int i = count; i < this.idleMarker; ++i )
            {
                this.Particles[i - count] = this.Particles[i];
            }

            this.idleMarker -= count;
        }

        /// <summary>
        /// Causes all <see cref="Modifier"/>s to be applied to the given <see cref="Particle"/>.
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
        private void ProcessModifiers( float totalSeconds, float elapsedSeconds, ref Particle particle )
        {
            for( int i = 0; i < this.Modifiers.Count; ++i )
            {
                this.Modifiers[i].Process( totalSeconds, elapsedSeconds, ref particle );
            }
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
        protected virtual void GenerateParticleOffset( float totalSeconds, ref Xna.Vector2 triggerPosition, out Xna.Vector2 offset )
        {
            offset = Xna.Vector2.Zero;
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
        protected virtual void GenerateParticleForce( ref Xna.Vector2 offset, out Xna.Vector2 force )
        {
            float radians = this.Rand.RandomRange( 0.0f, Constants.TwoPi );

            force = new Xna.Vector2
            {
                X = (float)Math.Sin( radians ),
                Y = (float)Math.Cos( radians )
            };
        }

        #endregion
    }
}