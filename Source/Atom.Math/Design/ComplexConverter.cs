// <copyright file="ComplexConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Design.ComplexConverter class.
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
    /// Provides a unified way of converting <see cref="Complex"/> values to other types, 
    /// as well as for accessing standard values and subproperties.
    /// This class can't be inherited.
    /// </summary>
    public sealed class ComplexConverter : MathTypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexConverter"/> class.
        /// </summary>
        public ComplexConverter()
        {
            Type type = typeof( Complex );

            var properties = new PropertyDescriptor[] {
                new FieldPropertyDescriptor( type.GetField( "Real" ) ),
                new FieldPropertyDescriptor( type.GetField( "Imag" ) ) 
            };

            this.PropertyDescriptions = new PropertyDescriptorCollection( properties );
            this.PropertyDescriptions.Sort( new string[] { "Real", "Imag" } );
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information. 
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="culture">The current culture.</param>
        /// <param name="value">The object to convert.</param>
        /// <returns>The converted value.</returns>
        public override object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object value )
        {
            string str = value as string;

            if( str != null )
            {
                float[] array = StringUtilities.ConvertToValues<float>( context, culture, str, 2, "Real; Imag" );
                return new Complex( array[0], array[1] );
            }

            return base.ConvertFrom( context, culture, value );
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
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
            
            if( value is Complex )
            {
                Complex complex = (Complex)value;

                if( destinationType == typeof( string ) )
                {
                    return StringUtilities.ConvertFromValues<float>( context, culture, new float[] { complex.Real, complex.Imag } );
                }
                else if( destinationType == typeof( InstanceDescriptor ) )
                {
                    var constructor = typeof( Complex ).GetConstructor( new Type[] { typeof( float ), typeof( float ) } );

                    if( constructor != null )
                        return new InstanceDescriptor( constructor, new object[] { complex.Real, complex.Imag } );
                }
            }

            return base.ConvertTo( context, culture, value, destinationType );
        }

        /// <summary>
        /// Creates an instance of the type that this ComplexConverter is associated with, 
        /// using the specified context, given a set of property values for the object. 
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="propertyValues">The new property values.</param>
        /// <returns>An object representing propertyValues, or null if the object cannot be created.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="propertyValues"/> is null.
        /// </exception>
        public override object CreateInstance( ITypeDescriptorContext context, System.Collections.IDictionary propertyValues )
        {
            if( propertyValues == null )
                throw new ArgumentNullException( "propertyValues" );

            return new Vector2( (float)propertyValues["Real"], (float)propertyValues["Imag"] );
        }
    }
}
