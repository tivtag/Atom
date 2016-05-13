// <copyright file="FloatRange.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.FloatRange structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a range of single-precision floating-point numbers that lie within [Minimum; Maximum].
    /// </summary>
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.FloatRangeConverter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct FloatRange : IEquatable<FloatRange>, ICultureSensitiveToStringProvider
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the minimum value of this <see cref="FloatRange"/>.
        /// </summary>
        public float Minimum
        {
            get
            {
                Contract.Ensures( Contract.Result<float>() <= this.Maximum );

                return this.minimum;
            }

            set
            {
                Contract.Requires<ArgumentException>( value <= this.Maximum );

                this.minimum = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of this <see cref="FloatRange"/>.
        /// </summary>
        public float Maximum
        {
            get
            {
                Contract.Ensures( Contract.Result<float>() >= this.Minimum );

                return this.maximum;
            }

            set
            {
                Contract.Requires<ArgumentException>( value >= this.Minimum );

                this.maximum = value;
            }
        }

        /// <summary>
        /// Gets the middle point of this FloatRange.
        /// </summary>
        public float Middle
        {
            get
            {
                return (this.minimum + this.maximum) * 0.5f;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the FloatRange structure.
        /// </summary>
        /// <param name="minimum">
        /// The minimum value of the new FloatRange.
        /// </param>
        /// <param name="maximum">
        /// The maximum value of the new FloatRange.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="minimum"/> is greater than <paramref name="maximum"/>.
        /// </exception>
        public FloatRange( float minimum, float maximum )
        {
            Contract.Requires<ArgumentException>( maximum >= minimum );

            this.minimum = minimum;
            this.maximum = maximum;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets a random value that lies within [<see cref="Minimum"/>; <see cref="Maximum"/>].
        /// </summary>
        /// <param name="rand">
        /// The random number generator to use.
        /// </param>
        /// <returns>
        /// A value that lies within this FloatRange.
        /// </returns>
        public float GetRandomValue( IRand rand )
        {
            Contract.Requires<ArgumentNullException>( rand != null );
            Contract.Ensures( Contract.Result<float>() >= this.minimum );
            Contract.Ensures( Contract.Result<float>() <= this.maximum );

            return rand.UncheckedRandomRange( this.minimum, this.maximum );
        }

        /// <summary>
        /// Gets a value indicating whether the given value is within the inteval [minimum..maximum].
        /// </summary>
        /// <param name="value">
        /// The value to check.
        /// </param>
        /// <returns>
        /// True if the value is contained in this range; -or- otherwise false.
        /// </returns>
        public bool Contains( float value )
        {
            return this.minimum <= value && value <= this.maximum;
        }

        /// <summary>
        /// Gets the hash code for this FloatRange instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed hash code.
        /// </returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.minimum );
            hashBuilder.AppendStruct( this.maximum );

            return hashBuilder.GetHashCode();
        }

        /// <summary>
        /// Explicitely converts a VariableFloat into a FloatRange.
        /// </summary>
        /// <param name="variableFloat">
        /// The VariableFloat to convert.
        /// </param>
        /// <returns>
        /// The converted FloatRange.
        /// </returns>
        public static explicit operator FloatRange( VariableFloat variableFloat )
        {
            float difference = variableFloat.Difference;
            float minimum = variableFloat.Anchor - difference;
            float maximum = variableFloat.Anchor + difference;

            if( minimum > maximum )
            {
                float temp = maximum;
                maximum = minimum;
                minimum = temp;
            }

            return new FloatRange( minimum, maximum );
        }

        #region > Equality <

        /// <summary>
        /// Returns whether the given FloatRange is equal to this FloatRange.
        /// </summary>
        /// <param name="other">
        /// The FloatRange to compare with this FloatRange.
        /// </param>
        /// <returns>
        /// True if they are equal; otherwise false.
        /// </returns>
        public bool Equals( FloatRange other )
        {
            return this.minimum.IsApproximate( other.minimum ) && this.maximum.IsApproximate( other.maximum );
        }

        /// <summary>
        /// Returns whether the given Object is equal to this FloatRange.
        /// </summary>
        /// <param name="obj">
        /// The Object to compare with this FloatRange.
        /// </param>
        /// <returns>
        /// True if they are equal; otherwise false.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is FloatRange )
            {
                return this.Equals( (FloatRange)obj );
            }

            return false;
        }

        /// <summary>
        /// Returns whether the specified <see cref="FloatRange"/> instances are equal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator==( FloatRange left, FloatRange right )
        {
            return left.Equals( right );
        }

        /// <summary>
        /// Returns whether the specified <see cref="FloatRange"/> instances are inequal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator!=( FloatRange left, FloatRange right )
        {
            return !left.Equals( right );
        }

        #endregion

        #region > String Conversation <

        /// <summary>
        /// Overriden to return a human-readable text representation of this FloatRange.
        /// </summary>
        /// <returns>A human-readable text representation of this FloatRange.</returns>
        public override string ToString()
        {
            return this.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of this FloatRange.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of this FloatRange.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format(
                formatProvider,
                "[{0}; {1}]",
                this.minimum.ToString( formatProvider ),
                this.maximum.ToString( formatProvider )
            );
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The storage field of the <see cref="Minimum"/> property.
        /// </summary>
        private float minimum;

        /// <summary>
        /// The storage field of the <see cref="Maximum"/> property.
        /// </summary>
        private float maximum;

        #endregion
    }
}
