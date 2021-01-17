// <copyright file="TypeStringConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.TypeStringConverter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.ComponentModel;

    /// <summary>
    /// Implements an <see cref="IStringConverter"/> that uses ITypeConverters to
    /// convert values to and from strings.
    /// </summary>
    public class TypeStringConverter : IStringConverter
    {
        /// <summary>
        /// Attempts to convert the given source object value into a string.
        /// </summary>
        /// <param name="value">
        /// The input source value.
        /// </param>
        /// <returns>
        /// The output value.
        /// </returns>
        public string ConvertToString( object value )
        {
            if( value == null )
            {
                return string.Empty;
            }

            var converter = TypeDescriptor.GetConverter( value.GetType() );

            if( converter == null )
            {
                throw new InvalidCastException( "Could not find type converter for type '" + value.GetType() + "'." );
            }

            return converter.ConvertToString( null, CultureInfo.InvariantCulture, value );
        }

        /// <summary>
        /// Attempts to convert the given target string value into
        /// the a source value.
        /// </summary>
        /// <param name="value">
        /// The input target value, encoded in a string.
        /// </param>
        /// <param name="targetType">
        /// The type the target value encodes.
        /// </param>
        /// <returns>
        /// The output source value.
        /// </returns>
        public object ConvertFromString( string value, Type targetType )
        {
            var converter = TypeDescriptor.GetConverter( targetType );

            if( converter == null )
            {
                throw new InvalidCastException( "Could not find type converter for type '" + targetType + "'." );
            }

            return converter.ConvertFromString( null, CultureInfo.InvariantCulture, value );
        }
    }
}
