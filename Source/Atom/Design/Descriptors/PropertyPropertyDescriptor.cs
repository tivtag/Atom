// <copyright file="PropertyPropertyDescriptor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Design.Descriptors.PropertyPropertyDescriptor class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Design.Descriptors
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Defines a <see cref="PropertyDescriptor"/> that provides an abstraction of a property on a class.
    /// This class can't be inherited.
    /// </summary>
    public sealed class PropertyPropertyDescriptor : MemberPropertyDescriptor
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the type of the property wrapped by the <see cref="PropertyPropertyDescriptor"/>.
        /// </summary>
        /// <value>A System.Type that represents the type of the property.</value>
        public override Type PropertyType
        {
            get
            {
                return this.property.PropertyType;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the PropertyPropertyDescriptor is read-only.
        /// </summary>
        /// <value>
        /// True if the underlying property is read-only; otherwise false.
        /// </value>
        public override bool IsReadOnly
        {
            get
            {
                return !this.property.CanWrite;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyPropertyDescriptor"/> class.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// If the given PropertyInfo is null.
        /// </exception>
        /// <param name="property">
        /// The <see cref="PropertyInfo"/> that descripes the property wrapped by the PropertyPropertyDescriptor.
        /// </param>
        public PropertyPropertyDescriptor( PropertyInfo property )
            : base( property )
        {
            this.property = property;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns the value of the property.
        /// </summary>
        /// <param name="component">The object whose property value will be returned.</param>
        /// <returns>The requested value.</returns>
        public override object GetValue( object component )
        {
            return this.property.GetValue( component, null );
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the property is read-only.
        /// </exception>
        /// <param name="component">The object whose property value will be set.</param>
        /// <param name="value">The value to assign.</param>
        public override void SetValue( object component, object value )
        {
            if( this.IsReadOnly )
                throw new InvalidOperationException( ErrorStrings.TheUnderlyingPropertyIsReadOnly );

            this.property.SetValue( component, value, null );
            this.OnValueChanged( component, EventArgs.Empty );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Contains information about the abstracted property.
        /// </summary>
        private readonly PropertyInfo property;

        #endregion
    }
}
