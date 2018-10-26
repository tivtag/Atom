// <copyright file="TextLineErrorMessageFormatter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.TextLineErrorMessageFormatter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Formatters
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents an <see cref="IErrorMessageFormatter"/> that always returns the same string.
    /// </summary>
    public class TextLineErrorMessageFormatter : IErrorMessageFormatter
    {
        /// <summary>
        /// Gets or sets the text this TextLineErrorMessageFormatter returns.
        /// </summary>
        public string Text
        {
            get
            {
                // Contract.Ensures( Contract.Result<string>() != null );

                return this.text;
            }

            set
            {
                Contract.Requires<ArgumentNullException>( value != null );

                this.text = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the TextLineErrorMessageFormatter class.
        /// </summary>
        public TextLineErrorMessageFormatter()
        {
            this.text = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the TextLineErrorMessageFormatter class.
        /// </summary>
        /// <param name="text">
        /// The text the new TextLineErrorMessageFormatter should return.
        /// </param>
        public TextLineErrorMessageFormatter( string text )
        {
            Contract.Requires<ArgumentNullException>( text != null );

            this.text = text;
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
            return this.Text;
        }

        /// <summary>
        /// Represents the storage field of the <see cref="Text"/> property.
        /// </summary>
        private string text;
    }
}
