﻿// <copyright file="CircleConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Design.CircleConverter class.
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
    /// Provides a unified way of converting <see cref="Circle"/> values to other types, 
    /// as well as for accessing standard values and subproperties.
    /// This class can't be inherited.
    /// </summary>
    public sealed class CircleConverter : MathTypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircleConverter"/> class.
        /// </summary>
        public CircleConverter()
        {
            Type type = typeof( Circle );

            var properties = new PropertyDescriptor[] {
                new FieldPropertyDescriptor( type.GetField( "Center" ) ),
                new FieldPropertyDescriptor( type.GetField( "Radius" ) ) 
            };

            this.PropertyDescriptions = new PropertyDescriptorCollection( properties );
            this.PropertyDescriptions.Sort( new string[] { "Center", "Radius" } );
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
                float[] array = StringUtilities.ConvertToValues<float>( context, culture, str, 3, "X; Y; Radius" );                
                return new Circle( new Vector2( array[0], array[1] ), array[2] );
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
            
            if( value is Circle )
            {
                Circle circle = (Circle)value;

                if( destinationType == typeof( string ) )
                {
                    return circle.Center.X.ToString( culture ) + culture.TextInfo.ListSeparator + " " +
                           circle.Center.Y.ToString( culture ) + culture.TextInfo.ListSeparator + " " +
                           circle.Radius.ToString( culture );
                }
                else if( destinationType == typeof( InstanceDescriptor ) )
                {
                    var constructor = typeof( Circle ).GetConstructor( new Type[] { typeof( Vector2 ), typeof( float ) } );
                    
                    if( constructor != null )
                        return new InstanceDescriptor( constructor, new object[] { circle.Center, circle.Radius } );
                }
            }

            return base.ConvertTo( context, culture, value, destinationType );
        }

        /// <summary>
        /// Creates an instance of the type that this CircleConverter is associated with, 
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

            return new Circle( (Vector2)propertyValues["Center"], (float)propertyValues["Radius"] );
        }
    }
}
