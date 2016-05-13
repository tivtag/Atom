// <copyright file="Plane3Converter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Design.Plane3Converter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
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
    /// Provides a unified way of converting <see cref="Plane3Converter"/> values to other types, 
    /// as well as for accessing standard values and subproperties.
    /// This class can't be inherited.
    /// </summary>
    public sealed class Plane3Converter : MathTypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Plane3Converter"/> class.
        /// </summary>
        public Plane3Converter()
        {
            Type type = typeof( Plane3 );

            this.PropertyDescriptions = new PropertyDescriptorCollection(
                new PropertyDescriptor[] { 
                    new FieldPropertyDescriptor( type.GetField( "Normal" ) ), 
                    new FieldPropertyDescriptor( type.GetField( "Distance" ) )
                }
            );

            this.PropertyDescriptions.Sort( new string[] { "Normal", "Distance" } );
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

            if( destinationType == typeof( InstanceDescriptor ) && value is Plane3 )
            {
                Plane3 plane = (Plane3)value;
                
                ConstructorInfo constructor = typeof( Plane3 ).GetConstructor( new Type[] { typeof( Vector3 ), typeof( float ) } );
                if( constructor != null )
                    return new InstanceDescriptor( constructor, new object[] { plane.Normal, plane.Distance } );
            }

            return base.ConvertTo( context, culture, value, destinationType );
        }

        /// <summary>
        /// Creates an instance of the type that this <see cref="Plane3Converter"/> is associated with, 
        /// using the specified context, given a set of property values for the object.
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="propertyValues">The new property values.</param>
        /// <returns>An object representing propertyValues, or null if the object cannot be created.</returns>
        public override object CreateInstance( ITypeDescriptorContext context, System.Collections.IDictionary propertyValues )
        {
            if( propertyValues == null )
                throw new ArgumentNullException( "propertyValues" );

            return new Plane3( (Vector3)propertyValues["Normal"], (float)propertyValues["Distance"] );
        }
    }
}