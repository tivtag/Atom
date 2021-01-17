// <copyright file="Association.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Association{TKey, TValue} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The Association performs the same function as a <see cref="KeyValuePair{TKey, TValue}"/>,
    /// but allows the <see cref="Key"/> and <see cref="Value"/> properties to be written to
    /// and is a reference type.
    /// </summary>
    /// <typeparam name="TKey">The type of the key for the association.</typeparam>
    /// <typeparam name="TValue">The type of the value for the association.</typeparam>
    [Serializable]
    public class Association<TKey, TValue>
    {      
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the key of this <see cref="Association{TKey, TValue}"/>.
        /// </summary>
        /// <value>
        /// The key that is associated to the <see cref="Value"/>.
        /// </value>
        public TKey Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of this <see cref="Association{TKey, TValue}"/>.
        /// </summary>
        /// <value>
        /// The value that is associated to the <see cref="Key"/>.
        /// </value>
        public TValue Value
        {
            get;
            set;
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Association{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="key">
        /// The key that is associated to the <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        /// The value that is associated to the <paramref name="key"/>.
        /// </param>
        public Association( TKey key, TValue value )
        {
            this.Key = key;
            this.Value = value;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Construct a KeyValuePair{TKey, TValue} object from this Association{TKey, TValue}.
        /// </summary>
        /// <returns>A key value pair representation of this <see cref="Association{TKey,TValue}"/>.</returns>
        public KeyValuePair<TKey, TValue> ToKeyValuePair()
        {
            return new KeyValuePair<TKey, TValue>( this.Key, this.Value );
        }

        /// <summary>
        /// Returns the hash code of the Association&lt;TKey, TValue&gt;.
        /// </summary>
        /// <returns> The compined hashcode of all objects. </returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.Append( this.Key );
            hashBuilder.Append( this.Value );

            return hashBuilder.GetHashCode();
        }

        /// <summary> 
        /// Returns a string representation of this <see cref="Association{TKey, TValue}"/>.
        /// </summary>
        /// <returns>
        /// A string in the format "Association( {0} -> {1} )".
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                System.Globalization.CultureInfo.CurrentCulture,
                "Association( {0} -> {1} )",
                Key   == null ? "null" : this.Key.ToString(),
                Value == null ? "null" : this.Value.ToString()
            );
        }

        #endregion
    }
}
