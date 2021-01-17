// <copyright file="GaussianUtilities.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.GaussianUtilities class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Provides utility methods related to Gaussian filtering.
    /// </summary>
    public static class GaussianUtilities
    {
        /// <summary>
        /// Fills the given weights array with the gaussian values
        /// computed using the given parameters.
        /// </summary>
        /// <param name="weights"></param>
        /// <param name="mean"></param>
        /// <param name="standardDeviation"></param>
        /// <param name="amplitude"></param>
        public static void Fill( float[] weights, float mean, float standardDeviation, float amplitude )
        {
            Contract.Requires<ArgumentNullException>( weights != null );
            Contract.Requires<ArgumentException>( (weights.Length % 2) == 1 );

            float sw = 0;
            int count = (weights.Length - 1) / 2;

            for( int i = 0; i < weights.Length; ++i )
            {
                float x = (i - (float)count) / (float)count;

                weights[i] = amplitude * ComputeGaussianValue( x, mean, standardDeviation );
                sw += weights[i];
            }

            for( int i = 0; i < weights.Length; ++i )
            {
                weights[i] /= sw;
            }
        }

        /// <summary>
        /// Fills the given weights array with the gaussian values
        /// computed using the given parameters.
        /// </summary>
        /// <param name="offsets"></param>
        /// <param name="pixelSize"></param>
        /// <param name="weights"></param>
        /// <param name="mean"></param>
        /// <param name="standardDeviation"></param>
        /// <param name="amplitude"></param>
        public static void Fill( float[] offsets, float pixelSize, float[] weights, float mean, float standardDeviation, float amplitude )
        {
            Contract.Requires<ArgumentNullException>( weights != null );
            Contract.Requires<ArgumentNullException>( offsets != null );
            Contract.Requires<ArgumentException>( (weights.Length % 2) == 1 );
            Contract.Requires<ArgumentException>( weights.Length == offsets.Length );
            
            float sw = 0;
            int count = (weights.Length - 1) / 2;

            for( int i = 0; i < weights.Length; ++i )
            {
                offsets[i] = (i - (float)count) * pixelSize;

                float x = (i - (float)count) / (float)count;

                weights[i] = amplitude * ComputeGaussianValue( x, mean, standardDeviation );
                sw += weights[i];
            }

            for( int i = 0; i < weights.Length; ++i )
            {
                weights[i] /= sw;
            }
        }

        /// <summary>
        /// Computes the gaussian function.
        /// </summary>
        /// <param name="x">
        /// The value on the x-axis.
        /// </param>
        /// <param name="mean">
        /// </param>
        /// <param name="standardDeviation">
        /// </param>
        /// <returns>
        /// The value on the y-axis.
        /// </returns>
        private static float ComputeGaussianValue( float x, float mean, float standardDeviation )
        {
            // The gaussian equation is defined as such:

            /*    
                                                             -(x - mean)^2
                                                             -------------
                                            1.0               2*stddev^2
                f(x,mean,stddev) = -------------------- * e^
                                    sqrt(2*pi*stddev^2)

            */

            double left = (1.0f / System.Math.Sqrt( 2.0f * Constants.Pi * standardDeviation * standardDeviation ));
            double right = System.Math.Exp( (-((x - mean) * (x - mean))) / (2.0f * standardDeviation * standardDeviation) );

            return (float)(left * right);
        }
    }
}
