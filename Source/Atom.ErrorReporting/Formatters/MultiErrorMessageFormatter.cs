// <copyright file="MultiErrorMessageFormatter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Formatters.MultiErrorMessageFormatter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Formatters
{
    using System.Text;
    using Atom.Collections;

    /// <summary>
    /// Implements an <see cref="IErrorMessageFormatter"/> that consists of multiple other IErrorMessageFormatters
    /// whose messages are appended in-order.
    /// </summary>
    public sealed class MultiErrorMessageFormatter : NonNullList<IErrorMessageFormatter>, IErrorMessageFormatter
    {
        /// <summary>
        /// Gets or sets the string that is inserted between the different messages
        /// of the IErrorMessageFormatters this MultiErrorMessageFormatter contains.
        /// </summary>
        /// <value>
        /// The default value is "\n". Allowed to be null.
        /// </value>
        public string Separator
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the MultiErrorMessageFormatter class.
        /// </summary>
        public MultiErrorMessageFormatter()
        {
            this.Separator = "\n";
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
            var sb = new StringBuilder();

            for( int index = 0; index < this.Count; ++index )
            {
                sb.Append( this[index].Format( error ) );

                if( this.Separator != null && index < this.Count - 1 )
                {
                    sb.Append( this.Separator );
                }
            }

            return sb.ToString();
        }
    }
}
