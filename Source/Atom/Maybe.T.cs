// <copyright file="Maybe.T.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Maybe{T} class.
// </summary>
// <author>
//     Paul Ennemoser
//     original version by Rinat Abdullin 
//         (http://abdullin.com/journal/2009/10/6/zen-development-practices-c-maybe-monad.html)
// </author>

namespace Atom
{
    using System;

    /// <summary>
    /// Provides static utility method for the <see cref="Maybe{T}"/> structure.
    /// </summary>
    public static class Maybe
    {
        /// <summary>
        /// Gets an instance of the Maybe monad that contains "No Value".
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value.
        /// </typeparam>
        /// <returns>
        /// The instance of the Maybe monad.
        /// </returns>
        public static Maybe<T> None<T>()
        {
            return Maybe<T>.None;
        }

        /// <summary>
        /// Gets an instance of the Maybe monad that contains
        /// the specified <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value.
        /// </typeparam>
        /// <param name="value">
        /// The value that the new instance of the Maybe monad
        /// should have.
        /// </param>
        /// <returns>
        /// The instance of the Maybe monad.
        /// </returns>
        public static Maybe<T> Some<T>( T value )
        {
            return new Maybe<T>( value );
        }
    }

    /// <summary>
    /// Represents an inmutable value that might or might not have any value.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the actual value.
    /// </typeparam>
    [Serializable]
    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        /// <summary>
        /// Represents an empty instance of type <typeparamref name="T"/>.
        /// </summary>
        public static readonly Maybe<T> None = new Maybe<T>();

        /// <summary>
        /// Gets the underlying value, if it is available.
        /// </summary>
        public T Value
        {
            get
            {
                if( !this.hasValue )
                {
                    throw new InvalidOperationException( ErrorStrings.MaybeHasNoValue );
                }

                return this.value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has value.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has value; otherwise, 
        /// <c>false</c>.
        /// </value>
        public bool HasValue
        {
            get
            {
                return this.hasValue;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the Maybe{T} structure.
        /// </summary>
        /// <param name="value">
        /// The actual value.
        /// </param>
        public Maybe( T value )
        {
            this.value = value;
            this.hasValue = true;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object.
        /// </summary>
        /// <param name="obj">
        /// An object to compare with this object.
        /// </param>
        /// <returns>
        /// true if the current object is equal to the other parameter; otherwise, false.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is Maybe<T> )
            {
                return base.Equals( (Maybe<T>)obj );
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        /// <returns>
        /// true if the current object is equal to the other parameter; otherwise, false.
        /// </returns>
        public bool Equals( Maybe<T> other )
        {
            if( !this.hasValue )
            {
                return !other.hasValue;
            }

            if( this.value == null )
            {
                return other.value == null;
            }

            return this.value.Equals( other.value );
        }

        /// <summary>
        /// Returns the hashcode of this Maybe{T} instance.
        /// </summary>
        /// <returns>
        /// The hashcode.
        /// </returns>
        public override int GetHashCode()
        {
            if( this.hasValue )
            {
                return this.value != null ? this.value.GetHashCode() : 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Represents the storage field of the <see cref="Value"/> property.
        /// </summary>
        private readonly T value;

        /// <summary>
        /// Represents the storage field of the <see cref="HasValue"/> property.
        /// </summary>
        private readonly bool hasValue;
    }
}