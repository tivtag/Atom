// <copyright file="Matrix4Converter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Design.Matrix4Converter class.
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
    /// Provides a unified way of converting <see cref="Matrix4"/> values to other types, 
    /// as well as for accessing standard values and subproperties.
    /// This class can't be inherited.
    /// </summary>
    public sealed class Matrix4Converter : MathTypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix4Converter"/> class.
        /// </summary>
        public Matrix4Converter()
        {
            Type type = typeof( Matrix4 );
        
            var descriptors = new PropertyDescriptorCollection( 
                new PropertyDescriptor[] {
                    TypeDescriptor.GetProperties( type ).Find( "Translation", true ),
                    new FieldPropertyDescriptor( type.GetField("M11") ),
                    new FieldPropertyDescriptor( type.GetField("M12") ), 
                    new FieldPropertyDescriptor( type.GetField("M13") ),
                    new FieldPropertyDescriptor( type.GetField("M14") ),
                    new FieldPropertyDescriptor( type.GetField("M21") ), 
                    new FieldPropertyDescriptor( type.GetField("M22") ), 
                    new FieldPropertyDescriptor( type.GetField("M23") ), 
                    new FieldPropertyDescriptor( type.GetField("M24") ),
                    new FieldPropertyDescriptor( type.GetField("M31") ),
                    new FieldPropertyDescriptor( type.GetField("M32") ),
                    new FieldPropertyDescriptor( type.GetField("M33") ),
                    new FieldPropertyDescriptor( type.GetField("M34") ),
                    new FieldPropertyDescriptor( type.GetField("M41") ),
                    new FieldPropertyDescriptor( type.GetField("M42") ),
                    new FieldPropertyDescriptor( type.GetField("M43") ), 
                    new FieldPropertyDescriptor( type.GetField("M44") )
                }
            );

            this.PropertyDescriptions = descriptors;
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

            if( destinationType == typeof( InstanceDescriptor ) && value is Matrix4 )
            {
                Matrix4         matrix      = (Matrix4)value;
                ConstructorInfo constructor = typeof( Matrix4 ).GetConstructor(
                    new Type[]
                    {
                        typeof( float ), typeof( float ), typeof( float ), typeof( float ),
                        typeof( float ), typeof( float ), typeof( float ), typeof( float ),
                        typeof( float ), typeof( float ), typeof( float ), typeof( float ), 
                        typeof( float ), typeof( float ), typeof( float ), typeof( float ) 
                    }
                );
                
                if( constructor != null )
                {
                    var arguments = new object[] {
                        matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                        matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                        matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                        matrix.M41, matrix.M42, matrix.M43, matrix.M44 
                    };

                    return new InstanceDescriptor( constructor, arguments );
                }
            }

            return base.ConvertTo( context, culture, value, destinationType );
        }

        /// <summary>
        /// Creates an instance of the type that this <see cref="Matrix4Converter"/> is associated with,
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

            return new Matrix4(
                (float)propertyValues["M11"], (float)propertyValues["M12"], (float)propertyValues["M13"], (float)propertyValues["M14"],
                (float)propertyValues["M21"], (float)propertyValues["M22"], (float)propertyValues["M23"], (float)propertyValues["M24"],
                (float)propertyValues["M31"], (float)propertyValues["M32"], (float)propertyValues["M33"], (float)propertyValues["M34"], 
                (float)propertyValues["M41"], (float)propertyValues["M42"], (float)propertyValues["M43"], (float)propertyValues["M44"]
            );
        }
    }
}
