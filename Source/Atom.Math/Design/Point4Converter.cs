// <copyright file="Point4Converter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Design.Point4Converter class.
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
    /// Provides a unified way of converting <see cref="Point4"/> values to other types, 
    /// as well as for accessing standard values and subproperties.
    /// This class can't be inherited.
    /// </summary>
    public sealed class Point4Converter : MathTypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point4Converter"/> class.
        /// </summary>
        public Point4Converter()
        {
            Type type = typeof( Point4 );

            var properties = new PropertyDescriptor[]  {
                new FieldPropertyDescriptor( type.GetField( "X" ) ),
                new FieldPropertyDescriptor( type.GetField( "Y" ) ),
                new FieldPropertyDescriptor( type.GetField( "Z" ) ),
                new FieldPropertyDescriptor( type.GetField( "W" ) )  
            };

            this.PropertyDescriptions = new PropertyDescriptorCollection( properties );
            this.PropertyDescriptions.Sort( new string[] { "X", "Y", "Z", "W" } );
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
            if( value is Vector4 )
            {
                Vector4 vector = (Vector4)value;
                return new Point4( (int)vector.X, (int)vector.Y, (int)vector.Z, (int)vector.W );
            }

            string str = value as string;
            if( str != null )
            {
                int[] array = StringUtilities.ConvertToValues<int>( context, culture, str, 4, "X; Y; Z; W" );
                return new Point4( array[0], array[1], array[2], array[3] );
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

            if( value is Point4 )
            {
                Point4 point = (Point4)value;

                if( destinationType == typeof( string ) )
                {
                    return StringUtilities.ConvertFromValues<int>( context, culture, new int[] { point.X, point.Y, point.Z, point.W } );
                }
                else if( destinationType == typeof( InstanceDescriptor ) )
                {
                    var constructorArgs = new Type[] { typeof( int ), typeof( int ), typeof( int ), typeof( int ) };
                    var constructor     = typeof( Point4 ).GetConstructor( constructorArgs );

                    if( constructor != null )
                        return new InstanceDescriptor( constructor, new object[] { point.X, point.Y, point.Z, point.W } );
                }
                else if( destinationType == typeof( Vector4 ) )
                {
                    return new Vector4( (float)point.X, (float)point.Y, (float)point.Z, (float)point.W );
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
            if( sourceType == typeof( Vector4 ) )
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
            if( destinationType == typeof( Vector4 ) )
                return true;

            return base.CanConvertTo( context, destinationType );
        }

        /// <summary>
        /// Creates an instance of the type that this Point4Converter is associated with, 
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

            return new Point4( (int)propertyValues["X"], (int)propertyValues["Y"], (int)propertyValues["Z"], (int)propertyValues["W"] );
        }
    }
}
