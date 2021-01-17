// <copyright file="MathTypeConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Design.MathTypeConverter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Design
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;

    /// <summary>
    /// Provides a unified way of converting math type values to other types, 
    /// as well as for accessing standard values and subproperties.
    /// </summary>
    public abstract class MathTypeConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// Gets or sets a collection of System.ComponentModel.PropertyDescriptor objects. 
        /// </summary>
        /// <value>A collection of System.ComponentModel.PropertyDescriptor objects.</value>
        protected PropertyDescriptorCollection PropertyDescriptions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether string conversion is supported by this MathTypeConverter.
        /// </summary>
        /// <value>
        /// If <see langword="true"/> this MathTypeConverter supports converting to and from string values.
        /// </value>
        protected bool SupportStringConvert
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MathTypeConverter"/> class.
        /// </summary>
        protected MathTypeConverter()
            : base()
        {
            this.SupportStringConvert = true;
        }

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="sourceType">The type you want to convert from.</param>
        /// <returns>true if this converter can perform the conversion; false otherwise.</returns>
        public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
        {
            if( SupportStringConvert )
            {
                if( sourceType == typeof( string ) )
                    return true;
            }

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
            if( destinationType == typeof( InstanceDescriptor ) )
                return true;

            if( destinationType == typeof( string ) )
                return SupportStringConvert;

            return base.CanConvertTo( context, destinationType );
        }
    
        /// <summary>
        /// Returns whether changing a value on this object 
        /// requires a call to CreateInstance to create a new value,
        /// using the specified context. 
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <returns>
        /// Returns true if changing a property on this object requires a call to CreateInstance to create a new value; 
        /// otherwise false.
        /// </returns>
        public override bool GetCreateInstanceSupported( ITypeDescriptorContext context )
        {
            return true;
        }

        /// <summary>
        /// Returns a collection of properties for the type of array specified by the value parameter.
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="value">The type of array for which to get properties.</param>
        /// <param name="attributes">An array to use as a filter.</param>
        /// <returns>The properties that are exposed for this data type, or null if there are no properties.</returns>
        public override PropertyDescriptorCollection GetProperties( ITypeDescriptorContext context, object value, Attribute[] attributes )
        {
            return this.PropertyDescriptions;
        }

        /// <summary>
        /// Returns whether this object supports properties, using the specified context.
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <returns>true if GetProperties should be called to find the properties of this object; false otherwise. </returns>
        public override bool GetPropertiesSupported( ITypeDescriptorContext context )
        {
            return true;
        }
    }
}
