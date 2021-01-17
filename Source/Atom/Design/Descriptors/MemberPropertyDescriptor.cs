// <copyright file="MemberPropertyDescriptor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.Descriptors.MemberPropertyDescriptor class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Design.Descriptors
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Provides an abstraction of a member on a class.
    /// </summary>
    public abstract class MemberPropertyDescriptor : PropertyDescriptor
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the type of the comonent descriped by the MemberPropertyDescriptor.
        /// </summary>
        /// <value>A System.Type that represents the type of the member.</value>
        public override Type ComponentType
        {
            get
            {
                return this.member.DeclaringType;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the MemberPropertyDescriptor is read-only.
        /// </summary>
        /// <value>
        /// Always returns false.
        /// </value>
        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberPropertyDescriptor"/> class.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// If the given MemberInfo is null.
        /// </exception>
        /// <param name="member">
        /// The MemberInfo object that descripes the wrapped member.
        /// </param>
        protected MemberPropertyDescriptor( MemberInfo member )
            : base( 
                member != null ? member.Name : null,
                member != null ? (Attribute[])member.GetCustomAttributes( typeof( Attribute ), true ) : null
            )
        {
            this.member = member;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Overridden to return whether the given Object is equal to the <see cref="MemberPropertyDescriptor"/>.
        /// </summary>
        /// <param name="obj">
        /// The object to compare with.
        /// </param>
        /// <returns>
        /// Returns true if they are equal; otherwise false.
        /// </returns>
        public override bool Equals( object obj )
        {
            var descriptor = obj as MemberPropertyDescriptor;
            if( descriptor != null )
                return descriptor.member.Equals( this.member );

            return false;
        }

        /// <summary>
        /// Overriden to return the hash-code of the wrapped member.
        /// </summary>
        /// <returns>
        /// The hash-code of this MemberPropertyDescriptor.
        /// </returns>
        public override int GetHashCode()
        {
            return this.member.GetHashCode();
        }

        /// <summary>
        /// Gets whether the value can be reset.
        /// </summary>
        /// <param name="component">
        /// This value is not used.
        /// </param>
        /// <returns>
        /// Always returns false.
        /// </returns>
        public override bool CanResetValue( object component )
        {
            return false;
        }

        /// <summary>
        /// This method is overridden to-do nothing.
        /// </summary>
        /// <param name="component">
        /// This value is not used.
        /// </param>
        public override void ResetValue( object component )
        {
        }

        /// <summary>
        /// Returns whether the value should be serialized.
        /// </summary>
        /// <param name="component">This value is not used.</param>
        /// <returns>
        /// Always returns true.
        /// </returns>
        public override bool ShouldSerializeValue( object component )
        {
            return true;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Stores the member information of the abstracted property.
        /// </summary>
        private readonly MemberInfo member;

        #endregion
    }
}