// <copyright file="ExceptionError.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Errors.ExceptionError class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Errors
{
    using System;
    using System.Diagnostics.Contracts;
    
    /// <summary>
    /// Represents an IError that relates to a specific <see cref="Exception"/>.
    /// </summary>
    public class ExceptionError : IExceptionError
    {
        /// <summary>
        /// Gets the type-name of the exception.
        /// </summary>
        public string Name
        {
            get
            {
                return this.exception.GetType().Name;
            }
        }

        /// <summary>
        /// Gets the message of the Exception.
        /// </summary>
        public string Description
        {
            get
            {
                return this.exception.Message;
            }
        }

        /// <summary>
        /// Get a value indicating whether the exception was fatal
        /// and required the application to shutdown.
        /// </summary>
        public bool IsFatal
        {
            get
            {
                return this.isFatal;
            }
        }

        /// <summary>
        /// Gets the actual <see cref="Exception"/> object.
        /// </summary>
        public Exception Exception
        {
            get
            {
                return this.exception;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ExceptionError class.
        /// </summary>
        /// <param name="exception">
        /// The actual <see cref="Exception"/> object.
        /// </param>
        /// <param name="isFatal">
        /// States whether the exception was fatal
        /// and requires the application to shutdown.
        /// </param>
        public ExceptionError( Exception exception, bool isFatal )
        {
            Contract.Requires<ArgumentNullException>( exception != null );

            this.exception = exception;
            this.isFatal = isFatal;
        }
        
        /// <summary>
        /// Creates and returns a string representation of the current ExceptionError.
        /// </summary>
        /// <returns>
        /// A string representation of the current error.
        /// </returns>
        public override string ToString()
        {
            return this.exception.ToString();
        }

        /// <summary>
        /// The actual <see cref="Exception"/> object.
        /// </summary>
        private readonly Exception exception;

        /// <summary>
        /// States whether the exception was fatal and requires the application to shutdown.
        /// </summary>
        private readonly bool isFatal;
    }
}
