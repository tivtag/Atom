// <copyright file="VariableFloat.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.VariableFloat structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    /// <summary>
    /// Represents a single-precision floating-point number that
    /// varier over a specific range of values.
    /// </summary>
    /// <seealso cref="FloatRange"/>
    public struct VariableFloat : System.IEquatable<VariableFloat>, ICultureSensitiveToStringProvider
    {
        /// <summary>
        /// Gets or sets the Anchor at which this VariableFloat starts.
        /// </summary>
        public float Anchor
        {
            get 
            {
                return this.anchor; 
            }

            set 
            {
                this.anchor = value;
            }
        }

        /// <summary>
        /// Gets or sets the 'range' of this VariableFloat.
        /// </summary>
        public float Variation
        {
            get 
            {
                return this.variation; 
            }
            
            set 
            {
                this.variation = value;
            }
        }

        /// <summary>
        /// Gets the difference starting from the anchor of this VariableFloat.
        /// </summary>
        public float Difference
        {
            get
            {
                return this.variation * this.anchor;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableFloat"/> structure.
        /// </summary>
        /// <param name="anchor">
        /// The initial anchor value of the new VariableFloat.
        /// </param>
        /// <param name="variation">
        /// The initial variation value of the new VariableFloat.
        /// </param>
        public VariableFloat( float anchor, float variation )
        {
            this.anchor = anchor;
            this.variation = variation;
        }

        /// <summary>
        /// Gets a randomis value that lies within this VariableFloat.
        /// </summary>
        /// <param name="rand">
        /// The random number generator to use.
        /// </param>
        /// <returns>
        /// A random floating-point values that lies within 
        /// [Anchor - (Anchor * Variation)] and
        /// [Anchor + (Anchor * Variation)].
        /// </returns>
        public float GetRandomValue( IRand rand )
        {
            if( this.Variation <= float.Epsilon )
            {
                return this.Anchor;
            }

            float difference = this.Variation * this.Anchor;
            float minimum = this.Anchor - difference;
            float maximum = this.Anchor + difference;

            return rand.RandomRange( minimum, maximum );
        }

        /// <summary>
        /// Implicit cast operator from float to VariableFloat.
        /// </summary>
        /// <param name="value">
        /// The floating-point value to convert into a VariableFloat.
        /// </param>
        public static implicit operator VariableFloat( float value )
        {
            return new VariableFloat( value, .0f );
        }

        /// <summary>
        /// Determines whether the specified Object is equal to this VariableFloat.
        /// </summary>
        /// <param name="obj">
        /// The Object to compare this VariableFloat against.
        /// </param>
        /// <returns>
        /// true if they are equal;
        /// otherwise false.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is VariableFloat )
                return this.Equals( (VariableFloat)obj );

            return false;
        }

        /// <summary>
        /// Determines whether the specified VariableFloat is equal to this VariableFloat.
        /// </summary>
        /// <param name="other">
        /// The other VariableFloat to compare this VariableFloat against.
        /// </param>
        /// <returns>
        /// true if they are equal;
        /// otherwise false.
        /// </returns>
        public bool Equals( VariableFloat other )
        {
            return this.anchor.IsApproximate( other.anchor ) &&
                   this.variation.IsApproximate( other.variation );
        }

        /// <summary>
        /// Returns whether the specified <see cref="VariableFloat"/> instances are equal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator==( VariableFloat left, VariableFloat right )
        {
            return left.Equals( right );
        }

        /// <summary>
        /// Returns whether the specified <see cref="VariableFloat"/> instances are inequal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator!=( VariableFloat left, VariableFloat right )
        {
            return !left.Equals( right );
        }

        /// <summary>
        /// Gets the hash code of this VariableFloat.
        /// </summary>
        /// <returns>
        /// The hash code.
        /// </returns>
        public override int GetHashCode()
        {
            var builder = new HashCodeBuilder();

            builder.AppendStruct( this.anchor );
            builder.AppendStruct( this.variation );

            return builder.GetHashCode();
        }

        /// <summary>
        /// Returns a human-readable representation of this VariableFloat.
        /// </summary>
        /// <returns>
        /// A string descriping this VariableFloat.
        /// </returns>
        public override string ToString()
        {
            return this.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable representation of this VariableFloat.
        /// </summary>
        /// <param name="formatProvider">
        /// The System.IFormatProvider that should be used to format the fields
        /// of this VariableFloat.
        /// </param>
        /// <returns>
        /// A string descriping this VariableFloat.
        /// </returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format(
                formatProvider,
                "VariableFloat( anchor={0} variation={1} )",
                this.anchor.ToString( formatProvider ),
                this.variation.ToString( formatProvider )
            );
        }

        /// <summary>
        /// The storage field of the <see cref="Anchor"/> property.
        /// </summary>
        private float anchor;

        /// <summary>
        /// The storage field of the <see cref="Variation"/> property.
        /// </summary>
        private float variation;
    }
}