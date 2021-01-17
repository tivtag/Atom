// <copyright file="ColorToBrushConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Converters.ColorToBrushConverter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Implements an <see cref="IValueConverter"/> that converts a <see cref="Color"/> 
    /// into a <see cref="SolidColorBrush"/>, and back.
    /// This class can't be inherited.
    /// </summary>
    [ValueConversion( typeof( Color ), typeof( SolidColorBrush ) )]
    public sealed class ColorToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts the specified <see cref="Color"/> into a <see cref="SolidColorBrush"/>.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture"> The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value is Color )
            {
                var color = (Color)value;
                return new SolidColorBrush( color );
            }

            return null;
        }

        /// <summary>
        /// Converts the specified <see cref="SolidColorBrush"/> back into a <see cref="Color"/>.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var brush = value as SolidColorBrush;

            if( brush != null )
                return brush.Color;

            return null;
        }
    }
}
