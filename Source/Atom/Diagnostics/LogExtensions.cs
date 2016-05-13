// <copyright file="LogExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Diagnostics.LogExtension extension class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Diagnostics
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Defines <see cref="ILog"/> related extension methods.
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        /// Writes a message by compositing the given string <paramref name="format"/> 
        /// with the given object <paramref name="args"/> to this BaseLog using
        /// the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="log">
        /// The <see cref="ILog"/> to write to.
        /// </param>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the composed message.
        /// </param>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="args">
        /// An System.Object array containing zero or more objects to format.
        /// </param>
        public static void Write( this ILog log, LogSeverities severity, string format, params object[] args )
        {
            var message = string.Format( CultureInfo.CurrentCulture, format, args );
            log.Write( severity, message );
        }

        /// <summary>
        /// Writes a message, followed by a new line, by compositing the 
        /// given string <paramref name="format"/> with the given object <paramref name="args"/> 
        /// to this BaseLog using the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="log">
        /// The <see cref="ILog"/> to write to.
        /// </param>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the composed message.
        /// </param>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="args">
        /// An System.Object array containing zero or more objects to format.
        /// </param>
        public static void WriteLine( this ILog log, LogSeverities severity, string format, params object[] args )
        {
            var message = string.Format( CultureInfo.CurrentCulture, format, args );
            log.WriteLine( severity, message );
        }

        /// <summary>
        /// Writes a message by compositing the given string <paramref name="format"/> 
        /// with the given object <paramref name="args"/> to this <see cred="ILog"/> using
        /// the default <see cref="LogSeverities"/> of this ILog.
        /// </summary>
        /// <param name="log">
        /// The <see cref="ILog"/> to write to.
        /// </param>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="args">
        /// An System.Object array containing zero or more objects to format.
        /// </param>
        public static void Write( this ILog log, string format, params object[] args )
        {
            log.Write( log.DefaultSeverity, format, args );
        }

        /// <summary>
        /// Writes a message, followed by a new line, by compositing the 
        /// given string <paramref name="format"/> with the given object <paramref name="args"/> 
        /// to this <see cred="ILog"/> using the default <see cref="LogSeverities"/> of this ILog.
        /// </summary>
        /// <param name="log">
        /// The <see cref="ILog"/> to write to.
        /// </param>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="args">
        /// An System.Object array containing zero or more objects to format.
        /// </param>
        public static void WriteLine( this ILog log, string format, params object[] args )
        {
            log.WriteLine( log.DefaultSeverity, format, args );
        }

        /// <summary>
        /// Writes the specified <paramref name="enumerable"/> into the <paramref name="log"/>
        /// by calling ToString() on every element, using the default <see cref="LogSeverities"/> of this ILog.
        /// </summary>
        /// <typeparam name="T"> 
        /// The type of the elements stored in the array. 
        /// </typeparam>
        /// <param name="log">
        /// The log to write to.
        /// </param>
        /// <param name="enumerable">
        /// The elements to write into the log. 
        /// The enumerable and any of its elements can be null
        /// (An empty entry is written if an element is null).
        /// </param>
        public static void Write<T>( this ILog log, IEnumerable<T> enumerable )
        {
            Write<T>( log, log.DefaultSeverity, enumerable );
        }

        /// <summary>
        /// Writes the specified <paramref name="enumerable"/> into the <paramref name="log"/>
        /// by calling ToString() on every element and adding a new line after each,
        /// using the default <see cref="LogSeverities"/> of this ILog.
        /// </summary>
        /// <typeparam name="T"> 
        /// The type of the elements stored in the array. 
        /// </typeparam>
        /// <param name="log">
        /// The log to write to.
        /// </param>
        /// <param name="enumerable">
        /// The enumerable to write into the log. 
        /// The enumerable and any of its elements can be null
        /// (An empty line is written if an element is null).
        /// </param>
        public static void WriteLine<T>( this ILog log, IEnumerable<T> enumerable )
        {
            WriteLine<T>( log, log.DefaultSeverity, enumerable );
        }

        /// <summary>
        /// Writes the specified <paramref name="enumerable"/> into the <paramref name="log"/>
        /// by calling ToString() on every element.
        /// </summary>
        /// <typeparam name="T"> 
        /// The type of the elements stored in the array. 
        /// </typeparam>
        /// <param name="log">
        /// The log to write to.
        /// </param>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the message.
        /// </param>
        /// <param name="enumerable">
        /// The elements to write into the log. 
        /// The enumerable and any of its elements can be null
        /// (An empty entry is written if an element is null).
        /// </param>
        public static void Write<T>( this ILog log, LogSeverities severity, IEnumerable<T> enumerable )
        {
            if( enumerable == null )
                return;

            foreach( T item in enumerable )
            {
                log.Write( severity, item == null ? string.Empty : item.ToString() );
            }
        }

        /// <summary>
        /// Writes the specified <paramref name="enumerable"/> into the <paramref name="log"/>
        /// by calling ToString() on every element and adding a new line after each.
        /// </summary>
        /// <typeparam name="T"> 
        /// The type of the elements stored in the array. 
        /// </typeparam>
        /// <param name="log">
        /// The log to write to.
        /// </param>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the message.
        /// </param>
        /// <param name="enumerable">
        /// The enumerable to write into the log. 
        /// The enumerable and any of its elements can be null
        /// (An empty line is written if an element is null).
        /// </param>
        public static void WriteLine<T>( this ILog log, LogSeverities severity, IEnumerable<T> enumerable )
        {
            if( enumerable == null )
                return;

            foreach( T item in enumerable )
            {
                log.WriteLine( severity, item != null ? item.ToString() : string.Empty );
            }
        }
                
        /// <summary>
        /// Gets a value indicating whether given given <paramref name="severities"/>
        /// are (partially) contained by this <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="severities">
        /// This LogSeverity.
        /// </param>
        /// <param name="severitiesToCheck">
        /// The LogSeverity to check for.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the given LogSeverities are set in this LogSeverity;
        /// or otherwise <see langword="false"/>.
        /// </returns>
        public static bool Contains( this LogSeverities severities, LogSeverities severitiesToCheck )
        {
            return (severities & severitiesToCheck) != 0;
        } 
    }
}
