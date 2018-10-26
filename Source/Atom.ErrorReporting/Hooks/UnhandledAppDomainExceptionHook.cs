// <copyright file="UnhandledAppDomainExceptionHook.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Hooks.UnhandledAppDomainExceptionHook class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Hooks
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Implements an <see cref="IErrorHook"/> that hooks onto <see cref="AppDomain.UnhandledException"/> to
    /// catch exceptions.
    /// </summary>
    public sealed class UnhandledAppDomainExceptionHook : IErrorHook
    {
        /// <summary>
        /// Initializes a new instance of the UnhandledAppDomainExceptionHook class,
        /// that hooks the current AppDomain.
        /// </summary>
        /// <param name="errorFactory">
        /// The IExceptionErrorFactory used to create IExceptionErrors given an Exception.
        /// </param>
        /// <param name="errorReporter">
        /// The IErrorReporter to which IExceptionErrors are passed.
        /// </param>
        public UnhandledAppDomainExceptionHook( IExceptionErrorFactory errorFactory,  IErrorReporter errorReporter )
            : this( AppDomain.CurrentDomain, errorFactory, errorReporter )
        {
        }

        /// <summary>
        /// Initializes a new instance of the UnhandledAppDomainExceptionHook class.
        /// </summary>
        /// <param name="appDomain">
        /// The AppDomain to hook on.
        /// </param>
        /// <param name="errorFactory">
        /// The IExceptionErrorFactory used to create IExceptionErrors given an Exception.
        /// </param>
        /// <param name="errorReporter">
        /// The IErrorReporter to which IExceptionErrors are passed.
        /// </param>
        public UnhandledAppDomainExceptionHook( AppDomain appDomain, IExceptionErrorFactory errorFactory, IErrorReporter errorReporter )
        {
            Contract.Requires<ArgumentNullException>( appDomain != null );
            Contract.Requires<ArgumentNullException>( errorFactory != null );
            Contract.Requires<ArgumentNullException>( errorReporter != null );

            this.appDomain = appDomain;
            this.errorReporter = errorReporter;
            this.errorFactory = errorFactory;
        }

        /// <summary>
        /// Hooks this IErrorHook.
        /// </summary>
        public void Hook()
        {
            this.appDomain.UnhandledException += this.OnUnhandledException;
        }

        /// <summary>
        /// Unhooks this IErrorHook.
        /// </summary>
        public void Unhook()
        {
            this.appDomain.UnhandledException += this.OnUnhandledException;
        }

        /// <summary>
        /// Called when an exception has occurred in the AppDomain this UnhandledAppDomainExceptionHook
        /// is hooked on.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="UnhandledExceptionEventArgs"/> that contain the event data.
        /// </param>
        private void OnUnhandledException( object sender, UnhandledExceptionEventArgs e )
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
        /// The <see cref="UnhandledExceptionEventArgs"/> that contain the event data.
        /// </param>
        /// <returns>
        /// The newly created IExceptionError.
        /// </returns>
        private IExceptionError CreateError( UnhandledExceptionEventArgs e )
        {
            return this.errorFactory.CreateError( (Exception)e.ExceptionObject, e.IsTerminating );
        }

        /// <summary>
        /// The AppDomain this UnhandledAppDomainExceptionHook hooks on.
        /// </summary>
        private readonly AppDomain appDomain;

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
