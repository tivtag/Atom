// <copyright file="IntegerRange.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.IntegerRange structure.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a range of intergers that lie within
    /// a given [Minimum; Maximum].
    /// </summary>
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.IntegerRangeConverter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct IntegerRange : IEquatable<IntegerRange>, ICultureSensitiveToStringProvider
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the minimum value of this <see cref="IntegerRange"/>.
        /// </summary>
        public int Minimum
        {
            get
            {
                // Contract.Ensures( Contract.Result<int>() <= this.Maximum );

                return this.minimum;
            }

            set
            {
                Contract.Requires<ArgumentException>( value <= this.Maximum );

                this.minimum = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of this <see cref="IntegerRange"/>.
        /// </summary>
        public int Maximum
        {
            get
            {
                // Contract.Ensures( Contract.Result<int>() >= this.Minimum );

                return this.maximum;
            }

            set
            {
                Contract.Requires<ArgumentException>( value >= this.Minimum );

                this.maximum = value;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the IntegerRange structure.
        /// </summary>
        /// <param name="minimum">
        /// The minimum value of the new IntegerRange.
        /// </param>
        /// <param name="maximum">
        /// The maximum value of the new IntegerRange.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="minimum"/> is greater than <paramref name="maximum"/>.
        /// </exception>
        public IntegerRange( int minimum, int maximum )
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
        /// A value that lies within this IntegerRange.
        /// </returns>
        public int GetRandomValue( IRand rand )
        {
            Contract.Requires<ArgumentNullException>( rand != null );
            // Contract.Ensures( Contract.Result<int>() >= this.minimum );
            // Contract.Ensures( Contract.Result<int>() <= this.maximum );

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
        public bool Contains( int value )
        {
            return this.minimum <= value && value <= this.maximum;
        }

        /// <summary>
        /// Gets the hash code for this IntegerRange instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.minimum );
            hashBuilder.AppendStruct( this.maximum );

            return hashBuilder.GetHashCode();
        }
        
        #region > Operators <

        /// <summary>
        /// Returns the result of adding the right IntegerRange to the left IntegerRange.
        /// </summary>
        /// <param name="left">
        /// The IntegerRange on the left side.
        /// </param>
        /// <param name="right">
        /// The IntegerRange on the right side.
        /// </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static IntegerRange operator+( IntegerRange left, IntegerRange right )
        {
            return new IntegerRange(
                left.minimum + right.minimum,
                left.maximum + right.maximum
            );
        }

        /// <summary>
        /// Returns the result of adding a value to an IntegerRange.
        /// </summary>
        /// <param name="range">
        /// The IntegerRange on the left side.
        /// </param>
        /// <param name="value">
        /// The value on the right side.
        /// </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static IntegerRange operator+( IntegerRange range, int value )
        {
            return new IntegerRange(
                range.minimum + value,
                range.maximum + value
            );
        }

        /// <summary>
        /// Returns the result of adding the right IntegerRange to the left IntegerRange.
        /// </summary>
        /// <param name="left">
        /// The IntegerRange on the left side.
        /// </param>
        /// <param name="right">
        /// The IntegerRange on the right side.
        /// </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static IntegerRange Add( IntegerRange left, IntegerRange right )
        {
            return left + right;
        }

        /// <summary>
        /// Returns the result of adding a value to an IntegerRange.
        /// </summary>
        /// <param name="range">
        /// The IntegerRange on the left side.
        /// </param>
        /// <param name="value">
        /// The value on the right side.
        /// </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static IntegerRange Add( IntegerRange range, int value )
        {
            return range + value;
        }

        #endregion

        #region > Equality <

        /// <summary>
        /// Returns whether the given IntegerRange is equal to this IntegerRange.
        /// </summary>
        /// <param name="other">
        /// The IntegerRange to compare with this IntegerRange.
        /// </param>
        /// <returns>
        /// True if they are equal; otherwise false.
        /// </returns>
        public bool Equals( IntegerRange other )
        {
            return this.minimum == other.minimum && this.maximum == other.maximum;
        }

        /// <summary>
        /// Returns whether the given Object is equal to this IntegerRange.
        /// </summary>
        /// <param name="obj">
        /// The Object to compare with this IntegerRange.
        /// </param>
        /// <returns>
        /// True if they are equal; otherwise false.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is IntegerRange )
            {
                return this.Equals( (IntegerRange)obj );
            }

            return false;
        }

        /// <summary>
        /// Returns whether the specified <see cref="IntegerRange"/> instances are equal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator ==( IntegerRange left, IntegerRange right )
        {
            return left.Equals( right );
        }

        /// <summary>
        /// Returns whether the specified <see cref="IntegerRange"/> instances are inequal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator !=( IntegerRange left, IntegerRange right )
        {
            return !left.Equals( right );
        }

        #endregion

        #region > String Conversation <

        /// <summary>
        /// Overriden to return a human-readable text representation of this IntegerRange.
        /// </summary>
        /// <returns>A human-readable text representation of this IntegerRange.</returns>
        public override string ToString()
        {
            return this.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of this IntegerRange.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of this IntegerRange.</returns>
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
        private int minimum;

        /// <summary>
        /// The storage field of the <see cref="Maximum"/> property.
        /// </summary>
        private int maximum;

        #endregion
    }
}