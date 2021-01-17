// <copyright file="IntegerRangeConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Design.IntegerRangeConverter class.
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
    /// Provides a unified way of converting <see cref="IntegerRange"/> values to other types, 
    /// as well as for accessing standard values and subproperties.
    /// This class can't be inherited.
    /// </summary>
    public sealed class IntegerRangeConverter : MathTypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerRangeConverter"/> class.
        /// </summary>
        public IntegerRangeConverter()
        {
            Type type = typeof( IntegerRange );

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
            if( value is Point2 )
            {
                Point2 point = (Point2)value;
                return new IntegerRange( point.X, point.Y );
            }

            string str = value as string;
            if( str != null )
            {
                int[] array = StringUtilities.ConvertToValues<int>( context, culture, str, 2, "Minimum; Maxmimum" );
                return new IntegerRange( array[0], array[1] );
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

            if( value is IntegerRange )
            {
                IntegerRange range = (IntegerRange)value;

                if( destinationType == typeof( string ) )
                {
                    return StringUtilities.ConvertFromValues<int>( context, culture, new int[] { range.Minimum, range.Maximum } );
                }
                else if( destinationType == typeof( InstanceDescriptor ) )
                {
                    ConstructorInfo constructor = typeof( IntegerRange ).GetConstructor( new Type[] { typeof( int ), typeof( int ) } );
                    if( constructor != null )
                    {
                        return new InstanceDescriptor( constructor, new object[] { range.Minimum, range.Maximum } );
                    }
                }
                else if( destinationType == typeof( Point2 ) )
                {
                    return new Point2( range.Minimum, range.Maximum );
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
            if( sourceType == typeof( Point2 ) )
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
            if( destinationType == typeof( Point2 ) )
                return true;

            return base.CanConvertTo( context, destinationType );
        }

        /// <summary>
        /// Creates an instance of the type that this IntegerRangeConverter is associated with, 
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

            return new IntegerRange( (int)propertyValues["Minimum"], (int)propertyValues["Maximum"] );
        }
    }
}
