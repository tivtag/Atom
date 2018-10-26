// <copyright file="RandMT.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.RandMT class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Generates pseudo-random numbers from a uniform distribution
    /// using the Mersenne Twister algorithm. 
    /// This class can't be inherited.
    /// </summary>
    /// <remarks>
    /// This is the 32-bit version of the algorithm. 
    /// </remarks>
    public sealed class RandMT : IRand
    {
        #region [ Properties ]

        /// <summary>
        /// Gets a value indicating whether this random number generator has been initialized (seeded).
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if it has been initialised; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsInitialized
        {
            get
            {
                return this.mti != (N + 1);
            }
        }

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
                return (this.RandomUnsignedInteger % 2) == 1;
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
                return (int)(this.RandomUnsignedInteger >> 1);
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
                return this.RandomUnsignedInteger * (1.0f / 4294967295.0f); // divided by 2^32-1
            }
        }

        /// <summary>
        /// Gets a random number in the interval [0.0, 1.0) with 53-bit resolution.
        /// </summary>
        /// <value>A random single-precision floating point value.</value>
        public float RandomSingleRes53
        {
            get
            {
                uint a = this.RandomUnsignedInteger >> 5;
                uint b = this.RandomUnsignedInteger >> 6;

                return ((a * 67108864.0f) + b) * (1.0f / 9007199254740992.0f);
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
                return this.RandomUnsignedInteger * (1.0d / 4294967295.0d); // divided by 2^32-1
            }
        }

        /// <summary>
        /// Gets a random number in the interval [0.0, 1.0) with 53-bit resolution.
        /// </summary>
        /// <value>A random double-precision floating point value.</value>
        public double RandomDoubleRes53
        {
            get
            {
                uint a = this.RandomUnsignedInteger >> 5;
                uint b = this.RandomUnsignedInteger >> 6;

                return ((a * 67108864.0d) + b) * (1.0d / 9007199254740992.0d);
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
                return this.RandomUnsignedInteger * (1.0m / 4294967295.0m); // divided by 2^32-1
            }
        }

        /// <summary> 
        /// Gets a random number in the interval [0.0, 1.0) with 53-bit resolution.
        /// </summary>
        /// <value>A random decimal value.</value>
        public decimal RandomDecimalRes53
        {
            get
            {
                uint a = this.RandomUnsignedInteger >> 5;
                uint b = this.RandomUnsignedInteger >> 6;

                return ((a * 67108864.0m) + b) * (1.0m / 9007199254740992.0m);
            }
        }

        #region UnsignedInteger

        /// <summary>
        /// Gets a random number in the interval [0,0xffffffff].
        /// Doesn't check if the random number generator has been initialized.
        /// </summary>
        /// <remarks>
        /// This is the main function which is used to generate random numbers.
        /// </remarks>
        /// <value>A random unsigned integer.</value>
        [System.CLSCompliant( false )]
        public uint RandomUnsignedInteger
        {
            get
            {
                uint y;

                if( this.mti >= N )
                {
                    int kk;

                    for( kk = 0; kk < NminM; ++kk )
                    {
                        y = (this.mt[kk] & UpperMask) | (this.mt[kk + 1] & LowerMask);
                        this.mt[kk] = this.mt[kk + M] ^ (y >> 1) ^ this.mag01[y & 0x1U];
                    }

                    for(; kk < Nmin1; ++kk )
                    {
                        y = (this.mt[kk] & UpperMask) | (this.mt[kk + 1] & LowerMask);
                        this.mt[kk] = this.mt[kk + MminN] ^ (y >> 1) ^ this.mag01[y & 0x1U];
                    }

                    y = (this.mt[Nmin1] & UpperMask) | (this.mt[0] & LowerMask);
                    this.mt[Nmin1] = this.mt[Mmin1] ^ (y >> 1) ^ this.mag01[y & 0x1U];

                    this.mti = 0;
                }

                y = this.mt[this.mti++];

                // Tempering
                y ^= y >> 11;
                y ^= (y <<  7) & 0x9d2c5680U;
                y ^= (y << 15) & 0xefc60000U;
                y ^= y >> 18;

                return y;
            }
        }

        #endregion

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="RandMT"/> class;
        /// by using the time of day in milliseconds as the seed.
        /// </summary>
        public RandMT()
        {
            this.Seed( (uint)DateTime.Now.Millisecond );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandMT"/> class;
        /// using the given <paramref name="seed"/>.
        /// </summary>
        /// <param name="seed">
        /// The seed used to initialize the pseudo random number generator.
        /// </param>
        public RandMT( int seed )
        {
            this.Seed( (uint)seed );
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="RandMT"/> class;
        /// using the given <paramref name="seed"/>.
        /// </summary>
        /// <param name="seed">
        /// The seed used to initialize the pseudo random number generator.
        /// </param>
        [System.CLSCompliant( false )]
        public RandMT( uint seed )
        {
            this.Seed( seed );
        }

        /// <summary>
        /// Seeds this <see cref="RandMT"/> using the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The seed used to initialize the pseudo random number generator.
        /// </param>
        [CLSCompliant( false )]
        public void Seed( uint value )
        {
            // Contract.Ensures( this.IsInitialized );

            this.mt[0] = value & 0xffffffffU;

            for( this.mti = 1; this.mti < N; ++this.mti )
            {
                this.mt[this.mti] = (uint)((1812433253U * (this.mt[this.mti - 1] ^ (this.mt[this.mti - 1] >> 30))) + this.mti);

                // for >32 bit machines
                this.mt[this.mti] &= 0xffffffffU;
            }
        }

        /// <summary>
        /// Seeds this <see cref="RandMT"/> using the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The seed used to initialize the pseudo random number generator.
        /// </param>
        public void Seed( int value )
        {
            this.Seed( (uint)value );
        }

        #endregion

        #region [ Constants ]

        /// <summary>
        /// The size of the internal state array (624).
        /// </summary>
        private const int N = 624;

        /// <summary>
        /// The size of the internal state array, minus one (623).
        /// </summary>
        private const int Nmin1 = 623;

        /// <summary>
        /// A constant that is used to index into the internal state array (397).
        /// </summary>
        private const int M = 397;

        /// <summary>
        /// The <see cref="M"/> constant, minus one (396).
        /// </summary>
        private const int Mmin1 = 396;

        /// <summary>
        /// The <see cref="N"/> constant minus the <see cref="M"/> constant (624 - 397).
        /// </summary>
        private const int NminM = N - M;

        /// <summary>
        /// The <see cref="M"/> constant minus the <see cref="N"/> constant (397 - 624).
        /// </summary>
        private const int MminN = M - N;

        /// <summary>
        /// The upper mask constant (0x80000000U).
        /// </summary>
        private const uint UpperMask = 0x80000000U;

        /// <summary>
        /// The lower mask constant (0x7fffffffU).
        /// </summary>
        private const uint LowerMask = 0x7fffffffU;

        /// <summary>
        /// The maximum random integer that can be created (0x7fffffff).
        /// </summary>
        private const int MaxRandomInt = 0x7fffffff;

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Stores the range of generated values.
        /// mag01[x] = x * MATRIX_A. for x = 0,1.
        /// </summary>
        private uint[] mag01 = { 0x0U, 0x9908b0dfU };

        /// <summary>
        /// The state point array.
        /// </summary>
        private uint[] mt = new uint[N];

        /// <summary>
        /// If mti==N+1 then that means that mt[N] has not yet been initialized.
        /// </summary>
        private int mti = N + 1;

        #endregion
    }
}

