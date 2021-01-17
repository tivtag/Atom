// <copyright file="SpiralEmitter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Emitters.SpiralEmitter class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles.Emitters
{
    using System;
    using Atom.Diagnostics.Contracts;
    using System.Threading;
    using Atom.Math;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Defines an <see cref="Emitter"/> which emitts <see cref="Particle"/>s in a spiral.
    /// </summary>
    public class SpiralEmitter : Emitter, IDisposable
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the radius of the spiral.
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// Gets or sets the direction the spiral is turning.
        /// </summary>
        public TurnDirection Direction { get; set; }

        #endregion

        #region [ Constructor ]

        /// <summary>
        /// Initializes a new instance of the <see cref="SpiralEmitter"/> class.
        /// </summary>
        /// <param name="budget">
        /// The number of particles that will be available to the emitter.
        /// </param>
        /// <param name="term">
        /// The amount of time in whole and fractional seconds that particles shall remain
        /// active once they are released.
        /// </param>
        /// <param name="rate">
        /// The amount of time in seconds it will take for the new SpiralEmitter to turn 1 revolution.
        /// </param>
        /// <exception cref="ArgumentException"> 
        /// If <paramref name="rate"/> is zero or negative.
        /// </exception>
        public SpiralEmitter( int budget, float term, int rate )
            : base( budget, term )
        {
            Contract.Requires<ArgumentException>( rate > 0 );
       
            this.increment = 1f / (float)rate;
            this.timer = new Timer( this.OnTick, null, 0, rate );
        }

        #endregion

        #region [ Methods ]
        
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
            offset.X = this.angleCos * this.Radius;
            offset.Y = this.angleSin * this.Radius;
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
            force.X = this.angleCos;
            force.Y = this.angleSin;
        }

        /// <summary>
        /// Called automaically each tick.
        /// </summary>
        /// <param name="stateInfo">This parameter is unused.</param>
        private void OnTick( object stateInfo )
        {
            if( this.Direction == TurnDirection.AntiClockwise )
            {
                this.rotationAngle += this.increment;

                if( this.rotationAngle > 1f )
                {
                    --this.rotationAngle;
                }
            }
            else
            {
                this.rotationAngle -= this.increment;
                
                if( this.rotationAngle < -1f )
                {
                    ++this.rotationAngle;
                }
            }

            float angle = MathUtilities.Lerp( 0f, Constants.TwoPi, this.rotationAngle );
            this.angleCos = (float)System.Math.Sin( angle );
            this.angleSin = (float)System.Math.Cos( angle );
        }

        /// <summary>
        /// Immediately releases all managed and unmanaged resources this IDisposable object
        /// has aquired.
        /// </summary>
        public void Dispose()
        {
            this.Dispose( true );
            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// Releases all resources this IDisposable object has aquired.
        /// </summary>
        /// <param name="releaseManaged">
        /// States whether managed resources should be disposed.
        /// </param>
        protected virtual void Dispose( bool releaseManaged )
        {
            this.timer.Dispose();
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The current rotation angle.
        /// </summary>
        private float rotationAngle;

        /// <summary>
        /// The actual rotation angle.
        /// </summary>
        private float angleCos, angleSin;

        /// <summary>
        /// The incrementation value applied to the rotationAngle each tick.
        /// </summary>
        private float increment;

        /// <summary>
        /// The timer object that automatically rotates the spiral.
        /// </summary>
        private readonly Timer timer;

        #endregion
    }
}