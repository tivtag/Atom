// <copyright file="ScaleModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Modifiers.ScaleModifier class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles.Modifiers
{
    using System;
    using Atom.Math;

    /// <summary>
    /// Defines a <see cref="Modifier"/> which interpolates the scale of a particle throughout its
    /// lifetime. This class can't be inherited.
    /// </summary>
    public sealed class ScaleModifier : Modifier
    {
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleModifier"/> class.
        /// </summary>
        /// <param name="initial">The initial scale of the particle in pixels upon its release.</param>
        /// <param name="final">The final scale of the particle in pixels upon its expiry.</param>
        public ScaleModifier( float initial, float final )
            : this( new Vector2[] { new Vector2( 0f, initial ), new Vector2( 1f, final ) })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleModifier"/> class; specifying a middle scale value.
        /// </summary>
        /// <param name="initial">The initial scale of the particle in pixels upon its release.</param>
        /// <param name="mid">The middle scale of the particle.</param>
        /// <param name="midSweep">The age at which the particle shall reach middle scale.</param>
        /// <param name="final">The final scale of the particle in pixels upon its expiry.</param>
        public ScaleModifier( float initial, float mid, float midSweep, float final )
            : this( 
                new Vector2[] { 
                    new Vector2( 0f, initial ), 
                    new Vector2( midSweep, mid ),
                    new Vector2( 1f, final ) 
                }
            )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleModifier"/> class; specifying an array of keys defining the scale curve.
        /// </summary>
        /// <param name="keys">
        /// An array of Vector2 objects defining the curve. The X component of the
        /// vector specifies the position on the curve, the Y component specifies the value at that
        /// position.
        /// </param>
        public ScaleModifier( Vector2[] keys )
        {
            if( keys == null )
                throw new ArgumentNullException( "keys" );

            this.scaleCurve = new CachedCurve( 512 );

            for( int i = 0; i < keys.Length; ++i )
            {
                Vector2 key = keys[i];
                this.scaleCurve.Keys.Add( new CurveKey( key.X, key.Y ) );
            }

            this.scaleCurve.PreCalculate();
        }

        #endregion

        #region [ Methods ]

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
            particle.Scale = this.scaleCurve.Evaluate( particle.Age );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The curve that is used to determine the current scaling value.
        /// </summary>
        private readonly CachedCurve scaleCurve;

        #endregion
    }
}
