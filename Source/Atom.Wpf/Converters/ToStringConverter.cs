// <copyright file="ToStringConverter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Converters.ToStringConverter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf.Converters
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// Implements a <see cref="IValueConverter"/> that converts a value <see cref="Object"/> into a <see cref="String"/>
    /// and back.
    /// This is a sealed class.
    /// </summary>
    [ValueConversion( typeof( Object ), typeof( String ) )]
    public sealed class ToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture"> The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if( value == null )
                return string.Empty;

            var cultureSensitiveConverter = value as ICultureSensitiveToStringProvider;
            if( cultureSensitiveConverter != null )
                return cultureSensitiveConverter.ToString( culture );

            Type inputType = value.GetType();

            if( inputType == typeof( float ) )
                return ((float)value).ToString( culture );

            if( inputType == typeof( double ) )
                return ((double)value).ToString( culture );

            if( inputType == typeof( decimal ) )
                return ((decimal)value).ToString( culture );

            if( inputType == typeof( int ) )
                return ((int)value).ToString( culture );

            if( inputType == typeof( long ) )
                return ((long)value).ToString( culture );

            if( inputType == typeof( short ) )
                return ((short)value).ToString( culture );

            if( inputType == typeof( ushort ) )
                return ((ushort)value).ToString( culture );

            if( inputType == typeof( uint ) )
                return ((uint)value).ToString( culture );

            if( inputType == typeof( ulong ) )
                return ((ulong)value).ToString( culture );

            return value.ToString();
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            string str = value as string;
            if( str == null )
                return null;

            if( targetType == typeof( float ) )
                return Single.Parse( str, culture );
            if( targetType == typeof( double ) )
                return Double.Parse( str, culture );
            if( targetType == typeof( decimal ) )
                return Decimal.Parse( str, culture );

            if( targetType == typeof( int ) )
                return Int32.Parse( str, culture );
            if( targetType == typeof( short ) )
                return Int16.Parse( str, culture );
            if( targetType == typeof( long ) )
                return Int64.Parse( str, culture );

            if( targetType == typeof( uint ) )
                return UInt32.Parse( str, culture );
            if( targetType == typeof( ushort ) )
                return UInt16.Parse( str, culture );
            if( targetType == typeof( ulong ) )
                return UInt64.Parse( str, culture );

            return null;
        }
    }
}
