// <copyright file="TextFileErrorMessageFormatter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Formatters.TextFileErrorMessageFormatter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.ErrorReporting.Formatters
{
    using System;
    using Atom.Diagnostics.Contracts;
    using System.IO;

    /// <summary>
    /// Implements an <see cref="IErrorMessageFormatter"/> that returns the content of a text file.
    /// </summary>
    public class TextFileErrorMessageFormatter : IErrorMessageFormatter
    {
        /// <summary>
        /// Initializes a new instance of the TextFileErrorMessageFormatter class.
        /// </summary>
        /// <param name="fileName">
        /// The full name of the file to read.
        /// </param>
        public TextFileErrorMessageFormatter( string fileName )
        {
            Contract.Requires<ArgumentNullException>( fileName != null );

            this.fileName = fileName;
        }

        /// <summary>
        /// Formats and then returns the error message for the given IError.
        /// </summary>
        /// <param name="error">
        /// The error whose error message should be formated.
        /// </param>
        /// <returns>
        /// The formated error message.
        /// </returns>
        public string Format( IError error )
        {
            try
            {
                using( var stream = new FileStream( this.fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
                {
                    var reader = new StreamReader( stream );
                    return reader.ReadToEnd();
                }
            }
            catch( IOException )
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// The full name of the file to read.
        /// </summary>
        private readonly string fileName;
    }
}
