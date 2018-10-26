// <copyright file="StringUtilities.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.StringUtilities class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;
    using System.ComponentModel;
    using Atom.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Defines static string-related utility methods.
    /// </summary>
    public static class StringUtilities
    {      
        /// <summary>
        /// Reverses the given <see cref="System.String"/>.
        /// </summary>
        /// <param name="str">
        /// The input string to reverse.
        /// </param>
        /// <returns>
        /// The reversed output string.
        /// </returns>
        public static string Reverse( string str )
        {
            Contract.Requires<ArgumentNullException>( str != null );
            // Contract.Ensures( Contract.Result<string>() != null );

            char[] reversed = new char[str.Length];

            for( int i = 0, j = reversed.Length - 1; i <= j; ++i, --j )
            {
                reversed[i] = str[j];
                reversed[j] = str[i];
            }

            return new string( reversed );
        }

        /// <summary>
        /// Extracts an integer string from the end of the given string.
        /// </summary>
        /// <param name="str">
        /// The input string.
        /// </param>
        /// <returns>
        /// The integer that has been extracted;
        /// or <see cref="String.Empty"/> if the given string doesn't
        /// end with an integer.
        /// </returns>
        public static string ExtractTrailingInteger( string str )
        {
            Contract.Requires<ArgumentNullException>( str != null );
            // Contract.Ensures( Contract.Result<string>() != null );

            string result = string.Empty;

            for( int index = str.Length - 1; index >= 0; --index )
            {
                char ch = str[index];

                if( Char.IsDigit( ch ) )
                {
                    result += ch;
                }
                else
                {
                    break;
                }
            }

            return Reverse( result );
        }

        /// <summary>
        /// Transforms the integer at the end of the given <see cref="System.String"/>.
        /// </summary>
        /// <param name="str">
        /// The input string.
        /// </param>
        /// <param name="transform">
        /// The transform to apply to the integer.
        /// </param>
        /// <returns>
        /// The transformed output string.
        /// </returns>
        public static string TransformTrailingInteger( string str, Func<int, string> transform )
        {
            Contract.Requires<ArgumentNullException>( str != null );
            Contract.Requires<ArgumentNullException>( transform != null );
            // Contract.Ensures( Contract.Result<string>() != null );

            string trailing = ExtractTrailingInteger( str );

            if( trailing.Length > 0 )
            {
                int integer = int.Parse( trailing, CultureInfo.InvariantCulture );

                str = str.Substring( 0, str.Length - trailing.Length );
                str += transform( integer );
            }

            return str;
        }

        /// <summary>
        /// Increments the integer at the end of the given <see cref="System.String"/>.
        /// </summary>
        /// <param name="str">
        /// The input string.
        /// </param>
        /// <returns>
        /// The output string.
        /// </returns>
        public static string IncrementTrailingInteger( string str )
        {
            Contract.Requires<ArgumentNullException>( str != null );
            // Contract.Ensures( Contract.Result<string>() != null );

            return TransformTrailingInteger( str, integer => (++integer).ToString( CultureInfo.InvariantCulture ) );
        }

        /// <summary>
        /// Decrements the integer at the end of the given <see cref="System.String"/>.
        /// </summary>
        /// <param name="str">
        /// The input string.
        /// </param>
        /// <returns>
        /// The output string.
        /// </returns>
        public static string DecrementTrailingInteger( string str )
        {
            Contract.Requires<ArgumentNullException>( str != null );
            // Contract.Ensures( Contract.Result<string>() != null );

            return TransformTrailingInteger( str, integer => (--integer).ToString( CultureInfo.InvariantCulture ) );
        }

        /// <summary>
        /// Converts the given values into a string.
        /// </summary>
        /// <typeparam name="T">
        /// The type of data to convert.
        /// </typeparam>
        /// <param name="values">
        /// The values to convert.
        /// </param>
        /// <returns>
        /// The converted string.
        /// </returns>
        public static string ConvertFromValues<T>( T[] values )
        {
            Contract.Requires<ArgumentNullException>( values != null );

            return ConvertFromValues( null, null, values );
        }

        /// <summary>
        /// Converts the given values into a string.
        /// </summary>
        /// <typeparam name="T">
        /// The type of data to convert.
        /// </typeparam>
        /// <param name="context">
        /// An <see cref="ITypeDescriptorContext"/> that provides a format context.
        /// </param>
        /// <param name="culture">
        /// A <see cref="System.Globalization.CultureInfo"/>. If null is passed, 
        /// the current culture is assumed.
        /// </param>
        /// <param name="values">
        /// The values to convert.
        /// </param>
        /// <returns>
        /// The converted string.
        /// </returns>
        public static string ConvertFromValues<T>( ITypeDescriptorContext context, CultureInfo culture, T[] values )
        {
            Contract.Requires<ArgumentNullException>( values != null );
            // Contract.Ensures( Contract.Result<string>() != null );

            if( culture == null )
                culture = CultureInfo.CurrentCulture;

            string separator = culture.TextInfo.ListSeparator + " ";
            return ConvertFromValues( context, culture, values, separator );
        }

        /// <summary>
        /// Converts the given values into a string.
        /// </summary>
        /// <typeparam name="T">
        /// The type of data to convert.
        /// </typeparam>
        /// <param name="context">
        /// An <see cref="ITypeDescriptorContext"/> that provides a format context.
        /// </param>
        /// <param name="culture">
        /// A <see cref="System.Globalization.CultureInfo"/>. If null is passed, 
        /// the current culture is assumed.
        /// </param>
        /// <param name="values">
        /// The values to convert.
        /// </param>
        /// <param name="separator">
        /// The string the values should be seperated by in the output string.
        /// </param>
        /// <returns>
        /// The converted string.
        /// </returns>
        public static string ConvertFromValues<T>( ITypeDescriptorContext context, CultureInfo culture, T[] values, string separator )
        {
            Contract.Requires<ArgumentNullException>( values != null );
            Contract.Requires<ArgumentNullException>( separator != null );
            // Contract.Ensures( Contract.Result<string>() != null );

            var converter = TypeDescriptor.GetConverter( typeof( T ) );
            var array = new string[values.Length];

            for( int i = 0; i < values.Length; ++i )
            {
                array[i] = converter.ConvertToString( context, culture, values[i] );
            }

            return string.Join( separator, array );
        }

        /// <summary>
        /// Tries to convert the given value string into the equivalent of real values.
        /// </summary>
        /// <typeparam name="T">
        /// The type of data to convert to.
        /// </typeparam>
        /// <param name="input">
        /// The string to convert into values.
        /// </param>
        /// <param name="expectedValueCount">
        /// The number of values expected.
        /// </param>
        /// <param name="message">
        /// A message string that descripes the format the paramters are in.
        /// </param>
        /// <returns>
        /// The converted values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// If the format of the value string is not as expected.
        /// </exception>
        public static T[] ConvertToValues<T>( string input, int expectedValueCount, string message )
        {
            Contract.Requires<ArgumentNullException>( input != null );

            return ConvertToValues<T>( null, null, input, expectedValueCount, message );
        }
        
        /// <summary>
        /// Tries to convert the given value string into the equivalent of real values.
        /// </summary>
        /// <typeparam name="T">
        /// The type of data to convert to.
        /// </typeparam>
        /// <param name="context">
        /// An <see cref="ITypeDescriptorContext"/> that provides a format context.
        /// </param>
        /// <param name="culture">
        /// A <see cref="System.Globalization.CultureInfo"/>. If null is passed, 
        /// the current culture is assumed.
        /// </param>
        /// <param name="input">
        /// The string to convert into values.
        /// </param>
        /// <param name="expectedValueCount">
        /// The number of values expected.
        /// </param>
        /// <param name="message">
        /// A message string that descripes the format the paramters are in.
        /// </param>
        /// <returns>
        /// The converted values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// If the format of the value string is not as expected.
        /// </exception>
        public static T[] ConvertToValues<T>(
            ITypeDescriptorContext context,
            CultureInfo culture,
            string input,
            int expectedValueCount,
            string message )
        {
            Contract.Requires<ArgumentNullException>( input != null );

            if( culture == null )
                culture = CultureInfo.CurrentCulture;

            string seperator = culture.TextInfo.ListSeparator;
            return ConvertToValues<T>( context, culture, input, expectedValueCount, message, seperator );
        }

        /// <summary>
        /// Tries to convert the given value string into the equivalent of real values.
        /// </summary>
        /// <typeparam name="T">
        /// The type of data to convert to.
        /// </typeparam>
        /// <param name="context">
        /// An <see cref="ITypeDescriptorContext"/> that provides a format context.
        /// </param>
        /// <param name="culture">
        /// A <see cref="System.Globalization.CultureInfo"/>. If null is passed, 
        /// the current culture is assumed.
        /// </param>
        /// <param name="input">
        /// The string to convert into values.
        /// </param>
        /// <param name="expectedValueCount">
        /// The number of values expected.
        /// </param>
        /// <param name="message">
        /// A message string that descripes the format the paramters are in.
        /// </param>
        /// <param name="separator">
        /// Tge string the individual values in the input are separated with.
        /// </param>
        /// <returns>
        /// The converted values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// If the format of the value string is not as expected.
        /// </exception>
        public static T[] ConvertToValues<T>(
            ITypeDescriptorContext context,
            CultureInfo culture,
            string input,
            int expectedValueCount,
            string message,
            string separator )
        {
            Contract.Requires<ArgumentNullException>( input != null );
            Contract.Requires<ArgumentNullException>( separator != null );
            
            var strArray = input.Trim().Split( new string[1] { separator }, StringSplitOptions.RemoveEmptyEntries );            
            var result = new T[strArray.Length];

            if( result.Length != expectedValueCount )
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        ErrorStrings.ExpectedDifferentNumberOfElementsInStringXSeparatedByYGotZInsteadOfW,
                        input,
                        separator,
                        result.Length.ToString( CultureInfo.CurrentCulture ),
                        expectedValueCount.ToString( CultureInfo.CurrentCulture )
                    ),
                    "expectedValueCount"
                );
            }

            var converter = TypeDescriptor.GetConverter( typeof( T ) );

            for( int i = 0; i < result.Length; ++i )
            {
                string str = strArray[i].Trim();

                try
                {
                    result[i] = (T)converter.ConvertFromString( context, culture, str );
                }
                catch( StackOverflowException )
                {
                    throw;
                }
                catch( OutOfMemoryException )
                {
                    throw;
                }
                catch( Exception exception )
                {
                    string exceptionMessage = string.Format(
                        CultureInfo.CurrentCulture,
                        ErrorStrings.InvalidStringXFormatYWithSeparatorZ,
                        str ?? string.Empty,
                        message ?? string.Empty,
                        separator
                    );

                    throw new ArgumentException( exceptionMessage, "input", exception );
                }
            }

            return result;
        }
    }
}