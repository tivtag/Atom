// <copyright file="VortexModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Modifiers.VortexModifier class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles.Modifiers
{
    using System;
    using Atom.Math;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a <see cref="Modifier"/> that applies a swirling force to <see cref="Particle"/>s within a given radius.
    /// </summary>
    [Serializable]
    public class VortexModifier : Modifier, IPositionalModifier
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the position of this VortexModifier in screen coordinates.
        /// </summary>
        public Xna.Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        /// <summary>
        /// Gets or sets the radius of this VortexModifier in pixels.
        /// </summary>
        public float Radius
        {
            get
            {
                return this.radius;
            }

            set
            {
                this.radius        = value;
                this.radiusSquared = value * value;
            }
        }

        /// <summary>
        /// Gets or sets the strength of this VortexModifier in pixels per second.
        /// </summary>
        public float Vorticity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this VortexModifier spins clockwise or anti-clockwise.
        /// </summary>
        /// <value>
        /// If <see langword="true"/> then the vortex is spinning clockwise;
        /// otherwise if <see langword="false"/> anti-clockwise.
        /// </value>
        public Atom.Math.TurnDirection Direction
        {
            get
            {
                return this.turnDirection;
            }

            set
            {
                this.turnDirection = value;

                float angle = value == TurnDirection.Clockwise ? -Atom.Math.Constants.PiOver2 : Atom.Math.Constants.PiOver2;
                this.rotation = Xna.Matrix.CreateRotationZ( angle );
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="VortexModifier"/> class.
        /// </summary>
        public VortexModifier()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VortexModifier"/> class.
        /// </summary>
        /// <param name="position">The position of the vortex in screen coordinates..</param>
        /// <param name="radius">The radius of the vortex in pixels.</param>
        /// <param name="vorticity">The strength of the vortex in pixels per second.</param>
        /// <param name="turnDirection">
        /// States into what direction the new VortexModifier should be spinning.
        /// </param>
        public VortexModifier( Xna.Vector2 position, float radius, float vorticity, TurnDirection turnDirection )
        {
            this.Position  = position;
            this.Radius    = radius;
            this.Vorticity = vorticity;
            this.Direction = turnDirection;
        }

        #endregion

        #region [ Methods ]
        
        /// <summary>
        /// Inverts the <see cref="Direction"/> of this VortexModifier.
        /// </summary>
        public void InvertDirection()
        {
            switch( this.Direction )
            {
                case TurnDirection.AntiClockwise:
                    this.Direction = TurnDirection.Clockwise;
                    break;

                case TurnDirection.Clockwise:
                    this.Direction = TurnDirection.AntiClockwise;
                    break;

                case TurnDirection.None:
                    break;
            }
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
            float distance;
            Xna.Vector2.DistanceSquared( ref this.position, ref particle.Position, out distance );

            if( distance < this.radiusSquared )
            {
                float effect = 1f - (distance / this.radiusSquared);

                Xna.Vector2 force;
                Xna.Vector2.Subtract( ref this.position, ref particle.Position, out force );
                Xna.Vector2.Normalize( ref force, out force );

                Xna.Vector2.Transform( ref force, ref this.rotation, out force );
                Xna.Vector2.Multiply( ref force, effect * elapsedSeconds * this.Vorticity, out force );

                particle.ApplyForce( ref force );
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The position of the vortex.
        /// </summary>
        private Xna.Vector2 position;

        /// <summary>
        /// The radius of the vortex.
        /// </summary>
        private float radius;

        /// <summary>
        /// The radius of the vortex, squared. This is a cached value.
        /// </summary>
        private float radiusSquared;

        /// <summary>
        /// States whether the vortex is spinning clockwise or anti-clockwise.
        /// </summary>
        private TurnDirection turnDirection;

        /// <summary>
        /// The cached rotation matrix applied to particle forces.
        /// </summary>
        private Xna.Matrix rotation;

        #endregion
    }
}
