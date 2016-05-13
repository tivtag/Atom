// <copyright file="TimeTick.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Story.TimeTick structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Story
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Represents a quantifized time position on a Timeline.
    /// </summary>
    [CLSCompliant( false )]
    [TypeConverter( typeof( Atom.Story.Design.TimeTickConverter ) )]
    public struct TimeTick : IComparable<TimeTick>, IEquatable<TimeTick>
    {
        /// <summary>
        /// The number of seconds between two TimeTicks: 0.5
        /// </summary>
        public const float TimeQuantity = 0.5f;

        /// <summary>
        /// Gets an TimeTick instance that represents the start of a timeline.
        /// </summary>
        public static TimeTick Zero
        {
            get
            {
                return new TimeTick();
            }
        }

        /// <summary>
        /// The value that encodes the position of this TimeTick.
        /// </summary>
        public readonly uint Value;

        /// <summary>
        /// Gets the seconds this TimeTick instance represents.
        /// </summary>
        public double Seconds
        {
            get
            {
                return this.Value * TimeQuantity;
            }
        }

        /// <summary>
        /// Gets the minutes this TimeTick instance represents.
        /// </summary>
        public double Minutes
        {
            get
            {
                return this.Seconds / 60.0f;
            }
        }

        /// <summary>
        /// Initializes a new instance of the TimeTick structure.
        /// </summary>
        /// <param name="value">
        /// Encodes the position of the new TimeTick.
        /// </param>
        [CLSCompliant( false )]
        public TimeTick( uint value )
        {
            this.Value = value;
        }

        /// <summary>
        /// Creates a new instance of the TimeTick structure,
        /// converting the given amount of seconds into tick time.
        /// </summary>
        /// <param name="seconds">
        /// The seconds to convert.
        /// </param>
        /// <returns>
        /// The converted TimeTick-
        /// </returns>
        public static TimeTick FromSeconds( uint seconds )
        {
            return new TimeTick( seconds * 2 );
        }

        /// <summary>
        /// Indicates whether the specified object is equal
        /// to this TimeTick instance.
        /// </summary>
        /// <param name="obj">
        /// The object to compare to.
        /// </param>
        /// <returns>
        /// true if they are equal; otherwise false.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is TimeTick )
            {
                return this.Equals( (TimeTick)obj );
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the specified TimeTick instance is equal
        /// to this TimeTick instance.
        /// </summary>
        /// <param name="other">
        /// The other TimeTick instance to compare to.
        /// </param>
        /// <returns>
        /// true if they are equal; otherwise false.
        /// </returns>
        public bool Equals( TimeTick other )
        {
            return this.Value == other.Value;
        }

        /// <summary>
        /// Gets the hashcode of this TimeTick instance.
        /// </summary>
        /// <returns>
        /// The hashcode of this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of this TimeTick instance.
        /// </summary>
        /// <returns>
        /// A string representation of this TimeTick instance.
        /// </returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        /// <summary>
        /// Compares this TimeTick instance to the specified TimeTick instance.
        /// </summary>
        /// <param name="other">
        /// The other TimeTick instance to compare to.
        /// </param>
        /// <returns>
        /// A value representing how this TimeTick instance is placed relative
        /// to the specified TimeTick instance.
        /// </returns>
        public int CompareTo( TimeTick other )
        {
            return this.Value.CompareTo( other.Value );
        }

        /// <summary>
        /// Implicitely converts an unsigned integer into a TimeTick instance.
        /// </summary>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted TimeTick.
        /// </returns>
        [CLSCompliant( false )]
        public static implicit operator TimeTick( uint value )
        {
            return new TimeTick( value );
        }

        /// <summary>
        /// Returns the result of substracting the TimeTick on the right side
        /// from the TimeTick on the left side.
        /// </summary>
        /// <param name="left">
        /// The TimeTick on the left side of the operator.
        /// </param>
        /// <param name="right">
        /// The TimeTick on the right side of the operator.
        /// </param>
        /// <returns>
        /// The result of the substraction.
        /// </returns>
        public static TimeTick operator -( TimeTick left, TimeTick right )
        {
            if( right.Value > left.Value )
            {
                return new TimeTick();
            }

            return new TimeTick( left.Value - right.Value );
        }

        /// <summary>
        /// Returns the result of adding the TimeTick on the right side
        /// to the TimeTick on the left side.
        /// </summary>
        /// <param name="left">
        /// The TimeTick on the left side of the operator.
        /// </param>
        /// <param name="right">
        /// The TimeTick on the right side of the operator.
        /// </param>
        /// <returns>
        /// The result of the addition.
        /// </returns>
        public static TimeTick operator +( TimeTick left, TimeTick right )
        {
            return new TimeTick( left.Value + right.Value );
        }

        /// <summary>
        /// Returns a value indicating whether the specified TimeTick on the left side of the comparison
        /// is less than the TimeTick on the right side.
        /// </summary>
        /// <param name="left">
        /// The TimeTick on the left side of the comparison.
        /// </param>
        /// <param name="right">
        /// The TimeTick on the right side of the comparison.
        /// </param>
        /// <returns>
        /// true if the left TimeTick is less than the right TimeTick;
        /// -or- otherwise false.
        /// </returns>
        public static bool operator <( TimeTick left, TimeTick right )
        {
            return left.Value < right.Value;
        }

        /// <summary>
        /// Returns a value indicating whether the specified TimeTick on the left side of the comparison
        /// is less than or equal the TimeTick on the right side.
        /// </summary>
        /// <param name="left">
        /// The TimeTick on the left side of the comparison.
        /// </param>
        /// <param name="right">
        /// The TimeTick on the right side of the comparison.
        /// </param>
        /// <returns>
        /// true if the left TimeTick is less than or equal the right TimeTick;
        /// -or- otherwise false.
        /// </returns>
        public static bool operator <=( TimeTick left, TimeTick right )
        {
            return left.Value <= right.Value;
        }

        /// <summary>
        /// Returns a value indicating whether the specified TimeTick on the left side of the comparison
        /// is greater than the TimeTick on the right side.
        /// </summary>
        /// <param name="left">
        /// The TimeTick on the left side of the comparison.
        /// </param>
        /// <param name="right">
        /// The TimeTick on the right side of the comparison.
        /// </param>
        /// <returns>
        /// true if the left TimeTick is greater than the right TimeTick;
        /// -or- otherwise false.
        /// </returns>
        public static bool operator >( TimeTick left, TimeTick right )
        {
            return left.Value > right.Value;
        }

        /// <summary>
        /// Returns a value indicating whether the specified TimeTick on the left side of the comparison
        /// is greater than or equal the TimeTick on the right side.
        /// </summary>
        /// <param name="left">
        /// The TimeTick on the left side of the comparison.
        /// </param>
        /// <param name="right">
        /// The TimeTick on the right side of the comparison.
        /// </param>
        /// <returns>
        /// true if the left TimeTick is greater than or equal the right TimeTick;
        /// -or- otherwise false.
        /// </returns>
        public static bool operator >=( TimeTick left, TimeTick right )
        {
            return left.Value >= right.Value;
        }

        /// <summary>
        /// Returns a value indicating whether the specified TimeTick instances
        /// represents the same tick.
        /// </summary>
        /// <param name="left">
        /// The TimeTick on the left side of the comparison.
        /// </param>
        /// <param name="right">
        /// The TimeTick on the right side of the comparison.
        /// </param>
        /// <returns>
        /// true if the left TimeTick is equal to the right TimeTick;
        /// -or- otherwise false.
        /// </returns>
        public static bool operator ==( TimeTick left, TimeTick right )
        {
            return left.Value == right.Value;
        }

        /// <summary>
        /// Returns a value indicating whether the specified TimeTick instances
        /// do not represents the same tick.
        /// </summary>
        /// <param name="left">
        /// The TimeTick on the left side of the comparison.
        /// </param>
        /// <param name="right">
        /// The TimeTick on the right side of the comparison.
        /// </param>
        /// <returns>
        /// true if the left TimeTick is not equal to the right TimeTick;
        /// -or- otherwise false.
        /// </returns>
        public static bool operator !=( TimeTick left, TimeTick right )
        {
            return left.Value != right.Value;
        }
    }
}
