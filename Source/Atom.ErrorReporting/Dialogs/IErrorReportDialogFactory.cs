// <copyright file="IErrorReportDialogFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.IErrorReportDialogFactory interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Dialogs
{
    /// <summary>
    /// Provides a mechanism for creating instances fo the <see cref="IErrorReportDialog"/> interface.
    /// </summary>
    public interface IErrorReportDialogFactory
    {
        /// <summary>
        /// Creates and then returns a new concrete instance of the IErrorReportDialog interface.
        /// </summary>
        /// <returns>
        /// The newly created IErrorReportDialog.
        /// </returns>
        IErrorReportDialog Build();
    }
}
