﻿// <copyright file="BoxConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Design.BoxConverter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Design
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;
    using System.Reflection;
    using Atom.Design.Descriptors;

    /// <summary>
    /// Provides a unified way of converting <see cref="Box"/> values to other types, 
    /// as well as for accessing standard values and subproperties.
    /// This class can't be inherited.
    /// </summary>
    public sealed class BoxConverter : MathTypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoxConverter"/> class. 
        /// </summary>
        public BoxConverter()
        {
            Type type = typeof( Box );
            this.PropertyDescriptions = new PropertyDescriptorCollection(
                new PropertyDescriptor[] {
                    new FieldPropertyDescriptor( type.GetField( "Minimum" ) ),
                    new FieldPropertyDescriptor( type.GetField( "Maximum" ) )
                }
            );

            PropertyDescriptions.Sort( new string[] { "Minimum", "Maximum" } );
            this.SupportStringConvert = false;
        }

        /// <summary>
        /// Converts the given value object to the specified type, 
        /// using the specified context and culture information.
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="culture">The culture to use in the conversion.</param>
        /// <param name="value">The object to convert.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>The converted value.</returns>
        public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType )
        {
            if( destinationType == null )
                throw new ArgumentNullException( "destinationType" );

            if( destinationType == typeof( InstanceDescriptor ) && value is Box )
            {
                Box box         = (Box)value;
                var constructor = typeof( Box ).GetConstructor( new Type[] { typeof( Vector3 ), typeof( Vector3 ) } );
                
                if( constructor != null )
                    return new InstanceDescriptor( constructor, new object[] { box.Minimum, box.Maximum } );
            }

            return base.ConvertTo( context, culture, value, destinationType );
        }

        /// <summary>
        /// Creates an instance of the type that this <see cref="BoxConverter"/> is associated with, 
        /// using the specified context, given a set of property values for the object.
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="propertyValues">The new property values.</param>
        /// <returns>
        /// An object representing propertyValues, or null if the object cannot be created.
        /// </returns>
        public override object CreateInstance( ITypeDescriptorContext context, System.Collections.IDictionary propertyValues )
        {
            if( propertyValues == null )
                throw new ArgumentNullException( "propertyValues" );

            return new Box( (Vector3)propertyValues["Minimum"], (Vector3)propertyValues["Maximum"] );
        }
    }
}
