// <copyright file="RadiansToDegreesStringConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Converters.RadiansToDegreesStringConverter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf.Converters
{
    using System;
    using System.Windows.Data;
    using Atom.Math;

    /// <summary>
    /// Implements an <see cref="IValueConverter"/> that converts any floating point
    /// value in radians into a <see cref="String"/> that represents that value in degrees and back. This is a sealed class.
    /// </summary>
    [ValueConversion( typeof( Single ), typeof( string ) )]
    [ValueConversion( typeof( Double ), typeof( string ) )]
    [ValueConversion( typeof( Decimal ), typeof( string ) )]
    public sealed class RadiansToDegreesStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> into degrees.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture"> The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if( targetType != typeof( string ) )
                return null;

            if( value is float )
                return MathUtilities.ToDegrees( (float)value ).ToString( culture );

            if( value is double )
                return MathUtilities.ToDegrees( (double)value ).ToString( culture );

            if( value is decimal )
                return MathUtilities.ToDegrees( (decimal)value ).ToString( culture );

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
        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            string valueStr = value as string;
            if( valueStr == null )
                return null;

            if( targetType == typeof( float ) )
            {
                if( valueStr.Length == 0 )
                    return 0.0f;

                return MathUtilities.ToRadians( float.Parse( valueStr, culture ) );
            }
            else if( targetType == typeof( double ) )
            {
                if( valueStr.Length == 0 )
                    return 0.0;

                return MathUtilities.ToRadians( double.Parse( valueStr, culture ) );
            }
            else if( targetType == typeof( decimal ) )
            {
                if( valueStr.Length == 0 )
                    return 0.0m;

                return MathUtilities.ToRadians( decimal.Parse( valueStr, culture ) );
            }

            return null;
        }
    }
}
