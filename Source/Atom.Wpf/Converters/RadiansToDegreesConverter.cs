// <copyright file="RadiansToDegreesConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Converters.RadiansToDegreesConverter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Atom.Math;

    /// <summary>
    /// Implements an <see cref="IValueConverter"/> that converts any floating point
    /// value from radians to degrees and back. This is a sealed class.
    /// </summary>
    [ValueConversion( typeof( Single ), typeof( Single ) )]
    [ValueConversion( typeof( Double ), typeof( Double ) )]
    [ValueConversion( typeof( Decimal ), typeof( Decimal ) )]
    public sealed class RadiansToDegreesConverter : IValueConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> into degrees.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture"> The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value is float || value is double || value is decimal )
            {
                if( targetType == typeof( float ) )
                {
                    return MathUtilities.ToDegrees( (float)value );
                }
                else if( targetType == typeof( double ) )
                {
                    return MathUtilities.ToDegrees( (double)value );
                }
                else if( targetType == typeof( decimal ) )
                {
                    return MathUtilities.ToDegrees( (decimal)value );
                }
            }

            return null;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> into radians.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value is float || value is double || value is decimal )
            {
                if( targetType == typeof( float ) )
                {
                    return MathUtilities.ToRadians( (float)value );
                }
                else if( targetType == typeof( double ) )
                {
                    return MathUtilities.ToRadians( (double)value );
                }
                else if( targetType == typeof( decimal ) )
                {
                    return MathUtilities.ToRadians( (decimal)value );
                }
            }

            return null;
        }
    }
}
