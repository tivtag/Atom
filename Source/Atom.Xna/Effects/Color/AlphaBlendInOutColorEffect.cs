// <copyright file="AlphaBlendInOutColorEffect.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.AlphaBlendInOutColorEffect class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Effects
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Implements an <see cref="ITimedColorEffect"/> that blends-in a color, 
    /// using the alpha channel, and then blends it out again after a specific time period.
    /// </summary>
    /// <remarks>
    /// TODO: # Consider adding a variable starting/maximum alpha value.
    ///       # Add needed properties.
    /// </remarks>
    public class AlphaBlendInOutColorEffect : ITimedColorEffect
    {
        /// <summary>
        /// Raised when this AlphaBlendInOutColorEffect has reached its final effect.
        /// </summary>
        public event EventHandler Ended;

        /// <summary>
        /// Gets or sets the starting alpha value.
        /// </summary>
        /// <value>The default value is 0.</value>
        public float StartAlpha 
        {
            get; 
            set; 
        }

        /// <summary>
        /// The alpha value that has been calculated.
        /// </summary>
        public float CurrentAlpha
        {
            get
            {
                return this.alpha;
            }
        }

        /// <summary>
        /// Gets or sets the time left until this AlphaBlendInOutColorEffect has reached the end.
        /// </summary>
        public float Time
        {
            get { return this.time; }
            set { this.time = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this AlphaBlendInOutColorEffect is blending out.
        /// </summary>
        public bool IsBlendingOut
        {
            get
            {
                return this.time > this.endMaxAlphaTime;
            }
        }

        /// <summary>
        /// The keytime when the maximum alpha value of the color should be reached.
        /// </summary>
        public float StartMaxAlphaTime
        {
            get
            {
                return this.startMaxAlphaTime;
            }
        }
        
        /// <summary>
        /// Gets the keytime when the maximum alpha value of the color should begin to fade to 0.
        /// </summary>
        public float EndMaxAlphaTime
        {
            get
            {
                return this.endMaxAlphaTime;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlphaBlendInOutColorEffect"/> class.
        /// </summary>
        /// <param name="duration">
        /// The total duration the effect is active.
        /// </param>
        /// <param name="startMaxAlphaTime">
        /// The keytime when the maximum alpha value of the color should be reached.
        /// </param>
        /// <param name="endMaxAlphaTime">
        /// The keytime when the maximum alpha value of the color should begin to fade to 0.
        /// </param>
        public AlphaBlendInOutColorEffect( float duration, float startMaxAlphaTime, float endMaxAlphaTime )
        {
            Contract.Requires<ArgumentException>( duration >= 0.0f );
            Contract.Requires<ArgumentException>( startMaxAlphaTime <= duration );
            Contract.Requires<ArgumentException>( endMaxAlphaTime <= duration );
            Contract.Requires<ArgumentException>( endMaxAlphaTime >= startMaxAlphaTime );

            this.totalDuration = duration;
            this.startMaxAlphaTime = startMaxAlphaTime;
            this.endMaxAlphaTime = endMaxAlphaTime;
        }

        /// <summary>
        /// Applies the current state of this AlphaBlendingColorEffect to the specified <paramref name="color"/>.
        /// </summary>
        /// <param name="color">
        /// The color to apply this IColorEffect on.
        /// </param>
        /// <returns>
        /// The output color that has this IColorEffect applied.
        /// </returns>
        public Xna.Color Apply( Xna.Color color )
        {
            return new Xna.Color( color.R, color.G, color.B, (byte)(this.alpha * byte.MaxValue) );
        }

        /// <summary>
        /// Updates this AlphaBlendingColorEffect.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            this.time += updateContext.FrameTime;

            if( this.time > this.totalDuration )
            {
                this.Ended.Raise( this );
                return;
            }

            if( this.time <= this.startMaxAlphaTime )
            {
                float factor = this.time / this.startMaxAlphaTime;
                this.alpha = MathUtilities.Coserp( this.StartAlpha, 1.0f, factor );
            }
            else if( this.time <= this.endMaxAlphaTime )
            {
                this.alpha = 1.0f;
            }
            else
            {
                float factor = (this.time - this.endMaxAlphaTime) / (this.totalDuration - this.endMaxAlphaTime);
                this.alpha = MathUtilities.Coserp( 1.0f, 0.0f, factor );
            }
        }

        /// <summary>
        /// Resets this AlphaBlendingColorEffect.
        /// </summary>
        public void Reset()
        {
            this.time = 0.0f;
        }

        /// <summary>
        /// The time the effect has been run.
        /// </summary>
        private float time;

        /// <summary>
        /// The total duration the effect is active.
        /// </summary>
        private float totalDuration;

        /// <summary>
        /// The keytime when the maximum alpha value of the color should be reached.
        /// </summary>
        private float startMaxAlphaTime;

        /// <summary>
        /// The keytime when the maximum alpha value of the color should begin to fade to 0.
        /// </summary>
        private float endMaxAlphaTime;

        /// <summary>
        /// The alpha value which has been calculated via this.
        /// </summary>
        private float alpha;
    }
}
