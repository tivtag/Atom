// <copyright file="ApplicationThreadExceptionHook.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Hooks.ApplicationThreadExceptionHook class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Hooks
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Windows.Forms;
    using Atom.ErrorReporting.Errors;

    /// <summary>
    /// Implements an <see cref="IErrorHook"/> that hooks onto <see cref="Application.ThreadException"/> to
    /// catch exceptions.
    /// </summary>
    public sealed class ApplicationThreadExceptionHook : IErrorHook
    {
        /// <summary>
        /// Initializes a new instance of the ApplicationThreadExceptionHook class.
        /// </summary>
        /// <param name="errorReporter">
        /// The IErrorReporter to which IExceptionErrors are passed.
        /// </param>
        public ApplicationThreadExceptionHook( IErrorReporter errorReporter )
            : this( errorReporter, new ExceptionErrorFactory() )
        {
        }

        /// <summary>
        /// Initializes a new instance of the ApplicationThreadExceptionHook class.
        /// </summary>
        /// <param name="errorReporter">
        /// The IErrorReporter to which IExceptionErrors are passed.
        /// </param>
        /// <param name="errorFactory">
        /// The IExceptionErrorFactory used to create IExceptionErrors given an Exception.
        /// </param>
        public ApplicationThreadExceptionHook( IErrorReporter errorReporter, IExceptionErrorFactory errorFactory )
        {
            Contract.Requires<ArgumentNullException>( errorReporter != null );
            Contract.Requires<ArgumentNullException>( errorFactory != null );

            this.errorReporter = errorReporter;
            this.errorFactory = errorFactory;
        }

        /// <summary>
        /// Hooks this IErrorHook.
        /// </summary>
        public void Hook()
        {
            Application.ThreadException += this.OnUnhandledException;
        }

        /// <summary>
        /// Unhooks this IErrorHook.
        /// </summary>
        public void Unhook()
        {
            Application.ThreadException -= this.OnUnhandledException;
        }

        /// <summary>
        /// Called when an exception has occurred.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="UnhandledExceptionEventArgs"/> that contain the event data.
        /// </param>
        private void OnUnhandledException( object sender, ThreadExceptionEventArgs e )
        {
            IExceptionError error = this.CreateError( e );
            this.ReportError( error );
        }

        /// <summary>
        /// Reports the specified IExceptionError.
        /// </summary>
        /// <param name="error">
        /// The IExceptionError that has occurred.
        /// </param>
        private void ReportError( IExceptionError error )
        {
            this.errorReporter.Report( error );
        }

        /// <summary>
        /// Creates an IExceptionError object given the event data.
        /// </summary>
        /// <param name="e">
        /// The <see cref="ThreadExceptionEventArgs"/> that contain the event data.
        /// </param>
        /// <returns>
        /// The newly created IExceptionError.
        /// </returns>
        private IExceptionError CreateError( ThreadExceptionEventArgs e )
        {
            return this.errorFactory.CreateError( (Exception)e.Exception, true );
        }

        /// <summary>
        /// The IExceptionErrorFactory used to create IExceptionErrors given an Exception.
        /// </summary>
        private readonly IExceptionErrorFactory errorFactory;

        /// <summary>
        /// The IErrorReporter to which IExceptionErrors are passed.
        /// </summary>
        private readonly IErrorReporter errorReporter;
    }
}
