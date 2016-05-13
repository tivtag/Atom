// <copyright file="TimeTickConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Story.Design.TimeTickConverter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Story.Design
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;
    using Atom.Design.Descriptors;
    using Atom.Math.Design;

    /// <summary>
    /// Provides a unified way of converting <see cref="TimeTick"/> values to other types, 
    /// as well as for accessing standard values and subproperties.
    /// This class can't be inherited.
    /// </summary>
    public sealed class TimeTickConverter : MathTypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeTickConverter"/> class.
        /// </summary>
        public TimeTickConverter()
        {
            Type type = typeof( TimeTick );

            var properties = new PropertyDescriptor[]  {
                new FieldPropertyDescriptor( type.GetField( "Value" ) ),                
                new PropertyPropertyDescriptor( type.GetProperty( "Minutes" ) ),                
                new PropertyPropertyDescriptor( type.GetProperty( "Seconds" ) )
            };

            this.PropertyDescriptions = new PropertyDescriptorCollection( properties );
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
                return new TimeTick( uint.Parse(str) );
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

            if( value is TimeTick )
            {
                TimeTick tick = (TimeTick)value;

                if( destinationType == typeof( string ) )
                {
                    return tick.Value.ToString( culture );
                }
                else if( destinationType == typeof( InstanceDescriptor ) )
                {
                    var constructor = typeof( TimeTick ).GetConstructor( new Type[] { typeof( uint ) } );

                    if( constructor != null )
                        return new InstanceDescriptor( constructor, new object[] { tick.Value } );
                }
            }

            return base.ConvertTo( context, culture, value, destinationType );
        }
        
        /// <summary>
        /// Creates an instance of the type that this TimeTickConverter is associated with, 
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

            return new TimeTick( (uint)propertyValues["Value"] );
        }
    }
}
