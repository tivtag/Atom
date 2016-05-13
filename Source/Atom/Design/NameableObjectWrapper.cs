// <copyright file="NameableObjectWrapper.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.NameableObjectWrapper{TObject} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Design
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents an object that wraps around another object, adding naming support to it.
    /// </summary>
    /// <typeparam name="TObject">
    /// The type of the object that is wrapped.
    /// </typeparam>
    public sealed class NameableObjectWrapper<TObject> : IReadOnlyNameable, IEquatable<NameableObjectWrapper<TObject>>
    {
        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        public string Name
        {
            get 
            {
                return this.nameMapper( obj );
            }
        }

        /// <summary>
        /// Gets the actual object this NameableObjectWrapper{TObject} wraps around.
        /// </summary>
        public TObject Object
        {
            get
            {
                return this.obj;
            }
        }

        /// <summary>
        /// Initializes a new instance of the NameableObjectWrapper{TObject} class.
        /// </summary>
        /// <param name="obj">
        /// The actual object the new NameableObjectWrapper{TObject} wraps around.
        /// </param>
        /// <param name="nameMapper">
        /// The map function the new NameableObjectWrapper{TObject} uses to receive the name of the object.
        /// </param>
        public NameableObjectWrapper( TObject obj, Func<TObject, string> nameMapper )
        {
            Contract.Requires<ArgumentNullException>( nameMapper != null );

            this.obj = obj;
            this.nameMapper = nameMapper;
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
            return base.Equals( obj as NameableObjectWrapper<TObject> );
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
        public bool Equals( NameableObjectWrapper<TObject> other )
        {
            if( other == null )
                return false;

            if( this.obj == null )
            {
                return other.obj == null;
            }

            return this.obj.Equals( other.obj );
        }

        /// <summary>
        /// Gets the hashcode of this NameableObjectWrapper{TObject} instance.
        /// </summary>
        /// <returns>
        /// The hashcode.
        /// </returns>
        public override int GetHashCode()
        {
            if( this.obj == null )
            {
                return 0;
            }

            return this.obj.GetHashCode();
        }

        /// <summary>
        /// Overriden to return the Name this NameableObjectWrapper{TObject} provides.
        /// </summary>
        /// <returns>
        /// The <see cref="Name"/> this NameableObjectWrapper{TObject} provides.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// The actual object this NameableObjectWrapper{TObject} wraps around.
        /// </summary>
        private readonly TObject obj;

        /// <summary>
        /// The map function that is used to receive the name of the object.
        /// </summary>
        private readonly Func<TObject, string> nameMapper;
    }
}
