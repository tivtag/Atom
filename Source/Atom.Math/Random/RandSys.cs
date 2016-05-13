// <copyright file="RandSys.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.RandSys class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    /// <summary>
    /// Defines a pseudo-random number generator that delgates 
    /// all request to a <see cref="System.Random"/> object.
    /// This class can't be inherited.
    /// </summary>
    public sealed class RandSys : IRand
    {
        #region [ Properties ]

        /// <summary>
        /// Gets a random boolean state value.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> 50% of the time,
        /// and <see langword="false"/> the other 50%.
        /// </value>
        public bool RandomBoolean
        {
            get
            {
                return (this.random.Next() % 2) == 1;
            }
        }

        /// <summary>
        /// Gets a random number in the interval [0,0x7fffffff].
        /// </summary>
        /// <value>A random integer.</value>
        public int RandomInteger
        {
            get
            {
                return this.random.Next();
            }
        }

        /// <summary>
        /// Gets a random number in the interval [0.0, 1.0].
        /// </summary>
        /// <value>A random single-precision floating point value.</value>
        public float RandomSingle
        {
            get
            {
                return (float)this.random.NextDouble();
            }
        }

        /// <summary>
        /// Gets a random number in the interval [0.0, 1.0].
        /// </summary>
        /// <value>A random double-precision floating point value.</value>
        public double RandomDouble
        {
            get
            {
                return this.random.NextDouble();
            }
        }

        /// <summary>
        /// Gets a random number in the interval [0.0, 1.0].
        /// </summary>
        /// <value>A random decimal value.</value>
        public decimal RandomDecimal
        {
            get
            {
                return (decimal)this.random.NextDouble();
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="RandSys"/> class;
        /// using the time of day in milliseconds as the seed.
        /// </summary>
        public RandSys()
        {
            this.random = new System.Random();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandSys"/> class; using the given <paramref name="seed"/>.
        /// </summary>
        /// <param name="seed">
        /// A number used to calculate a starting value for the pseudo-random number
        /// sequence. If a negative number is specified, the absolute value of the number
        /// is used.
        /// </param>
        public RandSys( int seed )
        {
            this.random = new System.Random( seed );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The System.Random object this <see cref="RandSys"/> is wrapping around.
        /// </summary>
        private readonly System.Random random;

        #endregion
    }
}
