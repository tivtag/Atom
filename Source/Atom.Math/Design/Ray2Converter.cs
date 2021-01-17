// <copyright file="Ray2Converter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Design.Ray2Converter class.
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
    /// Provides a unified way of converting <see cref="Ray2"/> values to other types, 
    /// as well as for accessing standard values and subproperties.
    /// </summary>
    public class Ray2Converter : MathTypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ray2Converter"/> class.
        /// </summary>
        public Ray2Converter()
        {
            Type type = typeof( Ray2 );

            this.PropertyDescriptions = new PropertyDescriptorCollection(
                new PropertyDescriptor[] { 
                    new FieldPropertyDescriptor( type.GetField( "Origin" ) ),
                    new FieldPropertyDescriptor( type.GetField( "Direction" ) ) 
                }
            );

            this.PropertyDescriptions.Sort( new string[] { "Origin", "Direction" } );
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

            if( destinationType == typeof( InstanceDescriptor ) && value is Ray2 )
            {
                Ray2 ray = (Ray2)value;
                var constructor = typeof( Ray2 ).GetConstructor( new Type[] { typeof( Vector2 ), typeof( Vector2 ) } );

                if( constructor != null )
                    return new InstanceDescriptor( constructor, new object[] { ray.Origin, ray.Direction } );
            }

            return base.ConvertTo( context, culture, value, destinationType );
        }

        /// <summary>
        /// Creates an instance of the type that this <see cref="Ray2Converter"/> is associated with,
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

            return new Ray2( (Vector2)propertyValues["Origin"], (Vector2)propertyValues["Direction"] );
        }
    }
}