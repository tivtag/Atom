// <copyright file="ObjectToSimpleTypeStringConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Converters.ObjectToSimpleTypeStringConverter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Implements a <see cref="IValueConverter"/> that returns the 
    /// simple type <see cref="String"/> "TypeName" of the <see cref="Object"/>.
    /// This class can't be inherited.
    /// </summary>
    [ValueConversion( typeof( object ), typeof( String ) )]
    public sealed class ObjectToSimpleTypeStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture"> The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value != null )
                return value.GetType().Name;

            return null;
        }

        /// <summary>
        /// This operation is not supported.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <exception cref="NotSupportedException">
        /// This operation is not supported.
        /// </exception>
        object IValueConverter.ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
