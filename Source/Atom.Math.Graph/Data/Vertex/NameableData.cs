// <copyright file="NameableData.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Data.NameableData class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Graph.Data
{
    using System;

    /// <summary>
    /// Provides a basic implementation of the <see cref="INameable"/> interface,
    /// mostly used to give vertices in a graph an (unique) name.
    /// </summary>
    public class NameableData : INameable, IEquatable<NameableData>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>That name that (should) uniquely identify the Edge or Vertex.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameableData"/> class.
        /// </summary>
        public NameableData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameableData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name that (should) uniquely identify the Edge or Vertex..
        /// </param>
        public NameableData( string name )
        {
            this.Name = name;
        }

        /// <summary>
        /// Returns whether this <see cref="NameableData"/> object
        /// is equal to the given <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The object to test against.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            return this.Equals( obj as NameableData );
        }

        /// <summary>
        /// Returns whether this <see cref="NameableData"/> object
        /// is equal to the given <see cref="NameableData"/>.
        /// </summary>
        /// <param name="other">The <see cref="NameableData"/> to test against.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( NameableData other )
        {
            if( other == null )
                return false;

            if( object.ReferenceEquals( this, other ) )
                return true;

            return this.Name == other.Name;
        }

        /// <summary>
        /// Gets the hash code of this <see cref="NameableData"/> instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            if( this.Name == null )
                return base.GetHashCode();

            return this.Name.GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString()
        {
            return this.Name ?? string.Empty;
        }
    }
}
