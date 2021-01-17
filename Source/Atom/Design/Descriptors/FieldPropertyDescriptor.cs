// <copyright file="FieldPropertyDescriptor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Design.Descriptors.FieldPropertyDescriptor class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Design.Descriptors
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Defines a <see cref="PropertyDescriptor"/> that provides an abstraction of a field on a class.
    /// This class can't be inherited.
    /// </summary>
    public sealed class FieldPropertyDescriptor : MemberPropertyDescriptor
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the type of the field wrapped by the <see cref="FieldPropertyDescriptor"/>.
        /// </summary>
        /// <value>A System.Type that represents the type of the field.</value>
        public override Type PropertyType
        {
            get
            {
                return this.field.FieldType;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldPropertyDescriptor"/> class.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// If the given FieldInfo is null.
        /// </exception>
        /// <param name="field">
        /// The <see cref="FieldInfo"/> that descripes the field wrapped by the FieldPropertyDescriptor.
        /// </param>
        public FieldPropertyDescriptor( FieldInfo field )
            : base( field )
        {
            this.field = field;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns the value of the field.
        /// </summary>
        /// <param name="component">
        /// The object whose field value will be returned.
        /// </param>
        /// <returns>
        /// The value of a field for a given component.
        /// </returns>
        public override object GetValue( object component )
        {
            return this.field.GetValue( component );
        }

        /// <summary>
        /// Sets the value of the field.
        /// </summary>
        /// <param name="component">The object whose field value will be set.</param>
        /// <param name="value">The value to assign.</param>
        public override void SetValue( object component, object value )
        {
            this.field.SetValue( component, value );
            this.OnValueChanged( component, EventArgs.Empty );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The field information.
        /// </summary>
        private readonly FieldInfo field;

        #endregion
    }
}
