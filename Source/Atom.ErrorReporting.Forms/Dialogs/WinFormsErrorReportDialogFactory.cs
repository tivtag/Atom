// <copyright file="WinFormsErrorReportDialogFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.WinFormsErrorReportDialogFactory class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Dialogs
{
    /// <summary>
    /// Implements a mechanism for creating instances fo the <see cref="IErrorReportDialog"/> interface.
    /// This class can't be inherited.
    /// </summary>
    public sealed class WinFormsErrorReportDialogFactory : IErrorReportDialogFactory
    {
        /// <summary>
        /// Initializes a new instance of the WinFormsErrorReportDialogFactory class.
        /// </summary>
        /// <param name="mailReporter">
        /// The optional reporter that allows users to send an error report by e-mail.
        /// </param>
        public WinFormsErrorReportDialogFactory( IErrorReporter mailReporter = null )
        {
            this.mailReporter = mailReporter;
        }

        /// <summary>
        /// Creates and then returns a new concrete instance of the IErrorReportDialog interface.
        /// </summary>
        /// <returns>
        /// The newly created IErrorReportDialog.
        /// </returns>
        public IErrorReportDialog Build()
        {
            return new ErrorReportDialog( this.mailReporter );
        }

        /// <summary>
        /// The optional reporter that allows users to send an error report by e-mail.
        /// </summary>
        private readonly IErrorReporter mailReporter;
    }
}
