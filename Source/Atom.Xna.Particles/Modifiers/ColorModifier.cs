// <copyright file="ColorModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Modifiers.ColorModifier class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Xna.Particles.Modifiers
{
    using System;
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Represents a <see cref="Modifier"/> that interpolates the color of a particle over its lifetime.
    /// This is a sealed class.
    /// </summary>
    public sealed class ColorModifier : Modifier
    {
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the ColorModifier class.
        /// </summary>
        public ColorModifier()
        {
            this.redCurve   = new CachedCurve( 512 );
            this.greenCurve = new CachedCurve( 512 );
            this.blueCurve  = new CachedCurve( 512 );
        }

        /// <summary>
        /// Initializes a new instance of the ColorModifier class; specifying an initial color and a final color.
        /// </summary>
        /// <param name="initial">The color of the particle upon release.</param>
        /// <param name="final">The color of the particle upon expiry.</param>
        public ColorModifier( Xna.Color initial, Xna.Color final )
            : this( CreateKeys( initial, final ) )
        {
        }

        /// <summary>
        /// Initializes a new instance of the ColorModifier class; specifying an initial color, final color, and sweepable mid color.
        /// </summary>
        /// <param name="initial">The color of the particle upon release.</param>
        /// <param name="mid">The color of the particle upon hitting the mid sweep.</param>
        /// <param name="midSweep">The position of the mid sweep on the curve.</param>
        /// <param name="final">The color of the particle upon expiry.</param>
        public ColorModifier( Xna.Color initial, Xna.Color mid, float midSweep, Xna.Color final )
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
        private static Xna.Vector4[] CreateKeys( Xna.Color initial, Xna.Color final )
        {
            return new Xna.Vector4[] { 
                new Xna.Vector4( initial.ToVector3(), 0f ),
                new Xna.Vector4( final.ToVector3(), 1f ) 
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
        private static Xna.Vector4[] CreateKeys( Xna.Color initial, Xna.Color mid, float midSweep, Xna.Color final )
        {
            return new Xna.Vector4[] { 
                new Xna.Vector4( initial.ToVector3(), 0f ),
                new Xna.Vector4( mid.ToVector3(), midSweep ), 
                new Xna.Vector4( final.ToVector3(), 1f )
            };
        }

        /// <summary>
        /// Initializes a new instance of the ColorModifier class; specifying an array of color keys.
        /// </summary>
        /// <param name="keys">The keys of the color curve.</param>
        public ColorModifier( Xna.Vector4[] keys )
        {
            if( keys == null )
                throw new ArgumentNullException( "keys" );

            this.redCurve   = new CachedCurve( 512 );
            this.greenCurve = new CachedCurve( 512 );
            this.blueCurve  = new CachedCurve( 512 );

            for( int i = 0; i < keys.Length; ++i )
            {
                Xna.Vector4 key = keys[i];

                this.redCurve.Keys.Add( new CurveKey( key.W, key.X ) );
                this.greenCurve.Keys.Add( new CurveKey( key.W, key.Y ) );
                this.blueCurve.Keys.Add( new CurveKey( key.W, key.Z ) );
            }

            this.redCurve.PreCalculate();
            this.greenCurve.PreCalculate();
            this.blueCurve.PreCalculate();
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
            float age = particle.Age;

            particle.ColorValue.X = this.redCurve.Evaluate( age );
            particle.ColorValue.Y = this.greenCurve.Evaluate( age );
            particle.ColorValue.Z = this.blueCurve.Evaluate( age );
        }
        
        #endregion

        #region [ Fields ]

        /// <summary>
        /// The curve for the red color component.
        /// </summary>
        private readonly CachedCurve redCurve;

        /// <summary>
        /// The curve for the green color component.
        /// </summary>
        private readonly CachedCurve greenCurve;

        /// <summary>
        /// The curve for the blue color component.
        /// </summary>
        private readonly CachedCurve blueCurve;

        #endregion
    }
}
