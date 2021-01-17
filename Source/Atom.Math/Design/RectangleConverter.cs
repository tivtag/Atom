// <copyright file="RectangleConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Design.RectangleConverter class.
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
    /// Provides a unified way of converting <see cref="Rectangle"/> values to other types,
    /// as well as for accessing standard values and subproperties.
    /// This class can't be inherited.
    /// </summary>
    public sealed class RectangleConverter : MathTypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the RectangleConverter class.
        /// </summary>
        public RectangleConverter()
        {
            Type type = typeof( Rectangle );

            this.PropertyDescriptions = new PropertyDescriptorCollection( 
                new PropertyDescriptor[] { 
                    new FieldPropertyDescriptor( type.GetField( "X" ) ),
                    new FieldPropertyDescriptor( type.GetField( "Y" ) ),
                    new FieldPropertyDescriptor( type.GetField( "Width" ) ),
                    new FieldPropertyDescriptor( type.GetField( "Height" ) ) 
                } 
            );

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

            if( value is Rectangle )
            {
                Rectangle rectangle = (Rectangle)value;

                if( destinationType == typeof( InstanceDescriptor ) )
                {
                    var constructor = typeof( Rectangle ).GetConstructor(
                        new Type[] { typeof( int ), typeof( int ), typeof( int ), typeof( int ) }
                    );

                    if( constructor != null )
                        return new InstanceDescriptor( constructor, new object[] { rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height } );
                }
                else if( destinationType == typeof( RectangleF ) )
                {
                    return new RectangleF( rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height );
                }
            }

            return base.ConvertTo( context, culture, value, destinationType );
        }

        /// <summary>
        /// Returns whether this converter can convert an object of one type to the type of this converter. 
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>true if this converter can perform the conversion; false otherwise.</returns>
        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType )
        {
            if( destinationType == typeof( RectangleF ) )
                return true;

            return base.CanConvertTo( context, destinationType );
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
            if( value is RectangleF )
            {
                RectangleF rectangle = (RectangleF)value;
                return new Rectangle( (int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height );
            }

            return base.ConvertFrom( context, culture, value );
        }

        /// <summary>
        /// Returns whether this converter can convert an object of the given type 
        /// to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="sourceType">The type you want to convert from.</param>
        /// <returns>true if this converter can perform the conversion; false otherwise.</returns>
        public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
        {
            if( sourceType == typeof( RectangleF ) )
                return true;

            return base.CanConvertFrom( context, sourceType );
        }

        /// <summary>
        /// Creates an instance of the type that this RectangleConverter is associated with, 
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
                throw new ArgumentNullException( "propertyValues");

            return new Rectangle( (int)propertyValues["X"], (int)propertyValues["Y"], (int)propertyValues["Width"], (int)propertyValues["Height"] );
        }
    }
}