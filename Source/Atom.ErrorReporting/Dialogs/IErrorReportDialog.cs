// <copyright file="IErrorReportDialog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Dialogs.IErrorReportDialog interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Dialogs
{
    /// <summary>
    /// Provides a mechanism for showing a report about an <see cref="IError"/> to the user.
    /// </summary>
    public interface IErrorReportDialog : System.IDisposable
    {
        /// <summary>
        /// Shows this IErrorReportDialog.
        /// </summary>
        /// <param name="error">
        /// The <see cref="IError"/> to report to the user.
        /// </param>
        void Show( IError error );
    }
}
