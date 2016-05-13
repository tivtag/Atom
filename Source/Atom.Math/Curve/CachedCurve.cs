// <copyright file="CachedCurve.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.CachedCurve class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Defines a <see cref="Curve"/> that is caching the result 
    /// of the last evaluation using an arabitary resolution.
    /// </summary>
    /// <remarks>
    /// A <see cref="CachedCurve"/> should be used over an <see cref="Curve"/>
    /// if the Evulation method is used frequently.
    /// </remarks>
    [Serializable]
    public class CachedCurve : Curve, ICloneable
    {
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedCurve"/> class.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// If <paramref name="sampleCount"/> is less than or equal to zero.
        /// </exception>
        /// <param name="sampleCount"> 
        /// The number of samples to create for the curve. 
        /// This value affects the resoultion of the Evulation data.
        /// </param>
        public CachedCurve( int sampleCount )
            : base()
        {
            Contract.Requires<ArgumentException>( sampleCount > 0 );
            
            this.sampleCount = sampleCount;
            this.data = new float[sampleCount + 1];
            this.Flush();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedCurve"/> class;
        /// cloning the given Curve.
        /// </summary>
        /// <param name="curve">The curve to clone.</param>
        public CachedCurve( CachedCurve curve )
            : base( curve )
        {
            this.sampleCount = curve.sampleCount;
            this.data = (float[])curve.data.Clone();
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Finds the value at a position of the <see cref="Curve"/>.
        /// </summary>
        /// <param name="position">
        /// The position on the Curve. A value between 0.0f and 1.0f.
        /// </param>
        /// <returns>
        /// The evulated value.
        /// </returns>
        public new float Evaluate( float position )
        {
            position = MathUtilities.Clamp( position, 0f, 1f );
            int sample = (int)(position * sampleCount);

            float value = this.data[sample];

            if( value != -1 )
            {
                return value;
            }
            else
            {
                value = base.Evaluate( position );
                this.data[sample] = value;
                return value;
            }
        }

        /// <summary>
        /// Pre-Calculates the data of the curve.
        /// </summary>
        public void PreCalculate()
        {
            for( int sample = 0; sample < this.sampleCount; ++sample )
            {
                float position = (float)sample / (float)sampleCount;
                this.data[sample]   = base.Evaluate( position );
            }
        }

        /// <summary>
        /// Resets all pre-calculated data.
        /// </summary>
        public void Flush()
        {
            for( int i = 1; i < sampleCount; ++i )
            {
                this.data[i - 1] = -1;
            }
        }

        /// <summary>
        /// Returns a clone of the <see cref="CachedCurve"/>.
        /// </summary>
        /// <returns>The clone.</returns>
        public new CachedCurve Clone()
        {
            return new CachedCurve( this );
        }

        /// <summary>
        /// Returns a clone of the <see cref="CachedCurve"/>.
        /// </summary>
        /// <returns>The clone.</returns>
        object ICloneable.Clone()
        {
            return new CachedCurve( this );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The number of samples that from the curve.
        /// </summary>
        private readonly int sampleCount;

        /// <summary>
        /// The cached data.
        /// </summary>
        [NonSerialized]
        private readonly float[] data;

        #endregion
    }
}
