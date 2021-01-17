// <copyright file="FloatRangeConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Design.FloatRangeConverter class.
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
    /// Provides a unified way of converting <see cref="FloatRange"/> values to other types, 
    /// as well as for accessing standard values and subproperties.
    /// This class can't be inherited.
    /// </summary>
    public sealed class FloatRangeConverter : MathTypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FloatRangeConverter"/> class.
        /// </summary>
        public FloatRangeConverter()
        {
            Type type = typeof( FloatRange );

            var properties = new PropertyDescriptor[] {
                new PropertyPropertyDescriptor( type.GetProperty( "Minimum" ) ),
                new PropertyPropertyDescriptor( type.GetProperty( "Maximum" ) ) 
            };

            this.PropertyDescriptions = new PropertyDescriptorCollection( properties );
            this.PropertyDescriptions.Sort( new string[] { "Minimum", "Maximum" } );
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
            if( value is Vector2 )
            {
                Vector2 pofloat = (Vector2)value;
                return new FloatRange( pofloat.X, pofloat.Y );
            }

            string str = value as string;
            if( str != null )
            {
                float[] array = StringUtilities.ConvertToValues<float>( context, culture, str, 2, "Minimum; Maxmimum" );
                return new FloatRange( array[0], array[1] );
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

            if( value is FloatRange )
            {
                FloatRange range = (FloatRange)value;

                if( destinationType == typeof( string ) )
                {
                    return StringUtilities.ConvertFromValues<float>( context, culture, new float[] { range.Minimum, range.Maximum } );
                }
                else if( destinationType == typeof( InstanceDescriptor ) )
                {
                    ConstructorInfo constructor = typeof( FloatRange ).GetConstructor( new Type[] { typeof( float ), typeof( float ) } );
                    if( constructor != null )
                    {
                        return new InstanceDescriptor( constructor, new object[] { range.Minimum, range.Maximum } );
                    }
                }
                else if( destinationType == typeof( Vector2 ) )
                {
                    return new Vector2( range.Minimum, range.Maximum );
                }
            }

            return base.ConvertTo( context, culture, value, destinationType );
        }

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="sourceType">The type you want to convert from.</param>
        /// <returns>true if this converter can perform the conversion; false otherwise.</returns>
        public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
        {
            if( sourceType == typeof( Vector2 ) )
                return true;

            return base.CanConvertFrom( context, sourceType );
        }

        /// <summary>
        /// Returns whether this converter can convert an object of one type to the type of this converter. 
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>true if this converter can perform the conversion; false otherwise.</returns>
        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType )
        {
            if( destinationType == typeof( Vector2 ) )
                return true;

            return base.CanConvertTo( context, destinationType );
        }

        /// <summary>
        /// Creates an instance of the type that this FloatRangeConverter is associated with, 
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

            return new FloatRange( (float)propertyValues["Minimum"], (float)propertyValues["Maximum"] );
        }
    }
}
