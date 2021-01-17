// <copyright file="ChangedValue.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.ChangedValue structure.</summary>
// <author>Paul Ennemoser</author>

namespace Atom
{
    using System;

    /// <summary>
    /// Defines a container that stores information on the change of a value.
    /// </summary>
    /// <remarks>
    /// These are no <see cref="EventArgs"/> because of perfomance reasons.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the value beeing changed.
    /// </typeparam>
    public struct ChangedValue<T> : IEquatable<ChangedValue<T>>
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the old value.
        /// </summary>
        /// <value>The old value; before the change.</value>
        public T OldValue
        {
            get
            { 
                return this.oldValue;
            }
        }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        /// <value>The new value; after the change.</value>
        public T NewValue
        {
            get
            { 
                return this.newValue; 
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangedValue{T}"/> struct.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public ChangedValue( T oldValue, T newValue )
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns the hash code of this <see cref="ChangedValue{T}"/> instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.Append<T>( this.oldValue );
            hashBuilder.Append<T>( this.newValue );

            return hashBuilder.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is
        /// equal to this <see cref="ChangedValue{T}"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to test this instance against.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public override bool Equals( object obj )
        {
            if( obj is ChangedValue<T> )
                return this.Equals( (ChangedValue<T>)obj );
            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="ChangedValue{T}"/> is
        /// equal to this <see cref="ChangedValue{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="ChangedValue{T}"/> to test this instance against.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public bool Equals( ChangedValue<T> other )
        {
            return (this.OldValue == null ? other.OldValue == null : this.OldValue.Equals( other.OldValue )) &&
                   (this.NewValue == null ? other.NewValue == null : this.NewValue.Equals( other.NewValue ));
        }

        /// <summary>
        /// Returns whether the specified <see cref="ChangedValue{T}"/> instances are equal.
        /// </summary>
        /// <param name="left">The <see cref="ChangedValue{T}"/> instance on the left side of the equation.</param>
        /// <param name="right">The <see cref="ChangedValue{T}"/> instance on the right side of the equation.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public static bool operator ==( ChangedValue<T> left, ChangedValue<T> right )
        {
            return left.Equals( right );
        }

        /// <summary>
        /// Returns whether the specified <see cref="ChangedValue{T}"/> instances are not equal.
        /// </summary>
        /// <param name="left">The <see cref="ChangedValue{T}"/> instance on the left side of the equation.</param>
        /// <param name="right">The <see cref="ChangedValue{T}"/> instance on the right side of the equation.</param>
        /// <returns>true if they are not equal; otherwise false.</returns>
        public static bool operator !=( ChangedValue<T> left, ChangedValue<T> right )
        {
            return !left.Equals( right );
        }

        /// <summary> 
        /// Returns a string representation of this <see cref="ChangedValue{T}"/>.
        /// </summary>
        /// <returns>
        /// A human readable string representation of this <see cref="ChangedValue{T}"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                System.Globalization.CultureInfo.CurrentCulture,
                "{0}; {1}",
                this.oldValue == null ? "null" : this.oldValue.ToString(),
                this.newValue == null ? "null" : this.newValue.ToString()
            );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The old value; before the change.
        /// </summary>
        private readonly T oldValue;

        /// <summary>
        /// The new value; after the change.
        /// </summary>
        private readonly T newValue;

        #endregion
    }
}
