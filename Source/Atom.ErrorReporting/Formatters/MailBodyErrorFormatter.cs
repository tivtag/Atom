// <copyright file="MailBodyErrorFormatter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Formatters.MailBodyErrorFormatter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Formatters
{
    using System;
    using System.Text;

    /// <summary>
    /// Implements the default <see cref="IErrorMessageFormatter"/> responsible for creating
    /// the error message shown in an e-mail.
    /// </summary>
    public sealed class MailBodyErrorFormatter : IErrorMessageFormatter
    {
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
            var exceptionError = error as IExceptionError;

            string body;

            if( exceptionError != null )
            {
                body = GetBody( exceptionError );
            }
            else
            {
                body = error.Description;
            }

            return body;
        }

        /// <summary>
        /// Gets the body of the e-mail.
        /// </summary>
        /// <param name="exceptionError">
        /// The exception error that was reported.
        /// </param>
        /// <returns>
        /// The body of the e-mail.
        /// </returns>
        private static string GetBody( IExceptionError exceptionError )
        {
            var exception = exceptionError.Exception;

            var sb = new StringBuilder();
            AppendException( exception, sb );

            sb.AppendLine();
            sb.AppendLine();

            sb.AppendLine( "Stack Trace" );
            sb.AppendLine( exception.StackTrace );

            return sb.ToString();
        }

        /// <summary>
        /// Appends information about the specified exception.
        /// </summary>
        /// <param name="exception">
        /// The exception to append.
        /// </param>
        /// <param name="sb">
        /// The StringBuilder to append to.
        /// </param>
        private static void AppendException( Exception exception, StringBuilder sb )
        {
            sb.AppendLine( exception.GetType().Name );
            sb.AppendLine( exception.Message );

            if( exception.InnerException != null )
            {
                sb.AppendLine();
                AppendException( exception.InnerException, sb );
            }
        }
    }
}
