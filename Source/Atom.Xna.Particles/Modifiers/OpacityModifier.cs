// <copyright file="OpacityModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Modifiers.OpacityModifier class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Xna.Particles.Modifiers
{
    using System;
    using Atom.Math;

    /// <summary>
    /// Represents a modifier that interpolates the opacity of a particle over its
    /// lifetime. This is a sealed class.
    /// </summary>
    [Serializable]
    public sealed class OpacityModifier : Modifier
    {
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="OpacityModifier"/> class.
        /// </summary>
        /// <param name="initial">The initial opacity of the particle upon its release.</param>
        /// <param name="final">The final opacity of the particle upon its expiry.</param>
        public OpacityModifier( float initial, float final )
            : this( CreateKeys( initial, final ) )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpacityModifier"/> class; speciying a middle sweep value.
        /// </summary>
        /// <param name="initial">The initial opacity of the particle upon its release.</param>
        /// <param name="mid">The opacity of the particle upon reaching the mid sweep age.</param>
        /// <param name="midSweep">The age at which the particle will reach mid opacity.</param>
        /// <param name="final">The final opacity of the particle upon its expiry.</param>
        public OpacityModifier( float initial, float mid, float midSweep, float final )
            : this( CreateKeys( initial, mid, midSweep, final ) )
        {
        }
        
        /// <summary>
        /// Creates an array of Curve Keys.
        /// </summary>
        /// <param name="initial">The initial value.</param>
        /// <param name="final">The maximum value.</param>
        /// <returns>
        /// The newly created Curve Keys array.
        /// </returns>
        private static Vector2[] CreateKeys( float initial, float final )
        {
            return new Vector2[] { 
                new Vector2( 0f, initial ), 
                new Vector2( 1f, final ) 
            };
        }

        /// <summary>
        /// Creates an array of Curve Keys.
        /// </summary>
        /// <param name="initial">The initial value.</param>
        /// <param name="mid">The middle value.</param>
        /// <param name="midSweep">The age at which the mid value should be used.</param>
        /// <param name="final">The maximum value.</param>
        /// <returns>
        /// The newly created Curve Keys array.
        /// </returns>
        private static Vector2[] CreateKeys( float initial, float mid, float midSweep, float final )
        {
            return new Vector2[] { 
                new Vector2( 0f, initial ), 
                new Vector2( midSweep, mid ),
                new Vector2( 1f, final ) 
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpacityModifier"/> class;
        /// specifying an array of opacity keys. The X component of the vector specifies
        /// the position on the curve, the Y component specifies the value at that position.
        /// </summary>
        /// <param name="keys">An array of keys defining the curve.</param>
        public OpacityModifier( Vector2[] keys )
        {
            if( keys == null )
                throw new ArgumentNullException( "keys" );

            this.alphaCurve = new CachedCurve( 512 );

            for( int i = 0; i < keys.Length; ++i )
            {
                Vector2 key = keys[i];
                this.alphaCurve.Keys.Add( new CurveKey( key.X, key.Y ) );
            }

            this.alphaCurve.PreCalculate();
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
            particle.Opacity = this.alphaCurve.Evaluate( particle.Age );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The CachedCurve that is used to calculate the current opacity value.
        /// </summary>
        private readonly CachedCurve alphaCurve;

        #endregion
    }
}
